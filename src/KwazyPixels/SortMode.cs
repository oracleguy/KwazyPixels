namespace KwazyPixels;

/// <summary>
/// The sorting mode of images within a gallery.
/// </summary>
public enum SortMode
{
    /// <summary>
    /// No sort mode, the files are read in the order provided by the file system.
    /// </summary>
    None,

    /// <summary>
    /// Sort the images by filename in ascending order.
    /// </summary>
    Filename,

    /// <summary>
    /// Sort the images by the date they were taken in descending order. Images without EXIF date information will fall back to the file creation date.
    /// </summary>
    DateTaken,

    /// <summary>
    /// Sort the images by the file creation date in descending order.
    /// </summary>
    DateCreated
}
