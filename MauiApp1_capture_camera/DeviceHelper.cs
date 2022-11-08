using static Microsoft.Maui.ApplicationModel.Permissions;

namespace MauiApp1_capture_camera
{
	public class DeviceHelper
	{
		public static async Task<bool> GetPermission<TPermission>(string rationalMessage = null)
			where TPermission : BasePermission, new()
		{
			var hasPermission = await CheckStatusAsync<TPermission>();

			if (hasPermission == PermissionStatus.Granted)
			{
				return true;
			}

			if (ShouldShowRationale<TPermission>())
			{
				string message = rationalMessage ?? $"{typeof(TPermission).Name} permission needed";
				await Shell.Current.DisplayAlert(null, message, "Ok");
			}

			PermissionStatus status = await RequestAsync<TPermission>();
			if (status != PermissionStatus.Granted)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
	}
}
