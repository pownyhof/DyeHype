using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{
    public string AppID;
    public string BannerAdID;
    public string InterstitialAdID;

    public AdPosition BanePosition;

    public bool TestDevice = false;

    public static AdManager Instance;

    private BannerView banner;
    private InterstitialAd interstitial; 

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        MobileAds.Initialize(initStatus => { });
        this.CreateBanner(CreateRequest());
        this.CreateInterstitialAd(CreateRequest());
    }

    private AdRequest CreateRequest()
    {
        AdRequest request;

        if (TestDevice)
            request = new AdRequest.Builder().AddTestDevice(SystemInfo.deviceUniqueIdentifier).Build();
        else
            request = new AdRequest.Builder().Build();

        return request;
    }

    #region InterstitialAd

    public void CreateInterstitialAd(AdRequest request)
    {
        this.interstitial = new InterstitialAd(InterstitialAdID);
        this.interstitial.LoadAd(request);
    }

    public void ShowInterstitialAd()
    {
        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
        }

        this.interstitial.LoadAd(CreateRequest());
    }
    #endregion

    #region BannerAd

    public void CreateBanner(AdRequest request)
    {

        this.banner = new BannerView(BannerAdID, AdSize.IABBanner, BanePosition);
        this.banner.LoadAd(request);
        HideBanner();
    }

    public void HideBanner()
    {
        banner.Hide();
    }

    public void ShowBanner()
    {
        banner.Show();
    }
    #endregion

}
