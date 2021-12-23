using Model;

namespace Manager.Interface
{
    public interface IBookManager
    {
        bool AddBook(BookModel bookmodel);
        BookModel GetBook(int bookId);
        bool UpdateBook(BookModel bookmodel);
    }
}