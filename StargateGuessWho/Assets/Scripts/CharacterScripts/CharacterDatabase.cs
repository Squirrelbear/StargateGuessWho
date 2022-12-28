using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    private List<CharacterCollection> loadedCollections;

    [SerializeReference]
    private List<CharacterButton> characterButtons;

    public CharacterCollection createCharacterCollectionFromIDs(int[] ids)
    {
        Debug.Assert(ids != null);
        Debug.Assert(ids.Length == 20);

        CharacterCollection collection = new CharacterCollection();

        for (int i = 0; i < ids.Length; i++)
        {
            Debug.Assert(ids[i] >= 0 && ids[i] < characters.Count);

            CharacterDatabaseEntry databaseEntry = new CharacterDatabaseEntry();
            databaseEntry.characterID = ids[i];
            databaseEntry.character = characters[ids[i]];

            collection.characters.Add(databaseEntry);
        }

        return collection;
    }

    public int[] createCharacterIDListFromNames(string[] names)
    {
        Debug.Assert(names != null);
        Debug.Assert(names.Length == 20);

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
        Debug.Assert(names.Length == 20);

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

    public void makeCurrentCharacterCollectionReal()
    {
        if (currentCollection == null)
        {
            Debug.LogAssertion("Failed to set characters because collection does not exist.");
            return;
        }

        if (currentCollection.hasValidCollection() == false)
        {
            Debug.LogAssertion("Invalid collection!");
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

#if UNITY_EDITOR
    private void testLoadDefaultCharacterCollection()
    {
        string[] defaultCharacters = getDefaultCharacterCollectionNames();

        int[] characterIDs = createCharacterIDListFromNames(defaultCharacters);

        CharacterCollection collection = createCharacterCollectionFromIDs(characterIDs);

        currentCollection = collection;
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
