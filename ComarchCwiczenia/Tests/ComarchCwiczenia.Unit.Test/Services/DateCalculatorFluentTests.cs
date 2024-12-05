using ComarchCwiczenia.Services;
using FluentAssertions;
using FluentAssertions.Extensions;

namespace ComarchCwiczenia.Unit.Test.Services;

[TestFixture]
public class DateCalculatorFluentTests
{
    private DateCalculator cut;

    [SetUp]
    public void Setup()
    {
        cut = new DateCalculator();
    }

    [Test]
    public void GetNextBusinessDayShouldReturnCorrectValue()
    {
        // Arrange
        DateTime day = 2.December(2024);
        DateTime expected = 3.December(2024);

        // Act
        DateTime actual = cut.GetNextBusinessDay(day);

        // Assert
        actual.Should().Be(expected, "3rd December is after 2nd December.");
    }

    [Test]
    public void GetNextBusinessDayShouldReturnCorrectValue2()
    {
        // Act
        cut.GetNextBusinessDay(2.December(2024)).Should().Be(3.December(2024));
        cut.GetNextBusinessDay(29.November(2024)).Should().Be(2.December(2024));
        cut.GetNextBusinessDay(30.November(2024)).Should().Be(2.December(2024));
        cut.GetNextBusinessDay(1.December(2024)).Should().Be(2.December(2024));

        // test na część czasu w datetime
        cut.GetNextBusinessDay(2.December(2024).At(15,30)).Should().Be(3.December(2024).At(15,30));
    }

    [Test]
    public void GetNextBusinessDayShouldBeClosedToInputDate()
    {
        DateTime date = 29.November(2024);
        cut.GetNextBusinessDay(date).Should().BeCloseTo(date, TimeSpan.FromDays(3));
    }

    public void GetNextBusinessDayShouldBeOnWeekDay()
    {
        DateTime date = 29.November(2024);

        DateTime nextDay = cut.GetNextBusinessDay(date);
        nextDay.DayOfWeek.Should().NotBe(DayOfWeek.Saturday).And.NotBe(DayOfWeek.Sunday);
    }

    [Test]
    public void GetNextBusinessDayShouldReturnOneOfExpectedDays()
    {
        var expectedDays = new List<DateTime>
        {
            29.October(2024),
            30.October(2024)
        };

        cut.GetNextBusinessDay(28.October(2024)).Should().BeOneOf(expectedDays);
    }

}