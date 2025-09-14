
namespace KwazyPixels.Images;

internal record Image(string Path, DateTime? DateTaken, DateTime DateCreated) : IImage;
