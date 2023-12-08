using System.Drawing;

CutAll();
GenerateCharacters(150);
MergeIntoOne();

static void MergeIntoOne()
{
    // merge all items
    var characters = new DirectoryInfo(@"C:\git\chars\merged\").GetFiles("*.png")
        .Select(x => new Bitmap(x.FullName))
        .ToArray();

    var height = characters.Length * 32 * 2;
    var width = 8 * 32;

    var allCharacters = new Bitmap(width, height);
    using var g = Graphics.FromImage(allCharacters);

    for (int i = 0; i < characters.Length; i++)
    {
        g.DrawImage(characters[i], 0, i * 32 * 2);
    }

    allCharacters.Save(@"c:\git\chars\all.png");
}

static void GenerateCharacters(int totalCharacters)
{
    var merged = @"C:\git\chars\merged\";

    var bodies = new DirectoryInfo(@"C:\git\chars\Bodies").GetFiles("*.png")
        .OrderBy(x => Guid.NewGuid())
        .Select(x => new Bitmap(x.FullName))
        .ToArray();

    var outfits = new DirectoryInfo(@"C:\git\chars\Outfits").GetFiles("*.png")
        .Select(x => new Bitmap(x.FullName))
        .ToArray();

    var hairstyle = new DirectoryInfo(@"C:\git\chars\Hairstyle").GetFiles("*.png")
        .OrderBy(x => Guid.NewGuid())
        .Select(x => new Bitmap(x.FullName))
        .ToArray();

    var eyes = new DirectoryInfo(@"C:\git\chars\Eyes").GetFiles("*.png")
        .OrderBy(x => Guid.NewGuid())
        .Select(x => new Bitmap(x.FullName))
        .ToArray();

    for (int i = 0; i < totalCharacters; i++)
    {
        var outfitIndex = i % outfits.Length;
        var bodyIndex = i % bodies.Length;
        var eyesIndex = i % eyes.Length;
        var hairIndex = new Random().Next(0, hairstyle.Length);

        var merge = MergeBitmaps(bodies[bodyIndex], outfits[outfitIndex], hairstyle[hairIndex], eyes[eyesIndex]);

        merge.Save($"{merged}{i}.png");
    }

}

static void CutAll()
{
    //CutAndSaveImage(@"C:\git\char - Copy.png");

    var dir = new DirectoryInfo(@"C:\\git\\chars");
    var allImages = dir.GetFiles("*.png", SearchOption.AllDirectories);

    foreach (var file in allImages)
    {
        Console.WriteLine(file.FullName);
        CutAndSaveImage(file.FullName);
    }
}

static Bitmap MergeBitmaps(Bitmap body, Bitmap outfit, Bitmap hair, Bitmap eyes)
{
    Bitmap mergedImage = new Bitmap(8 * 32, 2 * 32);

    using (Graphics g = Graphics.FromImage(mergedImage))
    {
        g.DrawImage(body, 0, 0, body.Width, body.Height);
        g.DrawImage(outfit, 0, 0, outfit.Width, outfit.Height);
        g.DrawImage(hair, 0, 0, hair.Width, hair.Height);
        g.DrawImage(eyes, 0, 0, eyes.Width, eyes.Height);
    }

    return mergedImage;
}


static void CutAndSaveImage(string inputImagePath)
{
    using (Bitmap newImage = new Bitmap(8 * 32, 2 * 32))
    {
        using (var originalImage = new Bitmap(inputImagePath))
        {
            CopyPixels(originalImage, newImage, new Rectangle(0*32, 0, 128, 64), new Rectangle(0, 0, 128, 64));
            CopyPixels(originalImage, newImage, new Rectangle(4*32, 0, 32, 64), new Rectangle(0 * 32, 8 * 32, 32, 64));
            CopyPixels(originalImage, newImage, new Rectangle(5*32, 0, 32, 64), new Rectangle(7 * 32, 8 * 32, 32, 64));
            CopyPixels(originalImage, newImage, new Rectangle(6*32, 0, 32, 64), new Rectangle(0 * 32, 10 * 32, 32, 64));
            CopyPixels(originalImage, newImage, new Rectangle(7*32, 0, 32, 64), new Rectangle(7 * 32, 10 * 32, 32, 64));
        }

        newImage.Save(inputImagePath);
    }
}

static void CopyPixels(Bitmap image, Bitmap newImage, Rectangle destRectangle, Rectangle sourceRectangle)
{
    using (Graphics g = Graphics.FromImage(newImage))
    {
        g.DrawImage(image, destRectangle, sourceRectangle, GraphicsUnit.Pixel);
    }
}
