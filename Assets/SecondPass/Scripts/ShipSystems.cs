using UnityEngine;
using System.Collections;

public class ShipSystems : MonoBehaviour, IShipSystems {
	[SerializeField] 
	private float _power;
	[SerializeField]
	private float _engineUse;
	[SerializeField]
	private float _maxPower = 20f;
	[SerializeField]
	private float _maxEngineUse = 15f;

	private GameObject _target;

	public float Power { get { return _power;} set { _power = value;} }
	public float EngineUse { get { return _engineUse;} set { _engineUse = value;} }
	public float MaxPower { get { return _maxPower;} }
	public float MaxEngineUse { get { return _maxEngineUse;} }
	public GameObject Target { get { return _target; } set { _target = value;} }

	void Awake()
	{
		PlayerController player = GetComponent<PlayerController> ();
		if (player) {
			player.systems = this;
		}
	}
}
