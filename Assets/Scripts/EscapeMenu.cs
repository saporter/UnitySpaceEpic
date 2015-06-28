using UnityEngine;
using System.Collections;

public class EscapeMenu : MonoBehaviour {
	[SerializeField] GameObject saveMenu;
	[SerializeField] GameObject loadMenu;
	[SerializeField] GameObject confirmWindow;

	void Awake()
	{
		subMenusOff ();
	}

	void OnEnable()
	{
		subMenusOff ();
	}


	public void SaveClicked()
	{
		saveMenu.SetActive (true);
		loadMenu.SetActive (false);
		confirmWindow.SetActive (false);
	}

	public void LoadClicked()
	{
		saveMenu.SetActive (false);
		loadMenu.SetActive (true);
		confirmWindow.SetActive (false);
	}

	void subMenusOff()
	{
		saveMenu.SetActive (false);
		loadMenu.SetActive (false);
		confirmWindow.SetActive (false);
	}
}
