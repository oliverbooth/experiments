using System.Reflection;

var random = new Random();

for (var i = 0; i < 10; i++)
{
    object instance = NextType(random);
    Console.WriteLine(instance.GetType());
    Console.WriteLine($"    {CallRandomMethod(random, instance)}");
}

return;

static object? CallRandomMethod(Random random, object o)
{
    MethodInfo[] methods = o.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(m => m.GetParameters().Length == 0).ToArray();
    if (methods.Length == 0) return null;
    return methods[random.Next(methods.Length)].Invoke(o, null);
}

static object NextType(Random random)
{
    Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
    if (assemblies.Length == 0) return new object();

    Assembly assembly = assemblies[random.Next(assemblies.Length)];
    Type[] types = assembly.GetTypes().Where(t => !t.ContainsGenericParameters && t.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Count(c => c.GetParameters().Length == 0) == 1).ToArray();
    if (types.Length == 0) return new object();
    Type type = types[random.Next(types.Length)];
    return Activator.CreateInstance(type)!;
}
