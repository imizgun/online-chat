namespace OnlineChat.DatabaseAccess.Abstraction;

public interface IBaseRepository<T>
{
	Task<Guid> CreateAsync(T item);
	Task<bool> DeleteAsync(Guid id);
	Task<T?> GetAsync(Guid id);
	Task<List<T>> GetAllAsync(int skip, int take);
}