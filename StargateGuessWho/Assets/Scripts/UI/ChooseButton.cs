using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseButton : MonoBehaviour
{
    private SelectedCharacter selected, yourCharacter;
    private Button buttonRef;

    // Start is called before the first frame update
    void Start()
    {
        buttonRef = GetComponent<Button>();
    }

    private void OnEnable()
    {
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
    }

    public void HandleSelectionChanged(int newCharacterID)
    {
        buttonRef.interactable = (newCharacterID != -1);
    }
}
