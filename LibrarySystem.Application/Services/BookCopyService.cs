using AutoMapper;
using LibrarySystem.Application.Dtos;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions;
using LibrarySystem.Domain.Interfaces;

namespace LibrarySystem.Application.Services;

public class BookCopyService : IBookCopyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BookCopyService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<string>> CreateCopiesAsync(BookCopyDtoForInsertion bookCopyDtoForInsertion)
    {
        var book = await _unitOfWork.Books.GetOneByConditionAsync(b => b.Id == bookCopyDtoForInsertion.BookId, false);
        if (book is null)
            throw new BookNotFoundException(bookCopyDtoForInsertion.BookId);

        var generatedBarcodes = new List<string>();
        for (int i = 0; i < bookCopyDtoForInsertion.Quantity; i++)
        {
            string barcode = GenerateBarcode(book.ISBN);
            var copy = new BookCopy
            {
                BookId = bookCopyDtoForInsertion.BookId,
                Barcode = barcode,
                IsAvailable = true,
                DateAdded = DateTime.UtcNow,
                Condition = bookCopyDtoForInsertion.Condition ?? "New",
                ReplacementCost = bookCopyDtoForInsertion.ReplacementCost
            };
            
            await _unitOfWork.BookCopies.AddAsync(copy);
            generatedBarcodes.Add(barcode);
        }
        await _unitOfWork.SaveChangesAsync();
        return generatedBarcodes;
    }

    public async Task DeleteCopyAsync(int id)
    {
        var copy = await _unitOfWork.BookCopies.GetOneByConditionAsync(b => b.Id == id,false);
        if(copy is null)
            throw new BookCopyNotFoundException(id);
        if(!copy.IsAvailable)
            throw new BookCopyNotAvailableException(id);
        
        _unitOfWork.BookCopies.Delete(copy);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<BookCopyDto>> GetAllCopiesByBookIdAsync(int bookId)
    {
        var copies = await _unitOfWork.BookCopies.GetManyByConditionAsync(b => b.BookId == bookId,false,b =>b.Book);
        return _mapper.Map<IEnumerable<BookCopyDto>>(copies);
    }

    public async Task UpdateCopyAsync(BookCopyDtoForUpdate bookCopyDtoForUpdate)
    {
        var copy = await _unitOfWork.BookCopies.GetOneByConditionAsync(b => b.Id == bookCopyDtoForUpdate.Id,true);

        if(copy is null)
            throw new BookCopyNotFoundException(bookCopyDtoForUpdate.Id);

        copy.Condition = bookCopyDtoForUpdate.Condition;
        copy.IsAvailable = bookCopyDtoForUpdate.IsAvailable;
        copy.ReplacementCost = bookCopyDtoForUpdate.ReplacementCost;

        _unitOfWork.BookCopies.Update(copy);
        await _unitOfWork.SaveChangesAsync();
    }
    
    private string GenerateBarcode(string ISBN)
    {
        string prefix = ISBN.Length >= 4 ? ISBN.Substring(ISBN.Length-4) : ISBN;
        string suffix = Guid.NewGuid().ToString().Substring(0,4).ToUpper();

        return $"{prefix}-{suffix}";
    }
}