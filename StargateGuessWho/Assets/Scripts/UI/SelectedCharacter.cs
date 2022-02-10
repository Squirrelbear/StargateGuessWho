using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedCharacter : MonoBehaviour
{
    public delegate void OnCharacterChanged(int newCharacterID);
    public event OnCharacterChanged onCharacterChanged;

    [SerializeField]
    private Sprite defaultImage;

    public int CharacterID { get; private set; }

    private Image selectedImageRef;

    // Start is called before the first frame update
    void Start()
    {
        selectedImageRef = GetComponent<Image>();
        ResetSelectedCharacter();
    }

    public void SetSelectedCharacter(int characterID, Sprite characterImage)
    {
        this.CharacterID = characterID;
        selectedImageRef.sprite = characterImage;
        onCharacterChanged?.Invoke(CharacterID);
    }

    public void ResetSelectedCharacter()
    {
        SetSelectedCharacter(-1, defaultImage);
    }


}
