using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character")]
public class Character : ScriptableObject
{
    public string characterName;
    public Sprite characterSprite;
    public SeriesTag seriesTag;
    public RaceTag raceTag;
    public bool isAntagonist;
    public List<CharacterTag> tags;
}
