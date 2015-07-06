using UnityEngine;
using System.Collections;

public class PrimaryWeaponSlot : MonoBehaviour, IModuleSlot {
	private GameObject moduleClone = null;

	#region IModuleSlot implementation
	[SerializeField] GameObject _shipGameObject;
	public GameObject ShipGameObject {
		get { return _shipGameObject; }
		set { _shipGameObject = value; }
	}

	public void UpdateShip()
	{
		Destroy (moduleClone);
		ShipGameObject.GetComponent<IShooter> ().Primary = null;
		if (transform.childCount == 0)
			return;
		moduleClone = Instantiate(transform.GetChild(0).gameObject.GetComponent<IContainer>().Contents, 
		                           ShipGameObject.transform.position, 
		                           ShipGameObject.transform.rotation) as GameObject;
		moduleClone.transform.parent = ShipGameObject.transform;
		ShipGameObject.GetComponent<IShooter>().Primary = moduleClone.GetComponent<IWeapon>();
	}
	#endregion

	void OnDestroy()
	{
		//Debug.Log ("Confirmed! OnDestroy called when supposed to...");
		if (moduleClone != null)
			Destroy (moduleClone);
	}
}
