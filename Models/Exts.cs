namespace Awake.Models
{
    public struct ExtItem
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public string GitUrl { get; set; }
        public string Hash { get; set; }
        public string Date { get; set; }
        public bool hasUpdate { get; set; }
        public string Path { get; set; }
    }
}
