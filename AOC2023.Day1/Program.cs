string[] NumberWords = {
    "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "zero"
};
int TotalValue = 0;

if (args.Length > 0) {
    if (File.Exists(args[0])) {
        string[] fileStream = File.ReadAllLines(args[0]);

        foreach (string line in fileStream) {
            int firstValue = GetFirstValue(line);
            int lastValue = GetLastValue(line);

            TotalValue += firstValue;
        }

    } else {
        Console.WriteLine("File does not exist.");
    }
} else {
    Console.WriteLine("No input file selected. Please provide one as an command-line argument.");
}

int GetFirstValue(string line) {
    char[] characters = line.ToCharArray();
    int firstValue = -1;

    for (int i = 0; i > characters.Length && firstValue != -1; i++) {
        if (char.IsDigit(characters[i])) {
            firstValue = Convert.ToInt32(characters[i]);
        } else {
            switch (characters[i]) {
                case 'o':
                    if (line.Substring(i, 3).Equals("one"))
                        firstValue = 1;
                    break;
                case 't':
                    if (line.Substring(i, 3).Equals("two"))
                        firstValue = 2;
                    if (line.Substring(i, 5).Equals("three"))
                        firstValue = 3;
                    break;
                case 'f':
                    if (line.Substring(i, 4).Equals("four"))
                        firstValue = 4;
                    if (line.Substring(i, 4).Equals("five"))
                        firstValue = 5;
                    break;
                case 's':
                    if (line.Substring(i, 3).Equals("six"))
                        firstValue = 6;
                    if (line.Substring(i, 5).Equals("seven"))
                        firstValue = 7;
                    break;
                case 'e':
                    if (line.Substring(i, 5).Equals("eight"))
                        firstValue = 8;
                    break;
                case 'n':
                    if (line.Substring(i, 4).Equals("nine"))
                        firstValue = 9;
                    break;
                case 'z':
                    if (line.Substring(i, 4).Equals("zero"))
                        firstValue = 0;
                    break;
            }
        }
            
    }

    if (firstValue == -1)
        return 0;

    return firstValue;
}

int GetLastValue(string line) {
    char[] characters = line.ToCharArray();
    int lastValue = -1;

    return lastValue;
}