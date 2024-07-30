package com.vpnhood.inmobi.ads;

import android.content.Context;

import java.util.concurrent.CompletableFuture;

public class InMobiAdServiceFactory {
    public static IInMobiAdProvider create(Long placementId) {
        return new InMobiAdService(placementId);
    }

    public static CompletableFuture<Void> InitializeInMobi(Context context, String accountId, Boolean isDebugMode){
        return InMobiAdInitializer.Initialize(context, accountId, isDebugMode);
    }
}
