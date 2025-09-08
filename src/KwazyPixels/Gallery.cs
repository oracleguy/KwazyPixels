namespace KwazyPixels;

/// <summary>
/// Represents a gallery of images.
/// </summary>
public class Gallery
{
    private readonly FileSystemWatcher watcher;
    private static readonly string[] ImageExtensions = [".jpg", ".jpeg", ".png", ".webp", ".heif", ".heic"];
    private readonly List<string> images = new();
    private int currentIndex = 0;
    private readonly Lock imagesLock = new();

    public Gallery(string name, string path)
    {
        Name = name;
        Path = path;

        PopulateImages();

        watcher = new FileSystemWatcher(path)
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
            return image;
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
                        images.Add(e.FullPath);
                        break;
                    case WatcherChangeTypes.Deleted:
                        images.Remove(e.FullPath);
                        break;
                    case WatcherChangeTypes.Renamed:
                        var x = (RenamedEventArgs)e;
                        var index = images.IndexOf(x.OldFullPath);
                        if (index >= 0)
                        {
                            images[index] = x.FullPath;
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
                        images.Add(file);
                    }
                }
            }
        }
    }
}
