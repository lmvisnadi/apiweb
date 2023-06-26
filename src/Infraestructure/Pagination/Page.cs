namespace Infraestructure.Pagination
{
    public class Page<T> : IEquatable<Page<T>>
    {
        public int TotalPages { get; set; }

        public int? TotalItems { get; set; }

        public IEnumerable<T> Items { get; set; } = new List<T>();

        public bool Equals(Page<T>? other)
        {
            if (other == null)
            {
                return false;
            }

            return TotalPages == other.TotalPages
                && Items.SequenceEqual(other.Items);
        }
    }
}
