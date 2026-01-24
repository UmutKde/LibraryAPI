using AutoMapper;
using LibrarySystem.Application.Dtos.CategoryDtos;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions;
using LibrarySystem.Domain.Interfaces;

namespace LibrarySystem.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CategoryDto> CreateCategoryAsync(CategoryDtoForInsertion categoryDtoForInsertion)
    {
        var category = _mapper.Map<Category>(categoryDtoForInsertion);
        
        await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<CategoryDto>(category);
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var category = await _unitOfWork.Categories.GetOneByConditionAsync(b => b.Id == id,true);
        if(category == null)
            throw new CategoryNotFoundException(id);
        _unitOfWork.Categories.Delete(category);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoryAsync()
    {
        var category = await _unitOfWork.Categories.GetAllAsync(false);
        return _mapper.Map<IEnumerable<CategoryDto>>(category);
    }

    public async Task<CategoryDto> GetOneCategoryAsync(int id)
    {
        var category = await _unitOfWork.Categories.GetOneByConditionAsync(b => b.Id == id,false);
        if(category is null)
            throw new CategoryNotFoundException(id);
        return _mapper.Map<CategoryDto>(category);
    }

    public async Task UpdateCategoryAsync(CategoryDtoForUpdate categoryDtoForUpdate)
    {
        // Id == categoryDtoForUpdate.Id kısmı kontrol edilecek
        var category = await _unitOfWork.Categories.GetOneByConditionAsync(b => b.Id == categoryDtoForUpdate.Id,true);
        if(category is null)
            throw new CategoryNotFoundException(categoryDtoForUpdate.Id);
        _mapper.Map(categoryDtoForUpdate,category);
        
        _unitOfWork.Categories.Update(category);
        await _unitOfWork.SaveChangesAsync();
    }
}