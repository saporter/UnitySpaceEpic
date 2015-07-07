using UnityEngine;
using System.Collections;

public class Chassis : MonoBehaviour, IChassis, IDamageable {
	[SerializeField] float _maxHealth = 20f;
	[SerializeField] GameObject Schematic;
	[SerializeField] GameObject DestroyedEffect;
	float _currentHealth;
	IModuleSlot[] moduleSlots;

	#region IChassis implementation
	public GameObject SchematicUI {
		get {
			return Schematic;
		}
		set {
			Schematic = value;
			UpdateChassis();
		}
	}
	#endregion

	#region IDamageable implementation
	public float MaxHealth { get { return _maxHealth; } }
	public float CurrentHealth { get { return _currentHealth; } }
	#endregion

	void Awake()
	{
		UpdateChassis ();
	}

	void Start()
	{
		LoadVars ();
		_currentHealth = _maxHealth;
	}

	private void LoadVars()
	{
		IXmlLoader XmlLoader = GameManager.GM.GetComponentInChildren<IXmlLoader>();

		if (!XmlLoader.FloatVars.TryGetValue ("Chassis.MaxHealth", out _maxHealth))
			Debug.LogError ("Could not find Chassis.MaxHealth from XmlLoader");
	}

	private void UpdateChassis()
	{
		moduleSlots = Schematic.GetComponentsInChildren<IModuleSlot> ();
		foreach (IModuleSlot m in moduleSlots) {
			m.ShipGameObject = this.gameObject;
			m.UpdateShip();
		}
	}

	#region IDamageable implementation
	
	public void ApplyDamage (float damage)
	{
		_currentHealth -= damage;
		Events.instance.Raise (new ShipDamagedEvent (gameObject));
		if (_currentHealth < 0f) {
			GameObject clone = Instantiate(DestroyedEffect, transform.position, transform.rotation) as GameObject;
			Destroy(clone, 1.5f);
			Destroy(this.gameObject);
		}
	}

	public void SetHealth(float toHealth)
	{
		_currentHealth = toHealth;
		if (_currentHealth > _maxHealth)
			_currentHealth = _maxHealth;
		else if (_currentHealth <= 0f)
			ApplyDamage (0f); // I'm destroyed
	}
	
	#endregion

	void OnCollisionExit()
	{
		GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
	}
}
