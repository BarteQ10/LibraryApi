using LibraryApi.DTOs.Loan;
using LibraryApi.Entites;

namespace LibraryApi.Services.Interfaces
{
    public interface ILoanService
    {
        Task<IEnumerable<GetLoanDTO>> GetLoansByUserId(int userId);
        Task<GetLoanDTO> GetLoanById(int loanId);
        Task<LoanResult> CreateLoan(CreateLoanDTO request);
        Task<LoanResult> FinishLoan(FinishLoanDTO request, int userId);
        Task<LoanResult> DeleteLoan(int loanId, int userId);
    }
}