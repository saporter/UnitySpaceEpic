using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {
	[SerializeField] GameObject gameMenu;

	GameObject shipMenu;
	GameObject mapMenu;
	GameObject dockedMenu;

	void Awake()
	{
		shipMenu = GameObject.FindGameObjectWithTag ("Ship Menu");
		mapMenu = GameObject.FindGameObjectWithTag ("Map Menu");
		dockedMenu = GameObject.FindGameObjectWithTag ("Docked Menu");
		gameMenu.SetActive (false);
	}

	void Start()
	{
		GameObject.FindGameObjectWithTag ("Player").GetComponent<IChassis> ().SchematicUIClone.transform.SetParent (shipMenu.transform.GetChild(0).transform, false);
		Events.instance.AddListener<ShipDamagedEvent> (ShipDamaged);
	}

	void OnDestroy()
	{
		Events.instance.RemoveListener<ShipDamagedEvent> (ShipDamaged);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Game Menu")) {
			ToggleGameMenu ();
		} else if (Input.GetButtonDown ("Ship Menu")) {
			ToggleGameMenu (true);
			shipMenu.transform.GetChild (1).GetComponentInChildren<Button> ().onClick.Invoke ();
		} else if (Input.GetButtonDown ("Map Menu")) {
			ToggleGameMenu (true);
			mapMenu.transform.GetChild (1).GetComponentInChildren<Button> ().onClick.Invoke ();
		} else if (Input.GetButtonDown ("Docked Menu")) {
			ToggleGameMenu (true);
			dockedMenu.transform.GetChild (1).GetComponentInChildren<Button> ().onClick.Invoke ();
		}
		else if (Input.GetButtonDown ("Cancel") && gameMenu.activeSelf) {
			gameMenu.SetActive(false);
			UnPause();
		}
	}

	public void ToggleGameMenu(bool active)
	{
		gameMenu.SetActive(active);
		if(gameMenu.activeSelf){
			Pause();
		}else{
			UnPause();
		}
	}

	private void ToggleGameMenu()
	{
		ToggleGameMenu (!gameMenu.activeSelf);
	}

	private void Pause()
	{
		Time.timeScale = 0f;
	}

	private void UnPause()
	{
		Time.timeScale = 1f;
	}

	void ShipDamaged(ShipDamagedEvent e)
	{
		if (e.Ship.tag == "Player" && e.Ship.GetComponent<IDamageable>().CurrentHealth < 0f) {
			Destroy(e.Ship.GetComponent<IChassis>().SchematicUIClone);
		}
	}
}
