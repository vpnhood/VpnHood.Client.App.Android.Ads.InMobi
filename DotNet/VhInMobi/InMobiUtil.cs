using Com.Vpnhood.Inmobi.Ads;
using VpnHood.Core.Common.Utils;
using VpnHood.Core.Client.Device.Droid.Utils;

namespace VpnHood.AppLib.Droid.Ads.VhInMobi;

public class InMobiUtil
{
    private static readonly AsyncLock InitLock = new();
    public static bool IsInitialized { get; private set; }

    public static async Task Initialize(Activity activity, string accountId, bool isDebugMode,
        CancellationToken cancellationToken)
    {
        using var lockAsync = await InitLock.LockAsync(cancellationToken);
        if (IsInitialized)
            return;

        // initialize
        var initTask = await AndroidUtil.RunOnUiThread(activity, () => InMobiAdServiceFactory.InitializeInMobi(activity, accountId,
                Java.Lang.Boolean.ValueOf(isDebugMode))!.AsTask())
            .WaitAsync(cancellationToken)
            .ConfigureAwait(false);

        // wait for completion
        await initTask
            .WaitAsync(cancellationToken)
            .ConfigureAwait(false);

        IsInitialized = true;
    }
}