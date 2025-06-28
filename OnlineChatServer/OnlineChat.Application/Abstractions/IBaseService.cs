namespace OnlineChat.Application.Abstractions;

public interface IBaseService<T> where T : class
{
	Task<Guid> CreateAsync(T item);
	Task<bool> DeleteAsync(Guid id);
	Task<T?> GetAsync(Guid id);
	Task<List<T>> GetAllAsync(int skip, int take);
}