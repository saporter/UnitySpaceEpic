using UnityEngine;
using System.Collections;

public interface IShipSystems {
	float Power { get; set; }
	float EngineUse { get; set; }
	float MaxPower { get; }
	float MaxEngineUse { get; }

	GameObject Target { get; set; }
}
