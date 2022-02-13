using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class CoreGameScreen : MonoBehaviour
{
    [SerializeField]
    private GameStateManager gameStateManagerRef;

    [SerializeField]
    private NetworkManager networkManagerRef;

    private void OnEnable()
    {
        NetworkManager.OnServerResponse += HandleNetworkResponse;
        InvokeRepeating("requestSessionState", 1, 0.5f);
    }

    private void OnDisable()
    {
        NetworkManager.OnServerResponse -= HandleNetworkResponse;
        CancelInvoke();
    }

    private void requestSessionState()
    {
        networkManagerRef.GetSessionState();
    }

    private void HandleNetworkResponse(NetworkMessage.MessageTemplate request, JSONNode result, bool isError, string errorMessage)
    {
        if (request is NetworkMessage.GetStateMessage)
        {
            JSONNode player0 = result[0];
            JSONNode player1 = result[1];

            if(gameStateManagerRef.gameState == GameStateManager.GameState.ChooseCharacter)
            {
                // If both players have selected characters
                if (player0["chosenID"] != -1 && player1["chosenID"] != -1)
                {
                    gameStateManagerRef.TransitionToCoreGame();
                }
            }
            else if(gameStateManagerRef.gameState == GameStateManager.GameState.CoreGame)
            {
                // Both players have made a guess
                if (player0["guessID"] != -1 && player1["guessID"] != -1)
                {
                    gameStateManagerRef.TransitionToGameEnded();
                }
            }
        }
    }

    public void HandleSelectionChoice(int characterID)
    {
        networkManagerRef.SendCharacterCommand(characterID, "choose");
    }

    public void HandleSelectionToggle(int characterID, bool isUp)
    {
        networkManagerRef.SendCharacterCommand(characterID, isUp ? "setUp" : "setDown");
    }

    public void HandleSelectionGuess(int characterID)
    {
        networkManagerRef.SendCharacterCommand(characterID, "guess");
    }
}
