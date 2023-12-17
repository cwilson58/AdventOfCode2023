using System.Text.RegularExpressions;

public class Program
{
    const string filePath = "PuzzleInput.txt";
    const string regexSymbols = @"[!@#$%&*_=+/-]{1,}";
    const string regexNumbers = @"(\D){0,1}(\d{1,}(\D{0,1}))";
    static readonly List<string> OtherGuysBS = new();

    public static void Main()
    {
        SolvePartOne();
    }

    public static void SolvePartOne(){
        using var file = File.Open(filePath,FileMode.Open,FileAccess.Read);
        using var stream = new StreamReader(file);
        var line1 = stream.ReadLine();
        var line2 = stream.ReadLine();
        // process line 1
        if(line1 == null) return;
        var sum = 0;
        var numberOfMatches = 0;
        var matches = Regex.Matches(line1,regexNumbers);
        foreach(Match match in matches)
        {
            var number = match.Value;
            if(number != null)
            {
                var index = line1.IndexOf(number);
                if(index > 0 && number[0] >= '0' && number[0] <= '9'){
                    number = line1.Substring(index - 1,number.Length + 1); //we have run into an overlap, add the previous character back in
                    index = line1.IndexOf(number);
                }
                if(index > -1)
                {
                    var length = number.Length;
                    //account for number being at the start of the line
                    if(index + length > line1.Length)
                    {
                        length -= 1;
                    }
                    var isSymbolSameLine = Regex.IsMatch(line1.Substring(index, length),regexSymbols);
                    if(line2 == null) line2 = "";
                    var isSymbolLineBelow = Regex.IsMatch(line2.Substring(index, length),regexSymbols);
                    if(isSymbolSameLine || isSymbolLineBelow)
                    {
                        numberOfMatches++;
                        var num = 0;
                        if(Regex.IsMatch(number,@"\D\d{1,}.?"))
                        {
                            num = Convert.ToInt32(number.Substring(1, number.Length-2));
                        }
                        else if(number[0] >= '0' && number[0] <= '9')
                        {
                            num = Convert.ToInt32(number.Substring(0, number.Length-1));
                        }
                        else
                        {
                            num = Convert.ToInt32(number.Substring(1,number.Length -1));
                        }
                        sum += num;
                    }
                }
            }
        }

        // go down the data reading the middle line every single time for numbers
        var line3 = stream.ReadLine();
        while(line3 != null && line2 != null && line1 != null){
            matches = Regex.Matches(line2,regexNumbers);
            foreach(Match match in matches)
            {
                var number = match.Value;
                if(number == null) continue;
                var index = line2.IndexOf(number);
                if(index > 0 && number[0] >= '0' && number[0] <= '9'){
                    while(line2[index - 1] >= '0' && line2[index - 1] <= '9'){
                        // we are at a case of ....Y72...........72 which we do not want. Regex tells us its in the line.
                        index = line2.IndexOf(number,index + 1); //start one ahead of before and look until we find a valid case.
                    }
                    number = line2.Substring(index - 1,number.Length + 1); //we have run into an overlap, add the previous character back in
                    index = line2.IndexOf(number);
                }
                if(index > -1)
                {
                    var length = number.Length;

                    if(index + length > line2.Length){
                        length -= 1;
                    }
                    var line1Sub = line1.Substring(index, length);
                    var line2Sub = line2.Substring(index, length);
                    var line3Sub = line3.Substring(index, length);
                    var isSymbolSameLine = Regex.IsMatch(line2Sub, regexSymbols);
                    var isSymbolAboveLine = Regex.IsMatch(line1Sub, regexSymbols);
                    var isSymbolBelowLine = Regex.IsMatch(line3Sub, regexSymbols);

                    if(isSymbolSameLine || isSymbolAboveLine || isSymbolBelowLine)
                    {
                        numberOfMatches++;
                        var num = 0;
                        if(Regex.IsMatch(number,@"\D\d{1,}\D"))
                        {
                            num = Convert.ToInt32(number.Substring(1, number.Length-2));
                        }
                        else if (number[0] >= '0' && number[0] <= '9')
                        {
                            if(number == "*466"){ Console.WriteLine("GAE");}
                            num = Convert.ToInt32(number.Substring(0, number.Length-1));
                        }
                        else
                        {
                            num = Convert.ToInt32(number.Substring(1));
                        }
                        sum += num;
                    }
                }
            }
            line1 = line2;
            line2 = line3;
            line3 = stream.ReadLine();
        }

        // we still have not processed the last line
        matches = Regex.Matches(line2!,regexNumbers);
        foreach(Match match in matches)
        {
            var number = match.Value;
            if(number == null) continue;

            var index = line2!.IndexOf(number);
            var length = number.Length;
            if(index > 0 && number[0] >= '0' && number[0] <= '9'){
                number = line2.Substring(index - 1,number.Length + 1); //we have run into an overlap, add the previous character back in
                index = line2.IndexOf(number);
            }
            if(index + length > line2.Length){
                length -= 1;
            }

            var isSymbolSameLine = Regex.IsMatch(line2.Substring(index, length), regexSymbols);
            var isSymbolAboveLine = Regex.IsMatch(line1!.Substring(index, length), regexSymbols);

            if(isSymbolSameLine || isSymbolAboveLine)
            {
                numberOfMatches++;
                var num = 0;
                if(Regex.IsMatch(number,@"\D\d{1,}.?"))
                {
                    num += Convert.ToInt32(number.Substring(1, number.Length-2));
                }
                else
                {
                    num += Convert.ToInt32(number.Substring(0, number.Length-1));
                }
                sum += num;
            }
        }

        Console.WriteLine($"Part One Sum is {sum}");

    }
}
