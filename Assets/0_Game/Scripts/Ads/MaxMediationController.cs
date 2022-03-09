using System;
using System.Collections;
using System.Collections.Generic;
using AppsFlyerSDK;
using UnityEngine;

public class MaxMediationController : MonoBehaviour
{

    private const string MaxSdkKey = "fJLLShLBCbJkP9zMNpEPfHd_gNeG_nXN1QF_mcP-PuqDM23Isg-XK57Iurb-q7DmpQfPQUFR91X5psWWTTHT9m";
    private const string InterstitialAdUnitId = "145a16ac07d162ca";
    private const string RewardedAdUnitId = "3f2a24837081356f";
    private const string RewardedInterstitialAdUnitId = "ENTER_REWARD_INTER_AD_UNIT_ID_HERE";
    private const string BannerAdUnitId = "ca6d93016ea5b331";
    private const string MRecAdUnitId = "";


    private bool isBannerShowing;
    private bool isMRecShowing;

    private int interstitialRetryAttempt;
    private int rewardedRetryAttempt;
    private int rewardedInterstitialRetryAttempt;

    public TypeAdsMax TypeAdsUse;

    public void Awake()
    {
        Init();
    }

    public void Init()
    {
        MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
        {
            // AppLovin SDK is initialized, configure and start loading ads.
            Debug.Log("MAX SDK Initialized");
            if (TypeAdsUse.HasFlag(TypeAdsMax.Inter))
                InitializeInterstitialAds();

            if (TypeAdsUse.HasFlag(TypeAdsMax.Reward))
                InitializeRewardedAds();

            if (TypeAdsUse.HasFlag(TypeAdsMax.Inter_Reward))
                InitializeRewardedInterstitialAds();

            if (TypeAdsUse.HasFlag(TypeAdsMax.Banner))
                InitializeBannerAds();

            if (TypeAdsUse.HasFlag(TypeAdsMax.MRec))
                InitializeMRecAds();

            //MaxSdk.ShowMediationDebugger();
        };
        MaxSdk.SetUserId(AppsFlyer.getAppsFlyerId());
        MaxSdk.SetSdkKey(MaxSdkKey);
        MaxSdk.InitializeSdk();
    }

    #region Interstitial Ad Methods

    private void InitializeInterstitialAds()
    {
        // Attach callbacks
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialFailedToDisplayEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialDismissedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterstitialRevenuePaidEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;

        // Load the first interstitial
        LoadInterstitial();
    }

    public void LoadInterstitial()
    {
        //if (MaxSdk.IsInterstitialReady(InterstitialAdUnitId))
        //{
        //    return;
        //}

        MaxSdk.LoadInterstitial(InterstitialAdUnitId);
    }

    public void ShowInterstitial(string placement)
    {
        if (MaxSdk.IsInterstitialReady(InterstitialAdUnitId))
        {
            MaxSdk.ShowInterstitial(InterstitialAdUnitId, placement);
        }
    }

