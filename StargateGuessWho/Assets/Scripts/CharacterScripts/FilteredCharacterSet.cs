using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilteredCharacterSet : MonoBehaviour
{
    [SerializeField]
    private CharacterDatabase characterDatabase;

    [SerializeField]
    private List<int> characterIDs;

    FilteredCharacterSet()
    {
        characterIDs = new List<int>();
    }

    void addIDs(List<int> characterIDsToAdd)
    {
        foreach (int characterID in characterIDsToAdd)
        {
            if (characterIDs.Contains(characterID) == false)
            {
                characterIDsToAdd.Add(characterID);
            }
        }
    }

    void removeIDs(List<int> characterIDToRemove)
    {
        characterIDs.RemoveAll(i => characterIDToRemove.Contains(i));
    }

    CharacterCollection generateCharacterCollection()
    {
        return characterDatabase.createCharacterCollectionFromIDs(characterIDs.ToArray());
    }

    List<string> getAsNameList()
    {
        List<string> nameList = new List<string>();
        List<Character> databaseEntries = characterDatabase.getAllCharacters();

        foreach (int characterID in characterIDs)
        {
            nameList.Add(databaseEntries[characterID].name);
        }

        return nameList;
    }

    void initFromNameList(List<string> names)
    {
        int[] idList = characterDatabase.createCharacterIDListFromNames(names.ToArray());

        characterIDs.Clear();
        characterIDs.AddRange(idList);
    }

    string toJSONString()
    {
        string[] nameList = getAsNameList().ToArray();

        return JsonUtility.ToJson(nameList);
    }

    void loadFromJSONString(string JSONString)
    {
        JSONNode node = JSONNode.Parse(JSONString);
        
        List<string> nameStrings = new List<string>();
        foreach (var value in node.Values)
        {
            nameStrings.Add(value.ToString());
        }

        initFromNameList(nameStrings);
    }
}
