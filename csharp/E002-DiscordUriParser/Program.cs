var message = "This is a test https://canary.discord.com/channels/779115633837211659/815556722722209803/944679403420524654";
Console.WriteLine(E002_DiscordUriParser.DiscordUrlParser.UsingUri(message));

message = "This is a test https://beta.discord.com/channels/779115633837211659/815556722722209803/944679403420524654";
Console.WriteLine(E002_DiscordUriParser.DiscordUrlParser.UsingUri(message));
