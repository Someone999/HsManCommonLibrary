namespace CommonLibrary.Collections.Transactional.Lists;

public struct IndexRange
{
    public IndexRange(int start, int end)
    {
        Start = start;
        End = end;
    }

    public int Start { get; set; }
    public int End { get; set; }
}