namespace LibraryApi.Entites
{
    public class Loan
    {
        public int Id { get; set; }
        public DateTime? BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned { get; set; }
        public Book Book { get; set; }
        public User User { get; set; }
    }
}
