namespace MauiAppNet8;

public partial class MainPage : ContentPage
{
    private volatile static Queue<Image> Images = [];

    public MainPage()
    {
        InitializeComponent();
        var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(300));
        cameraView.CamerasLoaded += (sdnr, se) =>
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                cameraView.Camera = cameraView.Cameras.First();
                await cameraView.StopCameraAsync();
                var smallest = cameraView.Camera.AvailableResolutions.MinBy(it => it.Width * it.Height);
                await cameraView.StartCameraAsync(smallest);
            });
        };
        CaptureButton.Clicked += (sender, e) => Capture();
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            while (await timer.WaitForNextTickAsync())
            {
                if (AutoCapture.IsChecked) Capture();
                while (Images.TryDequeue(out var img))
                {
                    Container.Add(img);
                }
            }
        });
    }

    private void Capture()
    {
        var thread = new Thread(new ThreadStart(() =>
        {
            Images.Enqueue(new Image
            {
                WidthRequest = 60,
                HeightRequest = 60,
                Source = cameraView.GetSnapShot(Camera.MAUI.ImageFormat.JPEG),
            });
        }));
        thread.Start();
    }
}