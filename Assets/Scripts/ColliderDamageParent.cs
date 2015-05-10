using UnityEngine;
using System.Collections;

public class ColliderDamageParent : MonoBehaviour, IDamageable {
	#region IDamageable implementation

	public float MaxHealth {
		get {
			return transform.parent.GetComponent<IDamageable> ().MaxHealth;
		}
	}

	public float CurrentHealth {
		get {
			return transform.parent.GetComponent<IDamageable> ().CurrentHealth;
		}
	}

	#endregion

	#region IDamageable implementation

	public void ApplyDamage (float damage)
	{
		transform.parent.GetComponent<IDamageable> ().ApplyDamage (damage);
	}

	#endregion



}
