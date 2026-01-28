using AutoMapper;
using LibrarySystem.Application.Dtos;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Constants;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions;
using LibrarySystem.Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LibrarySystem.Application.Services;

public class LoanService : ILoanService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public LoanService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<LoanResponseDto> CreateLoan(LoanDtoForInsertion loanDtoForInsertion)
    {
        var bookcopy = await _unitOfWork.BookCopies.GetOneByConditionAsync(b => b.Barcode == loanDtoForInsertion.Barcode, true);
        if (bookcopy is null)
            throw new BookCopyNotFoundException(loanDtoForInsertion.Barcode); // bookcopy bulunamadı hatası gelecek buraya
        if (bookcopy.IsAvailable == false)
            throw new BookCopyStayShelfException(bookcopy.Barcode); // sistem hatası kitap birinin elinde kütüphanede olmaması gerekiyordu
        var activeCount = await _unitOfWork.Loans.GetManyByConditionAsync(b => b.UserId == loanDtoForInsertion.UserId && b.ReturnDate == null, true);
        if ((activeCount).Count() >= 3)
            throw new UserLoanLimitExceededException(activeCount.Count()); // kullacınının elinde 3 kitap var alamaz hatası dönülcek

        int loanDuration = 14;
        var potentialDueDate = DateTime.UtcNow.AddDays(loanDuration);

        if (potentialDueDate.DayOfWeek == DayOfWeek.Sunday)
            loanDuration += 1;

        var loan = new Loan
        {
            UserId = loanDtoForInsertion.UserId,
            BookCopyId = bookcopy.Id,
            LoanDate = DateTime.UtcNow,
            ReturnDate = null,
            DueDate = DateTime.UtcNow.AddDays(loanDuration),
        };
        bookcopy.IsAvailable = false;

        _unitOfWork.BookCopies.Update(bookcopy);
        await _unitOfWork.Loans.AddAsync(loan);
        await _unitOfWork.SaveChangesAsync();


        var user = await _unitOfWork.Users.GetOneByConditionAsync(b => b.Id == loanDtoForInsertion.UserId, true);

        return new LoanResponseDto
        {
            UserNameSurname = $"{user.Name} {user.Surname}",
            Barcode = loanDtoForInsertion.Barcode,
            DueDate = loan.DueDate,
            Message = "The book has been successfully loan to the user.",
        };
    }

    public async Task<IEnumerable<LoanDto>> GetActiveLoansByUserIdAsync(int userId)
    {
        var activeLoans = await _unitOfWork.Loans.GetManyByConditionAsync(b => b.UserId == userId && b.ReturnDate == null,false,b=> b.User,b => b.BookCopy,b => b.BookCopy.Book);
        
        return _mapper.Map<IEnumerable<LoanDto>>(activeLoans);
    }

    public async Task<IEnumerable<LoanDto>> GetAllActiveLoanAsync()
    {
        var activeLoan = await _unitOfWork.Loans.GetManyByConditionAsync(b => b.ReturnDate == null,false,b => b.User,b => b.BookCopy);

        return _mapper.Map<IEnumerable<LoanDto>>(activeLoan);
    }

    public async Task<IEnumerable<LoanDto>> GetAllLoansAsync()
    {
        var loans = await _unitOfWork.Loans.GetAllAsync(false);
        return _mapper.Map<IEnumerable<LoanDto>>(loans);
    }

    public async Task<LoanDto> GetLoanByBarcode(string barcode)
    {
        var loan = await _unitOfWork.Loans.GetOneByConditionAsync(b => b.BookCopy.Barcode == barcode && b.ReturnDate == null,false, b=> b.User,b => b.BookCopy,b => b.BookCopy.Book);
        if(loan is null)
            throw new NotImplementedException();
        return _mapper.Map<LoanDto>(loan);
    }

    public async Task<LoanDto> GetLoanByIdAsync(int id)
    {
        var loan = await _unitOfWork.Loans.GetOneByConditionAsync(b => b.Id == id,false,b=> b.User,b => b.BookCopy,b => b.BookCopy.Book);
        if(loan is null)
            throw new NotImplementedException();
        return _mapper.Map<LoanDto>(loan);
    }

    public async Task<IEnumerable<LoanDto>> GetLoansByUserIdAsync(int userId)
    {
        var loans = await _unitOfWork.Loans.GetManyByConditionAsync(b => b.UserId == userId,false,b=> b.User,b => b.BookCopy,b => b.BookCopy.Book);
        if(loans is null)
            throw new NotImplementedException();
        return _mapper.Map<IEnumerable<LoanDto>>(loans);
    }

    public async Task<(decimal totalCost, string message)> UpdateLoan(LoanDtoForUpdate loanDtoForUpdate)
    {
        decimal totalCost = 0;
        List<string> costDetails = new();

        var bookcopy = await _unitOfWork.BookCopies.GetOneByConditionAsync(b => b.Barcode == loanDtoForUpdate.Barcode, true);
        if (bookcopy is null)
            throw new NotImplementedException(); // kitap yok 

        var activeLoan = await _unitOfWork.Loans.GetOneByConditionAsync(b => b.BookCopyId == bookcopy.Id && b.ReturnDate == null, true);


        decimal bookPrice = bookcopy.ReplacementCost ?? 100;
        if (bookcopy.Condition != BookCondition.Damaged && loanDtoForUpdate.Condition == BookCondition.Damaged)
        {
            totalCost += bookPrice;
            costDetails.Add($"Damage Cost: {bookPrice} TL ");
        }

        if (DateTime.UtcNow > activeLoan.DueDate)
        {
            var lateDays = (int)(DateTime.UtcNow - activeLoan.DueDate).TotalDays;
            var dailyPenalty = bookPrice * 0.05m;
            var totalLateFee = dailyPenalty * lateDays;

            totalCost += totalLateFee;
            costDetails.Add($"Late Fee ({lateDays} Day): {totalLateFee} TL");
        }

        activeLoan.ReturnDate = DateTime.UtcNow;
        bookcopy.Condition = loanDtoForUpdate.Condition;

        if ((int)loanDtoForUpdate.Condition >= 4)
            bookcopy.IsAvailable = false;
        else
            bookcopy.IsAvailable = true;

        await _unitOfWork.SaveChangesAsync();

        string resultMessage = totalCost > 0 
        ? string.Join(", ", costDetails) 
        : "Refund successfully received, no outstanding debt.";


        return(totalCost,resultMessage);

    }
}