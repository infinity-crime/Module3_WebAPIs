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

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(5, 5, 10)]
        public void Add_WithVariousInputs_ReturnsExpectedSum(int a, int b, int expected)
        {
            // Act
            int result = AddCalculator.Add(a, b);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
