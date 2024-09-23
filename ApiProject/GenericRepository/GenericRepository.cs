using ApiProject.Context;
using Microsoft.EntityFrameworkCore;

namespace ApiProject.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApiDbContext _context;
        private readonly DbSet<T> table;

        public GenericRepository(ApiDbContext context)
        {
            this._context = context;
            table = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await table.ToListAsync();
        }

        public async Task<T> GetById(object id)
        {
            return await table.FindAsync(id);
        }

        public async Task<T> Insert(T obj)
        {
            await table.AddAsync(obj);
            return obj;
        }

        public async Task<T> Update(T obj)
        {
            table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
            return obj;
        }

        public async Task Delete(object id)
        {
            T existing = await table.FindAsync(id);
            if (existing != null)
            {
                table.Remove(existing);
            }
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
