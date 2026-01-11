using System.Linq.Expressions;

namespace LibrarySystem.Domain.Interfaces;


// T yapısı generic yapı için book da gelse user da gelse fark etmiyor
// sonraki yazdığımız where T : class kısmı ise sadece class almamızı sağlıyor 
//bu sayede buraya int string tarzı veriler giremiyor
public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();     // hepsini listelemek için
    Task<T> GetByIdAsync(int id);          // id' ye göre veri getirmek için
    Task<IEnumerable<T>> FindByFiltredValueAsync(Expression<Func<T,bool>> expression); // filtremize göre veri getirmek için
    
    // Ekleme Silme Güncelleme
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}