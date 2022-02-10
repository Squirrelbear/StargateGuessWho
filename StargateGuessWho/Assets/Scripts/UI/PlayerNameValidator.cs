using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlayerNameValidator : MonoBehaviour
{
    [SerializeField]
    private Sprite crossSprite, tickSprite;
    [SerializeField]
    private InputField targetField;
    [SerializeField]
    private Button createSessionButton, joinSessionButton;
    [SerializeField]
    private SessionCodeValidator sessionCodeValidator;

    private Image imageRef;

    public bool IsValidated { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        imageRef = GetComponent<Image>();
        imageRef.color = new Color(1, 1, 1, 0);
        imageRef.sprite = null;
        IsValidated = false;
    }

    void OnEnable()
    {
        targetField.onValueChanged.AddListener(HandleNameChanged);
    }

    void OnDisable()
    {
        targetField.onValueChanged.RemoveListener(HandleNameChanged);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleNameChanged(string newName)
    {
        if(newName.Length == 0)
        {
            imageRef.color = new Color(1, 1, 1, 0);
            imageRef.sprite = null;
            createSessionButton.interactable = false;
            joinSessionButton.interactable = false;
            IsValidated = false;
            return;
        }
        imageRef.color = new Color(1, 1, 1, 1);
        if (newName.Length < 3 || newName.Length > 20 || !newName.All(char.IsLetterOrDigit))
        {
            imageRef.sprite = crossSprite;
            createSessionButton.interactable = false;
            joinSessionButton.interactable = false;
            IsValidated = false;
        }
        else
        {
            imageRef.sprite = tickSprite;
            createSessionButton.interactable = true;
            IsValidated = true;
            // Set the status based on the other validator.
            joinSessionButton.interactable = sessionCodeValidator.IsValidated;
        }
    }
}
