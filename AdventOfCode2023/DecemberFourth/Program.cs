using System.Text.RegularExpressions;

public class Program
{
    private static string FilePath = "SampleInput.txt";

    public static void Main(string[] args){
        if(args.Length == 1){
            FilePath = args[0];
        }
        Console.WriteLine($"Part One Soln: {SolvePartOne()}");
        Console.WriteLine($"Part Two Soln: {SolvePartTwo()}");
    }

    private static int SolvePartOne()
    {
        using var file = File.Open(FilePath, FileMode.Open, FileAccess.Read);
        using var reader = new StreamReader(file);
        var sumOfPoints = 0;
        var line = reader.ReadLine();
        while(line != null)
        {
            var matches = GetMatches(line);

            if(matches.Any())
            {
                sumOfPoints += (1 << matches.Count - 1); // Doubles for every match, same as left shift.
            }

            line = reader.ReadLine();
        }
        return sumOfPoints;
    }

    private static List<int> GetMatches(string line)
    {

        var split = line.Substring(line.IndexOf(":") + 2).Split("|");
        var winningNumbersInt = Regex.Matches(split[0],@"\d{1,}").Select(x => Convert.ToInt32(x.Value));
        return Regex.Matches(split[1],@"\d{1,}")
                    .Select(x => Convert.ToInt32(x.Value))
                    .Where(x => winningNumbersInt.Contains(x))
                    .ToList();
    }

    private static int SolvePartTwo()
    {
        // Dictionary of Game # [1,INT_MAX], # of cards [1,INT_MAX]
        var cardNumbersById = new Dictionary<int,int>();
        using var file = File.Open(FilePath,FileMode.Open,FileAccess.Read);
        using var reader = new StreamReader(file);
        var line = reader.ReadLine();
        var gameNumber = 0;
        while(line != null)
        {
            gameNumber++;
            var matches = GetMatches(line);
            if(!cardNumbersById.ContainsKey(gameNumber))
            {
                cardNumbersById.Add(gameNumber,1);
            }
            else
            {
                cardNumbersById[gameNumber] += 1;
            }

            if(matches.Any())
            {
                for(var i = gameNumber + 1; i <= gameNumber + matches.Count; i++)
                {
                    if(!cardNumbersById.ContainsKey(i))
                    {
                        cardNumbersById.Add(i,cardNumbersById[gameNumber]);
                    }
                    else
                    {
                        cardNumbersById[i] += cardNumbersById[gameNumber];
                    }
                }
            }

            line = reader.ReadLine();
        }
        return cardNumbersById.Where(x => x.Key <= gameNumber).Sum(x => x.Value);
    }
}

