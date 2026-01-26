using AutoMapper;
using LibrarySystem.Application.Dtos;
using LibrarySystem.Application.Dtos.BookDtos;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions;
using LibrarySystem.Domain.Interfaces;

namespace LibrarySystem.Application.Services;

public class AuthorService : IAuthorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AuthorService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AuthorDto> CreateAuthorAsync(AuthorDtoForInsertion authorDtoForInsertion)
    {
        var existingAuthor = await _unitOfWork.Authors
        .GetOneByConditionAsync(a =>
            a.Name.ToLower() == authorDtoForInsertion.Name.ToLower() &&
            a.Surname.ToLower() == authorDtoForInsertion.Surname.ToLower(), false);

        if (existingAuthor is not null)
            throw new AuthorAlreadyExistsException((authorDtoForInsertion.Name)+(authorDtoForInsertion.Surname));
        var author = _mapper.Map<Author>(authorDtoForInsertion);

        await _unitOfWork.Authors.AddAsync(author);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<AuthorDto>(author);
    }

    public async Task DeleteAuthorAsync(int id)
    {
        var author = await _unitOfWork.Authors.GetOneByConditionAsync(b => b.Id == id, true);
        if (author == null)
            throw new AuthorNotFoundException(id);
        _unitOfWork.Authors.Delete(author);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync()
    {
        var authors = await _unitOfWork.Authors.GetAllAsync(false);
        return _mapper.Map<IEnumerable<AuthorDto>>(authors);
    }

    public async Task<AuthorDto> GetAuthorByIdAsync(int id)
    {
        var author = await _unitOfWork.Authors.GetOneByConditionAsync(b => b.Id == id, false);
        if (author is null)
            throw new AuthorNotFoundException(id);
        return _mapper.Map<AuthorDto>(author);
    }

    public async Task UpdateAuthorAsync(AuthorDtoForUpdate authorDtoForUpdate)
    {
        var author = await _unitOfWork.Authors.GetOneByConditionAsync(b => b.Id == authorDtoForUpdate.Id, true);
        if (author is null)
            throw new AuthorNotFoundException(authorDtoForUpdate.Id);
        _mapper.Map(authorDtoForUpdate, author);

        _unitOfWork.Authors.Update(author);
        await _unitOfWork.SaveChangesAsync();
    }
}