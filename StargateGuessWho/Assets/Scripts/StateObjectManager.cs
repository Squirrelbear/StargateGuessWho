using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateObjectManager : MonoBehaviour
{
    [SerializeField]
    private GameStateManager.GameState onOldState;
    [SerializeField]
    private GameStateManager.GameState onNewState;

    [SerializeField]
    private bool enableGameObjects;

    [SerializeField]
    private List<GameObject> objectsToChange;

    private void OnEnable()
    {
        GameStateManager.OnStateTransition += HandleStateChange;
    }

    private void OnDisable()
    {
        GameStateManager.OnStateTransition -= HandleStateChange;
    }

    public void HandleStateChange(GameStateManager.GameState oldState, GameStateManager.GameState newState)
    {
        if((onOldState == GameStateManager.GameState.AnyState || oldState == onOldState) 
            && (onNewState == GameStateManager.GameState.AnyState || newState == onNewState))
        {
            foreach(GameObject obj in objectsToChange)
            {
                obj.SetActive(enableGameObjects);
            }
        }
    }
}
