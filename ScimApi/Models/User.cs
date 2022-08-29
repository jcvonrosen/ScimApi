namespace ScimApi.Models
{
    public class User
    {
        public List<string> Schemas { get; set; }
        public string Id { get; set; }
        public UserMeta Meta { get; set; }
        public Name Name { get; set; }
        public string UserName { get; set; }
        public List<PhoneNumber> PhoneNumbers { get; set; }
        public List<Email> Emails { get; set; }
        public bool Active { get; set; }
        public List<Group> Groups { get; set; }
    }
}
