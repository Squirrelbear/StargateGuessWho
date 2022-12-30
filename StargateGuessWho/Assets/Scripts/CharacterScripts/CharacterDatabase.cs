using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class CharacterDatabase : MonoBehaviour
{
    [SerializeReference]
    private List<Character> characters;

    [SerializeReference]
    private CharacterCollection currentCollection;

    [SerializeReference]
    private FilteredCharacterSet currentFilterSet;

    [SerializeReference]
    private List<CharacterButton> characterButtons;

    public List<Character> getAllCharacters()
    {
        return characters;
    }

    public CharacterCollection createCharacterCollectionFromIDs(int[] ids, bool allowRandom = true)
    {
        Debug.Assert(ids != null);
        Debug.Assert(ids.Length >= 20);

        List<int> idsCopy = new List<int>(ids);
        if (allowRandom)
        {
            idsCopy = generateRandomCharacterIDs(idsCopy);
        }

        CharacterCollection collection = new CharacterCollection();

        for (int i = 0; i < idsCopy.Count; i++)
        {
            Debug.Assert(idsCopy[i] >= 0 && idsCopy[i] < characters.Count);

            CharacterDatabaseEntry databaseEntry = new CharacterDatabaseEntry();
            databaseEntry.characterID = idsCopy[i];
            databaseEntry.character = characters[idsCopy[i]];

            collection.characters.Add(databaseEntry);
        }

        return collection;
    }

    public int[] createCharacterIDListFromNames(string[] names)
    {
        Debug.Assert(names != null);

        int[] resultIDs = new int[names.Length];

        for (int i = 0; i < resultIDs.Length; i++)
        {
            resultIDs[i] = findCharacterIDForName(names[i]);
        }

        return resultIDs;
    }

    public CharacterCollection createCharacterCollectionFromNames(string[] names)
    {
        Debug.Assert(names != null);
        Debug.Assert(names.Length >= 20);

        int[] characterIDs = createCharacterIDListFromNames(names);

        CharacterCollection collection = createCharacterCollectionFromIDs(characterIDs);

        return collection;
    }

    public int findCharacterIDForName(string name)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].name == name)
            {
                return i;
            }
        }

        return -1;
    }

    public List<int> generateRandomCharacterIDs(List<int> validIDs)
    {
        List<int> resultIDs = new List<int>(validIDs);
        int index = 0;

        while (index < resultIDs.Count)
        {
            int randomIndex = Random.Range(index, resultIDs.Count);
            (resultIDs[randomIndex], resultIDs[index]) = (resultIDs[index], resultIDs[randomIndex]);
            index++;
        }

        return resultIDs.Take(20).ToList();
    }

    public List<int> getListOfAllValidCharacterIDs()
    {
        List<int> resultIDs = new List<int>();
        for (int i = 0; i < characters.Count; i++)
        {
            resultIDs.Add(i);
        }

        return resultIDs;
    }

    public void makeCurrentCharacterCollectionReal()
    {
        if (currentCollection == null)
        {
            Debug.LogAssertion("Failed to set characters because collection does not exist.");
            return;
        }

        if (currentCollection.hasValidCollection(out string error) == false)
        {
            Debug.LogAssertionFormat("Invalid collection! {0}", error);
            return;
        }

        if (characterButtons == null || characterButtons.Count != 20)
        {
            Debug.LogAssertion("Invalid character buttons!");
            return;
        }

        foreach (CharacterButton button in characterButtons)
        {
            Character character = currentCollection.characters[button.getGridID()].character;
            button.setToCharacter(character);
        }

        Debug.LogFormat("Chars Set To: {0}", currentCollection.toHexIDString());
    }

    public static string[] getDefaultCharacterCollectionNames()
    {
        string[] result = new string[20];

        result[0] = "Anubis";
        result[1] = "Baal";
        result[2] = "Bratac";
        result[3] = "DanielJackson";
        result[4] = "Fifth";
        result[5] = "GeneralHammond";
        result[6] = "GeneralLandry";
        result[7] = "GeneralWest";
        result[8] = "Hathor";
        result[9] = "JackOneill";
        result[10] = "JanetFraiser";
        result[11] = "JonasQuinn";
        result[12] = "LordYu";
        result[13] = "Nirrti";
        result[14] = "PriorDamaris";
        result[15] = "RodneyMcKaySG1";
        result[16] = "SamanthaCarter";
        result[17] = "SelmakJacobCarter";
        result[18] = "Tealc";
        result[19] = "ValaMalDoran";

        return result;
    }

    public List<int> getIDsMatchingAnyTag(CharacterTag[] tags)
    {
        List<int> result = new List<int>();
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].hasAnyTag(tags))
            {
                result.Add(i);
            }
        }
        return result;
    }

    public List<int> getIDsMatchingSeries(SeriesTag series)
    {
        List<int> result = new List<int>();
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].seriesTag == series)
            {
                result.Add(i);
            }
        }
        return result;
    }

    public List<int> getIDsMatchingRace(RaceTag raceTag)
    {
        List<int> result = new List<int>();
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].raceTag == raceTag)
            {
                result.Add(i);
            }
        }
        return result;
    }

    public List<int> getIDsMatchingAntagonist(bool isAntagonist)
    {
        List<int> result = new List<int>();
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].isAntagonist == isAntagonist)
            {
                result.Add(i);
            }
        }
        return result;
    }

    public List<int> collectIDsMatchingCondition(List<int> characterIDs, CharacterTag[] tags, bool matchAny)
    {
        List<int> result = new List<int>();
        foreach (int characterID in characterIDs)
        {
            if (characters[characterID].hasAnyTag(tags) == matchAny)
            {
                result.Add(characterID);
            }
        }
        return result;
    }

    public List<int> collectIDsMatchingCondition(List<int> characterIDs, SeriesTag seriesTag, bool matchAny)
    {
        List<int> result = new List<int>();
        foreach (int characterID in characterIDs)
        {
            if (matchAny && characters[characterID].seriesTag == seriesTag)
            {
                result.Add(characterID);
            }
            else if (matchAny == false && characters[characterID].seriesTag != seriesTag)
            {
                result.Add(characterID);
            }
        }
        return result;
    }

    public List<int> collectIDsMatchingCondition(List<int> characterIDs, RaceTag raceTag, bool matchAny)
    {
        List<int> result = new List<int>();
        foreach (int characterID in characterIDs)
        {
            if (matchAny && characters[characterID].raceTag == raceTag)
            {
                result.Add(characterID);
            }
            else if (matchAny == false && characters[characterID].raceTag != raceTag)
            {
                result.Add(characterID);
            }
        }
        return result;
    }

    public List<int> collectIDsMatchingCondition(List<int> characterIDs, bool isAntagonist)
    {
        List<int> result = new List<int>();
        foreach (int characterID in characterIDs)
        {
            if (characters[characterID].isAntagonist == isAntagonist)
            {
                result.Add(characterID);
            }
        }
        return result;
    }

    public void setCurrentFilterSet(FilteredCharacterSet filteredCharacterSet)
    {
        this.currentFilterSet = filteredCharacterSet;
    }

    public string generateNextCharacterCollectionFromFilterSet()
    {
        if (currentFilterSet == null)
        {
            // Probably not the host, just abort.
            return "";
        }

        currentCollection = currentFilterSet.generateCharacterCollection();

        makeCurrentCharacterCollectionReal();

        return currentCollection.toHexIDString();
    }

    public void setCharacterCollectionFromHex(string hexdata)
    {
        if (currentCollection != null)
        {
            string currentCollectionHex = currentCollection.toHexIDString();
            if (currentCollectionHex == hexdata)
            {
                return;
            }
        }

        int[] characterIDs = CharacterCollection.hexStringToCharacterIDList(hexdata);
        // Prevent the random reordering because this is sent via the server as a fixed order
        currentCollection = createCharacterCollectionFromIDs(characterIDs, false);

        makeCurrentCharacterCollectionReal();
    }

