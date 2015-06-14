using UnityEngine;
using System.Collections;

public interface IMappable {
	GameObject FollowGameObject { get; set; }
	
	void SnapToGameObject();
}
