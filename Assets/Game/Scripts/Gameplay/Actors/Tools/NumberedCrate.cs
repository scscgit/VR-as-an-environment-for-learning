using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class NumberedCrate : MonoBehaviour
{
    public int Number;

    /// <summary>
    /// There is a Unity glitch that deleting a Canvas (either through Destroy() or by deleting from the Editor)
    /// causes Unity Editor to crash. This is a working workaround.
    /// </summary>
    public void DestroyMyselfSafely()
    {
        gameObject.SetActive(false);
        Destroy(gameObject, 0.1f);
    }

    private void Start()
    {
        var texts = GetComponentsInChildren<Text>();
        foreach (var text in texts)
        {
            text.text = Number.ToString();
        }
    }
}
