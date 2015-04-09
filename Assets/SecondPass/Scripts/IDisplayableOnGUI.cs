using UnityEngine;
using System.Collections;

public interface IDisplayableOnGUI {
	GraphicData selector { get; }

	void Show();
	void Hide();
	void UpdatePosition();
	void ShowOnPlayerMovement(bool show);
}

[System.Serializable]
public class GraphicData
{
	/// <summary>
	/// GUI Prefab 
	/// </summary>
	public GameObject UIElement;
	
	/// <summary>
	/// offset from GameObject
	/// </summary>
	public Vector2 offset;

	/// <summary>
	/// Max distance this GUI should travel when moving or following something (e.g. mouse position)
	/// </summary>
	public float followRadius = 10f;

	/// <summary>
	/// How fast this GUI should move to it's radius 
	/// </summary>
	public float lerpSpeed = 100f;
}