using Microsoft.Extensions.Configuration;
using Model;

namespace Repository.Interface
{
    public interface IBookRepository
    {
       bool AddBook(BookModel bookmodel);
       BookModel GetBook(int bookId);
       bool UpdateBook(BookModel bookmodel);
    }
}