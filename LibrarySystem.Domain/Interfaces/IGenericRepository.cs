using System.Linq.Expressions;

namespace LibrarySystem.Domain.Interfaces;


// T yapısı generic yapı için book da gelse user da gelse fark etmiyor
// sonraki yazdığımız where T : class kısmı ise sadece class almamızı sağlıyor 
//bu sayede buraya int string tarzı veriler giremiyor
public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(bool trackChanges);     // hepsini listelemek için
    Task<T> GetOneByConditionAsync(Expression<Func<T,bool>> expression,bool trackChanges,params Expression<Func<T,object>>[] includes);     // filtreye göre tek bir veri getirmek için
    Task<IEnumerable<T>> GetManyByConditionAsync (Expression<Func<T,bool>> expression, bool trackChanges,params Expression<Func<T,object>>[] includes); // filtremize göre veri getirmek için
    
    // Ekleme Silme Güncelleme
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}