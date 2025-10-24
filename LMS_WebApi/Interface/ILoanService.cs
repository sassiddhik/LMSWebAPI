namespace LMS_WebApi.Interface
{
    public interface ILoanService<T> where T : class
    {
        Task<IEnumerable<T>> GetLoans();
        Task<T> BorrowBook(Guid id);
        Task<T> ReturnBook(string email);
    }
}
