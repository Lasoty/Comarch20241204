using ComarchCwiczenia.Services;

namespace ComarchCwiczenia.Unit.Test.Services;

[TestFixture]
public class CalculatorServiceTests
{
    private CalculatorService cut;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {

    }

    [SetUp]
    public void Setup()
    {
        cut = new CalculatorService();
    }

    [TearDown]
    public void TearDown()
    {
        cut = null!;
    }

    [OneTimeTearDown]
    public Task OneTimeTearDown()
    {
       return Task.CompletedTask;
    }

    [TestCase(2,3,5)]
    [TestCase(4,5,9)]
    [TestCase(1,1,2)]
    [TestCase(0,0,0)]
    public void CalculatorAddShouldReturnCorrectSum(int x, int y, int expected)
    {
        // Arrange
        int actual = 0;

        // Act
        actual = cut.Add(x, y);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }
}