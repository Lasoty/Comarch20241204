using ComarchCwiczenia.Services;

namespace ComarchCwiczenia.Unit.Test.Services;

[TestFixture]
public class DateCalculatorTests
{
    private DateCalculator cut;
    [SetUp]
    public void Setup()
    {
        cut = new DateCalculator();
    }

    [TestCase(2024, 11, 29, 2024, 12, 2)]
    [TestCase(2024, 11, 30, 2024, 12, 2)]
    [TestCase(2024, 12, 1, 2024, 12, 2)]
    public void GetNextBusinessDayShouldSkipWeekends(
        int dYear, int dMonth, int dDay,
        int eYear, int eMonth, int eDay)
    {
        // Arrange
        DateTime day = new DateTime(dYear, dMonth, dDay);
        DateTime expected = new DateTime(eYear, eMonth, eDay);

        // Act
        DateTime actual = cut.GetNextBusinessDay(day);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [TestCase(2024, 11, 25)]
    [TestCase(2024, 11, 26)]
    [TestCase(2024, 11, 27)]
    [TestCase(2024, 11, 28)]
    public void GetNextBusinessDayShouldBeAfterInputDate(
        int dYear, int dMonth, int dDay)
    {
        // Arrange
        DateTime day = new DateTime(dYear, dMonth, dDay);
        DateTime expected = day.AddDays(1);

        // Act
        DateTime actual = cut.GetNextBusinessDay(day);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }
}