namespace ScimApi.Models
{
    public class ResourceList
    {
        public int TotalResults { get; set; }
        public int StartIndex { get; set; }
        public int ItemsPerPage { get; set; }

        public List<string> Schemas { get; set; }
    }
}