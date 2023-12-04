List<char[]> CharacterTable = new();    // once populated, this should be useful like a char[][]
int PartSum = 0;

if (args[0] != null) {
    if (File.Exists(args[0])) {
        // create a character matrix (2D table) out of the input file  
        string[] lines = File.ReadAllLines(args[0]);
        foreach (string line in lines)
            CharacterTable.Add(line.ToCharArray());

        // using for instead of foreach should be better since we will be needing the index numbers
        for(int row = 0; row < CharacterTable.Count;  row++) {
            bool numberStartFlag = false;
            int numberStartIndex = 0;
            int numberLength = 0;

            for (int col = 0; col < CharacterTable[row].Length; col++) {
                if (numberStartFlag) {

                    // if on edge and still on number
                    if (col == CharacterTable[row].Length - 1) {            
                        CheckAdjacents(row, numberStartIndex, numberLength + 1, true);
                        numberStartFlag = false; numberLength = 0;  // reset flag and num length
                    } 
                    // else if character is in number but current char is not a number anymore
                    else if (!char.IsDigit(CharacterTable[row][col])) {
                        CheckAdjacents(row, numberStartIndex, numberLength, false);
                        numberStartFlag = false; numberLength = 0;  // reset flag and num length
                    }
                    // else if character is in number but next character is not a number anymore
                    else if (!char.IsDigit(CharacterTable[row][col + 1])) {
                        CheckAdjacents(row, numberStartIndex, numberLength + 1, false);
                        numberStartFlag = false; numberLength = 0;  // reset flag and num length
                    }
                    // else 
                    else {
                        numberLength++;
                    }
   
                } else {

                    // found a digit
                    if (char.IsDigit(CharacterTable[row][col])) {
                        // check if this is single digit on edge
                        if (col == CharacterTable[row].Length - 1) {
                            CheckAdjacents(row, col, 1, true);
                        } else {
                            numberStartFlag = true;
                            numberStartIndex = col;
                            numberLength = 1;
                        }
                    }

                }
            }
        }
    }

    Console.WriteLine($"Part sum: {PartSum}");

} else {
    Console.WriteLine("No input file selected.");
}

void CheckAdjacents(int rowIndex, int columnIndex, int numberLength, bool onRightEdge) {
    string number = string.Empty;
    int containWidth = numberLength + 1;
    bool hasAdjacent = false;
    bool leftEdge = false, rightEdge = onRightEdge;

    // get the number according to the given parameters
    for (int index = 0; index < numberLength; index++)
        number += CharacterTable[rowIndex][columnIndex + index].ToString();
    
    // check whether number is on the left edge or not
    if (columnIndex == 0) {
        leftEdge = true;
    } else if (rightEdge) {
        containWidth--;
    }

    // check sides of the box
    if (!hasAdjacent) {
        if (!leftEdge) {
            if (CharacterTable[rowIndex][columnIndex - 1] != '.')
                hasAdjacent = true;
        }
        if (!rightEdge) { 
            if (CharacterTable[rowIndex][columnIndex + numberLength] != '.')
                hasAdjacent = true;
        }
    }

    // check top of the character box
    if (rowIndex != 0 && !hasAdjacent) {
        int iterationStart = -1;
        int topLength = containWidth;

        if (leftEdge)
            iterationStart = 0;

        for (int iteration = iterationStart; iteration < topLength && !hasAdjacent; iteration++) {
            if (CharacterTable[rowIndex - 1][columnIndex + iteration] != '.')
                hasAdjacent = true;
        }
    }

    // check bottom of the character box
    if (rowIndex != CharacterTable.Count - 1 && !hasAdjacent) {
        int iterationStart = -1;
        int bottomLength = containWidth;

        if (leftEdge)
            iterationStart = 0;

        for (int iteration = iterationStart; iteration < bottomLength && !hasAdjacent; iteration++) {
            if (CharacterTable[rowIndex + 1][columnIndex + iteration] != '.')
                hasAdjacent = true;
        }
    }

    Console.WriteLine("=======================================");
    Console.WriteLine($" Number found: {int.Parse(number)}");
    Console.WriteLine($"    Left Edge: {leftEdge}");
    Console.WriteLine($"   Right Edge: {rightEdge}");
    Console.WriteLine($"Number length: {numberLength}");
    Console.WriteLine($" Has Adjacent: {hasAdjacent}");
    Console.WriteLine("=======================================");

    if (hasAdjacent) {
        // add the number to the total sum
        PartSum += int.Parse(number);
    } 
}