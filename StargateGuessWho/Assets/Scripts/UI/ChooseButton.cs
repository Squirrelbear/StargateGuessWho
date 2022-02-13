using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseButton : MonoBehaviour
{
    [SerializeField]
    private CoreGameScreen coreGameRef;

    private SelectedCharacter selected, yourCharacter;
    private Button buttonRef;

    private void OnEnable()
    {
        buttonRef = GetComponent<Button>();
        selected = GameObject.Find("SelectedCharacterFrame").GetComponent<SelectedCharacter>();
        yourCharacter = GameObject.Find("YourCharacterSelection").GetComponent<SelectedCharacter>();

        selected.onCharacterChanged += HandleSelectionChanged;
    }

    public void OnDisable()
    {
        selected.onCharacterChanged -= HandleSelectionChanged;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleButtonPress()
    {
        yourCharacter.SetSelectedCharacter(selected.CharacterID, selected.GetComponent<Image>().sprite);
        if(selected.CharacterID != -1)
        {
            coreGameRef.HandleSelectionChoice(selected.CharacterID);
        }
    }

    public void HandleSelectionChanged(int newCharacterID)
    {
        buttonRef.interactable = (newCharacterID != -1);
    }
}
