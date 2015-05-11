using UnityEngine;
using System.Collections;

public class SteeringMover : MonoBehaviour, IMover {
	#region IMover implementation
	private IEngine _engines;
	public IEngine Engines {
		get { return _engines; }
		set {
			currentStage = 0f;
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			_engines = value; 
		}
	}

	#endregion

	public float thrustStages = 2f;
	public float currentStage = 0f;

	private Rigidbody rigidBody;
	private bool m_verticalAxisUsed = false;

	void Awake () {
		rigidBody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (_engines == null)
			return;

		float change = throttleChange ();
		if (change != 0f) {
			currentStage += change;
			currentStage = (int)currentStage;
			if(currentStage > thrustStages) currentStage = thrustStages;
			if(currentStage < -thrustStages) currentStage = -thrustStages;

			float newVelocity = (currentStage / thrustStages) * _engines.MaxSpeed;
			rigidBody.velocity = transform.forward * (currentStage > 0 ? newVelocity : _engines.ReversePenalty * newVelocity);

		}

		float dir = directionChange ();
		if (dir != 0) {
			Vector3 rot = rigidBody.rotation.eulerAngles;
			rigidBody.angularVelocity = Vector3.zero;
			rigidBody.rotation = Quaternion.Euler(rot.x, rot.y + dir * _engines.RotationSpeed, rot.z); 
			rigidBody.velocity = currentStage > 0 ? transform.forward * rigidBody.velocity.magnitude : transform.forward * -rigidBody.velocity.magnitude;
		}
	}
	
	private float throttleChange()
	{
		float change = Input.GetAxisRaw ("Vertical");
		if (change != 0f) {
			if(!m_verticalAxisUsed)
			{
				m_verticalAxisUsed = true;
				return change;
			}
		} 
		else {
			m_verticalAxisUsed = false;
		}
		return 0f;
	}

	private float directionChange()
	{
		return Input.GetAxisRaw ("Horizontal");
	}

	void OnCollisionExit(){
		if (_engines == null)
			return;
		currentStage = currentStage > 0 ? rigidBody.velocity.magnitude / _engines.MaxSpeed : -rigidBody.velocity.magnitude / _engines.MaxSpeed;
	}
}
