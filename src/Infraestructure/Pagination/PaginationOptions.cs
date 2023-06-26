namespace Infraestructure.Pagination
{
    public class PaginationOptions
    {
        private int _page = 1;
        public int Page
        {
            get { return _page; }
            set
            {
                if (value == 0)
                    _page = 1;
                else
                    _page = value;
            }
        }
        public int? AmountPerPage { get; set; }
    }
}
