using Com.Vpnhood.Inmobi.Ads;
using Java.Util.Concurrent;
using Java.Util.Functions;
using Object = Java.Lang.Object;

namespace SampleShowAd
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            _ = ShowAd();

        }

        private async Task ShowAd()
        {
            try
            {
                await InMobiAdServiceFactory.InitializeInMobi(this, "YOUR_IN_MOBI_ACCOUNT_ID", Java.Lang.Boolean.True)!.AsTask();
                var appAdService = InMobiAdServiceFactory.Create(Java.Lang.Long.ValueOf("YOUR_PLACEMENT_ID_AS_LONG"))!;
                await appAdService.LoadAd(this)!.AsTask();
                await appAdService.ShowAd(this)!.AsTask();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}


public class CompletableFutureTask : Java.Lang.Object, IBiConsumer
{
    private readonly TaskCompletionSource<string?> _taskCompletionSource;
    public Task<string?> Task => _taskCompletionSource.Task;

    private CompletableFutureTask(CompletableFuture completableFuture)
    {
        _taskCompletionSource = new TaskCompletionSource<string?>();
        completableFuture.WhenComplete(this);
    }

    public static Task<string?> Create(CompletableFuture completableFuture)
    {
        var listener = new CompletableFutureTask(completableFuture);
        return listener.Task;
    }

    public void Accept(Object? t, Object? ex)
    {
        if (ex != null)
            _taskCompletionSource.TrySetException(new Exception(ex.ToString()));
        else
            _taskCompletionSource.TrySetResult(t?.ToString());
    }
}

public static class CompletableFutureExtensions
{
    public static Task<string?> AsTask(this CompletableFuture completableFuture)
    {
        return CompletableFutureTask.Create(completableFuture);
    }
}