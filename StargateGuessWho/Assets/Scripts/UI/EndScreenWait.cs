using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class EndScreenWait : MonoBehaviour
{
    [SerializeField]
    private GameStateManager gameStateManagerRef;

    [SerializeField]
    private NetworkManager networkManagerRef;

    private void OnEnable()
    {
        NetworkManager.OnServerResponse += HandleNetworkResponse;
    }

    private void OnDisable()
    {
        NetworkManager.OnServerResponse -= HandleNetworkResponse;
        CancelInvoke();
    }

    public void BeginWaitForHandshake()
    {
        InvokeRepeating("requestSessionState", 1, 0.5f);
    }

    private void requestSessionState()
    {
        networkManagerRef.GetSessionState();
    }

    private void HandleNetworkResponse(NetworkMessage.MessageTemplate request, JSONNode result, bool isError, string errorMessage)
    {
        if (request is NetworkMessage.GetStateMessage)
        {
            if (networkManagerRef.gameNum != result[0]["gameNum"])
            {
                networkManagerRef.gameNum = result[0]["gameNum"];
                gameStateManagerRef.TransitionToChooseCharacter();
            }
        }
    }
}
