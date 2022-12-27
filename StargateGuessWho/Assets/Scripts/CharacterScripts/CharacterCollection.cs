using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterCollection
{
    [SerializeReference]
    public List<CharacterDatabaseEntry> characters;

    public CharacterCollection()
    {
        characters = new List<CharacterDatabaseEntry>();
    }
}
