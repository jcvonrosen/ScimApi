namespace ScimApi.Core.Models
{
    public class Group
    {
        public List<string> Schemas { get; set; }
        public string Value { get; set; }
        public string Display { get; set; }
        public List<Member> Members { get; set; }
    }
}
