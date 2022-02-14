using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;

public class CoreGameScreen : MonoBehaviour
{
    [SerializeField]
    private GameStateManager gameStateManagerRef;

    [SerializeField]
    private NetworkManager networkManagerRef;

    [SerializeField]
    private Text otherPlayerStatusText;

    [SerializeField]
    private GameObject validatingStatusText;

    [SerializeField]
    private GameObject failedGuessTipText;

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
                // Count the number of characters still available to choose for the other player.
                if (player0["name"] == networkManagerRef.playerName)
                {
                    UpdateOtherHiddenNumber(player1);

                    failedGuessTipText.SetActive(player0["guessID"] != -1 && player0["guessID"] != player1["chosenID"]);
                    validatingStatusText.SetActive(validatingStatusText.activeSelf && player0["guessID"] == -1);
                }
                else
                {
                    UpdateOtherHiddenNumber(player0);

                    failedGuessTipText.SetActive(player1["guessID"] != -1 && player1["guessID"] != player0["chosenID"]);
                    validatingStatusText.SetActive(validatingStatusText.activeSelf && player1["guessID"] == -1);
                }

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

    private void UpdateOtherHiddenNumber(JSONNode data)
    {
        int count = 0;
        foreach(var charState in data["characterStates"])
        {
            if(!charState.Value["isUp"].AsBool)
            {
                count++;
            }
        }
        otherPlayerStatusText.text = count + "/" + data["characterStates"].Count;
    }
}
