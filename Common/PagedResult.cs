namespace PaymentCoreServiceApi.Common;

public class PagedResult<T> 
{ 
    public IEnumerable<T> Items { get; set; } 
    public int TotalCount { get; set; } 
    public int PageIndex { get; set; } // skip / take 
    public int PageSize { get; set; } 
    public bool HasNextPage => PageIndex * PageSize < TotalCount; 
    public bool HasPreviousPage => PageIndex > 1; 
 
    public PagedResult(IEnumerable<T> items, int totalCount, int pageIndex, int pageSize) 
    { 
        Items = items; 
        TotalCount = totalCount; 
        PageIndex = pageIndex; 
        PageSize = pageSize; 
    } 
}