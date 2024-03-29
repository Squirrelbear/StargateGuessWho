using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterButton : MonoBehaviour
{
    public bool IsEliminated { get; private set; }

    [SerializeField]
    private int buttonID;

    private Image imageRef;
    private GameObject childText;
    private Button buttonRef;
    private SelectedCharacter targetCharacter;
    [SerializeField]
    private string characterName = "Error Unknown";

    // Start is called before the first frame update
    void Start()
    {
        targetCharacter = GameObject.Find("SelectedCharacterFrame").GetComponent<SelectedCharacter>();
    }

    private void OnEnable()
    {
        if(buttonRef == null)
        {
            buttonRef = GetComponent<Button>();
            imageRef = GetComponent<Image>();
            childText = transform.GetChild(0).gameObject;
        }

        buttonRef.onClick.AddListener(HandleClick);
        SetEliminated(false);
    }

    private void OnDisable()
    {
        buttonRef.onClick.RemoveListener(HandleClick);
    }

    public void HandleClick()
    {
        targetCharacter.SetSelectedCharacter(buttonID, imageRef.sprite, characterName);
    }

    public Sprite getSprite()
    {
        return imageRef.sprite;
    }

    public string getCharacterName()
    {
        return characterName;
    }

    public int getGridID()
    {
        return buttonID;
    }

    public void setToCharacter(Character character)
    {
        Image imageRef = GetComponent<Image>();
        imageRef.sprite = character.characterSprite;
        characterName = character.characterName;
    }

    public void SetEliminated(bool isEliminated)
    {
        this.IsEliminated = isEliminated;
        if(isEliminated)
        {
            imageRef.color = new Color(0.4f, 0.4f, 0.4f);
        } 
        else
        {

            imageRef.color = new Color(1, 1, 1);
            
        }
        childText.SetActive(isEliminated);
        //Debug.Log("Active: " + childText.activeSelf);
    }
}