    public bool IsLoadInterstitial()
    {
        return MaxSdk.IsInterstitialReady(InterstitialAdUnitId);
    }

    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Reset retry attempt
        interstitialRetryAttempt = 0;
    }

    private void OnInterstitialFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Interstitial ad failed to load. We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds).
        interstitialRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, interstitialRetryAttempt));

        Invoke("LoadInterstitial", (float)retryDelay);
    }

    private void OnInterstitialFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. We recommend loading the next ad
        //DebugCustom.Log("Interstitial failed to display with error code: " + errorCode);
        LoadInterstitial();
    }

    private void OnInterstitialDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is hidden. Pre-load the next ad
        LoadInterstitial();
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Reset retry attempt
        interstitialRetryAttempt = 0;
        AppsFlyer.sendEvent("event_interstitial_ad_clicked", new Dictionary<string, string>() { { "interstitial_ad_clicked", "interstitial_ad_clicked" } });
    }

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Reset retry attempt
        //interstitialRetryAttempt = 0;  
        Debug.Log("InterstitialDisplayedEvent");
        AppsFlyer.sendEvent("event_interstitial_ad_impression", new Dictionary<string, string>() { { "event_interstitial_ad_impression", "event_interstitial_ad_impression" } });
    }

    private void OnInterstitialRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad revenue paid. Use this callback to track user revenue.
        Debug.Log("Interstitial revenue paid");

        // Ad revenue
        double revenue = adInfo.Revenue;

        // Miscellaneous data
        string countryCode = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD" in most cases!
        string networkName = adInfo.NetworkName; // Display name of the network that showed the ad (e.g. "AdColony")
        string adUnitIdentifier = adInfo.AdUnitIdentifier; // The MAX Ad Unit ID
        string placement = adInfo.Placement; // The placement this ad's postbacks are tied to

        //var data = new ImpressionData();
        //data.AdFormat = "interstitial";
        //data.AdUnitIdentifier = adUnitIdentifier;
        //data.CountryCode = countryCode;
        //data.NetworkName = networkName;
        //data.Placement = placement;
        //data.Revenue = revenue;

        //AnalyticsRevenueAds.SendEvent(data, AdFormat.interstitial);
    }

    #endregion

    #region Rewarded Ad Methods

    private void InitializeRewardedAds()
    {
        // Attach callbacks
        //MaxSdkCallbacks.OnRewardedAdLoadedEvent += OnRewardedAdLoadedEvent;
        //MaxSdkCallbacks.OnRewardedAdLoadFailedEvent += OnRewardedAdFailedEvent;
        //MaxSdkCallbacks.OnRewardedAdFailedToDisplayEvent += OnRewardedAdFailedToDisplayEvent;
        //MaxSdkCallbacks.OnRewardedAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        //MaxSdkCallbacks.OnRewardedAdClickedEvent += OnRewardedAdClickedEvent;
        //MaxSdkCallbacks.OnRewardedAdHiddenEvent += OnRewardedAdDismissedEvent;
        //MaxSdkCallbacks.OnRewardedAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;


        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdDismissedEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;

        // Load the first RewardedAd
        LoadRewardedAd();
    }

    public void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd(RewardedAdUnitId);
    }

    public bool IsLoadRewardedAd()
    {
        return MaxSdk.IsRewardedAdReady(RewardedAdUnitId);
    }

    public void ShowRewardedAd(string placeId)
    {
        if (MaxSdk.IsRewardedAdReady(RewardedAdUnitId))
        {
            //AppOpenAdManager.Instance.IsShowAds = false;
            MaxSdk.ShowRewardedAd(RewardedAdUnitId, placeId);

        }
    }


    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is ready to be shown. MaxSdk.IsRewardedAdReady(rewardedAdUnitId) will now return 'true'
        Debug.Log("Rewarded ad loaded");

        // Reset retry attempt
        rewardedRetryAttempt = 0;
    }

    private void OnRewardedAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load. We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds).
        rewardedRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, rewardedRetryAttempt));

        //Debug.Log("Rewarded ad failed to load with error code: " + error);

        Invoke("LoadRewardedAd", (float)retryDelay);
    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. We recommend loading the next ad
        //Debug.Log("Rewarded ad failed to display with error code: " + errorCode);
        LoadRewardedAd();
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Rewarded ad displayed");
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {

        AppsFlyerSDK.AppsFlyer.sendEvent("event_video_reward_clicked", new Dictionary<string, string>() { { "clicked_video", "clicked_video" } });
    }

    private void OnRewardedAdDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is hidden. Pre-load the next ad
        Debug.Log("Rewarded ad dismissed");
        LoadRewardedAd();
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad was displayed and user should receive the reward
        //DebugCustom.Log("Rewarded ad received reward");
        //if (adsManager.onGetReward != null)
        //{
        //    adsManager.onGetReward(1);
        //}

        //GameManager.Instance.Profile.SetNumberPlay(-1);
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad revenue paid. Use this callback to track user revenue.
        Debug.Log("Rewarded ad revenue paid");

        // Ad revenue
        double revenue = adInfo.Revenue;

        // Miscellaneous data
        string countryCode = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD" in most cases!
        string networkName = adInfo.NetworkName; // Display name of the network that showed the ad (e.g. "AdColony")
        string adUnitIdentifier = adInfo.AdUnitIdentifier; // The MAX Ad Unit ID
        string placement = adInfo.Placement; // The placement this ad's postbacks are tied to

        //var data = new ImpressionData();
        //data.AdFormat = "video_reward";
        //data.AdUnitIdentifier = adUnitIdentifier;
        //data.CountryCode = countryCode;
        //data.NetworkName = networkName;
        //data.Placement = placement;
        //data.Revenue = revenue;

        //AnalyticsRevenueAds.SendEvent(data, AdFormat.video_rewarded);
    }

    #endregion

    #region Rewarded Interstitial Ad Methods

    private void InitializeRewardedInterstitialAds()
    {
        // Attach callbacks
        //MaxSdkCallbacks.OnRewardedInterstitialAdLoadedEvent += OnRewardedInterstitialAdLoadedEvent;
        //MaxSdkCallbacks.OnRewardedInterstitialAdLoadFailedEvent += OnRewardedInterstitialAdFailedEvent;
        //MaxSdkCallbacks.OnRewardedInterstitialAdFailedToDisplayEvent += OnRewardedInterstitialAdFailedToDisplayEvent;
        //MaxSdkCallbacks.OnRewardedInterstitialAdDisplayedEvent += OnRewardedInterstitialAdDisplayedEvent;
        //MaxSdkCallbacks.OnRewardedInterstitialAdClickedEvent += OnRewardedInterstitialAdClickedEvent;
        //MaxSdkCallbacks.OnRewardedInterstitialAdHiddenEvent += OnRewardedInterstitialAdDismissedEvent;
        //MaxSdkCallbacks.OnRewardedInterstitialAdReceivedRewardEvent += OnRewardedInterstitialAdReceivedRewardEvent;

        // Load the first RewardedInterstitialAd
        LoadRewardedInterstitialAd();
    }

    public void LoadRewardedInterstitialAd()
    {
        if (MaxSdk.IsRewardedInterstitialAdReady(RewardedInterstitialAdUnitId))
        {
            return;
        }

        MaxSdk.LoadRewardedInterstitialAd(RewardedInterstitialAdUnitId);
    }

    public bool IsRewardedInterstitialAdReady()
    {
        return MaxSdk.IsRewardedInterstitialAdReady(RewardedInterstitialAdUnitId);
    }

    public void ShowRewardedInterstitialAd(string placeId)
    {
        if (MaxSdk.IsRewardedInterstitialAdReady(RewardedInterstitialAdUnitId))
        {
            MaxSdk.ShowRewardedInterstitialAd(RewardedInterstitialAdUnitId, placeId);
        }
    }

    private void OnRewardedInterstitialAdLoadedEvent(string adUnitId)
    {
        // Rewarded interstitial ad is ready to be shown. MaxSdk.IsRewardedInterstitialAdReady(rewardedInterstitialAdUnitId) will now return 'true'
        Debug.Log("Rewarded interstitial ad loaded");

        // Reset retry attempt
        rewardedInterstitialRetryAttempt = 0;
    }

    private void OnRewardedInterstitialAdFailedEvent(string adUnitId, int errorCode)
    {
        // Rewarded interstitial ad failed to load. We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds).
        rewardedInterstitialRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, rewardedInterstitialRetryAttempt));
        Debug.Log("Rewarded interstitial ad failed to load with error code: " + errorCode);

        Invoke("LoadRewardedInterstitialAd", (float)retryDelay);
    }

    private void OnRewardedInterstitialAdFailedToDisplayEvent(string adUnitId, int errorCode)
    {
        // Rewarded interstitial ad failed to display. We recommend loading the next ad
        Debug.Log("Rewarded interstitial ad failed to display with error code: " + errorCode);
        LoadRewardedInterstitialAd();
    }

    private void OnRewardedInterstitialAdDisplayedEvent(string adUnitId)
    {
        Debug.Log("Rewarded interstitial ad displayed");
    }

    private void OnRewardedInterstitialAdClickedEvent(string adUnitId)
    {
        Debug.Log("Rewarded interstitial ad clicked");
    }

    private void OnRewardedInterstitialAdDismissedEvent(string adUnitId)
    {
        // Rewarded interstitial ad is hidden. Pre-load the next ad
        Debug.Log("Rewarded interstitial ad dismissed");
        LoadRewardedInterstitialAd();
    }

    private void OnRewardedInterstitialAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward)
    {
        // Rewarded interstitial ad was displayed and user should receive the reward
        Debug.Log("Rewarded interstitial ad received reward");
        //if (adsManager.onGetReward != null)
        //{
        //    adsManager.onGetReward(1);
        //}
    }

    #endregion

    #region Banner Ad Methods

    private void InitializeBannerAds()
    {
        //if (GameManager.Instance.PlayerDataManager.IsNoAds())
        //    return;

        MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdFailedEvent;
        MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;

        // Banners are automatically sized to 320x50 on phones and 728x90 on tablets.
        // You may use the utility method `MaxSdkUtils.isTablet()` to help with view sizing adjustments.
        MaxSdk.CreateBanner(BannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);

        // Set background or background color for banners to be fully functional.
        MaxSdk.SetBannerBackgroundColor(BannerAdUnitId, Color.black);
        MaxSdk.ShowBanner(BannerAdUnitId);
    }

    private void ToggleBannerVisibility()
    {
        if (!isBannerShowing)
        {
            MaxSdk.ShowBanner(BannerAdUnitId);
        }
        else
        {
            MaxSdk.HideBanner(BannerAdUnitId);
        }

        isBannerShowing = !isBannerShowing;
    }

    public bool ShowBanner()
    {
        //MaxSdk.ShowBanner(BannerAdUnitId);
        return true;
    }

    public void HideBanner()
    {
        MaxSdk.HideBanner(BannerAdUnitId);
    }

    private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Banner ad is ready to be shown.
        // If you have already called MaxSdk.ShowBanner(BannerAdUnitId) it will automatically be shown on the next ad refresh.
        Debug.Log("Banner ad loaded");
        // GameManager.Instance.ShowBannerAds();
    }

    private void OnBannerAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Banner ad failed to load. MAX will automatically try loading a new ad internally.
        Debug.Log("Banner ad failed to load with error code: " + errorInfo.Code);
    }

    private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Banner ad clicked");
    }

    private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Banner ad revenue paid. Use this callback to track user revenue.
        Debug.Log("Banner ad revenue paid");

        // Ad revenue
        double revenue = adInfo.Revenue;

        // Miscellaneous data
        string countryCode = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD" in most cases!
        string networkName = adInfo.NetworkName; // Display name of the network that showed the ad (e.g. "AdColony")
        string adUnitIdentifier = adInfo.AdUnitIdentifier; // The MAX Ad Unit ID
        string placement = adInfo.Placement; // The placement this ad's postbacks are tied to

        //var data = new ImpressionData();
        //data.AdFormat = "banner";
        //data.AdUnitIdentifier = adUnitIdentifier;
        //data.CountryCode = countryCode;
        //data.NetworkName = networkName;
        //data.Placement = placement;
        //data.Revenue = revenue;

        //AnalyticsRevenueAds.SendEvent(data, AdFormat.banner);


    }
    #endregion

    #region MREC Ad Methods

    private void InitializeMRecAds()
    {
        // MRECs are automatically sized to 300x250.
        MaxSdk.CreateMRec(MRecAdUnitId, MaxSdkBase.AdViewPosition.BottomCenter);
    }

    private void ToggleMRecVisibility()
    {
        if (!isMRecShowing)
        {
            MaxSdk.ShowMRec(MRecAdUnitId);
        }
        else
        {
            MaxSdk.HideMRec(MRecAdUnitId);
        }

        isMRecShowing = !isMRecShowing;
    }

    #endregion
}

[Flags]
public enum TypeAdsMax
{
    Inter = 1 << 0,
    Banner = 1 << 1,
    Reward = 1 << 2,
    Inter_Reward = 1 << 3,
    MRec = 1 << 4
}
