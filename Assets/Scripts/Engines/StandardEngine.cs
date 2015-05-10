using UnityEngine;
using System.Collections;

public class StandardEngine : MonoBehaviour, IEngine {
	[SerializeField]
	private float _maxSpeed = 6f;
	[SerializeField]
	private float _rotationSpeed = 3f;
	[SerializeField]
	private float _reversePenalty = .5f;
	[SerializeField]
	private float _acceleration = 1f;

	#region IEngine implementation

	public float MaxSpeed { get { return _maxSpeed; } }

	public float RotationSpeed { get { return _rotationSpeed; } }

	public float ReversePenalty { get { return _reversePenalty; } }

	public float Acceleration { get { return _acceleration; } }

	#endregion
}
