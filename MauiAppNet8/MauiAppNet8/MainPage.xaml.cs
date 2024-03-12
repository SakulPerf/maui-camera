namespace MauiAppNet8;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        cameraView.CamerasLoaded += (sdnr, se) =>
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                cameraView.Camera = cameraView.Cameras.First();
                await cameraView.StopCameraAsync();
                await cameraView.StartCameraAsync();
            });
        };
        CaptureButton.Clicked += (sender, e) => ImagePreview.Source = cameraView.GetSnapShot();
    }
}