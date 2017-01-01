using UnityEngine;

/// <summary>
/// Toggle and action support for Menu window.
/// </summary>
public class MenuController : MonoBehaviour
{
	public GameObject menu;
	public bool MenuAtStart;

	UIController ui;
	Animator animator;

	public void QuitGame()
	{
		Application.Quit();
		Debug.Log("The application was quit");
	}

	void Start()
	{
		this.ui = menu.GetComponent<UIController>();
		this.animator = menu.GetComponent<Animator>();

		if (!MenuAtStart)
		{
			// Pausing the animator, disabling the Menu
			animator.enabled = false;
			// Last started animation was Show, we explicitly set Hide
			ui.Hide();
		}
	}

	void Update()
	{
		if (GameInput.Instance.GetButtonDown("Cancel"))
		{
			animator.enabled = true;
			var show = ui.isShow;
			if (show)
			{
				ui.Hide();
			}
			else
			{
				ui.Show();
			}
		}
	}
}
