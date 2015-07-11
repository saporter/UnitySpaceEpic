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

	void Awake()
	{
		LoadVars ();
	}

	void LoadVars()
	{
		IDataLoader XmlLoader = GameManager.GM.DataLoader;//GameObject.FindGameObjectWithTag ("XmlLoader").GetComponent<IDataLoader>();
		
		if (!XmlLoader.FloatVars.TryGetValue ("StandardEngine.MaxSpeed", out _maxSpeed))
			Debug.LogError ("Could not find StandardEngine.MaxSpeed from XmlLoader");
		if (!XmlLoader.FloatVars.TryGetValue ("StandardEngine.RotationSpeed", out _rotationSpeed))
			Debug.LogError ("Could not find StandardEngine.RotationSpeed from XmlLoader");
		if (!XmlLoader.FloatVars.TryGetValue ("StandardEngine.ReversePenalty", out _reversePenalty))
			Debug.LogError ("Could not find StandardEngine.ReversePenalty from XmlLoader");
		if (!XmlLoader.FloatVars.TryGetValue ("StandardEngine.Acceleration", out _acceleration))
			Debug.LogError ("Could not find StandardEngine.Acceleration from XmlLoader");
	}
}
