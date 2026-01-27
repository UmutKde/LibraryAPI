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

    public LoanService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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
        var potentialDueDate = DateTime.Now.AddDays(loanDuration);

        if (potentialDueDate.DayOfWeek == DayOfWeek.Sunday)
            loanDuration += 1;

        var loan = new Loan
        {
            UserId = loanDtoForInsertion.UserId,
            BookCopyId = bookcopy.Id,
            LoanDate = DateTime.Now,
            ReturnDate = null,
            DueDate = DateTime.Now.AddDays(loanDuration),
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

        if (DateTime.Now > activeLoan.DueDate)
        {
            var lateDays = (int)(DateTime.Now - activeLoan.DueDate).TotalDays;
            var dailyPenalty = bookPrice * 0.05m;
            var totalLateFee = dailyPenalty * lateDays;

            totalCost += totalLateFee;
            costDetails.Add($"Late Fee ({lateDays} Day): {totalLateFee} TL");
        }

        activeLoan.ReturnDate = DateTime.Now;
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