using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct GlobalDataEntry
{
	public string Key;
	public int Value;
}

[System.Serializable]
public struct Rarity
{
	public int Id;
	public string Name;
	public string Color;
}

[System.Serializable]
public struct Skill
{
	public int Id;
	public string Effect;
	public string Description;
}

[System.Serializable]
public struct Card
{
	public int Id;
	public string Title;
	public string Text;
	public int Cost;
	public int Attack;
	public int Defence;
	public int Rarity;
	public List<int> Skills;
}

[CreateAssetMenu(fileName = "GameDataHolder", menuName = "Game/GameDataHolder", order = 1)]
public class GameData : ScriptableObject
{
    public string Version = "0";
	public List<GlobalDataEntry> GlobalData = new List<GlobalDataEntry>();
	public List<Rarity> Rarities = new List<Rarity>();
	public List<Skill> Skills = new List<Skill>();
	public List<Card> Cards = new List<Card>();
}
