using E029_CircularBitShift;

CircularShiftingInt n = 0b01011000000000000000011000000000;
Console.WriteLine(Convert.ToString(n, 2).PadLeft(32, '0'));
n <<= 5;
Console.WriteLine(Convert.ToString(n, 2).PadLeft(32, '0'));