#if UNITY_EDITOR
    private void testLoadDefaultCharacterCollection()
    {
        string[] defaultCharacters = getDefaultCharacterCollectionNames();

        int[] characterIDs = createCharacterIDListFromNames(defaultCharacters);

        currentCollection = createCharacterCollectionFromIDs(characterIDs, false);
    }

    private void testRandomiseCharacterCollection()
    {
        List<int> allValidIDs = getListOfAllValidCharacterIDs();
        List<int> randomSelectionOnly = generateRandomCharacterIDs(allValidIDs);
        int[] characterIDs = randomSelectionOnly.ToArray();
        currentCollection = createCharacterCollectionFromIDs(characterIDs);
    }


    private void reloadCharacterList()
    {
        characters = LoadAllCharacters();
    }

    private static List<Character> LoadAllCharacters()
    {
        string[] guids = AssetDatabase.FindAssets("", new[] { "Assets/Data/Characters" });
        List<Character> characters = new List<Character>();
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            Character character = AssetDatabase.LoadAssetAtPath<Character>(path);

            characters.Add(character);
        }

        return characters;
    }

    public bool loadAllCharactersButton = false;
    public bool testInitDefaultCharacterCollection = false;
    public bool testConvertCurrentCollectionButton = false;
    public bool loadCurrentCharactersAsDefault = false;
    public bool reverseCharacterCollectionTest = false;
    public bool generateRandomCollection = false;

    void OnValidate()
    {
        if (loadAllCharactersButton)
        {
            reloadCharacterList();

            loadAllCharactersButton = false;
        }

        if (testInitDefaultCharacterCollection)
        {
            testLoadDefaultCharacterCollection();

            testInitDefaultCharacterCollection = false;
        }

        if (testConvertCurrentCollectionButton)
        {
            currentCollection.testConvert();

            testConvertCurrentCollectionButton = false;
        }

        if (loadCurrentCharactersAsDefault)
        {
            makeCurrentCharacterCollectionReal();

            loadCurrentCharactersAsDefault = false;
        }

        if (reverseCharacterCollectionTest)
        {
            currentCollection.characters.Reverse();

            reverseCharacterCollectionTest = false;
        }

        if (generateRandomCollection)
        {
            testRandomiseCharacterCollection();

            generateRandomCollection = false;
        }
    }
#endif
}

// Wrapper to hold database entries with their character ID
[System.Serializable]
public class CharacterDatabaseEntry
{
    public int characterID;
    public Character character;
}
