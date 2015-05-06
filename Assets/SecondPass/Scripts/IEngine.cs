using UnityEngine;
using System.Collections;

public interface IEngine {
	float MaxSpeed { get; }
	float RotationSpeed { get; }
	float ReversePenalty { get; }
	float Acceleration { get; }
}
