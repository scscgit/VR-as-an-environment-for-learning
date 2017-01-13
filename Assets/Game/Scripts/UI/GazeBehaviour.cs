using System;
using UnityEngine;

/// <summary>
/// Convenience GazeTimer MonoBehaviour implementation superclass
/// </summary>
public abstract class GazeBehaviour : MonoBehaviour
{
    public float GazeTime = 2;
    [NonSerialized] public GazeTimer GazeTimer;

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
        GazeTimer.PointerClick();
    }

    protected abstract void Click();

    protected void OnEnable()
    {
        GazeTimer = new GazeTimer(Click, () => GazeTime);
    }

    protected void Update()
    {
        GazeTimer.Update();
    }
}
