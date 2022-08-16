namespace Reactivities.Core.Helpers.Paging;

public class PagingParams
{
    public int PageNumber { get; set; } = 1;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > _maxPageSize) ? _maxPageSize : value;
    }
    
    private int _pageSize = 10;
    private const int _maxPageSize = 50;
}