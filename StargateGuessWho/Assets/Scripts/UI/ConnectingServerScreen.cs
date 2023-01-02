using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;

public class ConnectingServerScreen : MonoBehaviour
{
    [SerializeField]
    private GameStateManager gameStateManagerRef;

    [SerializeField]
    private NetworkManager networkManagerRef;

    [SerializeField]
    private FilteredCharacterSetManager characterSetManagerRef;

    [SerializeField]
    private CharacterDatabase characterDatabaseRef;

    [SerializeField]
    private Text sessionCodeTextObject;

    [SerializeField]
    private InputField nameInputField;

    [SerializeField]
    private InputField sessionCodeInputField;

    [SerializeField]
    private bool joiningAsHost;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        NetworkManager.OnServerResponse += HandleNetworkResponse;
    }

    private void OnDisable()
    {
        NetworkManager.OnServerResponse -= HandleNetworkResponse;
        CancelInvoke();
    }

    private void HandleNetworkResponse(NetworkMessage.MessageTemplate request, JSONNode result, bool isError, string errorMessage)
    {
        if (request is NetworkMessage.CreatePlayerMessage)
        {
            if (joiningAsHost)
            {
                Debug.Log("Player created... Creating Session...");
                networkManagerRef.CreateSessionOnServer();
            } 
            else
            {
                Debug.Log("Player created... Joining Session...");
                networkManagerRef.JoinSessionOnServer(sessionCodeInputField.text);
            }
        } 
        else if (request is NetworkMessage.CreateServerMessage)
        {
            Debug.Log("Session created... Passing character set.");
            sessionCodeTextObject.text = result["sessionCode"];

            characterSetManagerRef.setCharacterSetFromDropdown();
            string hexData = characterDatabaseRef.generateNextCharacterCollectionFromFilterSet();

            networkManagerRef.SendHostCharacterSetChange(hexData);
        }
        else if (request is NetworkMessage.SetCharacterCollectionMessage)
        {
            Debug.Log("Character set confirmed. Moving to Wait for Other.");
            gameStateManagerRef.TransitionToWaitingForPlayer();
            InvokeRepeating("requestSessionState", 1, 0.5f);
        }
        else if (request is NetworkMessage.JoinServerMessage)
        {
            Debug.Log("Session joined...");
            string hexData = result["characterCollection"];
            characterDatabaseRef.setCharacterCollectionFromHex(hexData);
            // TODO need to collect data about other player
            gameStateManagerRef.TransitionToChooseCharacter();
        }
        else if (request is NetworkMessage.GetStateMessage)
        {
            Debug.Log("Checking server...");
            // Wait until there are two player objects and a config object
            if(result.AsArray.Count == 3)
            {
                string hexData = result[2]["characterCollection"];
                characterDatabaseRef.setCharacterCollectionFromHex(hexData);

                // TODO need to collect data about other player 
                gameStateManagerRef.TransitionToChooseCharacter();
            }
        }
    }

    private void requestSessionState()
    {
        networkManagerRef.GetSessionState();
    }

    public void BeginJoinSession()
    {
        joiningAsHost = false;
        characterDatabaseRef.setCurrentFilterSet(null);
        networkManagerRef.CreatePlayerOnServer(nameInputField.text);
    }

    public void BeginCreateSession()
    {
        joiningAsHost = true;
        networkManagerRef.CreatePlayerOnServer(nameInputField.text);
    }
}
