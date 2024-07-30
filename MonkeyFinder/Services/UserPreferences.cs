using System;
namespace MonkeyFinder.Services
{
	public class UserPreferences : IUserPreferences
	{
        public void Initialize()
        {
            _ = DeviceId;
        }

        public bool IsInitialized
        {
            get => !string.IsNullOrWhiteSpace(Preferences.Get(nameof(DeviceId), string.Empty));
        }

        public string DeviceId
        {
            get
            {
                var deviceId = Preferences.Get(nameof(DeviceId), string.Empty);
                if (string.IsNullOrWhiteSpace(deviceId))
                {
                    deviceId = Guid.NewGuid().ToString();
                    Preferences.Set(nameof(DeviceId), deviceId);
                }
                return deviceId;
            }
        }

        public int GridItemsLayoutSpan
        {
            get => Preferences.Get(nameof(GridItemsLayoutSpan), 3);
            set => Preferences.Set(nameof(GridItemsLayoutSpan), value);
        }
    }
}

