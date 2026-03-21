using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyPortfolio.DataAccessLayer.Abstract
{
    public interface IGenericDal <T> where T : class
    {
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<List<T>> GetListAsync();
        Task<T?> GetByIdAsync(int id);

        // Özel sorgular atabilmek için (Örn: Sadece durumu aktif olanları getir)
        Task<List<T>> GetByFilterAsync(Expression<Func<T, bool>> filter);
    }
}
