using UnityEngine;
using System.Collections;

public interface IWeapon {
	string Name { get; }
	bool FireButtonDown(IDamageable target);
	bool FireButtonDown(Vector3 target);
	void FireButtonUp();
}
