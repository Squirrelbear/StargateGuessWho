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
        if(request is NetworkMessage.CreatePlayerMessage)
        {
            if (joiningAsHost)
            {
                Debug.Log("Player created... Creating Session...");
                networkManagerRef.CreateSessionOnServer();
            } 
            else
            {
                Debug.Log("Player created... Joining Session...");
                // TODO join session
            }
        } 
        else if(request is NetworkMessage.CreateServerMessage)
        {
            Debug.Log("Session created... Moving to Wait for Other.");
            sessionCodeTextObject.text = result["sessionCode"];
            gameStateManagerRef.TransitionToWaitingForPlayer();
            InvokeRepeating("requestSessionState", 1, 0.5f);
        } 
        else if(request is NetworkMessage.JoinServerMessage)
        {
            // TODO
        }
        else if(request is NetworkMessage.GetStateMessage)
        {
            Debug.Log("Checking server...");
        }
    }

    private void requestSessionState()
    {
        networkManagerRef.GetSessionState();
    }

    public void BeginJoinSession()
    {
        joiningAsHost = false;
        networkManagerRef.CreatePlayerOnServer(nameInputField.text);
    }

    public void BeginCreateSession()
    {
        joiningAsHost = true;
        networkManagerRef.CreatePlayerOnServer(nameInputField.text);
    }
}
