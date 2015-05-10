using UnityEngine;
using System.Collections;

public class ShipSystems : MonoBehaviour, IShipSystems {
	private float _power = 0f;
	private float _engineUse = 0f;
	private GameObject _target;
	
	public float Power { get { return _power;} set { _power = value;} }
	public float MaxPower { 
		get { 
			IPowerSource[] powerSources = GetComponentsInChildren<IPowerSource>();
			float max = 0f;
			foreach(IPowerSource p in powerSources)
				max += p.PowerCapacity;
			return max;
		} 
	}
	public float EngineUse { get { return _engineUse; } set { _engineUse += value; } }
	public float MaxEngineUse { 
		get { 
//			IEngine[] engines = GetComponentsInChildren<IEngine>();
//			float max = 0f;
//			foreach(IEngine e in engines)
//				max += e.EngineCapacity;
//			return max;
			return 0f;
		} 
	}
	public GameObject Target { get { return _target; } set { _target = value;} }

}
