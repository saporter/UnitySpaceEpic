using UnityEngine;
using System.Collections;

public class StandardChassis : MonoBehaviour {
	[SerializeField]
	private GameObject _primaryWeaponPrefab;
	[SerializeField]
	private GameObject _secondaryWeaponPrefab;
	[SerializeField]
	private GameObject _enginesPrefab;

	public GameObject PrimaryWeapon {
		get { return _primaryWeaponPrefab;}
		set { _primaryWeaponPrefab = value; }
	}
	public GameObject SecondaryWeapon{
		get { return _secondaryWeaponPrefab;}
		set { _secondaryWeaponPrefab = value; }
	}
	public GameObject Engines{
		get { return _enginesPrefab;}
		set { _enginesPrefab = value; }
	}

	void Awake()
	{
		if (_enginesPrefab != null) {
			GameObject clone = Instantiate(_enginesPrefab, transform.position, transform.rotation) as GameObject;
			clone.transform.parent = this.transform;
			GetComponent<IMover>().Engines = clone.GetComponent<IEngine>();
		}
		if (_primaryWeaponPrefab != null) {
			GameObject clone = Instantiate(_primaryWeaponPrefab, transform.position, transform.rotation) as GameObject;
			clone.transform.parent = this.transform;
			GetComponent<IShooter>().Primary = clone.GetComponent<IWeapon>();
		}
		if (_secondaryWeaponPrefab != null) {
			GameObject clone = Instantiate(_secondaryWeaponPrefab, transform.position, transform.rotation) as GameObject;
			clone.transform.parent = this.transform;
			GetComponent<IShooter>().Secondary = clone.GetComponent<IWeapon>();
		}
	}
}
