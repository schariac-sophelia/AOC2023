if (File.Exists(args[0])) {
    string[] caliberationFile = File.ReadAllLines(args[0]);
    int sum = 0;

    foreach (string line in caliberationFile) {
        char[] array = line.ToCharArray();

        char firstDigit = array.AsQueryable().First(x => char.IsDigit(x));
        char lastDigit = array.AsQueryable().Last(x => char.IsDigit(x));
        int value = Convert.ToInt32(String.Format("{0}{1}", firstDigit, lastDigit));

        sum += value;
        Console.WriteLine($"First digit: {firstDigit} | Last digit: {lastDigit} | Combine: {value}");
    }

    Console.WriteLine($"Result: {sum}");
} else {
    Console.WriteLine("File does not exist.");
}
