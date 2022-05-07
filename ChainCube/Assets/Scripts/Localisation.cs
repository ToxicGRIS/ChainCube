using System;
using UnityEngine;
using TMPro;

[Serializable]
public class Localisation : MonoBehaviour
{
	#region Properties

	[SerializeField] private Score score;

	[SerializeField] private TextMeshProUGUI playText;
    [SerializeField] private TextMeshProUGUI langButton;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI newGameText;

	[SerializeField] private static Language currentLanguage;

	private TextMeshProUGUI[] texts;
	private static string[] ru =
	{
		"Играть",
		"ENG",
		"ИГРА ОКОНЧЕНА :(",
		"Очки: ",
		"Начать заново"
	};
	private static string[] eng =
	{
		"Play",
		"RU",
		"GAME OVER :(",
		"Score: ",
		"New game"
	};
	public static string[] CurrentLocalisation
	{
		get
		{
			if (currentLanguage == Language.ENG) return eng;
			else if (currentLanguage == Language.RU) return ru;
			else return eng;
		}
	}

	#endregion
	#region Start

	private void Start()
	{
		if(PlayerPrefs.HasKey("ChainCubeLanguage"))
		{
			currentLanguage = (Language)PlayerPrefs.GetInt("ChainCubeLanguage");
		}
		else
		{
			currentLanguage = Language.ENG;
			PlayerPrefs.SetInt("ChainCubeLanguage", (int)currentLanguage);
		}

		texts = new TextMeshProUGUI[]
		{
			playText,
			langButton,
			gameOverText,
			scoreText,
			newGameText
		};
	}

	#endregion

	public void UpdateTexts()
	{
			for (int i = 0; i < eng.Length; i++)
			{
				texts[i].text = CurrentLocalisation[i];
			}
		texts[3].text += score.CurrentScore;
	}

	public void SwitchLanguage()
	{
		if (currentLanguage == Language.ENG) currentLanguage = Language.RU;
		else if (currentLanguage == Language.RU) currentLanguage = Language.ENG;
		UpdateTexts();
	}
}
