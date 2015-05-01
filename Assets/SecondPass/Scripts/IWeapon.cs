using UnityEngine;
using System.Collections;

public interface IWeapon {
	string Name { get; }
	void FireOn(IDamageable target, IShipSystems systems);
}
