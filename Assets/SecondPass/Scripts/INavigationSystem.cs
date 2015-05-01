using UnityEngine;
using System.Collections;

public interface INavigationSystem {
	void Move(Vector3 toPoint, IShipSystems systems);
}
