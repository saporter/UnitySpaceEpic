﻿using UnityEngine;
using System.Collections;

public class Chassis : MonoBehaviour, IChassis, IDamageable {
	[SerializeField] float _maxHealth = 20f;
	[SerializeField] GameObject Schematic;
	[SerializeField] GameObject DestroyedEffect;
	GameObject _schematicClone;
	float _currentHealth;
	IModuleSlot[] moduleSlots;

	#region IChassis implementation
	public GameObject SchematicUIClone {
		get {
			return _schematicClone;
		}
	}
	#endregion

	#region IDamageable implementation
	public float MaxHealth { get { return _maxHealth; } }
	public float CurrentHealth { get { return _currentHealth; } }
	#endregion

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

	void Start()
	{
		LoadVars ();
		_currentHealth = _maxHealth;
	}

	void LoadVars()
	{
		IXmlLoader XmlLoader = GameObject.FindGameObjectWithTag ("XmlLoader").GetComponent<IXmlLoader>();

		if (!XmlLoader.FloatVars.TryGetValue ("Chassis.MaxHealth", out _maxHealth))
			Debug.LogError ("Could not find Chassis.MaxHealth from XmlLoader");
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
		if (_currentHealth < 0f) {
			GameObject clone = Instantiate(DestroyedEffect, transform.position, transform.rotation) as GameObject;
			Destroy(clone, 1.5f);
			Destroy(this.gameObject);
		}
	}
	
	#endregion

	void OnCollisionExit()
	{
		GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
	}
}
