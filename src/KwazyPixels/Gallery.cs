namespace KwazyPixels;

/// <summary>
/// Represents a gallery of images.
/// </summary>
public class Gallery
{
    private readonly FileSystemWatcher watcher;
    private static readonly string[] ImageExtensions = [".jpg", ".jpeg", ".png", ".webp", ".heif", ".heic"];
    private readonly List<IImage> images = new();
    private int currentIndex = 0;
    private readonly Lock imagesLock = new();
    private readonly IImageFactory ImageFactory;

    /// <summary>
    /// Creates a new instance of the <see cref="Gallery"/> class.
    /// </summary>
    /// <param name="config"></param>
    /// <param name="imageFactory"></param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="config"/> or <paramref name="imageFactory"/> is null.
    /// </exception>
    public Gallery(GalleryConfig config, IImageFactory imageFactory)
    {
        ImageFactory = imageFactory ?? throw new ArgumentNullException(nameof(imageFactory));
        ArgumentNullException.ThrowIfNull(config, nameof(config));
        Name = config.Name;
        Path = config.Path;

        PopulateImages();

        watcher = new FileSystemWatcher(config.Path)
        {
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite,
            Filter = "*.*",
            IncludeSubdirectories = true
        };

        watcher.Created += OnImageChanged;
        watcher.Changed += OnImageChanged;
        watcher.Renamed += OnImageChanged;
        watcher.Deleted += OnImageChanged;
        watcher.EnableRaisingEvents = true;
    }

    /// <summary>
    /// Gets the name of the gallery.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the path to the gallery on disk.
    /// </summary>
    public string Path { get; }

    /// <summary>
    /// Gets the total number of images in the gallery.
    /// </summary>
    public int TotalImageCount => images.Count;

    /// <summary>
    /// Gets the next image in the gallery, cycling back to the start if at the end.
    /// </summary>
    /// <returns></returns>
    public string GetNextImage()
    {
        lock (imagesLock)
        {
            if (images.Count == 0)
                return string.Empty;

            // Adjust index if images were removed
            if (currentIndex >= images.Count)
                currentIndex = 0;

            var image = images[currentIndex];
            currentIndex = (currentIndex + 1) % images.Count;
            return image.Path;
        }
    }

    private void OnImageChanged(object sender, FileSystemEventArgs e)
    {
        if (IsImageFile(e.FullPath))
        {
            lock (imagesLock)
            {
                switch (e.ChangeType)
                {
                    case WatcherChangeTypes.Created:
                        images.Add(ImageFactory.Create(e.FullPath));
                        break;
                    case WatcherChangeTypes.Deleted:
                        var image = images.FirstOrDefault(img => img.Path.Equals(e.FullPath, StringComparison.InvariantCultureIgnoreCase));
                        if(image != null)
                            images.Remove(image);
                        break;
                    case WatcherChangeTypes.Renamed:
                        var x = (RenamedEventArgs)e;
                        var index = images.FindIndex(img => img.Path.Equals(x.OldFullPath, StringComparison.InvariantCultureIgnoreCase));
                        if (index >= 0)
                        {
                            images[index] = ImageFactory.Create(x.FullPath);
                        }
                        break;
                }
            }
        }
    }

    private static bool IsImageFile(string file)
    {
        return ImageExtensions.Contains(System.IO.Path.GetExtension(file), StringComparer.InvariantCultureIgnoreCase);
    }

    private void PopulateImages()
    {
        lock (imagesLock)
        {
            images.Clear();
            if (Directory.Exists(Path))
            {
                foreach (var file in Directory.EnumerateFiles(Path, "*.*", SearchOption.AllDirectories))
                {
                    if (IsImageFile(file))
                    {
                        images.Add(ImageFactory.Create(file));
                    }
                }
            }
        }
    }
}
