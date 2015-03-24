using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {
	public Text turn;
	public string[] turnStrings;	// Set in UI. To be displayed based on current value of TurnPhase enum

	private GameController gameController;

	void Start () {
		gameController = GameObject.FindWithTag ("GameController").GetComponent<GameController>();
		turn.text = turnStrings[(int)gameController.Phase];

		Events.instance.AddListener<TransitionToEnemyTurnEvent> (StateChange);
		Events.instance.AddListener<TransitionToPlayerTurnEvent> (StateChange);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void StateChange(GameEvent e)
	{
		turn.text = turnStrings[(int)gameController.Phase];
	}
}
