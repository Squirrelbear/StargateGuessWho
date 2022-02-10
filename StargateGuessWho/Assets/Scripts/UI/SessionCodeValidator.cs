using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SessionCodeValidator : MonoBehaviour
{
    [SerializeField]
    private Sprite crossSprite, tickSprite;
    [SerializeField]
    private InputField targetField;
    [SerializeField]
    private Button joinSessionButton;
    [SerializeField]
    private PlayerNameValidator playerNameValidator;

    private Image imageRef;

    public bool IsValidated { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        imageRef = GetComponent<Image>();
        imageRef.color = new Color(1, 1, 1, 0);
        imageRef.sprite = null;
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
        if (newName.Length == 0)
        {
            imageRef.color = new Color(1, 1, 1, 0);
            imageRef.sprite = null;
            joinSessionButton.interactable = false;
            IsValidated = false;
            return;
        }
        imageRef.color = new Color(1, 1, 1, 1);
        if (newName.Length != 5 || !newName.All(char.IsLetterOrDigit))
        {
            imageRef.sprite = crossSprite;
            joinSessionButton.interactable = false;
            IsValidated = false;
        }
        else
        {
            imageRef.sprite = tickSprite;
            IsValidated = true;
            // Set the status based on the other validator.
            joinSessionButton.interactable = playerNameValidator.IsValidated;
        }
    }
}
