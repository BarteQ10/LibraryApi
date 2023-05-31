using System.Text.Json.Serialization;

namespace LibraryApi.Entites
{
    public class User
    {
        public int Id { get; set; }
        public string PasswordHash { get; set; }
        public DateTime TokenExpires { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public Role Role { get; set; }
        [JsonIgnore]
        public ICollection<Loan>? Loans { get; set; }
    }
    public enum Role
    {
        User,
        Librarian,
        Admin
    }
}
