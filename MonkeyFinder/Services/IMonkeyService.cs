using System;
namespace MonkeyFinder.Services
{
	public interface IMonkeyService
	{
		Task<List<Monkey>> GetMonkeys(CancellationToken cancellationToken);
	}
}

