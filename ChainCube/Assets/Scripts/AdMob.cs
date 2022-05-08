using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using GoogleMobileAds.Api;

public class AdMob : MonoBehaviour
{
	[SerializeField] private bool adsEnabled;

	/*private InterstitialAd interstitialAd;
	private string interUnitId = "ca-app-pub-3940256099942544/1033173712";
	
	private void Awake()
	{
		MobileAds.Initialize(e => { });
	}

	private void OnEnable()
	{
		interstitialAd = new InterstitialAd(interUnitId);
		AdRequest adRequest = new AdRequest.Builder().Build();
		interstitialAd.LoadAd(adRequest);
	}

	public void ShowAd()
	{
		if (interstitialAd.IsLoaded() && adsEnabled)
			interstitialAd.Show();
	}*/
}
