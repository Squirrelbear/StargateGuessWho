using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class NetworkManager : MonoBehaviour
{
    public string playerAuth;
    public string sessionCode;
    public string playerName;

    public delegate void ServerResponseEvent(NetworkMessage.MessageTemplate request, JSONNode result, bool isError, string errorMessage);
    public static event ServerResponseEvent OnServerResponse;


    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(GetWebData("http://localhost:7000", "?action=createPlayer&playerName=Peter"));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreatePlayerOnServer(string playerName)
    {
        this.playerName = playerName;
        var message = new NetworkMessage.CreatePlayerMessage(playerName);
        StartCoroutine(GetWebData("http://localhost:7000", message));
    }

    public void CreateSessionOnServer()
    {
        var message = new NetworkMessage.CreateServerMessage(playerAuth);
        StartCoroutine(GetWebData("http://localhost:7000", message));
    }

    public void JoinSessionOnServer(string sessionCode)
    {
        this.sessionCode = sessionCode;
        var message = new NetworkMessage.JoinServerMessage(playerAuth, sessionCode);
        StartCoroutine(GetWebData("http://localhost:7000", message));
    }

    public void StartSessionOnServer()
    {
        var message = new NetworkMessage.StartRoundMessage(playerAuth, sessionCode);
        StartCoroutine(GetWebData("http://localhost:7000", message));
    }

    public void SendCharacterCommand(int characterID, string command)
    {
        var message = new NetworkMessage.UpdateSelectionMessage(playerAuth, sessionCode, characterID, command);
        StartCoroutine(GetWebData("http://localhost:7000", message));
    }

    public void GetSessionState()
    {
        var message = new NetworkMessage.GetStateMessage(playerAuth, sessionCode);
        StartCoroutine(GetWebData("http://localhost:7000", message));
    }

    void ProcessServerResponse(string rawResponse, NetworkMessage.MessageTemplate requestedMessage)
    {
        JSONNode node = JSONNode.Parse(rawResponse);
        if(node.HasKey("error"))
        {
            Debug.Log("ERROR: " + node["error"]);
            // TODO: Maybe resend the request?
            return;
        } 
        else if(requestedMessage is NetworkMessage.CreatePlayerMessage)
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
        } 
        else
        {
            //Debug.Log(www.downloadHandler.text);

            ProcessServerResponse(www.downloadHandler.text, message);
        }
    }
}
