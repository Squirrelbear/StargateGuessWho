using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class FilteredCharacterSetManager : MonoBehaviour
{
    [SerializeReference]
    private List<FilteredCharacterSet> filteredCharacterSets = new List<FilteredCharacterSet>();

    [SerializeField]
    private CharacterDatabase database;

    [SerializeField]
    private UnityEngine.UI.Dropdown filteredSetManagerDropdown;

    private void Start()
    {
        loadFromPlayerPrefs();
    }

    [System.Serializable]
    public class CharacterSetSaveFormat
    {
        public string characterSetName;
        public bool isCustom;
        public List<string> characterNames;
    }

    [System.Serializable]
    public class SaveDataFormat
    {
        public List<CharacterSetSaveFormat> characterSets;
    }

    public void loadFromPlayerPrefs()
    {
        filteredCharacterSets.Clear();

        if (PlayerPrefs.HasKey("charactersets") == false)
        {

            setToDefaultCharacterSets();

            return;
        }

        string rawString = PlayerPrefs.GetString("charactersets");
        SaveDataFormat loadedData = JsonUtility.FromJson<SaveDataFormat>(rawString);
        foreach (var characterSet in loadedData.characterSets)
        {
            filteredCharacterSets.Add(new FilteredCharacterSet(database, characterSet));
        }
    }

    public void clearSavedPlayerPrefs()
    {
        PlayerPrefs.DeleteKey("charactersets");
    }

    public void saveToPlayerPrefs()
    {
        SaveDataFormat saveData = new SaveDataFormat();
        saveData.characterSets = new List<CharacterSetSaveFormat>();

        foreach(var characterSet in filteredCharacterSets)
        {
            saveData.characterSets.Add(characterSet.toCharacterSetSaveFormat());
        }

        PlayerPrefs.SetString("charactersets", JsonUtility.ToJson(saveData));
    }

    public void populateHostDropdown()
    {
        filteredSetManagerDropdown.ClearOptions();

        List<string> currentOptions = new List<string>();
        foreach (var characterSet in filteredCharacterSets)
        {
            currentOptions.Add(characterSet.getName());
        }

        filteredSetManagerDropdown.AddOptions(currentOptions);
    }

    public void setCharacterSetFromDropdown()
    {
        FilteredCharacterSet setToUse = filteredCharacterSets[filteredSetManagerDropdown.value];
        database.setCurrentFilterSet(setToUse);
    }

    private void setToDefaultCharacterSets()
    {
        FilteredCharacterSet originalDefault = new FilteredCharacterSet(database, false);
        string[] originalDefaultNames = CharacterDatabase.getDefaultCharacterCollectionNames();
        originalDefault.initFromNameList("Simple SG1", new List<string>(originalDefaultNames));
        filteredCharacterSets.Add(originalDefault);

        FilteredCharacterSet everything = new FilteredCharacterSet(database, database.getListOfAllValidCharacterIDs(), "All Characters", false);
        filteredCharacterSets.Add(everything);

        FilteredCharacterSet filteredSG1Only = new FilteredCharacterSet(database, SeriesTag.SG1, false);
        filteredCharacterSets.Add(filteredSG1Only);

        FilteredCharacterSet filteredSGAOnly = new FilteredCharacterSet(database, SeriesTag.SGA, false);
        filteredCharacterSets.Add(filteredSGAOnly);

        FilteredCharacterSet filteredSGUOnly = new FilteredCharacterSet(database, SeriesTag.SGU, false);
        filteredCharacterSets.Add(filteredSGUOnly);

        FilteredCharacterSet filteredAntagonistOnly = new FilteredCharacterSet(database, true, false);
        filteredCharacterSets.Add(filteredAntagonistOnly);

        FilteredCharacterSet filteredNotAntagonistOnly = new FilteredCharacterSet(database, false, false);
        filteredCharacterSets.Add(filteredNotAntagonistOnly);
    }

#if UNITY_EDITOR
    public bool testLoadButton = false;
    public bool testSaveButton = false;
    public bool testDeleteSaveButton = false;
    public int currentFilterSetIDToUse = 0;
    public bool testSetCurrentFilterSet = false;
    public bool clearCurrentFilterSet = false;
    public bool generateNextCharacterCollection = false;
    public string testHexValue = "";
    public bool testSetCharacterCollectionFromHex = false;

    private void OnValidate()
    {
        if (testLoadButton)
        {
            loadFromPlayerPrefs();
            testLoadButton = false;
        }

        if (testSaveButton)
        {
            saveToPlayerPrefs();
            testSaveButton = false;
        }

        if (testDeleteSaveButton)
        {
            clearSavedPlayerPrefs();
            testDeleteSaveButton = false;
        }

        if (testSetCurrentFilterSet)
        {
            database.setCurrentFilterSet(filteredCharacterSets[currentFilterSetIDToUse]);

            testSetCurrentFilterSet = false;
        }

        if (clearCurrentFilterSet)
        {
            database.setCurrentFilterSet(null);

            clearCurrentFilterSet = false;
        }

        if (generateNextCharacterCollection)
        {
            database.generateNextCharacterCollectionFromFilterSet();

            generateNextCharacterCollection = false;
        }

        if (testSetCharacterCollectionFromHex)
        {
            database.setCharacterCollectionFromHex(testHexValue);

            testSetCharacterCollectionFromHex = false;
        }
    }

#endif
}
