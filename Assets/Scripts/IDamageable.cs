using UnityEngine;
using System.Collections;

public interface IDamageable {
	float MaxHealth { get; }
	float CurrentHealth { get; }


	void ApplyDamage(float damage);
	void SetHealth(float toHealth);
}
