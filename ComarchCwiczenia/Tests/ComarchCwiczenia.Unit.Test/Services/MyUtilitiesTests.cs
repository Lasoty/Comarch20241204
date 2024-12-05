using ComarchCwiczenia.Services;
using FluentAssertions;

namespace ComarchCwiczenia.Unit.Test.Services;

[TestFixture]
public class MyUtilitiesTests
{
    #region ReverseString

    [Test]
    public void ReverseStringShouldReturnReversedStringWhenInputIsValid()
    {
        // Arrange
        string input = "hello";

        // Act
        string result = MyUtilities.ReverseString(input);

        // Assert
        result.Should().Be("olleh");
    }

    [Test]
    public void ReverseStringShouldReturnEmptyStringWhenInputIsNull()
    {
        // Act
        string result = MyUtilities.ReverseString(null);

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void ReverseStringShouldReturnEmptyStringWhenInputIsWhitespace()
    {
        // Act
        string result = MyUtilities.ReverseString("   ");

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void ReverseStringShouldHandleLongStringCorrectly()
    {
        // Arrange
        string input = new string('a', 10000);
        string expected = new string('a', 10000);

        // Act
        string result = MyUtilities.ReverseString(input);

        // Assert
        result.Should().Be(expected);
    }

    #endregion

    #region IsDateInPast

    [Test]
    public void IsDateInPastShouldReturnTrueWhenDateIsInThePast()
    {
        // Arrange
        DateTime pastDate = DateTime.Now.AddDays(-1);

        // Act
        bool result = MyUtilities.IsDateInPast(pastDate);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void IsDateInPastShouldReturnFalseWhenDateIsInTheFuture()
    {
        // Arrange
        DateTime futureDate = DateTime.Now.AddDays(1);

        // Act
        bool result = MyUtilities.IsDateInPast(futureDate);

        // Assert
        result.Should().BeFalse();
    }


    //UWAGA: TEST NIGDY NIE BĘDZIE PASSED! TEST JEST NIEDERTEMISTYCZNY. 
    //[Test]
    //public void IsDateInPastShouldReturnFalseWhenDateIsNow()
    //{
    //    // Arrange
    //    DateTime currentDate = DateTime.Now;

    //    // Act
    //    bool result = MyUtilities.IsDateInPast(currentDate);

    //    // Assert
    //    result.Should().BeFalse();
    //}

    [Test]
    public void IsDateInPastShouldReturnTrueForMinValueDate()
    {
        // Arrange
        DateTime minDate = DateTime.MinValue;

        // Act
        bool result = MyUtilities.IsDateInPast(minDate);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void IsDateInPastShouldReturnFalseForMaxValueDate()
    {
        // Arrange
        DateTime maxDate = DateTime.MaxValue;

        // Act
        bool result = MyUtilities.IsDateInPast(maxDate);

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region FilterUniqueNumbersAboveThreshold

    [Test]
    public void FilterUniqueNumbersAboveThresholdShouldReturnOrderedDistinctValuesWhenInputIsValid()
    {
        // Arrange
        IEnumerable<int> numbers = [1, 5, 5, 10, 2, 8];
        int threshold = 3;

        // Act
        var result = MyUtilities.FilterUniqueNumbersAboveThreshold(numbers, threshold);

        // Assert
        result.Should().ContainInOrder(5, 8, 10);
    }

    [Test]
    public void FilterUniqueNumbersAboveThresholdShouldReturnEmptyWhenNoNumbersAboveThreshold()
    {
        // Arrange
        IEnumerable<int> numbers = [1, 2, 3];
        int threshold = 5;

        // Act
        var result = MyUtilities.FilterUniqueNumbersAboveThreshold(numbers, threshold);

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void FilterUniqueNumbersAboveThresholdShouldHandleEmptyCollection()
    {
        // Arrange
        IEnumerable<int> numbers = [];
        int threshold = 3;

        // Act
        var result = MyUtilities.FilterUniqueNumbersAboveThreshold(numbers, threshold);

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void FilterUniqueNumbersAboveThresholdShouldThrowArgumentNullExceptionWhenNumbersIsNull()
    {
        // Arrange
        IEnumerable<int> numbers = null;

        // Act
        Action action = () => MyUtilities.FilterUniqueNumbersAboveThreshold(numbers, 5);

        // Assert
        action.Should().Throw<ArgumentNullException>()
            .WithMessage("Numbers collection cannot be null*");
    }

    [Test]
    public void FilterUniqueNumbersAboveThresholdShouldThrowArgumentOutOfRangeExceptionWhenThresholdIsNegative()
    {
        // Arrange
        IEnumerable<int> numbers = [1, 2, 3];

        // Act
        Action action = () => MyUtilities.FilterUniqueNumbersAboveThreshold(numbers, -1);

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("Threshold must be non-negative*");
    }

    #endregion

}