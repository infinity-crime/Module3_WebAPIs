using LibCalculator;
using Xunit;

namespace LibCalculator.Tests
{
    public class LibCalculatorTests
    {
        [Fact]
        public void Add_TwoPositiveNumbers_ReturnsSum()
        {
            // Arange
            int a = 2;
            int b = 3;

            // Act
            int result = AddCalculator.Add(a, b);

            // Assert
            Assert.Equal(5, result);
        }
    }
}
