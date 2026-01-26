using AutoMapper;
using LibrarySystem.Application.Dtos.BookDtos;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions;
using LibrarySystem.Domain.Interfaces;

namespace LibrarySystem.Application.Services;

public class BookService : IBookService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BookService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<BookDto> CreateBookAsync(BookDtoForInsertion bookDtoForInsertion)
    {
        var existingBook = await _unitOfWork.Books
        .GetOneByConditionAsync(b => b.ISBN == bookDtoForInsertion.ISBN, false);

        if (existingBook is not null)
        {
            throw new BookAlreadyExistsException(bookDtoForInsertion.ISBN);
        }
        var book = _mapper.Map<Book>(bookDtoForInsertion);

        if (bookDtoForInsertion.AuthorIds != null)
        {
            foreach (var authorId in bookDtoForInsertion.AuthorIds)
            {
                var author = await _unitOfWork.Authors.GetOneByConditionAsync(b => b.Id == authorId, true);
                if (author != null)
                    book.Authors.Add(author);
                else
                    throw new AuthorNotFoundException(authorId);
            }
        }
        if (bookDtoForInsertion.CategoryIds != null)
        {
            foreach (var categoryId in bookDtoForInsertion.CategoryIds)
            {
                var category = await _unitOfWork.Categories.GetOneByConditionAsync(b => b.Id == categoryId, true);
                if (category != null)
                    book.Categories.Add(category);
                else
                    throw new CategoryNotFoundException(categoryId);
            }
        }
        if (bookDtoForInsertion.PublisherId != null)
        {
            var publisher = await _unitOfWork.Publishers.GetOneByConditionAsync(b => b.Id == bookDtoForInsertion.PublisherId.Value, true);
            if (publisher != null)
                book.Publisher = publisher;
            else
                throw new PublisherNotFoundException(bookDtoForInsertion.PublisherId.Value);
        }

        await _unitOfWork.Books.AddAsync(book);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<BookDto>(book);
    }

    public async Task DeleteBookAsync(int id)
    {
        var book = await _unitOfWork.Books.GetOneByConditionAsync(b => b.Id == id, true);
        if (book == null)
            throw new BookNotFoundException(id);
        _unitOfWork.Books.Delete(book);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<BookDto>> GetAllBooksAsync()
    {
        var books = await _unitOfWork.Books.GetAllBooksWithDetailsAsync(false);
        return _mapper.Map<IEnumerable<BookDto>>(books);
    }

    public async Task<BookDto> GetBookByIdAsync(int id)
    {
        var book = await _unitOfWork.Books.GetOneBookWithDetailsAsync(id, false);
        if (book is null)
            throw new BookNotFoundException(id);
        return _mapper.Map<BookDto>(book);
    }

    public async Task UpdateBookAsync(BookDtoForUpdate bookDtoForUpdate)
    {
        var book = await _unitOfWork.Books.GetOneBookWithDetailsAsync(bookDtoForUpdate.Id, true);
        if (book is null)
            throw new BookNotFoundException(bookDtoForUpdate.Id);
        _mapper.Map(bookDtoForUpdate, book);

        book.Authors.Clear();
        if (bookDtoForUpdate.AuthorIds != null)
        {
            foreach (var authorId in bookDtoForUpdate.AuthorIds)
            {
                var author = await _unitOfWork.Authors.GetOneByConditionAsync(b => b.Id == authorId, true);
                if (author != null)
                    book.Authors.Add(author);
                else
                    throw new AuthorNotFoundException(authorId);
            }
        }

        book.Categories.Clear();
        if (bookDtoForUpdate.CategoryIds != null)
        {
            foreach (var catId in bookDtoForUpdate.CategoryIds)
            {
                var category = await _unitOfWork.Categories.GetOneByConditionAsync(b => b.Id == catId, true);
                if (category != null)
                    book.Categories.Add(category);
                else
                    throw new CategoryNotFoundException(catId);
            }
        }

        if (book.PublisherId != bookDtoForUpdate.PublisherId)
        {
            var publisher = await _unitOfWork.Publishers.GetOneByConditionAsync(b => b.Id == bookDtoForUpdate.PublisherId, true);
            if (publisher != null)
                book.Publisher = publisher;
            else
                throw new PublisherNotFoundException(bookDtoForUpdate.PublisherId);
        }
        if (book.Summary != null)
            book.Summary.Summary = bookDtoForUpdate.Summary;
        else if (!string.IsNullOrEmpty(bookDtoForUpdate.Summary))
            book.Summary = new BookSummary { Summary = bookDtoForUpdate.Summary };

        _unitOfWork.Books.Update(book);
        await _unitOfWork.SaveChangesAsync();
    }

}