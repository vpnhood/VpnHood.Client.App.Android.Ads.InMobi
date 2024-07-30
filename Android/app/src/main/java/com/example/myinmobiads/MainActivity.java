package com.example.myinmobiads;

import android.os.Bundle;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import com.vpnhood.inmobi.ads.IInMobiAdProvider;
import com.vpnhood.inmobi.ads.InMobiAdServiceFactory;

import java.util.concurrent.CompletableFuture;

public class MainActivity extends AppCompatActivity {
    public IInMobiAdProvider _iInMobiAdProvider;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_main);
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        CompletableFuture<Void> initTask = InMobiAdServiceFactory.InitializeInMobi(this,
                InMobiCredential.AccountId, true);
        initTask.thenAccept(result -> {
            _iInMobiAdProvider = InMobiAdServiceFactory.create(InMobiCredential.PlacementId);
            _iInMobiAdProvider.LoadAd(this).thenAccept(
                    result2 -> _iInMobiAdProvider.ShowAd(this)
            );
        });
    }
}
