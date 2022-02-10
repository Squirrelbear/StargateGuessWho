using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomChoiceButton : MonoBehaviour
{
    [SerializeField]
    private List<CharacterButton> characterButtons;

    public void HandleButtonClick()
    {
        CharacterButton randomButton = characterButtons[Random.Range(0, characterButtons.Count)];
        randomButton.HandleClick();
    }
}
