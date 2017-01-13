using Game.Scripts.Gameplay.Games;
using UnityEngine;

/// <summary>
/// A.K.A. Game Launcher
/// </summary>
[SelectionBase]
public class Horse : GazeBehaviour
{
    public MonoBehaviour Game;

    /// <summary>
    /// Callback from a Game that has been won
    /// </summary>
    public void OnGameWon()
    {
    }

    protected override void Click()
    {
        var game = Game as IGame;
        if (game != null)
        {
            game.StartGame(this);
        }
    }
}
