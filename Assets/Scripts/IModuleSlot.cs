using UnityEngine;
using System.Collections;

public interface IModuleSlot {
	GameObject ShipGameObject { get; set; }
	void UpdateShip();
}
