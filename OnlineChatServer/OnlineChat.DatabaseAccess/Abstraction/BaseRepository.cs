using Microsoft.EntityFrameworkCore;

namespace OnlineChat.DatabaseAccess.Abstraction;

public class BaseRepository<T> : IBaseRepository<T> where T : class, IIdentifiable
{
	protected ChatDbContext _context;
	protected DbSet<T> _dbSet;

	public BaseRepository(ChatDbContext context)
	{
		_context = context;
		_dbSet = _context.Set<T>();
	}
	
	public virtual async Task<Guid> CreateAsync(T item)
	{
		await _dbSet.AddAsync(item);
		await _context.SaveChangesAsync();
		return item.Id;
		
	}

	public virtual async Task<bool> DeleteAsync(Guid id)
	{
		var res = await _dbSet
			.AsNoTracking()
			.Where(x => x.Id == id)
			.ExecuteDeleteAsync();
		
		await _context.SaveChangesAsync();
		return res > 0;
	}

	public virtual async Task<T?> GetAsync(Guid id)
	{
		return await _dbSet
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Id == id);
	}

	public virtual async Task<List<T>> GetAllAsync(int skip, int take)
	{
		return await _dbSet
			.AsNoTracking()
			.Skip(skip * take)
			.Take(take)
			.ToListAsync();
	}
}