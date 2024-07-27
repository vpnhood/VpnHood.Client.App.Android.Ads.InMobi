using Com.Vpnhood.Inmobi.Ads;
using VpnHood.Client.App.Droid.Common.Utils;
using VpnHood.Common.Utils;

namespace VpnHood.Client.App.Droid.Ads.VhInMobi;

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

        await InMobiAdServiceFactory.InitializeInMobi(activity, accountId, Java.Lang.Boolean.ValueOf(isDebugMode))!.AsTask();

        IsInitialized = true;
    }
}
