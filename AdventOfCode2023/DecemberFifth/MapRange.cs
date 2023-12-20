public class MapRange
{
    private long _destination;

    private long _source;

    private long _length;

    private long _conversionFactor;

    public MapRange(long destination, long source, long length)
    {
        // Puzzle table is DESTINATION SOURCE LENGTH
        _destination = destination;
        _source = source;
        _length = length;
        _conversionFactor = source - destination;
    }

    public bool IsInRange(long other)
    {
        return _source <= other && other < (_source +  _length);
    }

    public long ConvertSourceToDestination(long source)
    {
        return source - _conversionFactor;
    }
}
