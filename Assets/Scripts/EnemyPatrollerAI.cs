using UnityEngine;
using System.Collections;

public class EnemyPatrollerAI : MonoBehaviour, IMover, IShooter {
	#region IMover implementation
	private IEngine _engines;
	public IEngine Engines {
		get { return _engines; }
		set { _engines = value; }
	}

	#endregion

	#region IShooter implementation
	private IWeapon _primary;
	private IWeapon _secondary;

	public IWeapon Primary {
		get { return _primary; }
		set { _primary = value; }
	}
	
	public IWeapon Secondary {
		get { return _secondary; }
		set { _secondary = value; }
	}

	#endregion


	[SerializeField] float patrolRadius = 5f;
	[SerializeField] float sensorRadius = 5f;
	[SerializeField] float patrolSpeedFactor = .5f;
	Vector3 patrolPoint;
	Vector3? targetLastSeen;
	float error = .1f;
	GameObject target;
	Rigidbody rigidBody;
	private bool pFire = false;
	private bool sFire = false;
	private float time = 0f;

	void Awake()
	{
		Events.instance.AddListener<ShipDamagedEvent> (EngagedByEnemy);
		Events.instance.AddListener<PlayerDockedEvent> (TargetDocked);
		patrolPoint = transform.position;
		TargetDocked (new PlayerDockedEvent (false));
		rigidBody = GetComponent<Rigidbody> ();
	}

	void OnDestroy()
	{
		Events.instance.RemoveListener<ShipDamagedEvent> (EngagedByEnemy);
		Events.instance.RemoveListener<PlayerDockedEvent> (TargetDocked);
	}

	void FixedUpdate()
	{
		if (target != null && (target.transform.position - transform.position).magnitude <= sensorRadius)
			Attack ();
		else if (targetLastSeen.HasValue) 
			Chase ();
		else 
		    Patrol ();

		RotateToward (rigidBody.velocity);
	}

	void Attack()
	{
		time += Time.deltaTime;
		if (time % 4 < 1f && _primary != null) {
			_primary.FireButtonDown (target.transform.position);
			pFire = true;
		} else if (pFire) {
			_primary.FireButtonUp();
			pFire = false;
		}
		
		if (time % 4 > 2f && time % 4 < 3f && _secondary != null) {
			_secondary.FireButtonDown (target.transform.position);
			sFire = true;
		}else if (sFire) {
			_secondary.FireButtonUp();
			sFire = false;
		}
		
		targetLastSeen = target.transform.position;
	}

	void Chase()
	{
		WeaponsOff ();
		MoveTo (targetLastSeen.Value);
		if ((targetLastSeen.Value - transform.position).magnitude < error) {
			targetLastSeen = null;
		}
	}

	void Patrol()
	{
		WeaponsOff ();

		Vector3 point = transform.position - patrolPoint;
		if (Mathf.Abs(point.magnitude - patrolRadius) > error) {
			if(point.magnitude == 0f)
				point += new Vector3(0f, 0f, 1f);
			MoveTo (patrolPoint + (point.normalized * patrolRadius));
		}
		else
			Orbit (patrolPoint);

		targetLastSeen = null;
	}

	void MoveTo (Vector3 point)
	{
		rigidBody.velocity = (point - transform.position).normalized * patrolSpeedFactor * (_engines != null ? _engines.MaxSpeed : 0f);

	}

	void Orbit (Vector3 patrolPoint)
	{
		rigidBody.velocity = Vector3.Cross(transform.up, transform.position - patrolPoint).normalized * patrolSpeedFactor * (_engines != null ? _engines.MaxSpeed : 0f);

	}

	void RotateToward (Vector3 direction)
	{
		transform.rotation = Quaternion.Lerp (transform.rotation,  Quaternion.LookRotation(direction), Time.fixedDeltaTime * (_engines != null ? _engines.RotationSpeed : 0f));
	}

	void WeaponsOff()
	{
		if (pFire) {
			_primary.FireButtonUp ();
			pFire = false;
		}
		if (sFire) {
			_secondary.FireButtonUp ();
			sFire = false;
		}
	}

	void EngagedByEnemy(ShipDamagedEvent e)
	{
		if (target == null)
			return;
		targetLastSeen = target.transform.position;
	}

	void TargetDocked (PlayerDockedEvent e)
	{
		if (e.playerDocked) {
			target = null;
			targetLastSeen = null;
		}
		else
			target = GameObject.FindGameObjectWithTag ("Player");
	}
}
