using UnityEngine;
using System.Collections;

public interface IWeapon {
	string Name { get; }
	bool FireButtonDown(IDamageable target, IShipSystems systems);
	bool FireButtonDown(Vector3 target, IShipSystems systems);
	void FireButtonUp();
}
