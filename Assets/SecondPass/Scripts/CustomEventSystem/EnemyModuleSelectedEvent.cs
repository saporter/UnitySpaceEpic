using UnityEngine;
using System.Collections;

public class EnemyModuleSelectedEvent : GameEvent {
	public IDamageable target;

	public EnemyModuleSelectedEvent(IDamageable _target){
		target = _target;
	}
}
