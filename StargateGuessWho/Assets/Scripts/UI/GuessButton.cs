using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuessButton : MonoBehaviour
{
    [SerializeField]
    private CoreGameScreen coreGameRef;

    private SelectedCharacter selected;
    private Button buttonRef;

    private void OnEnable()
    {
        buttonRef = GetComponent<Button>();
        selected = GameObject.Find("SelectedCharacterFrame").GetComponent<SelectedCharacter>();

        selected.onCharacterChanged += HandleSelectionChanged;
    }

    public void OnDisable()
    {
        selected.onCharacterChanged -= HandleSelectionChanged;
    }

    public void HandleButtonPress()
    {
        if (selected.CharacterID != -1)
        {
            coreGameRef.HandleSelectionGuess(selected.CharacterID);
        }
    }

    public void HandleSelectionChanged(int newCharacterID)
    {
        buttonRef.interactable = (newCharacterID != -1);
    }
}
