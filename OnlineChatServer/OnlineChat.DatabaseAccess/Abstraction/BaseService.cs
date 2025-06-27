using Microsoft.EntityFrameworkCore;

namespace OnlineChat.DatabaseAccess.Abstraction;

public class BaseService<T> : IBaseService<T> where T : class, IIdentifiable
{
	protected ChatDbContext _context;
	protected DbSet<T> _dbSet;

	public BaseService(ChatDbContext context)
	{
		_context = context;
	}
	
	public async Task<Guid> CreateAsync(T item)
	{
		await _dbSet.AddAsync(item);
		return item.Id;
	}

	public async Task<bool> DeleteAsync(Guid id)
	{
		var res = await _dbSet.Where(x => x.Id == id).ExecuteDeleteAsync();
		return res > 0;
	}

	public async Task<T?> GetAsync(Guid id)
	{
		return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
	}

	public async Task<List<T>> GetAllAsync(int? skip, int? take)
	{
		var notNullTake = take ?? 50;
		var notNullSkip = skip ?? 0;
		return await _dbSet.Skip(notNullSkip * notNullTake).Take(notNullTake).ToListAsync();
	}
}