using UnityEngine;
using System.Collections;

public class Chassis : MonoBehaviour, IChassis, IDamageable {
	#region IChassis implementation
	private GameObject _schematicClone;
	public GameObject SchematicUIClone {
		get {
			return _schematicClone;
		}
	}

	#endregion

	#region IDamageable implementation
	[SerializeField] float _maxHealth = 20f;
	public float MaxHealth { get { return _maxHealth; } }

	float _currentHealth;
	public float CurrentHealth { get { return _currentHealth; } }

	#endregion

	[SerializeField] GameObject Schematic;
	[SerializeField] GameObject DestroyedEffect;


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

		_currentHealth = _maxHealth;

	}

	void OnDestroy()
	{
		Events.instance.RemoveListener<ContainerChangedEvent> (moduleInstalled);
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

	#region IDamageable implementation
	
	public void ApplyDamage (float damage)
	{
		_currentHealth -= damage;
		Events.instance.Raise (new ShipDamagedEvent(this.gameObject));
		if (_currentHealth < 0f) {
			GameObject clone = Instantiate(DestroyedEffect, transform.position, transform.rotation) as GameObject;
			Destroy(clone, 1.5f);
			Destroy(this.gameObject);
		}
	}
	
	#endregion
}
