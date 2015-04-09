using UnityEngine;
using System.Collections;

public interface IMovable {
	void Move(Vector3 toPoint, IShipSystems systems);
}
