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

    private void Start()
    {
        loadFromPlayerPrefs();
    }

    [System.Serializable]
    public class CharacterSetSaveFormat
    {
        public string characterSetName;
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

    private void setToDefaultCharacterSets()
    {
        FilteredCharacterSet originalDefault = new FilteredCharacterSet(database);
        string[] originalDefaultNames = CharacterDatabase.getDefaultCharacterCollectionNames();
        originalDefault.initFromNameList("Simple SG1", new List<string>(originalDefaultNames));
        filteredCharacterSets.Add(originalDefault);

        FilteredCharacterSet filteredSG1Only = new FilteredCharacterSet(database, SeriesTag.SG1);
        filteredCharacterSets.Add(filteredSG1Only);

        FilteredCharacterSet filteredSGAOnly = new FilteredCharacterSet(database, SeriesTag.SGA);
        filteredCharacterSets.Add(filteredSGAOnly);

        FilteredCharacterSet filteredSGUOnly = new FilteredCharacterSet(database, SeriesTag.SGU);
        filteredCharacterSets.Add(filteredSGUOnly);

        FilteredCharacterSet filteredAntagonistOnly = new FilteredCharacterSet(database, true);
        filteredCharacterSets.Add(filteredAntagonistOnly);

        FilteredCharacterSet filteredNotAntagonistOnly = new FilteredCharacterSet(database, false);
        filteredCharacterSets.Add(filteredNotAntagonistOnly);
    }

#if UNITY_EDITOR
    public bool testLoadButton = false;
    public bool testSaveButton = false;
    public bool testDeleteSaveButton = false;

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
    }

#endif
}
