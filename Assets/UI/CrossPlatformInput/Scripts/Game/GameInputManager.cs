using System;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;

/// <summary>
/// Manages the GameInput by linking all provided or auto-found dependencies, allowing the ActiveInputMethod change.
/// Should be created as a GameObject with the name "GameInputManager".
/// If it is not found, one will be automatically created under a GameObject with the name "Managers", or in a root.
/// [ExecuteInEditMode] prevents old static instance value blocking the refresh, e.g. on Scene's Discard Changes.
/// </summary>
[ExecuteInEditMode]
public class GameInputManager : MonoBehaviour
{
    public static readonly GameInput.ActiveInputMethodType DefaultInputMethod = GameInput.ActiveInputMethodType.Vr;

    [NonSerialized] private GameInput _gameInput;

    public GameInput GameInput
    {
        get { return _gameInput; }
        private set
        {
            if (GameInput == null)
            {
                _gameInput = value;
            }
        }
    }

    private static GameInputManager _instance;

    // Fast accessor assuming a singleton instance, creating the Manager if it's not found by name
    public static GameInputManager Instance
    {
        get { return _instance ?? (_instance = FindOrCreateGameInputManager()); }
    }

    // Percentage value for the tilt offset limit within VR that won't cause any movement
    [Range(0f, 1f)] public float TiltMovementVrThreshold = 0.1f;

    public Cardboard Cardboard;

    public GameObject CheckmarkNonVrKeyboard;
    public GameObject CheckmarkNonVrPhone;
    public GameObject CheckmarkVr;

    // Proxy for the GameInput.ActiveInputMethod
    public GameInput.ActiveInputMethodType ActiveInputMethod;

    [NonSerialized] public IList<MobileControlRig> MobileControlRigs = new List<MobileControlRig>();

    public bool IsMobileControlRigActive()
    {
        return ActiveInputMethod == GameInput.ActiveInputMethodType.NonVrPhone;
    }

    public void SetActiveInputMethodNonVrKeyboard()
    {
        ActiveInputMethod = GameInput.ActiveInputMethodType.NonVrKeyboard;
        SetActiveInputMethodCheckmark(CheckmarkNonVrKeyboard);
        Cardboard.VRModeEnabled = false;
    }

    public void SetActiveInputMethodNonVrPhone()
    {
        ActiveInputMethod = GameInput.ActiveInputMethodType.NonVrPhone;
        SetActiveInputMethodCheckmark(CheckmarkNonVrPhone);
        Cardboard.VRModeEnabled = false;
    }

    public void SetActiveInputMethodVr()
    {
        ActiveInputMethod = GameInput.ActiveInputMethodType.Vr;
        SetActiveInputMethodCheckmark(CheckmarkVr);
        Cardboard.VRModeEnabled = true;
    }

    /// <summary>
    /// Methods using this common functionality should be bound to OnClick() events of Menu Buttons
    /// with their individual Checkmark GameObjects being provided beforehand so they can receive a feedback.
    /// </summary>
    private void SetActiveInputMethodCheckmark(GameObject activeCheckmark)
    {
        GameObject[] checkmarks =
        {
            CheckmarkNonVrKeyboard, CheckmarkNonVrPhone, CheckmarkVr
        };
        foreach (var checkmark in checkmarks)
        {
            if (checkmark)
            {
                checkmark.SetActive(checkmark.Equals(activeCheckmark));
            }
        }
    }

    private static GameInputManager FindOrCreateGameInputManager()
    {
        GameInputManager manager;
        var managerGameObject = GameObject.Find("GameInputManager");
        if (null == managerGameObject)
        {
            managerGameObject = new GameObject("GameInputManager", typeof(GameInputManager));
            var allManagersGameObject = GameObject.Find("Managers");
            if (null != allManagersGameObject)
            {
                managerGameObject.transform.parent = allManagersGameObject.transform;
            }
            manager = managerGameObject.GetComponent<GameInputManager>();
            manager.Reset();
        }
        else
        {
            manager = managerGameObject.GetComponent<GameInputManager>();
        }
        manager.GameInput = new GameInput(manager);
        return manager;
    }

    // Unity calls this only when in the Editor, needs to be called manually after the AddComponent call during runtime
    private void Reset()
    {
        ActiveInputMethod = DefaultInputMethod;
        var cardboardGameObject = GameObject.Find("CardboardMain");
        Cardboard = cardboardGameObject.GetComponent<Cardboard>();
    }

    // During the Awake or Start, Manager does not have the GameInput instance initialized yet!
    void Awake()
    {
        // If the Editor created this component, this will initialize it by getting an instance
        if (Instance)
        {
        }
    }

    // Proxy updates the real value of ActiveInputMethod, triggering Input changes
    void Update()
    {
        // Ensuring a singleton by a more aggressive means
        if (GameInput == null)
        {
            throw new Exception(
                "There can be only one GameInputManager and it needs be in GameObject with the name GameInputManager");
        }
        GameInput.UpdateActiveInputMethod();
    }

    // When returning to the Editor, we need to receive a current Manager instance to prevent an invalid old reference
    void OnDestroy()
    {
        _instance = null;
    }
}
