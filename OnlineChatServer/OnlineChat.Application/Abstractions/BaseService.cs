using AutoMapper;
using OnlineChat.DatabaseAccess.Abstraction;

namespace OnlineChat.Application.Abstractions;

public class BaseService<T, TDto, TRepository> : IBaseService<TDto> 
	where TDto : class
	where TRepository : IBaseRepository<T>
{
	protected readonly IMapper _mapper;
	protected readonly TRepository _repository;

	public BaseService(IMapper mapper, TRepository repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	
	public virtual async Task<Guid> CreateAsync(TDto item)
	{
		return await _repository.CreateAsync(_mapper.Map<TDto, T>(item));
	}

	public async Task<bool> DeleteAsync(Guid id)
	{
		return await _repository.DeleteAsync(id);
	}

	public async Task<TDto?> GetAsync(Guid id)
	{
		var res = await _repository.GetAsync(id);
		
		if (res == null) return null;
		
		return  _mapper.Map<T, TDto>(res);
	}

	public async Task<List<TDto>> GetAllAsync(int skip, int take)
	{
		var res = await _repository.GetAllAsync(skip, take);
		return _mapper.Map<List<TDto>>(res);
	}
}