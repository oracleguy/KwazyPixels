namespace KwazyPixels;

internal class ImageSorter : Comparer<IImage>
{
    private readonly SortMode SortMode;

    public ImageSorter(SortMode sortMode)
    {
        SortMode = sortMode;
    }

    public override int Compare(IImage? x, IImage? y)
    {
        if(x is null && y is null)
        {
            return 0;
        }

        if(x is null)
        {
            return -1;
        }

        if(y is null)
        {
            return 1;
        }

        switch (SortMode)
        {
            case SortMode.Filename:
                return string.Compare(Path.GetFileName(x.Path), Path.GetFileName(y.Path), StringComparison.OrdinalIgnoreCase);
            case SortMode.DateCreated:
                return -1 * DateTime.Compare(x.DateCreated, y.DateCreated);
            case SortMode.DateTaken:
                return 1;
            default:
                return 0;
        }
    }
}
