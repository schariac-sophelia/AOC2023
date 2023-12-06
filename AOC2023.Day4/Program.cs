using System.Text.RegularExpressions;

List<Card> CardCollection = new();
int TotalPoints = 0;
int TotalCards = 0;

if (args[0] != null && File.Exists(args[0])) {
    string[] filestream = File.ReadAllLines(args[0]);
    
    // for loop is probably better than foreach so that we could have something to assign the card ID on printing
    // this should be fine since the card IDs contained in the input are always in sequential manner
    for (int index = 0; index < filestream.Length; index++) {
        // take advantage of the line pattern "card game: winning numbers | draw numbers"
        string[] splitLine = filestream[index].Split(':', '|');
        Card newCard = new Card();

        Regex rx = new Regex(@"\d+");
        MatchCollection drawNumbers = rx.Matches(splitLine[1]);
        MatchCollection winningNumbers = rx.Matches(splitLine[2]);

        Console.WriteLine($"Card {index + 1}");

        Console.Write("Winning Numbers: ");      
        foreach (var number in drawNumbers) {
            int.TryParse(number.ToString(), out int parsedNumber);   
            newCard.WinningNumbers.Add(parsedNumber);

            Console.Write($"{parsedNumber} ");
        }
        Console.Write("\n");

        Console.Write("   Draw Numbers: ");
        foreach (var number in winningNumbers) {
            int.TryParse(number.ToString(), out int parsedNumber);
            newCard.DrawNumbers.Add(parsedNumber);

            Console.Write($"{parsedNumber} ");
        }
        Console.Write("\n");
            

        CardCollection.Add(newCard); // just for keeping it saved

        // get points 
        int points = newCard.GetPoints();
        TotalPoints += points;

        Console.WriteLine("     Points: {0} {1}", points, points == 1 ? "point" : "points");
    }

    for (int index = 0; index < CardCollection.Count; index++) {
        int matches = CardCollection[index].GetMatches();

        if (matches > 0 && ((index + 1) < CardCollection.Count))
            AddCopy(index + 1, CardCollection[index].CardCopy, matches);

        TotalCards += CardCollection[index].CardCopy;
        Console.WriteLine($"Card {index + 1}: {CardCollection[index].CardCopy} copy");
    }

    Console.WriteLine($"Total points: {TotalPoints}");
    Console.WriteLine($" Total cards: {TotalCards}");
    
} else {
    Console.WriteLine("Input file not found.");
}

void AddCopy(int cardIndex, int winningCardCount, int matches) {
    // we kinda have to use recursions
    CardCollection[cardIndex].CardCopy += winningCardCount;
    matches--;

    if (matches > 0 && ((cardIndex + 1) < CardCollection.Count))
        AddCopy(cardIndex + 1, winningCardCount, matches);
}

public class Card {
    public int CardCopy { get; set; } = 1;
    public List<int> WinningNumbers { get; set; } = new();
    public List<int> DrawNumbers { get; set; } = new();
    
    public int GetPoints() {
        // point system is (1, 2, 4, 8, 16, 32), which is a classic example of a geometric sequence
        // there's some LINQ magic that will help us count all the matches
        // all that remains is solving the geometric sequence to find the points per card
        int matches = this.GetMatches();
        int points = (int) (1 * Math.Pow(2, matches - 1));

        return points;
    }

    public int GetMatches() {
        return DrawNumbers.Intersect(WinningNumbers).Count();
    }
}