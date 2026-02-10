#nullable disable
using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using LibrarySystem.Application.Dtos;
using LibrarySystem.Application.Services;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions;
using LibrarySystem.Domain.Interfaces;
using Moq;

namespace LibrarySystem.Application.Tests;

public class LoanServiceTest
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IGenericRepository<BookCopy>> _mockBookCopyRepo;
    private readonly LoanService _service;

    public LoanServiceTest()
    {
        _mockMapper = new Mock<IMapper>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockBookCopyRepo = new Mock<IGenericRepository<BookCopy>>();

        // --- BAĞLANTIYI KURDUĞUMUZ YER ---
        // UnitOfWork'e diyoruz ki: "Kitap deposunu sorarlarsa, bizim sahte depoyu göster."
        _mockUnitOfWork.Setup(x => x.BookCopies).Returns(_mockBookCopyRepo.Object);
        // ----------------------------------

        _service = new LoanService(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task CreateLoan_ShouldThrowBookCopyNotFoundException_WhenBarcodeDoesNotExist()
    {
        // ARRANGE
        var hatali_barcode = "BOYLE-BIR-BARCODE-YOK";
        var loanDto = new LoanDtoForInsertion { Barcode = hatali_barcode, UserId = 1 };

        // Depocuya talimat: Ne sorulursa sorulsun NULL (Yok) de.
        _mockBookCopyRepo.Setup(x => x.GetOneByConditionAsync(
            It.IsAny<Expression<Func<BookCopy, bool>>>(), 
            It.IsAny<bool>()))
            .ReturnsAsync((BookCopy)null);

        // ACT
        Func<Task> action = async () => await _service.CreateLoan(loanDto);

        // ASSERT
        await action.Should().ThrowAsync<BookCopyNotFoundException>();
    }
}