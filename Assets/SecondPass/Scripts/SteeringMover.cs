using UnityEngine;
using System.Collections;

public class SteeringMover : MonoBehaviour {
	public float speed = 6f;
	public float rotationSpeed = 3f;
	public float reversePenalty = .5f;
	public float thrustStages = 2f;
	public float currentStage = 0f;

	private Rigidbody rigidBody;
	private bool m_verticalAxisUsed = false;

	void Awake () {
		rigidBody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float change = throttleChange ();
		if (change != 0f) {
			currentStage += change;
			currentStage = (int)currentStage;
			if(currentStage > thrustStages) currentStage = thrustStages;
			if(currentStage < -thrustStages) currentStage = -thrustStages;

			float newVelocity = (currentStage / thrustStages) * speed;
			rigidBody.velocity = transform.forward * (currentStage > 0 ? newVelocity : reversePenalty * newVelocity);

		}

		float dir = directionChange ();
		if (dir != 0) {
			Vector3 rot = rigidBody.rotation.eulerAngles;
			rigidBody.rotation = Quaternion.Euler(rot.x, rot.y + dir * rotationSpeed, rot.z); 
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
		rigidBody.angularVelocity = Vector3.zero;
		currentStage = currentStage > 0 ? rigidBody.velocity.magnitude / speed : -rigidBody.velocity.magnitude / speed;
	}
}
