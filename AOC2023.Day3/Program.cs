List<char[]> CharacterTable = new();    // once populated, this should be useful like a char[][]
List<GearInfo> GearList = new();
int PartSum = 0;
int GearRatioSum = 0;

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
                        CheckSymbolAdjacents(row, numberStartIndex, numberLength + 1, true);
                        numberStartFlag = false; numberLength = 0;  // reset flag and num length
                    } 
                    // else if character is in number but current char is not a number anymore
                    else if (!char.IsDigit(CharacterTable[row][col])) {
                        CheckSymbolAdjacents(row, numberStartIndex, numberLength, false);
                        numberStartFlag = false; numberLength = 0;  // reset flag and num length
                    }
                    // else if character is in number but next character is not a number anymore
                    else if (!char.IsDigit(CharacterTable[row][col + 1])) {
                        CheckSymbolAdjacents(row, numberStartIndex, numberLength + 1, false);
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
                            CheckSymbolAdjacents(row, col, 1, true);
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

    Console.WriteLine("Calculating gear factors: ");
    foreach (var gear in GearList) {
        if (gear.HasTwoAdjacentNos()) {
            GearRatioSum += gear.GetFactor();
        }
    }

    Console.WriteLine($"Gear factor: {GearRatioSum}");
    Console.WriteLine($"Part sum: {PartSum}");
    
} else {
    Console.WriteLine("No input file selected.");
}

void CheckSymbolAdjacents(int rowIndex, int columnIndex, int numberLength, bool onRightEdge) {
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
            if (CharacterTable[rowIndex][columnIndex - 1] == '*')
                CheckGearAdjacents(rowIndex, columnIndex - 1, int.Parse(number));
        }
        if (!rightEdge) { 
            if (CharacterTable[rowIndex][columnIndex + numberLength] != '.')
                hasAdjacent = true;
            if (CharacterTable[rowIndex][columnIndex + numberLength] == '*')
                CheckGearAdjacents(rowIndex, columnIndex + numberLength, int.Parse(number));
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
            if (CharacterTable[rowIndex - 1][columnIndex + iteration] == '*')
                CheckGearAdjacents(rowIndex - 1, columnIndex + iteration, int.Parse(number));
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
            if (CharacterTable[rowIndex + 1][columnIndex + iteration] == '*')
                CheckGearAdjacents(rowIndex + 1, columnIndex + iteration, int.Parse(number));
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

void CheckGearAdjacents(int x, int y, int sourceNumber) {
    var gear = GearList.Find(gear => gear.Row == x && gear.Column == y);

    if (gear == null) {
        // create new gear info
        var newGear = new GearInfo() { 
            Row = x,
            Column = y,
            Num1 = sourceNumber
        };
        GearList.Add(newGear);
    } else {
        // change the num2 on the index
        int index = GearList.FindIndex(gear => gear.Row == x && gear.Column == y);
        GearList[index].Num2 = sourceNumber;
    }
}

public class GearInfo {
    public int Row { get; set; }
    public int Column { get; set; }

    public int Num1 { get; set; } = -1;
    public int Num2 { get; set; } = -1;

    public bool HasTwoAdjacentNos() {
        if (Num1 != -1 && Num2 != -1)
            return true;
        else
            return false;
    }

    public int GetFactor() {
        return Num1 * Num2;
    }
}