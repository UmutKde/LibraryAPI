using AutoMapper;
using LibrarySystem.Application.Dtos;
using LibrarySystem.Application.Dtos.BookDtos;
using LibrarySystem.Application.Dtos.CategoryDtos;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Mappings;

public class GeneralMapping : Profile
{
    public GeneralMapping()
    {
        CreateMap<Book, BookDto>()
            .ForMember(dest => dest.PublisherName, opt => opt.MapFrom(src => src.Publisher.PublisherName))
            .ForMember(dest => dest.Authors, opt => opt.MapFrom(src => src.Authors.Select(a => $"{a.Name} {a.Surname}").ToList()))
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories.Select(a => a.CategoryName).ToList()))
            .ForMember(dest => dest.Summary, opt => opt.MapFrom(src => src.Summary != null ? src.Summary.Summary : null));

        CreateMap<BookDtoForInsertion, Book>()
            .ForMember(dest => dest.Authors, opt => opt.Ignore())
            .ForMember(dest => dest.Categories, opt => opt.Ignore())
            .ForMember(dest => dest.Publisher, opt => opt.Ignore())
            .ForMember(dest => dest.Summary, opt => opt.MapFrom(src => new BookSummary { Summary = src.Summary }));

        CreateMap<BookDtoForUpdate, Book>()
            .ForMember(dest => dest.Authors, opt => opt.Ignore())
            .ForMember(dest => dest.Categories, opt => opt.Ignore())
            .ForMember(dest => dest.Publisher, opt => opt.Ignore())
            .ForMember(dest => dest.Summary, opt => opt.Ignore());


        CreateMap<Category, CategoryDto>();
        CreateMap<CategoryDtoForInsertion, Category>();
        CreateMap<CategoryDtoForUpdate, Category>();

        CreateMap<Author, AuthorDto>();
        CreateMap<AuthorDtoForInsertion, Author>();
        CreateMap<AuthorDtoForUpdate, Author>();

        CreateMap<Publisher, PublisherDto>();
        CreateMap<PublisherDtoForInsertion, Publisher>();
        CreateMap<PublisherDtoForUpdate, Publisher>();

        CreateMap<BookCopy, BookCopyDto>()
            .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.BookName));
        CreateMap<BookCopyDtoForUpdate, BookCopy>().ReverseMap();
    
        CreateMap<Loan, LoanDto>()
    // İlişkili tablolardan veri çekme (Navigation Properties)
    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => $"{src.User.Name} {src.User.Surname}"))
    .ForMember(dest => dest.BookName, opt => opt.MapFrom(src => src.BookCopy.Book.BookName))
    .ForMember(dest => dest.Barcode, opt => opt.MapFrom(src => src.BookCopy.Barcode));
    }
}