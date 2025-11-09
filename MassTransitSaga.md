Что касается самого понятия Saga, то это паттерн для управления долгоживущими распределенными транзакциями. Saga разбивает сложный бизнес-процесс на последовательность
локальных транзакций, где каждый шаг имеет компенсирующее действие для отката в случае сбоя.
MassTransit предоставляет реализацию этого паттерна через State Machine Sagas, используя бибилотеку Automatonymous для создания конечных автоматов, которые как раз и 
управляют состоянием и потоком выполнения саги.

Наша логика: зарезервировать билет, ждать оплату (максимум 15 минут), подтвердить продажу (если оплата пришла) или отменить резерв (если таймаут или отказ).

Saga выступает оркестратором, т.е. публикует команды/события другим сервисам (Inventory/Payment например) и отслеживает состояние.

Можно сделать следующие сообщения:
public record ReserveTicket(Guid OrderId, Guid TicketId, int Quantity, DateTimeOffset RequestedAt);
public record TicketReserved(Guid OrderId, Guid ReservationId, Guid TicketId, int Quantity);
public record TicketReservationFailed(Guid OrderId, string Reason);

public record PaymentSubmitted(Guid OrderId, Guid PaymentId, decimal Amount);
public record PaymentSucceeded(Guid OrderId, Guid PaymentId);
public record PaymentFailed(Guid OrderId, Guid PaymentId, string Reason);

public record PaymentTimeoutExpired(Guid OrderId);
public record OrderConfirmed(Guid OrderId, Guid ReservationId);
public record OrderCancelled(Guid OrderId, string Reason);
Примечание: по OrderId будут связываться сообщения (корреляционный идентификатор).

Модель состояния:
public class OrderState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }           
    public string CurrentState { get; set; }
    public Guid? ReservationId { get; set; }
    public Guid? PaymentId { get; set; }
    public DateTimeOffset? ReservationExpiresAt { get; set; }
    public Guid? PaymentTimeoutTokenId { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}

Затем используя generic MassTransitStateMachine API можно выделить важные моменты:
Initially — при получении TicketReserved переходим в WaitingForPayment.
Настраиваем Schedule внутри state machine — MassTransit создаёт consumer для планируемого сообщения.
При PaymentSucceeded — подтверждение и Finalize().
При PaymentTimeoutExpired или PaymentFailed — отмена и компенсация (освобождение резерва), затем Finalize().
SetCompletedWhenFinalized() для очистки instance.

Также можем рассмотреть сценарий:

1. Клиент инициирует заказ -> сервис бронирования публикует TicketReserved(OrderId, ReservationId, ...).

2. Saga создаётся и сохраняет ReservationId, планирует PaymentTimeoutExpired через 15 минут и публикует команду/уведомление платежному сервису о необходимости оплатить (или просто ждёт сообщения от платежника).

3. Клиент платит -> PaymentSucceeded приходит в шину. Saga отменяет запланированный таймаут, публикует OrderConfirmed, финализирует состояние.

4. Если 15 минут прошли и PaymentSucceeded не пришло, то планировщик отправляет PaymentTimeoutExpired → Saga публикует OrderCancelled и финализирует. 
Также публикует команду на отмену резерва в Inventory.

5. При PaymentFailed — аналогично таймауту: отмена.