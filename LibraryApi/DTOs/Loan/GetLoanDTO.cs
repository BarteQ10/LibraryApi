using LibraryApi.DTOs.User;
using LibraryApi.Entites;

namespace LibraryApi
{
    public class GetLoanDTO
    {
        public int Id { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned { get; set; }
        public Book Book { get; set; }
        public GetUserDTO User { get; set; }
    }
}
