using Developer.Service.Algorithms;
using Developer.Service.Utils;

namespace Developer.Service.Tests.Algorithms;

public class AlgorithmTest
{
    [Fact]
    public void CanGetFactorial_Success()
    {
        Assert.Equal(24, Algorithm.GetFactorial(4));
    }

    [Theory]
    [InlineData(100)]
    [InlineData(-100)]
    public void CanGetFactorial_TooLargeValue_Or_Negative_Failure(int testValue)
    {
        // Arrange


        // Act / Assert
        if (testValue >= 0)
        {
            Assert.Throws<OverflowException>(() => Algorithm.GetFactorial(testValue));
        }
        else
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Algorithm.GetFactorial(testValue));
        }
    }

    [Fact]
    public void CanGetFactorialBig_Success()
    {
        // Arrange

        // Act/Assert
        Assert.True(Numerical.AreNearlyEqual(3.04140932e+64, Algorithm.GetFactorialDouble(50), 1e-9));
    }

    [Theory]
    [InlineData(1000)]
    [InlineData(-100)]
    public void CanGetFactorialBig_TooLargeValue_Or_Negative_Failure(int testValue)
    {
        // Arrange

        // Act/Assert
        if (testValue >= 0)
        {
            Assert.Throws<OverflowException>(() => Algorithm.GetFactorial(testValue));
        }
        else
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Algorithm.GetFactorial(testValue));
        }
    }

    [Theory]
    [InlineData("a, b and c", "a", "b", "c")]
    [InlineData("a and b", "a", "b")]
    [InlineData("a", "a")]
    public void CanFormatSeparators(string expected, params string[] values)
    {
        Assert.Equal(expected, Algorithm.FormatSeparators(values));
    }
}
