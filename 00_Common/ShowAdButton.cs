using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ShowAdButton : MonoBehaviour
{
    [SerializeField] AdType adType;
    Button button;

    void Awake()
    {
        TryGetComponent(out button);
        button.onClick.AddListener(() =>
        {
            ShowRewardedInterstitialAd();
            button.interactable = false;
        });

        LoadRewardedInterstitialAd();
    }
    
    // These ad units are configured to always serve test ads.
    #if UNITY_ANDROID
    static string[] _adUnitId =
    {
        "ca-app-pub-3920142147368711/2814553999", 
        "ca-app-pub-3920142147368711/7765883379",
        "ca-app-pub-3920142147368711/4544823668",
        "ca-app-pub-3920142147368711/8099521950",
        "ca-app-pub-3920142147368711/7468114870"
    };
    #elif UNITY_IPHONE
    static string _adUnitId = "ca-app-pub-3940256099942544/6978759866";
    #else
    static string _adUnitId = "unused";
    #endif

    RewardedAd rewardedInterstitialAd;

    /// <summary>
    /// Loads the rewarded interstitial ad.
    /// </summary>
    public void LoadRewardedInterstitialAd()
    {
        // Clean up the old ad before loading a new one.
        if (rewardedInterstitialAd != null)
        {
            rewardedInterstitialAd.Destroy();
            rewardedInterstitialAd = null;
        }

        Debug.Log("Loading the rewarded interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // send the request to load the ad.
        RewardedAd.Load(_adUnitId[(int)adType], adRequest,
        (RewardedAd ad, LoadAdError error) =>
        {
            // if error is not null, the load request failed.
            if (error != null || ad == null)
            {
                Debug.LogError("rewarded interstitial ad failed to load an ad " +
                                "with error : " + error);
                return;
            }

            Debug.Log("Rewarded interstitial ad loaded with response : "
                        + ad.GetResponseInfo());

            rewardedInterstitialAd = ad;
            RegisterEventHandlers(rewardedInterstitialAd);
            RegisterReloadHandler(rewardedInterstitialAd);
        });
    }

    public void ShowRewardedInterstitialAd()
    {
        if (rewardedInterstitialAd != null && rewardedInterstitialAd.CanShowAd())
        {
            rewardedInterstitialAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                switch (adType)
                {
                    case AdType.Attendance:
                    Managers.Event.RecieveAttendanceRewardEvent?.Invoke(true);
                    break;
                    case AdType.RefreshRank:
                    break;
                    case AdType.FreeDiamond:
                    break;
                    case AdType.FreeGold:
                    break;
                    case AdType.CollectBonus:
                    break;
                }
            });
        }
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        // ad.OnAdPaid += (AdValue adValue) =>
        // {
        //     Debug.Log(String.Format("Rewarded interstitial ad paid {0} {1}.",
        //         adValue.Value,
        //         adValue.CurrencyCode));
        // };
        // // Raised when an impression is recorded for an ad.
        // ad.OnAdImpressionRecorded += () =>
        // {
        //     Debug.Log("Rewarded interstitial ad recorded an impression.");
        // };
        // // Raised when a click is recorded for an ad.
        // ad.OnAdClicked += () =>
        // {
        //     Debug.Log("Rewarded interstitial ad was clicked.");
        // };
        // // Raised when an ad opened full screen content.
        // ad.OnAdFullScreenContentOpened += () =>
        // {
        //     Debug.Log("Rewarded interstitial ad full screen content opened.");
        // };
        // // Raised when the ad closed full screen content.
        // ad.OnAdFullScreenContentClosed += () =>
        // {
        //     Debug.Log("Rewarded interstitial ad full screen content closed.");
        // };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Managers.Alarm.Warning($"광고 초기화에 실패했습니다. {error}");
            // Debug.LogError("Rewarded interstitial ad failed to open full screen content " +
            //             "with error : " + error);
        };
    }

    private void RegisterReloadHandler(RewardedAd ad)
    {
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded interstitial ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedInterstitialAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded interstitial ad failed to open full screen content " +
                        "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedInterstitialAd();
        };
    }
}