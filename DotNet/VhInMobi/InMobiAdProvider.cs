using Com.Vpnhood.Inmobi.Ads;
using VpnHood.Client.App.Abstractions;
using VpnHood.Client.Device;
using VpnHood.Client.Device.Droid;
using VpnHood.Client.Device.Droid.Utils;
using VpnHood.Common.Exceptions;

namespace VpnHood.Client.App.Droid.Ads.VhInMobi;

public class InMobiAdProvider(string accountId, long placementId, bool isDebugMode) 
    : IAppAdProvider
{
    private IAppAdService? _vhInMobiAdService;

    public string NetworkName => "InMobi";
    public AppAdType AdType => AppAdType.InterstitialAd;
    public DateTime? AdLoadedTime { get; private set; }
    public TimeSpan AdLifeSpan { get; } = TimeSpan.FromMinutes(45);

    public static InMobiAdProvider Create(string accountId, long placementId, bool isDebugMode)
    {
        var ret = new InMobiAdProvider(accountId, placementId, isDebugMode);
        return ret;
    }
    
    public async Task LoadAd(IUiContext uiContext, CancellationToken cancellationToken)
    {
        var appUiContext = (AndroidUiContext)uiContext;
        var activity = appUiContext.Activity;
        if (activity.IsDestroyed)
            throw new AdException("MainActivity has been destroyed before loading the ad.");

        // reset the last loaded ad
        AdLoadedTime = null;

        // initialize
        await InMobiUtil.Initialize(activity, accountId, isDebugMode, cancellationToken);
        _vhInMobiAdService = InMobiAdServiceFactory.Create(Java.Lang.Long.ValueOf(placementId))
                             ?? throw new AdException($"The {AdType} ad is not initialized");

        await _vhInMobiAdService.LoadAd(activity)!.AsTask();
        AdLoadedTime = DateTime.Now;
    }

    public async Task ShowAd(IUiContext uiContext, string? customData, CancellationToken cancellationToken)
    {
        var appUiContext = (AndroidUiContext)uiContext;
        var activity = appUiContext.Activity;
        if (activity.IsDestroyed)
            throw new AdException("MainActivity has been destroyed before showing the ad.");

        try
        {
            if (AdLoadedTime == null || _vhInMobiAdService == null)
                throw new AdException($"The {AdType} has not been loaded.");

            Task? task = null;
            // wait for show or dismiss
            activity.RunOnUiThread(() =>
            {
                task = _vhInMobiAdService.ShowAd(activity)!.AsTask();
            });

            if (task != null)
                await task.ConfigureAwait(false);
        }
        finally
        {
            _vhInMobiAdService = null;
            AdLoadedTime = null;
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}