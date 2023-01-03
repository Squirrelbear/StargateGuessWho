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
    private CharacterDatabase characterDatabaseRef;

    [SerializeField]
    private Text otherPlayerStatusText;

    [SerializeField]
    private GameObject otherPlayerStatusPrefix;
    [SerializeField]
    private GameObject otherPlayerFailedGuess;

    [SerializeField]
    private GameObject validatingStatusText;

    [SerializeField]
    private GameObject failedGuessTipText;

    private int otherSolutionID, yourGuessID;

    [SerializeField]
    private Text endScreenPlayerNameYours;

    [SerializeField]
    private Text endScreenPlayerNameOther;

    [SerializeField]
    private SelectedCharacter endScreenChosenYours;

    [SerializeField]
    private SelectedCharacter endScreenChosenOther;

    [SerializeField]
    private SelectedCharacter endScreenGuessedYours;

    [SerializeField]
    private SelectedCharacter endScreenGuessedOther;

    [SerializeField]
    private Text endScreenYourScore;

    [SerializeField]
    private Text endScreenOtherScore;

    [SerializeField]
    private Text endScreenDrawsScore;

    [SerializeField]
    private Text endScreenOutcomeText;

    [SerializeField]
    private List<CharacterButton> characterButtons;

    private int yourScore = 0, otherScore = 0, draws = 0;

    [SerializeField]
    private GameObject endGameScreen;

    [SerializeField]
    private string cachedNextHexData = "";

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

            // Force update to the current character collection
            JSONNode settingsData = result[2];
            string hexData = settingsData["characterCollection"];
            if (hexData != cachedNextHexData)
            {
                Debug.Log("Hex data queued for next round.");
                cachedNextHexData = hexData;
            }

            if(gameStateManagerRef.gameState == GameStateManager.GameState.ChooseCharacter)
            {
                // If both players have selected characters
                if (player0["chosenID"] != -1 && player1["chosenID"] != -1)
                {
                    otherSolutionID = yourGuessID = -1;
                    gameStateManagerRef.TransitionToCoreGame();
                }
            }
            else if(gameStateManagerRef.gameState == GameStateManager.GameState.CoreGame)
            {
                // Count the number of characters still available to choose for the other player.
                if (player0["name"] == networkManagerRef.playerName)
                {
                    if (player1["guessID"] == -1)
                    {
                        UpdateOtherHiddenNumber(player1);
                    }
                    else if(!otherPlayerFailedGuess.activeSelf)
                    {
                        // TODO need to check if it was an actual fail
                        otherPlayerFailedGuess.SetActive(true);
                        otherPlayerStatusPrefix.SetActive(false);
                        otherPlayerStatusText.gameObject.SetActive(false);
                    }

                    otherSolutionID = player1["chosenID"];
                }
                else
                {
                    if (player0["guessID"] == -1)
                    {
                        UpdateOtherHiddenNumber(player0);
                    }
                    else if (!otherPlayerFailedGuess.activeSelf)
                    {
                        // TODO need to check if it was an actual fail
                        otherPlayerFailedGuess.SetActive(true);
                        otherPlayerStatusPrefix.SetActive(false);
                        otherPlayerStatusText.gameObject.SetActive(false);
                    }

                    otherSolutionID = player0["chosenID"];
                }

                // Both players have made a guess
                if ((player0["guessID"] != -1 && player1["guessID"] != -1))
                {
                    EndRound(player0["name"] == networkManagerRef.playerName ? player0 : player1,
                        player0["name"] == networkManagerRef.playerName ? player1 : player0);

                    // Update the game view ready for the next round
                    characterDatabaseRef.setCharacterCollectionFromHex(cachedNextHexData);
                }
            }
        } 
        else if(request is NetworkMessage.UpdateSelectionMessage)
        {
            if ((request as NetworkMessage.UpdateSelectionMessage).CharacterAction == "guess")
            {
                validatingStatusText.SetActive(false);
                if (yourGuessID != otherSolutionID)
                {
                    failedGuessTipText.SetActive(true);
                }

                if (networkManagerRef.IsHost)
                {
                    string hexData = characterDatabaseRef.generateNextCharacterCollectionFromFilterSet(false);
                    networkManagerRef.SendHostCharacterSetChange(hexData);
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
        yourGuessID = characterID;
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

    CharacterButton getCharacterButtonForID(int id)
    {
        foreach (CharacterButton button in characterButtons)
        {
            if (button.getGridID() == id)
            {
                return button;
            }
        }

        return null;
    }

    private void EndRound(JSONNode yourPlayer, JSONNode otherPlayer)
    {
        endGameScreen.SetActive(true);

        endScreenPlayerNameYours.text = yourPlayer["name"];
        endScreenPlayerNameOther.text = otherPlayer["name"];

        int yourChosenID = yourPlayer["chosenID"];
        int yourGuessID = yourPlayer["guessID"];
        CharacterButton yourChosenCharacter = getCharacterButtonForID(yourChosenID);
        CharacterButton yourGuessedCharacter = getCharacterButtonForID(yourGuessID);
        endScreenChosenYours.SetSelectedCharacter(yourChosenID, yourChosenCharacter.getSprite(), yourChosenCharacter.getCharacterName());
        endScreenGuessedYours.SetSelectedCharacter(yourGuessID, yourGuessedCharacter.getSprite(), yourGuessedCharacter.getCharacterName());

        int otherChosenID = otherPlayer["chosenID"];
        int otherGuessID = otherPlayer["guessID"];
        CharacterButton otherChosenCharacter = getCharacterButtonForID(otherChosenID);
        CharacterButton otherGuessedCharacter = getCharacterButtonForID(otherGuessID);
        endScreenChosenOther.SetSelectedCharacter(otherChosenID, otherChosenCharacter.getSprite(), otherChosenCharacter.getCharacterName());
        endScreenGuessedOther.SetSelectedCharacter(otherGuessID, otherGuessedCharacter.getSprite(), otherGuessedCharacter.getCharacterName());

        if((yourChosenID == otherGuessID && otherChosenID == yourGuessID)
            || (yourChosenID != otherGuessID && otherChosenID != yourGuessID))
        {
            endScreenOutcomeText.text = "Draw!";
            draws++;
            endScreenDrawsScore.text = "Draws: " + draws;
        } 
        else if(yourChosenID == otherGuessID)
        {
            endScreenOutcomeText.text = "You Lost!";
            otherScore++;
            endScreenOtherScore.text = "Other's wins: " + otherScore;
        }
        else
        {
            endScreenOutcomeText.text = "You Win!";
            yourScore++;
            endScreenYourScore.text = "Your wins: " + yourScore;
        }

        gameStateManagerRef.TransitionToGameEnded();
    }
}
