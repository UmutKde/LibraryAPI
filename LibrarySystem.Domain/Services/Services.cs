using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Services;

public class Services
{
    public bool canBorrow(int UserId, int bookId, List<BookCopy> bookCopies, List<Loan> loans)
    {
        int userbooks = loans.Count(l => l.UserId == UserId);

        List<int> copyOfBooks = bookCopies.Where(bc => bc.BookId == bookId).Select(bc => bc.Id).ToList();

        List<int> borrowedBooks = loans.Where(l => l.BookId == bookId).Select(l => l.BookCopyId).ToList();

        if (userbooks < 3 && (copyOfBooks.Count() - borrowedBooks.Count()) > 0)
            return true;
        return false;
    }
}