using UnityEngine;
using System.Collections;

public interface IDamageable {
	GameObject GameObj { get; }

	void ApplyDamage(float damage);
}
