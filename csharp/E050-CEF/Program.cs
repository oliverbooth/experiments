using CefSharp;
using CefSharp.DevTools.Page;
using CefSharp.OffScreen;
using CefSharp.Structs;
using E050_CEF;
using SixLabors.ImageSharp;
using Cef = CefSharp.Core.Cef;
using CefSettingsBase = CefSharp.Core.CefSettingsBase;

string browserSubprocessPath;

if (Environment.OSVersion.Platform == PlatformID.Win32NT)
{
    string platformDirectory = Environment.Is64BitProcess ? "win-x64" : "win-x86";
    browserSubprocessPath = Path.Join("runtimes", platformDirectory, "native", "CefSharp.BrowserSubprocess.exe");
}
else
{
    browserSubprocessPath = Path.Join("runtimes", "unix", "native", "CefSharp.BrowserSubprocess");
}

browserSubprocessPath = Path.GetFullPath(browserSubprocessPath);
Console.WriteLine($"Using browser subprocess path: {browserSubprocessPath}");

string cachePath = Path.GetFullPath("cache");
Directory.CreateDirectory(cachePath);
Console.WriteLine($"Using cache path: {cachePath}");

AsyncContext.Run(async () =>
{
    Console.WriteLine("Initializing CEF...");
    if (!Cef.Initialize(new CefSettingsBase { CachePath = cachePath, BrowserSubprocessPath = browserSubprocessPath }))
    {
        Console.WriteLine("Cannot initialize CEF");
        return;
    }

    using var requestContext = new RequestContext();
    var browserSettings = new BrowserSettings { WindowlessFrameRate = 60 };

    Console.WriteLine("Opening browser to URL...");
    using var browser = new ChromiumWebBrowser("https://google.com/", browserSettings, requestContext);
    LoadUrlAsyncResponse loadResponse = await browser.WaitForInitialLoadAsync();
    if (!loadResponse.Success)
    {
        Console.WriteLine($"Failed to load page, Error={loadResponse.ErrorCode}, HttpStatus={loadResponse.HttpStatusCode}");
        return;
    }

    Console.WriteLine("Fuck your cookies!");
    await browser.EvaluateScriptAsync("document.getElementById('W0wltc').click();");

    Console.WriteLine("Modifying DOM...");
    await browser.EvaluateScriptAsync("document.querySelector('[name=q]').value = 'CefSharp Was Here!'");

    DomRect contentSize = await browser.GetContentSizeAsync();
    var viewport = new Viewport
    {
        Width = contentSize.Width,
        Height = contentSize.Height,
        Scale = 1.0
    };

    Console.WriteLine("Capturing screenshot...");
    byte[] bitmap = await browser.CaptureScreenshotAsync(viewport: viewport);
    using var image = Image.Load(bitmap);
    
    Console.WriteLine("Saving screenshot to image.png...");
    image.Save("image.png");
    
    Console.WriteLine("Done!");
});
