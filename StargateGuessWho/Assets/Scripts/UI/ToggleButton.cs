using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButton : MonoBehaviour
{
    [SerializeField]
    private CoreGameScreen coreGameScreenRef;

    [SerializeField]
    private SelectedCharacter selected;

    [SerializeField]
    private List<CharacterButton> characterButtons;

    public void handleClick()
    {
        if (selected.CharacterID == -1) 
            return;

        characterButtons[selected.CharacterID].SetEliminated(!characterButtons[selected.CharacterID].IsEliminated);
        coreGameScreenRef.HandleSelectionToggle(selected.CharacterID, !characterButtons[selected.CharacterID].IsEliminated);
    }
}
