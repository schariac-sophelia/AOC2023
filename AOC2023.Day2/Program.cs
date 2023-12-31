﻿if (args != null) {
    if (File.Exists(args[0])) {
        CubeCollection AllCubes = new CubeCollection();
        Dictionary<int, bool> GameValidity = new();
        List<CubeCollection> GameMinimumCubes = new();
        string[] filestream = File.ReadAllLines(args[0]);

        // declare how many cubes you have per color
        // maybe i can set these as command line arguments or console input but not gonna bother for now
        AllCubes.Red = 12;
        AllCubes.Green = 13;
        AllCubes.Blue = 14;

        foreach (string line in filestream) {
            List<string> gameRounds = line.Split(';', ':').ToList();
            CubeCollection minimum = new CubeCollection();
            bool isValidGame = true;

            // first element should always be 'Game ##', so taking advantage of that
            int gameID = int.Parse(gameRounds[0].Trim().Replace("game ", string.Empty, StringComparison.OrdinalIgnoreCase));
            gameRounds.Remove(gameRounds[0]);

            Console.WriteLine("============================================================");
            Console.WriteLine("Game {0}", gameID);
            foreach(string round in gameRounds) {
                CubeCollection currentCount = new CubeCollection();
                string cleanRound = round.Replace(",", string.Empty);   // clean the round string by removing commas
                string[] tokens = cleanRound.Trim().Split(' ');         // split each words/tokens as their own strings

                for (int index = 0; index < tokens.Length; index++) {
                    bool ifCount = int.TryParse(tokens[index], out int count);

                    if (ifCount) {
                        // index + 1 (taking advantage of the pattern 'count color')
                        switch (tokens[index + 1]) {
                            case "red":
                                currentCount.Red = count;
                                if (minimum.Red < count)
                                    minimum.Red = count;
                                break;
                            case "green":
                                currentCount.Green = count;
                                if (minimum.Green < count)
                                    minimum.Green = count;
                                break;
                            case "blue":
                                currentCount.Blue = count;
                                if (minimum.Blue < count)
                                    minimum.Blue = count;
                                break;
                        }
                    }
                }

                // Check if current round would be valid or not
                if (isValidGame) {
                    if (currentCount.Red > AllCubes.Red || currentCount.Green > AllCubes.Green || currentCount.Blue > AllCubes.Blue)
                        isValidGame = false;
                }

                Console.WriteLine("  Total: R: {0} | G: {1} | B: {2}", currentCount.Red, currentCount.Green, currentCount.Blue);
                
            }

            Console.WriteLine("\nMinimum: R: {0} | G: {1} | B: {2}", minimum.Red, minimum.Green, minimum.Blue);
            GameMinimumCubes.Add(minimum);

            Console.WriteLine(" Result: {0}", isValidGame? "Valid" : "Invalid");
            GameValidity.Add(gameID, isValidGame);
        }

        // Get all game IDs that contains valid game rounds
        var validKeys = GameValidity.Where(game => game.Value == true).Select(game => game.Key).ToList();
        int sumValidKeys = 0;
        double sumPowers = 0.0d;

        // Get sum of all powers of minimum cubes per game
        foreach (var minimumCount in GameMinimumCubes) {
            sumPowers += (minimumCount.Red * minimumCount.Green * minimumCount.Blue);
        }

        Console.WriteLine("============================================================");
        Console.Write("Valid keys: ");
        foreach (var key in validKeys) {
            Console.Write($"{key}, ");
            sumValidKeys += key;
        }
        Console.Write("\n");

        Console.WriteLine($"Sum of all valid keys: {sumValidKeys}");
        Console.WriteLine($"Sum of all powers: {sumPowers}");
    }
} else {
    Console.WriteLine("No input file selected.");
}

class CubeCollection {
    public int Red { get; set; } = 0;
    public int Green { get; set; } = 0;
    public int Blue { get; set; } = 0;
}