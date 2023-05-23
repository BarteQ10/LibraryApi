using LibraryApi.DTOs.Loan;
using LibraryApi.Entites;

namespace LibraryApi.Services.Interfaces
{
    public interface ILoanService
    {
        Task<IEnumerable<GetLoanDTO>> GetLoansByUserId(int userId);
        Task<GetLoanDTO> GetLoanById(int loanId, int userId);
        Task<Loan> CreateLoan(CreateLoanDTO request);
        Task<Loan> FinishLoan(FinishLoanDTO request, int userId);
        Task<bool> DeleteLoan(int loanId, int userId);
    }
}