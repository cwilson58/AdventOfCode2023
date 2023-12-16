using System.Text.RegularExpressions;

public class Program
{
    const string FilePath = "PuzzleInput.txt";

    public static void Main()
    {
        using var file = File.Open(FilePath,FileMode.Open,FileAccess.Read);
        using var reader = new StreamReader(file);
        var line = reader.ReadLine();
        var sum = 0;
        var powerSum = 0;
        while(line != null)
        {
            // Lines are Game #: # COLOUR, # COLOUR, # COLOUR; # COLOUR, # COLOUR with a newline to end.
            var matches = Regex.Matches(line,@"(\d{1,}): (.*)");
            var gameId = 0;

            if(matches.Any())
            {
                var groupData = matches[0].Groups;
                gameId = Convert.ToInt32(groupData[1].Value); // group 1 is the game is, 2 is the string we want
                var games = groupData[2].Value.Split(";");
                var isValid = true;
                var minRed = 0;
                var minBlue = 0;
                var minGreen = 0;
                foreach(var game in games)
                {
                    var colours = Regex.Matches(game,@"(\d{1,}) (\w*)");
                    var red = 0;
                    var blue = 0;
                    var green = 0;
                    foreach(Match match in colours)
                    {
                        var colour = match.Groups[2].Value;
                        if(colour == "red")
                        {
                            red += Convert.ToInt32(match.Groups[1].Value);
                        }
                        else if(colour == "blue")
                        {
                            blue += Convert.ToInt32(match.Groups[1].Value);
                        }
                        else if(colour == "green")
                        {
                            green += Convert.ToInt32(match.Groups[1].Value);
                        }

                        if(red > 12 || blue > 14 || green > 13){
                            isValid = false;
                        }

                        if(red > minRed)
                        {
                            minRed = red;
                        }
                        if(blue > minBlue)
                        {
                            minBlue = blue;
                        }
                        if(green > minGreen)
                        {
                            minGreen = green;
                        }
                    }
                }
                powerSum += minRed * minBlue * minGreen;
                if(isValid){
                    sum += gameId;
                }
            }

            line = reader.ReadLine();
        }
        Console.WriteLine("Sum of Game Ids: " + sum);
        Console.WriteLine("Sum of Game Powers: " + powerSum);
    }
}
