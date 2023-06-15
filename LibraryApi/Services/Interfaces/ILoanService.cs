using LibraryApi.DTOs.Loan;
using LibraryApi.Entites;

namespace LibraryApi.Services.Interfaces
{
    public interface ILoanService
    {
        Task<IEnumerable<GetLoanDTO>> GetLoansByUserId(int userId);
        Task<GetLoanDTO> GetLoanById(int loanId, int userId);
        Task<IEnumerable<GetLoanDTO>> GetAllLoans();
        Task<Loan> CreateLoan(CreateLoanDTO request);
        Task<bool> StartLoan(int id, DateTime startDate);
        Task<bool> FinishLoan(int id, DateTime endDate);
        Task<bool> DeleteLoan(int loanId, int userId);
    }
}