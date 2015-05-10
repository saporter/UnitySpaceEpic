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
	float error = .1f;
	GameObject target;
	Rigidbody rigidBody;
	private bool pFire = false;
	private bool sFire = false;
	private float time = 0f;

	void Awake()
	{
		patrolPoint = transform.position;
		target = GameObject.FindGameObjectWithTag ("Player");
		rigidBody = GetComponent<Rigidbody> ();
	}

	void FixedUpdate()
	{
		if ((target.transform.position - transform.position).magnitude > sensorRadius)
			Patrol ();
		else
			Attack ();

	}

	void Patrol()
	{
		if (pFire)
			_primary.FireButtonUp ();
		if (sFire)
			_secondary.FireButtonUp ();
		Vector3 point = transform.position - patrolPoint;
		if (Mathf.Abs(point.magnitude - patrolRadius) > error) {
			if(point.magnitude == 0f)
				point += new Vector3(0f, 0f, 1f);
			MoveTo (patrolPoint + (point.normalized * patrolRadius));
		}
		else
			Orbit (patrolPoint);
	}

	void Attack()
	{
		time += Time.deltaTime;
		if (time % 4 < 1f && _primary != null) {
			_primary.FireButtonDown (target.transform.position, null);
			pFire = true;
		} else if (pFire) {
			_primary.FireButtonUp();
			pFire = false;
		}

		if (time % 4 > 2f && time % 4 < 3f && _secondary != null) {
			_secondary.FireButtonDown (target.transform.position, null);
			sFire = true;
		}else if (sFire) {
			_secondary.FireButtonUp();
			sFire = false;
		}

	}

	void Chase()
	{
	}

	void MoveTo (Vector3 point)
	{
		rigidBody.velocity = (point - transform.position).normalized * patrolSpeedFactor * (_engines != null ? _engines.MaxSpeed : 0f);
		RotateToward (rigidBody.velocity);
	}

	void Orbit (Vector3 patrolPoint)
	{
		rigidBody.velocity = Vector3.Cross(transform.up, transform.position - patrolPoint).normalized * patrolSpeedFactor * (_engines != null ? _engines.MaxSpeed : 0f);
		RotateToward (rigidBody.velocity);
	}

	void RotateToward (Vector3 direction)
	{
		transform.rotation = Quaternion.Lerp (transform.rotation,  Quaternion.LookRotation(direction), Time.fixedDeltaTime * (_engines != null ? _engines.RotationSpeed : 0f));
	}
}
