var upArrow = ((char)0x18).ToString();
var downArrow = ((char)0x19).ToString();
var rightArrow = ((char)0x1A).ToString();
var leftArrow = ((char)0x1B).ToString();

while (true)
{
    Console.Write("Input: ");
    string? input = Console.ReadLine();

    if (input == upArrow) Console.WriteLine("Up arrow");
    else if (input == downArrow) Console.WriteLine("Down arrow");
    else if (input == rightArrow) Console.WriteLine("Right arrow");
    else if (input == leftArrow) Console.WriteLine("Left arrow");
    else if (input is null) Console.WriteLine("Null");
}
