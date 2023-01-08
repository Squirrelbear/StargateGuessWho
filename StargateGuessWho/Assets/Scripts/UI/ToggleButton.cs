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
        {
            return;
        }

        CharacterButton characterButton = null;
        foreach (CharacterButton button in characterButtons)
        {
            if (selected.CharacterID == button.getGridID())
            {
                characterButton = button; 
                break;
            }
        }

        if (characterButton == null)
        {
            return;
        }

        characterButton.SetEliminated(!characterButton.IsEliminated);
        coreGameScreenRef.HandleSelectionToggle(selected.CharacterID, !characterButton.IsEliminated);
    }
}
