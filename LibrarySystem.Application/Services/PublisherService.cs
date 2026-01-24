using AutoMapper;
using LibrarySystem.Application.Dtos;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LibrarySystem.Application.Services;

public class PublisherService : IPublisherService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PublisherService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PublisherDto> CreatePublisherAsync(PublisherDtoForInsertion publisherDtoForInsertion)
    {
        var publisher = _mapper.Map<Publisher>(publisherDtoForInsertion);
        
        await _unitOfWork.Publishers.AddAsync(publisher);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<PublisherDto>(publisher);
    }

    public async Task DeletePublisherAsync(int id)
    {
        var publisher = await _unitOfWork.Publishers.GetOneByConditionAsync(b => b.Id == id,true);
        if(publisher is null)
            throw new PublisherNotFoundException(id);
        _unitOfWork.Publishers.Delete(publisher);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<PublisherDto>> GetAllPublisherAsync()
    {
        var publishers = await _unitOfWork.Publishers.GetAllAsync(false);
        return _mapper.Map<IEnumerable<PublisherDto>>(publishers);
    }

    public async Task<PublisherDto> GetPublisherByIdAsync(int id)
    {
         var publisher = await _unitOfWork.Publishers.GetOneByConditionAsync(b => b.Id == id,false);
         if(publisher is null)
            throw new PublisherNotFoundException(id);
        return _mapper.Map<PublisherDto>(publisher);
    }

    public async Task UpdatePublisherAsync(PublisherDtoForUpdate publisherDtoForUpdate)
    {
        var publisher = await _unitOfWork.Publishers.GetOneByConditionAsync(b => b.Id == publisherDtoForUpdate.Id,false);
        if(publisher is null)
            throw new PublisherNotFoundException(publisherDtoForUpdate.Id);
        _mapper.Map(publisherDtoForUpdate,publisher);
        _unitOfWork.Publishers.Update(publisher);
        await _unitOfWork.SaveChangesAsync();
    }
}