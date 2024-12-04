
namespace ComarchCwiczenia.Services;

public class DateCalculator
{
    public DateTime GetNextBusinessDay(DateTime day)
    {
        do
        {
            day = day.AddDays(1);
        } while (day.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday);

        return day;
    }
}