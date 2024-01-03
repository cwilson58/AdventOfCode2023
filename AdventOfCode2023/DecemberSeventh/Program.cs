public class Program
{
    private static string _FilePath = "SampleInput.txt";

    public static void Main(string[] args)
    {
        if(args.Length > 0)
        {
            _FilePath = args[0];
        }

        Console.WriteLine($"Part One Solution: {SolvePartOne()}");
    }

    private static int SolvePartOne()
    {
        using var file = File.Open(_FilePath,FileMode.Open,FileAccess.Read);
        using var reader = new StreamReader(file);

        var hands = new List<Hand>();
        var line = reader.ReadLine();
        while(line != null)
        {
            var hand = new Hand(line);
            hands.Add(hand);
            line = reader.ReadLine();
        }
        hands.Sort((left,right) => left.CompareTo(right));
        var sum = 0;
        for(int i = 0; i < hands.Count(); i++)
        {
            sum += (i+1)*hands[i].Bid;
        }
        return sum;
    }
}
