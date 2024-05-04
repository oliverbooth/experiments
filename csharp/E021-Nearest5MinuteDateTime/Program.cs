DateTime now = DateTime.Now;
double minutes = Math.Round((now.Hour * 60 + now.Minute) / 5.0) * 5.0;
DateTime nearest5Minute = DateTime.Today + TimeSpan.FromMinutes(minutes);

Console.WriteLine($"The current time is {now}");
Console.WriteLine($"The closest 5-minute marker is {nearest5Minute}");
