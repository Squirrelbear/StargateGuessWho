using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterButton : MonoBehaviour
{
    public int buttonID;

    private Image imageRef;
    private bool isEliminated;
    private GameObject childText;
    private Button buttonRef;
    private NetworkManager networkManager;
    private Image targetCharacter;

    // Start is called before the first frame update
    void Start()
    {
        imageRef = GetComponent<Image>();
        childText = transform.GetChild(0).gameObject;
        networkManager = FindObjectOfType<NetworkManager>();
        targetCharacter = GameObject.Find("YourCharacterSelection").GetComponent<Image>();
    }

    private void OnEnable()
    {
        if(buttonRef == null)
        {
            buttonRef = GetComponent<Button>();
        }

        buttonRef.onClick.AddListener(HandleClick);
    }

    private void OnDisable()
    {
        buttonRef.onClick.RemoveListener(HandleClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleClick()
    {
        Debug.Log("Test" + buttonID);
        SetEliminated(!isEliminated);
        networkManager.SendCharacterCommand(buttonID, isEliminated ? "setDown" : "setUp");
        targetCharacter.sprite = imageRef.sprite;
    }

    public void SetEliminated(bool isEliminated)
    {
        this.isEliminated = isEliminated;
        if(isEliminated)
        {
            imageRef.color = new Color(0.4f, 0.4f, 0.4f);
        } 
        else
        {

            imageRef.color = new Color(1, 1, 1);
            
        }
        childText.SetActive(isEliminated);
        Debug.Log("Active: " + childText.activeSelf);
    }
}
