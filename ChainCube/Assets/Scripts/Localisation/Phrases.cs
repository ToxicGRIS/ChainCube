using System;
using UnityEngine;

[Serializable]
public class Phrases
{
	[SerializeField] private string[] phrases;

	public Phrases(string[] newPhrases)
	{
		phrases = newPhrases;
	}
	public string[] text => phrases;
}
