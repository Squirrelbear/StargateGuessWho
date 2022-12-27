using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class CharacterTest : MonoBehaviour
{
    [SerializeField]
    private Character[] characters = new Character[0];

#if UNITY_EDITOR
    private void OnValidate()
    {
        characters = LoadAllCharacters();
    }

    private static Character[] LoadAllCharacters()
    {
        string[] guids = AssetDatabase.FindAssets("", new[] { "Assets/Data/Characters" });
        int count = guids.Length;
        Character[] characters = new Character[count];
        for (int n = 0; n < count; n++)
        {
            var path = AssetDatabase.GUIDToAssetPath(guids[n]);
            characters[n] = AssetDatabase.LoadAssetAtPath<Character>(path);
            
        }

        //var testPathGUID = AssetDatabase.AssetPathToGUID("Assets/Data/Characters/AcastusKolya.asset");
        //var pathTest = AssetDatabase.GUIDToAssetPath(testPathGUID);
        //Debug.Log(pathTest);

        return characters;
    }
#endif
}
