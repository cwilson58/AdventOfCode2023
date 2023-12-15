using System.Text.RegularExpressions;

public class Program
{
    //Test data stored in some file read as raw text
    const string InputFilePath = "TestData.txt";
    public static void Main()
    {
        using var file = File.Open(InputFilePath, FileMode.Open, FileAccess.Read);
        using var reader = new StreamReader(file);
        var line = reader.ReadLine();
        var sum = 0;
        while (line != null)
        {
            line = ReplaceWordsWithDigit(line); // remove for part 1
            var digitsCollection = Regex.Matches(line.ToLower(), @"\d");
            if (digitsCollection.Any())
            {
                var number = Convert.ToInt32(digitsCollection[0].Value) * 10 + Convert.ToInt32(digitsCollection[^1].Value);
                sum += number;
            }

            line = reader.ReadLine();
        }
        Console.WriteLine(sum);
    }

    // By replacing the word with the digit and any potential ovelap characters we can handle that edge case with the regex above.
    private static string ReplaceWordsWithDigit(string input)
    {
        input = input.Replace("one", "o1e")
                    .Replace("two", "t2o")
                    .Replace("three", "t3e")
                    .Replace("four", "4")
                    .Replace("five", "5e")
                    .Replace("six", "6")
                    .Replace("seven", "7n")
                    .Replace("eight", "e8t")
                    .Replace("nine", "n9e");
        return input;
    }
}