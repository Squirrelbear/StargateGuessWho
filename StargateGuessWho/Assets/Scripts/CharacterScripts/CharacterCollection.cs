using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class CharacterCollection
{
    [SerializeReference]
    public List<CharacterDatabaseEntry> characters;

    public CharacterCollection()
    {
        characters = new List<CharacterDatabaseEntry>();
    }

    public int[] getCharacterIDs()
    {
        int[] result = new int[characters.Count];

        for (int i = 0; i < characters.Count; i++)
        {
            result[i] = characters[i].characterID;
        }

        return result;
    }

    static string intToHex(int value)
    {
        Debug.Assert(value >= 0 && value <= 255);
        return value.ToString("X2");
    }

    public string toHexIDString()
    {
        int[] ids = getCharacterIDs();

        string[] hexIDs = new string[ids.Length];

        for (int i = 0; i < ids.Length; i++)
        {
            hexIDs[i] = intToHex(ids[i]);
        }

        string result = string.Join("", hexIDs);

        return result;
    }

    public bool hasValidCollection(out string error)
    {
        if (characters == null)
        {
            error = "No characters!";
        }

        if (characters.Count != 20)
        {
            error = string.Format("Count != 20: {0}", characters.Count);
            return false;
        }

        foreach (CharacterDatabaseEntry characterReference in characters)
        {
            if (characterReference.character == null)
            {
                error = string.Format("Null Character: {0}", characterReference.characterID);
                return false;
            }
        }

        IEnumerable<string> duplicatedCharacters = from c in characters
                            group c by c.character.name into g
                            where g.Count() > 1
                            select g.Key;

        if (duplicatedCharacters.Any())
        {
            error = string.Format("Duplicates: {0} [{1}]", duplicatedCharacters.Count(), string.Join(", ", duplicatedCharacters));
            return false;
        }

        error = "";
        return true;
    }

    static int[] hexStringToCharacterIDList(string idsString)
    {
        // Based on: https://stackoverflow.com/questions/23130382/split-string-by-character-count-and-store-in-string-array
        var split = idsString.Select((c, index) => new { c, index })
                .GroupBy(x => x.index / 2)
                .Select(group => group.Select(elem => elem.c))
                .Select(chars => new string(chars.ToArray()));

        int[] result = new int[split.Count()];
        int index = 0;

        foreach (var hexStr in split)
        {
            result[index] = int.Parse(hexStr, System.Globalization.NumberStyles.HexNumber);
            index++;
        }

        return result;
    }

#if UNITY_EDITOR
    public void testConvert()
    {
        int[] characterIDs = getCharacterIDs();
        string hexString = toHexIDString();
        int[] convertResult = hexStringToCharacterIDList(hexString);

        string debugOutput = string.Format("Hex String: {0} Convert Success: {1}", hexString, characterIDs.Equals(convertResult).ToString());

        Debug.Log(debugOutput);
        Debug.Log(string.Join(", ", characterIDs));
    }
#endif
}