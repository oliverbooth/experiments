using E016_ProtoBufExtendedModel;
using ProtoBuf;

const string saveFile = "save.dat";

{
    Console.WriteLine("Writing old model...");
    Save(new SaveData { Health = 100 });

    Console.WriteLine("Reading old model...");
    var saveData = Load<SaveData>();
    Console.WriteLine($"Health is {saveData.Health}");
}

Console.WriteLine(BitConverter.ToString(File.ReadAllBytes(saveFile)));
Console.WriteLine("---");

{
    Console.WriteLine("Reading old data to new model...");
    var saveData = Load<ExtendedSaveData>();
    Console.WriteLine($"Health is {saveData.Health}");
    Console.WriteLine($"Score is {saveData.Score}"); // should be 50, the default

    saveData.Score = 100; // increase data
    Console.WriteLine("Writing new model...");
    Save(saveData);
}

Console.WriteLine(BitConverter.ToString(File.ReadAllBytes(saveFile)));
Console.WriteLine("---");

{
    Console.WriteLine("Reading new model...");
    var saveData = Load<ExtendedSaveData>();
    Console.WriteLine($"New loaded health is {saveData.Health}");
    Console.WriteLine($"New loaded score is {saveData.Score}");
}

Console.WriteLine(BitConverter.ToString(File.ReadAllBytes(saveFile)));
return;

T Load<T>()
{
    using FileStream stream = File.OpenRead(saveFile);
    return Serializer.Deserialize<T>(stream);
}

void Save<T>(T value)
{
    using FileStream stream = File.Create(saveFile);
    Serializer.Serialize(stream, value);
}
