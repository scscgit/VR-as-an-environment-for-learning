using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class NumberedCrate : GazeBehaviour
{
    public static readonly float WidthOfCrate = 1.55f;
    protected static readonly float PhysicsMultiplier = 20;
    protected static readonly Vector3 CenterOffset = new Vector3(0f, WidthOfCrate / 2, 0f);

    public int Number;

    private Rigidbody _rigidbody;
    private ParticleSystem _particleSystem;
    private float _zStartAxis;

    /// <summary>
    /// There is a Unity glitch that deleting a Canvas (either through Destroy() or by deleting from the Editor)
    /// causes Unity Editor to crash. This is a working workaround.
    /// </summary>
    public void DestroyMyselfSafely()
    {
        gameObject.SetActive(false);
        Destroy(gameObject, 0.1f);
    }

    protected override void Click()
    {
        _particleSystem.Play();
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

    private void Start()
    {
        var texts = GetComponentsInChildren<Text>();
        foreach (var text in texts)
        {
            text.text = Number.ToString();
        }
        _rigidbody = GetComponent<Rigidbody>();
        _particleSystem = GetComponent<ParticleSystem>();

        // Crate cannot be rotated in this game
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    /// <summary>
    /// The correct new HeadRotation value is only available in the LateUpdate when in VR, Update was not enough.
    /// </summary>
    protected void LateUpdate()
    {
        if (!GazeTimer.Clicked)
        {
            return;
        }

        // TODO: make sure why hot-swap breaks HeadRotation values
        var hit = RaycastPointFromCamera(GameInputManager.Instance.HeadRotation);
        var startPosition = transform.position + CenterOffset;
        var endPosition = new Vector3(hit.x, hit.y, _zStartAxis);

        _rigidbody.velocity = (endPosition - startPosition) * PhysicsMultiplier;

        // Draws lines describing the difference between positions
        Debug.DrawLine(Camera.main.transform.position, startPosition, Color.red);
        Debug.DrawLine(Camera.main.transform.position, endPosition, Color.yellow);
    }
}
