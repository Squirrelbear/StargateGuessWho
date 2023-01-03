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

    public bool hasAnyTag(CharacterTag[] tags)
    {
        for (int i = 0; i < tags.Length; i++)
        {
            if (hasTag(tags[i]))
            {
                return true;
            }
        }

        return false;
    }

    public bool hasTag(CharacterTag tag)
    {
        return tags.Contains(tag);
    }
}
