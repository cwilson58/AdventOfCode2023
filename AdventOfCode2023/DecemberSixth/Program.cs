public class Program
{
    private static string FilePath = "SampleInput.txt";

    public static void Main(string[] args)
    {
        if(args.Length == 1)
        {
            FilePath = args[0];
        }

        using var file = File.Open(FilePath, FileMode.Open,FileAccess.Read);
        using var reader = new StreamReader(file);
        var line1 = reader.ReadLine();
        if(line1 == null)
        {
            throw new Exception("File Error");
        }
        line1 = line1.Substring(5).Trim();
        var line2 = reader.ReadLine();
        if(line2 == null)
        {
            throw new Exception("Distances Not In File");
        }
        line2 = line2.Substring(9).Trim();
        Console.WriteLine($"Part One Solution {SolvePartOne(line1,line2)}");
        Console.WriteLine($"Part Two Solution {SolvePartTwo(line1,line2)}");
    }

    private static long SolvePartOne(string timeLine, string distanceLine)
    {
        var timeList = timeLine.Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
        var distanceList = distanceLine.Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
        var product = 1L;
        for(var i = 0; i<distanceList.Count; i++)
        {
            var roots = GetRoots(Convert.ToInt32(timeList[i]),Convert.ToInt32(distanceList[i]));
            product *= roots.r2 - roots.r1 + 1;
        }

        return product;
    }

    private static long SolvePartTwo(string timeLine, string distanceLine)
    {
        var time = Convert.ToInt64(timeLine.Replace(" ",""));
        var distance = Convert.ToInt64(distanceLine.Replace(" ",""));
        var roots = GetRoots(time,distance);
        return roots.r2 - roots.r1 + 1;
    }

    /*
     * Uses a model of each race to get the integer roots. Roots represent where you will tie the record.
     * d = (l - t)t - r
     * where d is the distance traveled, l is the length of the race, t is the time the button is held.
     * (l - t) comes from the time remaining after you release the button, and from the problem your acceleration is instant, and your speed is equal to the amount of time held.
     * subtracting r shifts the parobola down to make distance relative to the record
     * thus, d = -t^2 + lt - r
     * returns (-1,-1) when eqn is non-solvable. Root < 0 is invalid.
     */
    private static (long r1,long r2) GetRoots(long length, long record)
    {
        var radicand = ((long)length * (long)length) - 4 * record;
        if(radicand < 0)
        {
            return (-1,-1);
        }
        var radical = Math.Sqrt(radicand);
        // Always round up as typecast rounds proper, and we want to next largest after the decimal.
        var roots = ((int)Math.Ceiling((length - radical)/2),(int)Math.Floor((length + radical) / 2));
        // catch when the root is an integer, as we need > record not >= record for the puzzle
        roots.Item1 = GetDistance(length, record, roots.Item1) == 0 ? roots.Item1 + 1 : roots.Item1;
        roots.Item2 = GetDistance(length, record, roots.Item2) == 0 ? roots.Item2 - 1 : roots.Item2;
        return roots;
    }
    // Calculates -t^2 + lt - r
    private static long GetDistance(long length, long record, int time)
    {
        return -(time * time) + length * time - record;
    }
}
