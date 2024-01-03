public class Hand
{
    public int Bid {get;}

    public Strength Strength {get;}

    public char FirstCard {get;}

    public string FullHand {get;}

    /// <summary>
    /// Input must be in the format CCCCC ####
    /// The number can be any value within a normal C# int.
    /// </summary>
    public Hand(string input)
    {
        Bid = Convert.ToInt32(input.Substring(6));
        FirstCard = input[0];
        var hand = input.Substring(0,5);
        FullHand = hand;
        var firstCardCount = (hand.Length - hand.Replace(hand.Substring(0,1),"").Length);

        if(firstCardCount < 4)
        {
            Strength tmp = Strength.HighCard;
            var currHand = hand.Replace(hand.Substring(0,1),"");
            if(firstCardCount is 1 or 2)
            {
                tmp = (Strength)(firstCardCount - 1);
                // we can still be one pair, two pair, three of a kind, four of a kind or stay at high card here
                var tempCardCount = currHand.Length - currHand.Replace(currHand.Substring(0,1),"").Length;
                if(tempCardCount is 4)
                {
                    tmp = Strength.FourOfAKind;
                }
                else if(tempCardCount is 3)
                {
                    tmp = tmp == Strength.HighCard ? Strength.ThreeOfAKind : Strength.FullHouse;
                }
                else if(tempCardCount is 2)
                {
                    if(tmp == Strength.OnePair)
                    {
                        tmp = Strength.TwoPair;
                    }
                    else
                    {
                        tmp = Strength.OnePair;
                        currHand = currHand.Replace(currHand.Substring(0,1),"");
                        tempCardCount = currHand.Length - currHand.Replace(currHand.Substring(0,1),"").Length;
                        if(tempCardCount > 1)
                        {
                            tmp = Strength.TwoPair;
                        }
                    }
                }
                else
                {
                    // Card 2 is for sure one of a kind. We have 3 cards remaining at most.
                    currHand = currHand.Replace(currHand.Substring(0,1),"");
                    tempCardCount = currHand.Length - currHand.Replace(currHand.Substring(0,1),"").Length;
                    if(tempCardCount is 3)
                    {
                        tmp = Strength.ThreeOfAKind;
                    }
                    else if(tempCardCount is 2)
                    {
                        tmp = tmp == Strength.OnePair ? Strength.TwoPair : Strength.OnePair;
                    }
                    else
                    {
                        currHand = currHand.Replace(currHand.Substring(0,1),"");
                        tempCardCount = currHand.Length - currHand.Replace(currHand.Substring(0,1),"").Length;
                        tmp = tempCardCount is 2 ? Strength.OnePair : tmp;
                    }
                }

            }
            else if(firstCardCount is 3)
            {
                tmp = Strength.ThreeOfAKind;
                if(currHand.Length - currHand.Replace(currHand.Substring(0,1),"").Length is 2)
                {
                    tmp = Strength.FullHouse;
                }
            }

            Strength = tmp;
        }
        else if(firstCardCount is 4)
        {
            Strength = Strength.FourOfAKind;
        }
        else
        {
            Strength = Strength.FiveOfAKind;
        }
    }

    public override string ToString()
    {
        return $"{Strength} {Bid}";
    }

    public int GetCardNumericValue(int index) => (char)FullHand[index] switch
    {
        'A' => 14,
        'K' => 13,
        'Q' => 12,
        'J' => 11,
        'T' => 10,
        _ => Convert.ToInt32(FullHand[index].ToString()),
    };

    public int CompareTo(Hand? other)
    {
        if(other == null) return 1; // push all nulls to the end

        if(Strength == other.Strength)
        {
            for(int i = 0; i < FullHand.Length; i++)
            {
                var thisNumeric = this.GetCardNumericValue(i);
                var otherNumeric = other.GetCardNumericValue(i);
                if(thisNumeric != otherNumeric)
                {
                    return thisNumeric > otherNumeric ? 1 : -1;
                }
            }
            Console.WriteLine($"Equal? {this.FullHand} {this} | {other.FullHand} {other}");
            return 0;
        }

        return Strength > other.Strength ? 1 : -1;
    }

    public bool Equals(Hand other)
    {
        if(Object.ReferenceEquals(this,other)) return true;
        return this.FullHand == other.FullHand;
    }
}

public enum Strength
{
    HighCard = 0,
    OnePair = 1,
    TwoPair = 2,
    ThreeOfAKind = 3,
    FullHouse = 4,
    FourOfAKind = 5,
    FiveOfAKind = 6
}
