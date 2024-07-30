package com.vpnhood.inmobi.ads;

import android.content.Context;
import androidx.annotation.NonNull;
import com.inmobi.ads.AdMetaInfo;
import com.inmobi.ads.InMobiAdRequestStatus;
import com.inmobi.ads.InMobiInterstitial;
import com.inmobi.ads.listeners.InterstitialAdEventListener;

import java.util.Map;
import java.util.concurrent.CompletableFuture;

class InMobiAdService extends InterstitialAdEventListener implements IInMobiAdProvider {

    private InMobiInterstitial _interstitialAd;
    public final Long _placementId;
    public CompletableFuture<Void> _loadTask;
    public CompletableFuture<Void> _showTask;

    public InMobiAdService(Long placementId){
        _placementId = placementId;
    }

    @Override
    public CompletableFuture<Void> LoadAd(Context context) {
        _interstitialAd = new InMobiInterstitial(context, _placementId, this);
        _loadTask = new CompletableFuture<>();
        _interstitialAd.load();
        return _loadTask;
    }

    @Override
    public CompletableFuture<Void> ShowAd(Context context) {
        _showTask = new CompletableFuture<>();
        _interstitialAd.show();
        return _showTask;
    }

    @Override
    public void onAdLoadSucceeded(@NonNull InMobiInterstitial ad, @NonNull AdMetaInfo info) {
        _loadTask.complete(null);
    }

    @Override
    public void onAdLoadFailed(@NonNull InMobiInterstitial ad, @NonNull InMobiAdRequestStatus status) {
        _loadTask.completeExceptionally(new Throwable(status.getMessage()));
    }

    @Override
    public void onAdFetchSuccessful(@NonNull InMobiInterstitial ad, @NonNull AdMetaInfo info) {

    }

    @Override
    public void onAdClicked(@NonNull InMobiInterstitial ad, Map< Object, Object > params) {

    }

    @Override
    public void onAdWillDisplay(@NonNull InMobiInterstitial ad) {

    }

    @Override
    public void onAdDisplayed(@NonNull InMobiInterstitial ad, @NonNull AdMetaInfo info) {
    }

    @Override
    public void onAdDisplayFailed(@NonNull InMobiInterstitial ad) {
        _showTask.completeExceptionally(new Throwable("Ad failed to display"));
    }

    @Override
    public void onAdDismissed(@NonNull InMobiInterstitial ad) {
        _showTask.complete(null);
    }

    @Override
    public void onUserLeftApplication(@NonNull InMobiInterstitial ad) {

    }


    @Override
    public void onAdImpression(@NonNull InMobiInterstitial ad) {
    }

}
