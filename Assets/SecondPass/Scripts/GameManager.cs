using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	[SerializeField] GameObject gameMenu;

	void Awake()
	{
		gameMenu.SetActive (false);
	}

	void Start()
	{
		GameObject.FindGameObjectWithTag ("Player").GetComponent<IChassis> ().SchematicUIClone.transform.SetParent (gameMenu.transform, false);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Schematic")) {
			gameMenu.SetActive(!gameMenu.activeSelf);
			if(gameMenu.activeSelf){
				Pause();
			}else{
				UnPause();
			}
		}
		if (Input.GetButtonDown ("Cancel") && gameMenu.activeSelf) {
			gameMenu.SetActive(false);
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
}
