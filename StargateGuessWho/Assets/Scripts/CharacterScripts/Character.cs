using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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

static public class EnumHelpers
{
    public static string getEnumString(this Enum value)
    {
        // Get the type
        Type type = value.GetType();

        // Get fieldinfo for this type
        FieldInfo fieldInfo = type.GetField(value.ToString());

        // Get the stringvalue attributes
        StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
            typeof(StringValueAttribute), false) as StringValueAttribute[];

        // Return the first if there was a match.
        return attribs.Length > 0 ? attribs[0].StringValue : null;
    }
}

// Based on: https://weblogs.asp.net/stefansedich/enum-with-string-values-in-c
public class StringValueAttribute : Attribute
{
    public string StringValue { get; protected set; }

    public StringValueAttribute(string value)
    {
        this.StringValue = value;
    }
}

public enum CharacterTag : int
{
    [StringValue("System Lord")]
    SystemLord,
    [StringValue("Ascended")]
    Ascended,
    [StringValue("Team SG1")]
    TeamSG1,
    [StringValue("SGC")]
    SGC,
    [StringValue("Prior")]
    Prior,
    [StringValue("Base Leader")]
    BaseLeader,
    [StringValue("Earth Military")]
    EarthMilitary,
    [StringValue("Scientist")]
    Scientist,
    [StringValue("Doctor")]
    Doctor,
    [StringValue("NID")]
    NID,
    [StringValue("Earth Ship Crew")]
    EarthShipCrew
}

public enum SeriesTag : int
{
    [StringValue("Stargate SG1")]
    SG1,
    [StringValue("Stargate Atlantis")]
    SGA,
    [StringValue("Stargate Universe")]
    SGU
}

public enum RaceTag : int
{
    [StringValue("Goa'uld")]
    Goauld,
    [StringValue("Jaffa")]
    Jaffa,
    [StringValue("Tauri")]
    Tauri,
    [StringValue("Langaran")]
    Langaran,
    [StringValue("Ori Follower")]
    OriFollower,
    [StringValue("Tok'ra")]
    Tokra,
    [StringValue("Human")]
    Human,
    [StringValue("Abydonian")]
    Abydonian,
    [StringValue("Ancient")]
    Ancient,
    [StringValue("Asgard")]
    Asgard,
    [StringValue("Lucian Alliance")]
    LucianAlliance,
    [StringValue("Ori")]
    Ori,
    [StringValue("Replicator")]
    Replicator,
    [StringValue("Unas")]
    Unas,
    [StringValue("Nox")]
    Nox,
    [StringValue("Created")]
    Created,
    [StringValue("Satedan")]
    Satedan,
    [StringValue("Athosian")]
    Athosian,
    [StringValue("Genii")]
    Genii,
    [StringValue("Wraith")]
    Wraith,
    [StringValue("Tollan")]
    Tollan
}

