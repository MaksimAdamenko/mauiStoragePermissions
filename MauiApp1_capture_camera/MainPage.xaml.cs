using System.Diagnostics;

namespace MauiApp1_capture_camera;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnCounterClicked(object sender, EventArgs e)
	{
		if (!MediaPicker.IsCaptureSupported)
		{
			await DisplayAlert("Alert", "Camera capture is not supported", "Ok");
			return;
		}

		if (!await DeviceHelper.GetPermission<Permissions.Camera>())
		{
			await DisplayAlert("Alert", "App can't take a picture without permission to use the camera", "Ok");
			return;
		}

		if (!await DeviceHelper.GetPermission<Permissions.StorageWrite>())
		{
			await DisplayAlert("Alert", "App can't store a picture without permission to use the storage", "Ok");
			return;
		}


		FileResult photo = await MediaPicker.Default.CapturePhotoAsync();
		if (photo != null)
		{
			string localFilePath = await StorePicture(photo);
			Debug.WriteLine(localFilePath);
			ImageCtrl.Source = localFilePath;
		}
	}

	private async Task<string> StorePicture(FileResult photo)
	{
		// store file in the local storage
		string localFilePath = Path.Combine(FileSystem.AppDataDirectory, photo.FileName); // TODO: + "Media"

		using Stream sourceStream = await photo.OpenReadAsync();
		using FileStream localFileStream = File.OpenWrite(localFilePath);

		await sourceStream.CopyToAsync(localFileStream);

		return localFilePath;
	}
}

