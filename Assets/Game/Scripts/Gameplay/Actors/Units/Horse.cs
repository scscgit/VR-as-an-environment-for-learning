using System.Collections;
using Game.Scripts.Gameplay.Games;
using UnityEngine;

/// <summary>
/// A.K.A. Game Launcher
/// </summary>
[SelectionBase]
public class Horse : GazeBehaviour
{
    public MonoBehaviour Game;

    private TextMesh _text;
    private Coroutine _currentCoroutine;

    /// <summary>
    /// Callback from a Game that has been won
    /// </summary>
    public void OnGameWon()
    {
        _text.text = "Congratz, you've won!";
        StopCoroutine();
        _currentCoroutine = StartCoroutine(SetTextAfter(10f, ""));
    }

    protected override void Click()
    {
        var game = Game as IGame;
        if (game != null)
        {
            game.StartGame(this);
        }
        StopCoroutine();
        _text.text = "Sort all the crates!";
        _currentCoroutine = StartCoroutine(SetTextAfter(5f, ""));
    }

    private void Start()
    {
        _text = GetComponentInChildren<TextMesh>();
        _text.text = "";
        StopCoroutine();
        _currentCoroutine = StartCoroutine(SetTextAfter(2f, "Hey, wanna play a game?", 15f));
    }

    private new void Update()
    {
        base.Update();
        _text.transform.LookAt(Camera.main.transform);
        _text.transform.Rotate(transform.rotation.eulerAngles);
    }

    private void StopCoroutine()
    {
        if (_currentCoroutine == null) return;
        StopCoroutine(_currentCoroutine);
        _currentCoroutine = null;
    }

    private IEnumerator SetTextAfter(float waitTime, string text, float? lifetime = null)
    {
        yield return new WaitForSeconds(waitTime);
        _text.text = text;
        if (lifetime != null)
        {
            yield return SetTextAfter(lifetime.Value, "");
        }
    }
}
