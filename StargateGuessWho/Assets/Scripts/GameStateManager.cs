using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public enum GameState { StartScreen, CreateJoinScreen, ConnectingToServer, WaitingOtherPlayer, ChooseCharacter, CoreGame, GameEnded, AnyState }

    [SerializeField]
    private GameState gameState;

    public delegate void GameStateTransitionEvent(GameState oldState, GameState newState);
    public static event GameStateTransitionEvent OnStateTransition;
     
    public void TransitionToState(GameState newState)
    {
        GameState oldState = gameState;
        gameState = newState;
        OnStateTransition?.Invoke(oldState, newState);
    }

    public void TransitionToStartScreen()
    {
        TransitionToState(GameState.StartScreen);
    }

    public void TransitionToCreateJoinScreen()
    {
        TransitionToState(GameState.CreateJoinScreen);
    }

    public void TransitionToConnectServer()
    {
        TransitionToState(GameState.ConnectingToServer);
    }

    public void TransitionToWaitingForPlayer()
    {
        TransitionToState(GameState.WaitingOtherPlayer);
    }

    public void TransitionToChooseCharacter()
    {
        TransitionToState(GameState.ChooseCharacter);
    }
    public void TransitionToCoreGame()
    {
        TransitionToState(GameState.CoreGame);
    }

    public void TransitionToGameEnded()
    {
        TransitionToState(GameState.GameEnded);
    }
}