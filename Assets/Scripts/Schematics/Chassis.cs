using UnityEngine;
using System.Collections;

public class Chassis : MonoBehaviour, IChassis {
	#region IChassis implementation
	private GameObject _schematicClone;
	public GameObject SchematicUIClone {
		get {
			return _schematicClone;
		}
	}

	#endregion

 


	[SerializeField] GameObject Schematic;
	IModuleSlot[] moduleSlots;

	void Awake()
	{
		Events.instance.AddListener<ContainerChangedEvent> (moduleInstalled);
		_schematicClone = Instantiate (Schematic) as GameObject;
		_schematicClone.transform.SetParent(this.transform);
		moduleSlots = _schematicClone.GetComponentsInChildren<IModuleSlot> ();

		foreach (IModuleSlot m in moduleSlots) {
			m.ShipGameObject = this.gameObject;
			m.UpdateShip();
		}

	}

	void moduleInstalled(ContainerChangedEvent e)
	{
		if (e.OldSlot != null && e.OldSlot.GetComponent<IModuleSlot> () != null) {
			e.OldSlot.GetComponent<IModuleSlot>().UpdateShip();
		}
		if (e.Item.transform.parent != null && e.Item.transform.parent.gameObject.GetComponent<IModuleSlot> () != null) {
			e.Item.transform.parent.gameObject.GetComponent<IModuleSlot>().UpdateShip();
		}

	}
}
