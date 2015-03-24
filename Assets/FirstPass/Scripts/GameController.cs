using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	private TurnPhase phase;
	public TurnPhase Phase
	{
		get { return phase; }
	}
	

	// Use this for initialization
	void Start () {
		phase = TurnPhase.PlayerTurn;
		Events.instance.AddListener<EnemyFinishedTurnEvent> (TransitionTurn);
	}

	void Update () {

		if (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown (KeyCode.Tab) && phase == TurnPhase.PlayerTurn) {
			TransitionTurn(null);
		}else if(Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)){
			Events.instance.Raise(new FireWeaponKeyPressEvent());
		}else if(Input.GetKeyDown(KeyCode.Space)){
			Events.instance.Raise(new CommitToMovementKeyPressEvent());
		}else if(Input.GetKeyDown(KeyCode.Escape)){
			Events.instance.Raise(new ClearSelectionKeyPressEvent());
		}

	}

	void TransitionTurn(EnemyFinishedTurnEvent e)
	{
		switch (phase) 
		{
		case TurnPhase.PlayerTurn:
			phase = TurnPhase.EnemyTurn;
			Events.instance.Raise(new TransitionToEnemyTurnEvent());
			break;
		case TurnPhase.EnemyTurn:
			phase = TurnPhase.PlayerTurn;
			Events.instance.Raise(new TransitionToPlayerTurnEvent());
			break;
		}
	}
}

public enum TurnPhase
{
	PlayerTurn=0,
	EnemyTurn
}

public class TransitionToEnemyTurnEvent : GameEvent
{
}

public class TransitionToPlayerTurnEvent : GameEvent
{
}

public class FireWeaponKeyPressEvent : GameEvent
{
}

public class CommitToMovementKeyPressEvent : GameEvent
{
}

public class ClearSelectionKeyPressEvent : GameEvent
{
}