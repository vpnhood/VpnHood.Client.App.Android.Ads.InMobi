package com.vpnhood.inmobi.ads;

import android.content.Context;
import java.util.concurrent.CompletableFuture;

public interface IInMobiAdProvider {
    CompletableFuture<Void> LoadAd(Context context);
    CompletableFuture<Void> ShowAd(Context context);
}
