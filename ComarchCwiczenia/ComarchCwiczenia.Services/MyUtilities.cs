namespace ComarchCwiczenia.Services;

public static class MyUtilities
{
    // Metoda 1: Odwracanie stringa
    public static string ReverseString(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        return new string(input.Reverse().ToArray());
    }

    // Metoda 2: Sprawdzenie, czy data jest w przeszłości
    public static bool IsDateInPast(DateTime date)
    {
        return date < DateTime.Now;
    }

    // Metoda 3: Filtracja unikalnych liczb większych niż podany próg
    public static IEnumerable<int> FilterUniqueNumbersAboveThreshold(IEnumerable<int> numbers, int threshold)
    {
        if (numbers == null)
            throw new ArgumentNullException(nameof(numbers), "Numbers collection cannot be null");

        if (threshold < 0)
            throw new ArgumentOutOfRangeException(nameof(threshold), "Threshold must be non-negative");

        return numbers.Where(n => n > threshold).Distinct().OrderBy(n => n);
    }
}
