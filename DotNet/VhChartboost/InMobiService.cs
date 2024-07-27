using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Inmobi.Ads;
using Com.Vpnhood.Inmobi.Ads;
using Java.Lang;
using Java.Util.Concurrent;
using VpnHood.Client.App.Abstractions;
using VpnHood.Client.App.Droid.Common.Utils;
using VpnHood.Client.Device;
using VpnHood.Client.Device.Droid;
using VpnHood.Common.Exceptions;
using VpnHood.Common.Utils;

namespace VpnHood.Client.App.Droid.Ads.VhInMobi
{
    public class InMobiService(string accountId, long placementId, bool isDebugMode)
    {
        private Com.Vpnhood.Inmobi.Ads.IAppAdService? _vhInMobiAdService;

        public string NetworkName => "InMobi";
        public AppAdType AdType => AppAdType.InterstitialAd;
        public DateTime? AdLoadedTime { get; private set; }
        public TimeSpan AdLifeSpan { get; } = TimeSpan.FromMinutes(45);

        public static InMobiService Create(string accountId, long placementId, bool isDebugMode)
        {
            var ret = new InMobiService(accountId, placementId, isDebugMode);
            return ret;
        }

        public bool IsCountrySupported(string countryCode)
        {
            // Make sure it is upper case
            countryCode = countryCode.Trim().ToUpper();

            // these countries are not supported at all
            return true;
        }

        public async Task LoadAd(IUiContext uiContext, CancellationToken cancellationToken)
        {
            var appUiContext = (AndroidUiContext)uiContext;
            var activity = appUiContext.Activity;
            if (activity.IsDestroyed)
                throw new AdException("MainActivity has been destroyed before loading the ad.");

            // initialize
            await InMobiUtil.Initialize(activity, accountId, isDebugMode, cancellationToken);
            _vhInMobiAdService = InMobiAdServiceFactory.Create(Java.Lang.Long.ValueOf(placementId));

            // reset the last loaded ad
            AdLoadedTime = null;

            // Load a new Ad
            if (_vhInMobiAdService == null)
                throw new AdException($"The {AdType} ad is not initialized");

            await _vhInMobiAdService.LoadAd(activity)!.AsTask();
            AdLoadedTime = DateTime.Now;
        }

        public async Task ShowAd(IUiContext uiContext, CancellationToken cancellationToken)
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
}
