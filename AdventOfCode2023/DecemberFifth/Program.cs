using System.Text.RegularExpressions;

public class Program
{
    private static string FilePath = "SampleInput.txt";

    private static Dictionary<string,long> SeedMap = new();

    public static void Main(string[] args)
    {
        if(args.Length == 1)
        {
           FilePath = args[0];
        }

        Console.WriteLine($"Part One {SolvePartOne()}");
    }

    private static long SolvePartOne()
    {

        using var file = File.Open(FilePath,FileMode.Open,FileAccess.Read);
        using var reader = new StreamReader(file);
        var seedsAsString = reader.ReadLine();
        if(seedsAsString == null || !Regex.IsMatch(seedsAsString,@"seeds: (\d{1,} ){1,}(\d{1,})"))
        {
            throw new Exception("Input in the incorrect Format");
        }

        var cachedRangeMap = new List<MapRange>();

        reader.ReadLine(); // Discard the space after the seed list
        LoadMap(cachedRangeMap, reader);
        var seeds = seedsAsString.Substring(7).Split(" ");
        // Setup the original map
        foreach(var seed in seeds)
        {
            var isFound = false;
            foreach(var range in cachedRangeMap)
            {
                if(range.IsInRange(Convert.ToInt64(seed)))
                {
                    SeedMap.Add(seed,range.ConvertSourceToDestination(Convert.ToInt64(seed)));
                    isFound = true;
                    break;
                }
            }
            if(!isFound)
            {
                SeedMap.Add(seed,Convert.ToInt32(seed));
            }

        }

        // re-use the memory
        for(int i = 0; i < 6; i++)
        {
            cachedRangeMap.Clear();
            LoadMap(cachedRangeMap,reader);
            UpdateSeedMap(cachedRangeMap);

        }
        return SeedMap.Values.Min();
    }

    private static void UpdateSeedMap(List<MapRange> incomingMap)
    {

        var count = SeedMap.Values.Count;
        for(var i = 0; i < count; i++)
        {
            var cacheSeed = SeedMap.Values.ElementAt(i);
            var seed = SeedMap.Keys.ElementAt(i);
            foreach(var val in incomingMap)
            {
                if(val.IsInRange(cacheSeed))
                {
                    SeedMap[seed] = val.ConvertSourceToDestination(cacheSeed);
                }
            }
        }
    }

    /*
     * inputStream will go until it hits an empty string and then the function will stop loading. Do not pass in the map name.
     */
    private static void LoadMap(List<MapRange> map, StreamReader inputStream)
    {
        var line = inputStream.ReadLine();
        line = inputStream.ReadLine(); //Discard header
        while(line != null && line != " " && line != "")
        {
            var matches = Regex.Matches(line,@"(\d{1,}) (\d{1,}) (\d{1,})").First();
            var startingValue = Convert.ToInt64(matches.Groups[1].Value);
            var startingKey = Convert.ToInt64(matches.Groups[2].Value);
            var length = Convert.ToInt64(matches.Groups[3].Value);
            map.Add(new MapRange(startingValue,startingKey,length));
            line = inputStream.ReadLine();
        }
    }
}
