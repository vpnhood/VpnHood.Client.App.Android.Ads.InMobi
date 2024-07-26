package com.example.myinmobiads;

import android.os.Bundle;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import com.vpnhood.inmobi.ads.IAppAdService;
import com.vpnhood.inmobi.ads.InMobiAdServiceFactory;

import java.util.concurrent.CompletableFuture;

public class MainActivity extends AppCompatActivity {

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
                "YOUR_IN_MOBI_ACCOUNT_ID", true);
        initTask.thenAccept(result -> {
            IAppAdService appAdService = InMobiAdServiceFactory.create("YOUR_PLACEMENT_ID_AS_LONG");
            appAdService.LoadAd(this).thenAccept(result2 -> appAdService.ShowAd(this));
        });
    }
}
