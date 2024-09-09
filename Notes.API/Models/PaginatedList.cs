namespace Notes.API.Models
{
    public class PaginatedList <T>
    {
        public List<T> Items { get; }
        public int PageIndex { get; }
        public int TotalPage { get; }

        public bool HasPrevPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPage;

        public PaginatedList(List<T> item, int pageIndex, int totalPage)
        {
            Items = item;
            PageIndex = pageIndex;
            TotalPage = totalPage;
        }

    }
}
