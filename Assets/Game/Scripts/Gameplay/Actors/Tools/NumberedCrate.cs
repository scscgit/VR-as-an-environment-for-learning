using System;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class NumberedCrate : MonoBehaviour
{
    public static readonly float SizeXOfCrate = 1.55f;
    public static readonly float PhysicsForceMultiplier = 20;

    public int Number;
    public float GazeTime = 1;
    [NonSerialized] public GazeTimer GazeTimer;

    private ParticleSystem _particleSystem;
    private Vector3 _delta;
    private float _zStartAxis;

    private static Quaternion HeadRotation
    {
        get
        {
            var cardboardHead = GameInputManager.Instance.CardboardHead;
            var headRotation = Quaternion.Euler(
                cardboardHead.overrideVerticalReceiver.transform.localRotation.eulerAngles.x,
                cardboardHead.overrideHorizontalReceiver.transform.localRotation.eulerAngles.y,
                cardboardHead.overrideHorizontalReceiver.transform.localRotation.eulerAngles.z
            );
            return headRotation;
        }
    }

    /// <summary>
    /// There is a Unity glitch that deleting a Canvas (either through Destroy() or by deleting from the Editor)
    /// causes Unity Editor to crash. This is a working workaround.
    /// </summary>
    public void DestroyMyselfSafely()
    {
        gameObject.SetActive(false);
        Destroy(gameObject, 0.1f);
    }

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
        // Only the first click during a single gaze counts
        if (GazeTimer.Clicked)
        {
            return;
        }

        _particleSystem.Play();
        var hit = RaycastPointFromCamera(HeadRotation);
        _delta = new Vector3(
            transform.position.x - hit.x,
            transform.position.y - hit.y,
            0f
        );
        _zStartAxis = transform.position.z;
    }

    private Vector3 RaycastPointFromCamera(Quaternion headRotation)
    {
        RaycastHit hit;
        Physics.Raycast(
            Camera.main.transform.position,
            headRotation * Vector3.forward,
            out hit,
            LayerMask.GetMask("UI"));
        return hit.point;
    }

    private void OnEnable()
    {
        GazeTimer = new GazeTimer(PointerClick, () => GazeTime);
    }

    private void Start()
    {
        var texts = GetComponentsInChildren<Text>();
        foreach (var text in texts)
        {
            text.text = Number.ToString();
        }
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        GazeTimer.Update();
        if (!GazeTimer.Clicked)
        {
            return;
        }

        var hit = RaycastPointFromCamera(HeadRotation);
        var startPosition = transform.position + _delta;
        var endPosition = new Vector3(hit.x, hit.y, _zStartAxis);

        GetComponent<Rigidbody>().AddForce((endPosition - startPosition) * PhysicsForceMultiplier);

        // Draws lines describing the difference between positions
        Debug.DrawLine(Camera.main.transform.position, startPosition, Color.red);
        Debug.DrawLine(Camera.main.transform.position, endPosition, Color.yellow);
    }
}
