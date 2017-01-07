using System;
using Game.Scripts.Gameplay.Games;
using UnityEngine;

/// <summary>
/// A.K.A. Game Launcher
/// </summary>
[SelectionBase]
public class Horse : MonoBehaviour
{
    public MonoBehaviour Game;
    public float GazeTime = 2;
    [NonSerialized] public GazeTimer GazeTimer;

    /// <summary>
    /// Callback from a Game that has been won
    /// </summary>
    public void OnGameWon()
    {
    }

    public void PointerEnter()
    {
        GazeTimer.PointerEnter();
    }

    public void PointerExit()
    {
        GazeTimer.PointerExit();
    }

    public void PointerClick()
    {
        var game = Game as IGame;
        if (game != null)
        {
            game.StartGame(this);
        }
    }

    private void OnEnable()
    {
        GazeTimer = new GazeTimer(PointerClick, () => GazeTime);
    }

    private void Update()
    {
        GazeTimer.Update();
    }
}
