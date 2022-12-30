using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FilteredCharacterSet
{
    [SerializeField]
    private CharacterDatabase characterDatabase;

    [SerializeField]
    private List<int> characterIDs;

    [SerializeField]
    private string characterSetName;

    private FilteredCharacterSet()
    {
        characterIDs = new List<int>();
        characterSetName = "None";
    }

    public FilteredCharacterSet(CharacterDatabase characterDatabase)
        : this()
    {
        this.characterDatabase = characterDatabase;
    }

    public FilteredCharacterSet(CharacterDatabase characterDatabase, List<int> characterIDs, string characterSetName)
    {
        this.characterDatabase = characterDatabase;
        this.characterIDs = characterIDs;
        this.characterSetName = characterSetName;
    }

    public FilteredCharacterSet(CharacterDatabase characterDatabase, SeriesTag tag)
        : this(characterDatabase)
    {
        characterIDs = characterDatabase.getIDsMatchingSeries(tag);
        setName("All Series " + EnumHelpers.getEnumString(tag));
    }

    public FilteredCharacterSet(CharacterDatabase characterDatabase, RaceTag tag)
        : this(characterDatabase)
    {
        characterIDs = characterDatabase.getIDsMatchingRace(tag);
        setName("All " + EnumHelpers.getEnumString(tag));
    }

    public FilteredCharacterSet(CharacterDatabase characterDatabase, CharacterTag tag)
        : this(characterDatabase)
    {
        CharacterTag[] tagArray = { tag };
        characterIDs = characterDatabase.getIDsMatchingAnyTag(tagArray);
        setName("All " + EnumHelpers.getEnumString(tag));
    }

    public FilteredCharacterSet(CharacterDatabase characterDatabase, CharacterTag[] tags)
        : this(characterDatabase)
    {
        characterIDs = characterDatabase.getIDsMatchingAnyTag(tags);
        setName("Multiple Tags");
    }

    public FilteredCharacterSet(CharacterDatabase characterDatabase, bool isAntagonist)
        : this(characterDatabase)
    {
        characterIDs = characterDatabase.getIDsMatchingAntagonist(isAntagonist);
        setName("All " + (isAntagonist ? "Antagonists" : "Non-Antagonists"));
    }

    public FilteredCharacterSet(CharacterDatabase characterDatabase, FilteredCharacterSetManager.CharacterSetSaveFormat savedData)
        : this(characterDatabase)
    {
        initFromNameList(savedData.characterSetName, savedData.characterNames);
    }

    public void addIDs(List<int> characterIDsToAdd)
    {
        foreach (int characterID in characterIDsToAdd)
        {
            if (characterIDs.Contains(characterID) == false)
            {
                characterIDsToAdd.Add(characterID);
            }
        }
    }

    public void removeIDs(List<int> characterIDToRemove)
    {
        characterIDs.RemoveAll(i => characterIDToRemove.Contains(i));
    }

    public CharacterCollection generateCharacterCollection()
    {
        return characterDatabase.createCharacterCollectionFromIDs(characterIDs.ToArray());
    }

    public List<string> getAsNameList()
    {
        List<string> nameList = new List<string>();
        List<Character> databaseEntries = characterDatabase.getAllCharacters();

        foreach (int characterID in characterIDs)
        {
            nameList.Add(databaseEntries[characterID].name);
        }

        return nameList;
    }

    public void initFromNameList(string characterSetName, List<string> names)
    {
        int[] idList = characterDatabase.createCharacterIDListFromNames(names.ToArray());

        characterIDs.Clear();
        characterIDs.AddRange(idList);

        this.characterSetName = characterSetName;
    }

    public void initFromIDList(string characterSetName, List<int> ids)
    {
        characterIDs.Clear();
        characterIDs.AddRange(ids);

        this.characterSetName = characterSetName;
    }

    public string toJSONString()
    {
        string[] nameList = getAsNameList().ToArray();

        return JsonUtility.ToJson(nameList);
    }

    public void loadFromJSONString(string characterSetName, string JSONString)
    {
        JSONNode node = JSONNode.Parse(JSONString);
        
        List<string> nameStrings = new List<string>();
        foreach (var value in node.Values)
        {
            nameStrings.Add(value.ToString());
        }

        initFromNameList(characterSetName, nameStrings);
    }

    public FilteredCharacterSetManager.CharacterSetSaveFormat toCharacterSetSaveFormat()
    {
        FilteredCharacterSetManager.CharacterSetSaveFormat result = new FilteredCharacterSetManager.CharacterSetSaveFormat();
        result.characterSetName = characterSetName;
        result.characterNames = new List<string>(getAsNameList());
        return result;
    }

    public void setName(string name)
    {
        characterSetName = name;
    }

    public string getName()
    {
        return characterSetName;
    }
}
