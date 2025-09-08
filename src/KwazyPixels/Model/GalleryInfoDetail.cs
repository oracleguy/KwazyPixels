namespace KwazyPixels.Model;

public record GalleryInfoDetail(string Name, int Id, int ImageCount) : GalleryInfo(Name, Id)
{
}
