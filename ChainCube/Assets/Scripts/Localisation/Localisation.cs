using System;
using UnityEngine;
using TMPro;
using System.IO;

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
	[SerializeField] private TextMeshProUGUI recordText;

	[SerializeField] private static Language currentLanguage;

	private TextMeshProUGUI[] texts;
	private string localisationPath;

	[SerializeField] private static Phrases ru = new Phrases( new string[]
	{
		"Играть",
		"ENG",
		"ИГРА ОКОНЧЕНА :(",
		"Очки: ",
		"Начать заново",
		"Рекорд: "
	});
	[SerializeField] private static Phrases eng = new Phrases(new string[]
	{
		"Play",
		"RU",
		"GAME OVER :(",
		"Score: ",
		"New game",
		"Record: "
	});
	public static string[] CurrentLocalisation
	{
		get
		{
			if (currentLanguage == Language.ENG) return eng.text;
			else if (currentLanguage == Language.RU) return ru.text;
			else return eng.text;
		}
	}

	#endregion
	#region Start

	private void Awake()
	{
		localisationPath = Application.dataPath + "/local/";

		if (!File.Exists(localisationPath + "eng.json"))
		{
			File.Create(localisationPath + "eng.json").Close();
			File.WriteAllText(localisationPath + "eng.json", JsonUtility.ToJson(eng, true));
		}
		else eng = JsonUtility.FromJson<Phrases>(File.ReadAllText(localisationPath + "eng.json"));
		if (!File.Exists(localisationPath + "ru.json"))
		{
			File.Create(localisationPath + "ru.json").Close();
			File.WriteAllText(localisationPath + "ru.json", JsonUtility.ToJson(ru, true));
		}
		else ru = JsonUtility.FromJson<Phrases>(File.ReadAllText(localisationPath + "ru.json"));
	}

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
			newGameText,
			recordText
		};

		UpdateTexts();
	}

	#endregion

	public void UpdateTexts()
	{
			for (int i = 0; i < eng.text.Length; i++)
			{
				texts[i].text = CurrentLocalisation[i];
			}
		texts[(int)Phrase.Score].text += score.CurrentScore;
		texts[(int)Phrase.Record].text += PlayerPrefs.GetInt("ChainCuberecord");
	}

	public void SwitchLanguage()
	{
		if (currentLanguage == Language.ENG) currentLanguage = Language.RU;
		else if (currentLanguage == Language.RU) currentLanguage = Language.ENG;
		UpdateTexts();
	}
}
