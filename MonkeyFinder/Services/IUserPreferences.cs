using System;
namespace MonkeyFinder.Services
{
	public interface IUserPreferences
	{
        void Initialize();
        bool IsInitialized { get; }
        string DeviceId { get; }
        int GridItemsLayoutSpan { get; set; }
    }
}

