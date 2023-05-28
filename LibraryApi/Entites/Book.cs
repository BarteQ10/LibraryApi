using System.Text.Json.Serialization;

namespace LibraryApi.Entites
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public byte[] CoverImageData { get; set; }
        public bool IsAvailable { get; set; }
        [JsonIgnore]

        public ICollection<Loan>? Loans { get; set; }
    }

}
