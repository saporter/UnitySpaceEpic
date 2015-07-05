using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {
	public static GameManager GM;	// so I can reference GameManager without using FindGameObject methods.

	[SerializeField] GameObject gameMenu;
	[SerializeField] GameObject shipMenu;		
	[SerializeField] GameObject mapMenu;
	[SerializeField] GameObject dockedMenu;
	[SerializeField] GameObject escapeMenu;
	[SerializeField] GameObject player;

	public GameObject ShipMenu { get { return shipMenu; } } // whatever... it's the only one I need public right now...

	GameObject currentGameSubMenu;

	void Awake()
	{
		GM = this;
		
		Events.instance.AddListener<PlayerDockedEvent> (PlayerDockedOrExit);
		Events.instance.AddListener<LoadGameEvent> (NewGameLoaded);

		gameMenu.SetActive (true);
		currentGameSubMenu = gameMenu.transform.GetChild(gameMenu.transform.childCount - 1).gameObject;
		gameMenu.SetActive (false);
		escapeMenu.SetActive (false);
	
	}

	void Start()
	{
		if (PlayerPrefs.HasKey ("Load")) {
			Events.instance.Raise (new LoadGameEvent (PlayerPrefs.GetString ("Load")));
			PlayerPrefs.DeleteKey("Load");
		}
	}

	void OnDestroy()
	{
		Events.instance.RemoveListener<PlayerDockedEvent> (PlayerDockedOrExit);
		Events.instance.RemoveListener<LoadGameEvent> (NewGameLoaded);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Game Menu")) {
			ToggleGameMenu (gameMenu.transform.GetChild(gameMenu.transform.childCount - 1).gameObject);	// The currently shown menu
		} else if (Input.GetButtonDown ("Ship Menu")) {
			ToggleGameMenu (shipMenu, shipMenu != currentGameSubMenu || !gameMenu.activeSelf);
		} else if (Input.GetButtonDown ("Map Menu")) {
			ToggleGameMenu (mapMenu, mapMenu != currentGameSubMenu || !gameMenu.activeSelf);
		} else if (Input.GetButtonDown ("Docked Menu")) {
			ToggleGameMenu (dockedMenu, dockedMenu != currentGameSubMenu || !gameMenu.activeSelf);
		} else if (Input.GetButtonDown ("Cancel")) 
		{
			if(gameMenu.activeSelf){
				ToggleGameMenu (shipMenu, false);  // doesn't matter which menu I give
				ToggleEscapeMenu (false);
			}else {
				ToggleEscapeMenu (!escapeMenu.activeSelf);
			}
		}
	}

	// Used by "close" button in GameMenu GUI
	public void ToggleGameMenuOff(){
		ToggleGameMenu (shipMenu, false);
	}

	// Used by "close" button in GameMenu GUI
	public void ToggleEscapeMenuOff()
	{
		ToggleEscapeMenu (false);
	}

	private void ToggleGameMenu(GameObject subMenu, bool active)
	{
		gameMenu.SetActive(active);
		if(gameMenu.activeSelf){
			Pause();
			currentGameSubMenu = subMenu;
			subMenu.transform.GetChild (1).GetComponentInChildren<Button> ().onClick.Invoke ();
		}else{
			UnPause();
		}
	}

	private void ToggleGameMenu(GameObject subMenu)
	{
		ToggleGameMenu (subMenu, !gameMenu.activeSelf);
	}

	void ToggleEscapeMenu (bool active)
	{
		escapeMenu.SetActive (active);
		if(escapeMenu.activeSelf){
			Pause();
		}else{
			UnPause();
		}
	}

	private void Pause()
	{
		Time.timeScale = 0f;
	}

	private void UnPause()
	{
		Time.timeScale = 1f;
	}

	void PlayerDockedOrExit (PlayerDockedEvent e)
	{
		if (e.playerDocked) {
			ToggleGameMenu (dockedMenu, true);
		}
	}

	void NewGameLoaded (LoadGameEvent e)
	{
		ToggleEscapeMenu (false);
	}
}
