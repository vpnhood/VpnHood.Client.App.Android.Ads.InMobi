using VpnHood.Client.App.Droid.Ads.VhInMobi;
using VpnHood.Client.Device.Droid;
using VpnHood.Client.Device.Droid.ActivityEvents;

namespace SampleShowAd;

[Activity(Label = "@string/app_name", MainLauncher = true)]
public class MainActivity : ActivityEvent
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Set our view from the "main" layout resource
        // ReSharper disable once AccessToStaticMemberViaDerivedType
        SetContentView(Resource.Layout.activity_main);
        _ = ShowAd();
    }

    private async Task ShowAd()
    {
        try {
            var adService = InMobiAdProvider.Create(InMobiCredential.AccountId, InMobiCredential.PlacementId, true);
            await adService.LoadAd(new AndroidUiContext(this), CancellationToken.None);
            await adService.ShowAd(new AndroidUiContext(this), string.Empty, CancellationToken.None);
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
    }
}