using System;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;

/// <summary>
/// Demonstrates how to use the Google Mobile Ads app open ad format.
/// </summary>
[AddComponentMenu("GoogleMobileAds/Samples/AppOpenAdController")]
public class AppOpenAdController : MonoBehaviour
{



    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/9257395921";
#elif UNITY_IPHONE
   string _adUnitId = "ca-app-pub-3940256099942544/5575463023";
#else
  private string _adUnitId = "unused";
#endif

    public bool IsAdAvailable
    {
        get
        {
            return _adUnitId != null;
        }
    }

    public void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
        });
    }


    private AppOpenAd appOpenAd;

    /// <summary>
    /// Loads the app open ad.
    /// </summary>
    public void LoadAppOpenAd()
    {
        // Clean up the old ad before loading a new one.
        if (appOpenAd != null)
        {
            appOpenAd.Destroy();
            appOpenAd = null;
        }

        Debug.Log("Loading the app open ad.");

        // Create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        AppOpenAd.Load(_adUnitId, adRequest,
            (AppOpenAd ad, LoadAdError error) =>
            {
                // If the operation failed, an error is returned.
                if (error != null || ad == null)
                {
                    Debug.LogError("App open ad failed to load an ad with error : " +
                                    error);
                    return;
                }

                // If the operation completed successfully, no error is returned.
                Debug.Log("App open ad loaded with response : " + ad.GetResponseInfo());

                // App open ads can be preloaded for up to 4 hours.
                //_expireTime = DateTime.Now + TimeSpan.FromHours(4);

                appOpenAd = ad;
            });
    }

    private void RegisterEventHandlers(AppOpenAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("App open ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("App open ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("App open ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("App open ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("App open ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("App open ad failed to open full screen content " +
                           "with error : " + error);
        };
    }
 

    private void Awake()
    {
        // Use the AppStateEventNotifier to listen to application open/close events.
        // This is used to launch the loaded ad when we open the app.
        AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
    }

    private void OnDestroy()
    {
        // Always unlisten to events when complete.
        AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;
    }

    private void OnAppStateChanged(AppState state)
    {
        Debug.Log("App State changed to : " + state);

        // if the app is Foregrounded and the ad is available, show it.
        if (state == AppState.Foreground)
        {
            if (IsAdAvailable)
            {
                ShowAppOpenAd();
            }
        }
    }

    /// <summary>
    /// Shows the app open ad.
    /// </summary>
    public void ShowAppOpenAd()
    {
        if (appOpenAd != null && appOpenAd.CanShowAd())
        {
            Debug.Log("Showing app open ad.");
            appOpenAd.Show();
        }
        else
        {
            Debug.LogError("App open ad is not ready yet.");
        }
    }

    private void RegisterReloadHandler(AppOpenAd ad)
    {
        
    // Raised when the ad closed full screen content.
    ad.OnAdFullScreenContentClosed += () =>
    {
            Debug.Log("App open ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            LoadAppOpenAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("App open ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadAppOpenAd();
        };
    }
}