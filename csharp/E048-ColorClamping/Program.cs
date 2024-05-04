using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

using var image = Image.Load<Rgba32>("bliss.png");
image.Mutate(ctx =>
{
    // ReSharper disable AccessToDisposedClosure
    for (var y = 0; y < image.Height; y++)
    for (var x = 0; x < image.Width; x++)
    {
        const int n = 50;
        Rgba32 color = image[x, y];
        color = new Rgba32(color.R / n * n, color.G / n * n, color.B / n * n, color.A);
        ctx.FillPolygon(color, new PointF(x, y), new PointF(x + 1, y), new PointF(x + 1, y + 1), new PointF(x, y + 1));
    }
});

image.Save("result.png");
