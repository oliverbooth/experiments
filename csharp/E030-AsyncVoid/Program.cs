Foo();
Throw();
Console.WriteLine("You shouldn't see this because of the exception thrown in Throw, yet here we are.");
return;

static async void Foo()
{
    await Task.Delay(1000);
    Console.WriteLine("The output will not be written.");
}

static async void Throw()
{
    await Task.Delay(1000);
    throw new Exception("This exception will not be raised");
}
