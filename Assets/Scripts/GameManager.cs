using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// This script is run first before all other scripts so the static reference to GM can be set and other scrips can refernce in awake.  Set in Edit->Project Settings->Script Execution Order
/// </summary>
public class GameManager : MonoBehaviour {
	public static GameManager GM;	// so I can reference GameManager without using FindGameObject methods.

	private IDataLoader _dataLoader = null;
	private IPrefabManager _prefabManager = null;
	private float _startTimeAtLoad = 0f;

	[SerializeField] GameObject gameMenu;
	[SerializeField] GameObject shipMenu;		
	[SerializeField] GameObject mapMenu;
	[SerializeField] GameObject dockedMenu;
	[SerializeField] GameObject escapeMenu;
	[SerializeField] GameObject _player;
	[SerializeField] GameObject _mapCanvas;

	public IDataLoader DataLoader { 
		get {
			if(_dataLoader == null)
				_dataLoader = GetComponentInChildren<IDataLoader>();
			return _dataLoader;
		}
	}
	public IPrefabManager PrefabManager{
		get {
			if(_prefabManager == null)
				_prefabManager = GetComponentInChildren<IPrefabManager>();
			return _prefabManager;
		}
	}

	public GameObject Player {
		get {
			if(_player == null){
				_player = GameObject.FindGameObjectWithTag ("Player");
				// If still null, there's a problem
				if(_player == null)
					Debug.LogError("No GameObject with 'Player' tag exists, and a script needs a reference to player");
			}
			return _player;
		}
	}

	public GameObject MapCanvas {
		get {
			if(_mapCanvas == null){
				_mapCanvas = GameObject.FindGameObjectWithTag ("Map Canvas");
				// If still null, there's a problem
				if(_mapCanvas == null)
					Debug.LogError("No GameObject with 'Map Canvas' tag exists, and a script needs a reference to the Map Canvas");
			}
			return _mapCanvas;
		}
	}

	public GameObject ShipMenu { get { return shipMenu; } } // whatever... it's the only one I need public right now...

	public float GameTime {
		get{
			return _startTimeAtLoad + Time.timeSinceLevelLoad;
		}
	}

	GameObject currentGameSubMenu;

	void Awake()
	{
		GM = this;
		_dataLoader = GetComponentInChildren<IDataLoader>();
		_prefabManager = GetComponentInChildren<IPrefabManager>();
		_startTimeAtLoad = 0f;


		Events.instance.AddListener<PlayerDockedEvent> (PlayerDockedOrExit);
		Events.instance.AddListener<SaveGameEvent> (SavingGame);
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
		Events.instance.RemoveListener<SaveGameEvent> (SavingGame);
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

	void SavingGame (SaveGameEvent e)
	{
		ES2.Save (GameTime, e.File + "?tag=GameManager_GameTime");
	}

	/// <summary>
	/// This should be called first (Because GameManager's Awake() is called first, and it's listener added first).  Important because GameTime needs to be accurate for others Load.
	/// </summary>
	/// <param name="e">E.</param>
	void NewGameLoaded (LoadGameEvent e)
	{
		if (ES2.Exists (e.File + "?tag=GameManager_GameTime"))
			_startTimeAtLoad = ES2.Load<float> (e.File + "?tag=GameManager_GameTime");
		ToggleEscapeMenu (false);
	}
}
