using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(menuName = "Character Collection")]
public class CharacterCollectionFile : ScriptableObject
{
    public List<string> characterPaths;

    public string collectionFilePath;

    public string setName;

    public bool initFromFile(string filePath)
    {
        collectionFilePath = filePath;

        characterPaths.Clear();

        string path = Application.persistentDataPath + "/CharacterSets/" + filePath + ".charset";
        StreamReader reader = new StreamReader(path);

#nullable enable
        string? line;

        if ((line = reader.ReadLine()) != null)
        {
            setName = line.Trim();
        }
        else
        {
            return false;
        }

        while ((line = reader.ReadLine()) != null)
        {
            line = line.Trim();

            if (line.Length > 0)
            {
                characterPaths.Add(line);
            }
        }
#nullable disable

        reader.Close();

        if (characterPaths.Count != 20)
        {
            return false;
        }

        return true;
    }

    public bool writeToFile()
    {
        if (collectionFilePath.Length == 0 || characterPaths.Count != 20 || setName.Length == 0)
        {
            return false;
        }

        string path = Application.persistentDataPath + "/CharacterSets/" + collectionFilePath + ".charset";
        StreamWriter writer = new StreamWriter(path, false);

        writer.WriteLine(setName);

        for (int i = 0; i < 20; i++)
        {
            writer.WriteLine(characterPaths[i]);
        }

        writer.Close();

        return true;
    }
}
