#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


namespace UnityStandardAssets.CrossPlatformInput
{
    // this script enables or disables the child objects of a control rig
    // depending on whether the GameInputManager wants for it to be active.
    [ExecuteInEditMode]
    public class MobileControlRig : MonoBehaviour
    {

#if !UNITY_EDITOR
	void OnEnable()
	{
		CheckEnableControlRig();
	}
#endif

        private void Start()
        {
#if UNITY_EDITOR
            if (Application.isPlaying) //if in the editor, need to check if we are playing, as start is also called just after exiting play
#endif
            {
                UnityEngine.EventSystems.EventSystem system = GameObject.FindObjectOfType<UnityEngine.EventSystems.EventSystem>();

                if (system == null)
                {
                    //the scene have no event system, spawn one
                    GameObject o = new GameObject("EventSystem");

                    o.AddComponent<UnityEngine.EventSystems.EventSystem>();
                    o.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
                }
            }
        }

#if UNITY_EDITOR
        private void OnEnable()
        {
            EditorApplication.update += Update;
        }


        private void OnDisable()
        {
            EditorApplication.update -= Update;
        }


        private void Update()
        {
            CheckEnableControlRig();
        }
#endif


        public void CheckEnableControlRig()
        {
            if (!GameInputManager.Instance.MobileControlRigs.Contains(this))
            {
                GameInputManager.Instance.MobileControlRigs.Add(this);
            }
            EnableControlRig(GameInputManager.Instance.IsMobileControlRigActive());
        }


        private void EnableControlRig(bool enabled)
        {
            foreach (Transform t in transform)
            {
                t.gameObject.SetActive(enabled);
            }
        }
    }
}
