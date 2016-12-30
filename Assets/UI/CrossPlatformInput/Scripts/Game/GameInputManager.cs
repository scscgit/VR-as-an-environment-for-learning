using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class GameInputManager : MonoBehaviour
{
    public static readonly GameInput.ActiveInputMethodType DefaultInputMethod = GameInput.ActiveInputMethodType.Vr;

    public GameInput GameInput;
    public Cardboard Cardboard;

    public GameObject CheckmarkNonVrKeyboard;
    public GameObject CheckmarkNonVrPhone;
    public GameObject CheckmarkVr;

    // Proxy for the GameInput.ActiveInputMethod
    public GameInput.ActiveInputMethodType ActiveInputMethod;

    public IList<MobileControlRig> MobileControlRigs = new List<MobileControlRig>();

    public bool IsMobileControlRigActive()
    {
        return ActiveInputMethod == GameInput.ActiveInputMethodType.NonVrPhone;
    }

    public void SetActiveInputMethodNonVrKeyboard()
    {
        ActiveInputMethod = GameInput.ActiveInputMethodType.NonVrKeyboard;
        CheckmarkNonVrKeyboard.SetActive(true);
        CheckmarkNonVrPhone.SetActive(false);
        CheckmarkVr.SetActive(false);
    }

    public void SetActiveInputMethodNonVrPhone()
    {
        ActiveInputMethod = GameInput.ActiveInputMethodType.NonVrPhone;
        CheckmarkNonVrKeyboard.SetActive(false);
        CheckmarkNonVrPhone.SetActive(true);
        CheckmarkVr.SetActive(false);
    }

    public void SetActiveInputMethodVr()
    {
        ActiveInputMethod = GameInput.ActiveInputMethodType.Vr;
        CheckmarkNonVrKeyboard.SetActive(false);
        CheckmarkNonVrPhone.SetActive(false);
        CheckmarkVr.SetActive(true);
    }

    // Needs to be public, as the Reset is internally called by Unity only from the Editor, not during the AddComponent
    public void Reset()
    {
        ActiveInputMethod = DefaultInputMethod;
        var cardboardGameObject = GameObject.Find("CardboardMain");
        Cardboard = cardboardGameObject.GetComponent<Cardboard>();
    }

    // During the Awake, GameInput does not have this GameInputManager instance initialized yet!
    void Awake()
    {
        GameInput = new GameInput();
    }

    // Proxy updates the real value of ActiveInputMethod, triggering Input changes
    void Update()
    {
        GameInput.UpdateActiveInputMethod();
    }
}
