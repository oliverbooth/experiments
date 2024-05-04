var array = new SomeStruct[500];
array[100] = new SomeStruct {x = 42};

SomeStruct first = array.FirstOrDefault(item => !item.Equals(new SomeStruct()));
Console.WriteLine(first.x);

struct SomeStruct
{
    public int x;
}
