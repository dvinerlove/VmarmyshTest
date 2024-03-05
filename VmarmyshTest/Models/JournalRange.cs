namespace VmarmyshTest.Models
{
    public class Range<T>
    {
        public int Skip { get; set; }
        public int Count { get; set; }
        public ICollection<T> Items { get; set; } = new List<T>();
    }
}
