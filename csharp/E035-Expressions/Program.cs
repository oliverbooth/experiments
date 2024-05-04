using System.Linq.Expressions;

Foo(() => new Exception("Hello", new ArgumentException("World")));
return;

static void Foo<T>(Expression<Func<T>> expression)
{
    foreach (Expression argument in (expression.Body as NewExpression)!.Arguments)
    {
        Console.WriteLine(argument);
    }

    Console.WriteLine(string.Join(Environment.NewLine, expression.Parameters));
}
