using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{
    public string playerAuth;
    public string sessionCode;
    public string playerName;
    public int gameNum;
    public bool IsHost { get; set; }

    public delegate void ServerResponseEvent(NetworkMessage.MessageTemplate request, JSONNode result, bool isError, string errorMessage);
    public static event ServerResponseEvent OnServerResponse;

    [SerializeField]
    private GameObject errorOverlay;

    [SerializeField]
    private Text errorText;

    [SerializeField]
    private GameStateManager gameStateManager;

    [SerializeField]
    private string serverURL = "";

    // Start is called before the first frame update
    void Start()
    {
        setServerURL(PlayerPrefs.GetString("targetserver", ""));
    }

    public void setServerURL(string serverURL)
    {
        this.serverURL = serverURL;

        // Reset all the network properties back to default because they can't be trusted
        playerAuth = "";
        sessionCode = "";
        playerName = "";
        gameNum = 0;
    }

    public void CreatePlayerOnServer(string playerName)
    {
        gameNum = -1;
        this.playerName = playerName;
        var message = new NetworkMessage.CreatePlayerMessage(playerName);
        StartCoroutine(GetWebData(serverURL, message));
    }

    public void CreateSessionOnServer()
    {
        var message = new NetworkMessage.CreateServerMessage(playerAuth);
        StartCoroutine(GetWebData(serverURL, message));
    }

    public void JoinSessionOnServer(string sessionCode)
    {
        this.sessionCode = sessionCode;
        var message = new NetworkMessage.JoinServerMessage(playerAuth, sessionCode);
        StartCoroutine(GetWebData(serverURL, message));
    }

    public void StartSessionOnServer()
    {
        var message = new NetworkMessage.StartRoundMessage(playerAuth, sessionCode);
        StartCoroutine(GetWebData(serverURL, message));
    }

    public void SendCharacterCommand(int characterID, string command)
    {
        var message = new NetworkMessage.UpdateSelectionMessage(playerAuth, sessionCode, characterID, command);
        StartCoroutine(GetWebData(serverURL, message));
    }

    public void GetSessionState()
    {
        var message = new NetworkMessage.GetStateMessage(playerAuth, sessionCode);
        StartCoroutine(GetWebData(serverURL, message));
    }

    public void SendHostCharacterSetChange(string characterSet)
    {
        var message = new NetworkMessage.SetCharacterCollectionMessage(playerAuth, sessionCode, characterSet);
        StartCoroutine(GetWebData(serverURL, message));
    }

    void ProcessServerResponse(string rawResponse, NetworkMessage.MessageTemplate requestedMessage)
    {
        JSONNode node = JSONNode.Parse(rawResponse);
        if (node.HasKey("error"))
        {
            Debug.Log("ERROR: " + node["error"]);
            errorText.text = node["error"];
            errorOverlay.SetActive(true);
            gameStateManager.TransitionToStartScreen();
            // TODO: Maybe resend the request?
            return;
        }
        else if (requestedMessage is NetworkMessage.CreatePlayerMessage)
        {
            playerAuth = node["playerAuth"];
            //Debug.Log("Created player: " + playerAuth);
            // TODO trigger an event
            // TODO Remove this command after testing
            //CreateSessionOnServer();
        }
        else if (requestedMessage is NetworkMessage.CreateServerMessage)
        {
            sessionCode = node["sessionCode"];
            //Debug.Log("Created session: " + sessionCode);
            // TODO trigger an event
        }
        else if (requestedMessage is NetworkMessage.GetStateMessage)
        {
            if (gameNum == -1)
            {
                gameNum = node[0]["gameNum"];
            }
        }

        //Debug.Log("Name: " + node["playerName"] + "\n" + "Auth: " + node["playerAuth"]);
        OnServerResponse?.Invoke(requestedMessage, node, false, "");
    }

    IEnumerator GetWebData(string address, NetworkMessage.MessageTemplate message)
    {
        UnityWebRequest www = UnityWebRequest.Get(address + "?" + message.GetMessage());
        yield return www.SendWebRequest();

        if(www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Something went wrong: " + www.error);
            errorText.text = www.error;
            errorOverlay.SetActive(true);
            gameStateManager.TransitionToStartScreen();
        } 
        else
        {
            //Debug.Log(www.downloadHandler.text);

            ProcessServerResponse(www.downloadHandler.text, message);
        }
    }
}
