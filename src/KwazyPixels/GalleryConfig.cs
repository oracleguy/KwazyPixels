namespace KwazyPixels;

/// <summary>
/// The representation for the configuration of a gallery.
/// </summary>
public record GalleryConfig(string Name, string Path, SortMode SortMode = SortMode.None);
