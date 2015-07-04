using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

[ExecuteInEditMode]
public class UniqueID : MonoBehaviour {

	[SerializeField] public string UniqueName;
	
	void Start () 
	{
		#if UNITY_EDITOR
		if (string.IsNullOrEmpty(UniqueName))
			UniqueName = Guid.NewGuid().ToString();
		UniqueIdRegistry.Register(UniqueName, GetInstanceID());

		PropertyModification change = new PropertyModification();
		change.propertyPath = new SerializedObject(this).FindProperty("UniqueName").propertyPath;
		change.target = this;
		change.value = UniqueName;

		PropertyModification[] properties = PrefabUtility.GetPropertyModifications(this);
		List<PropertyModification> mods = new List<PropertyModification>(); 
		if(properties != null)
			mods.AddRange(properties);
		mods.Add (change);
		PrefabUtility.SetPropertyModifications(this, mods.ToArray());
		#endif
	}
	
	void OnDestroy() 
	{
		#if UNITY_EDITOR
		UniqueIdRegistry.Deregister(UniqueName);
		#endif
	}
	
	void Update() 
	{
		#if UNITY_EDITOR
		if(!UniqueIdRegistry.Contains(UniqueName) && !string.IsNullOrEmpty(UniqueName)){
			UniqueIdRegistry.Register(UniqueName, GetInstanceID());
		}else if (GetInstanceID() != UniqueIdRegistry.GetInstanceId(UniqueName)) {
			UniqueName = Guid.NewGuid().ToString();
			UniqueIdRegistry.Register(UniqueName, GetInstanceID());
		}
		#endif
	}
}

public static class UniqueIdRegistry
{

	public static Dictionary<string, int> Mapping = new Dictionary<string, int>();
	
	public static void Deregister(string id)
	{
		Mapping.Remove(id);
	}
	
	public static void Register(string id, int value)
	{
		if (!Mapping.ContainsKey(id))
			Mapping.Add(id, value);
	}
	
	public static int GetInstanceId(string id)
	{
		if (Contains (id))
			return Mapping [id];
		Debug.Log ("Could not find ID. Returning -1");
		return -1;
	}

	public static bool Contains(string id)
	{
		return Mapping.ContainsKey (id);
	}
}

//public class UniqueIdRegistry {
//	
//	private static List<string> registeredIds = new List<string>();
//	
//	public static void Register(string id)
//	{
//		if (registeredIds.Contains (id)) {
//			throw new ArgumentException("Registry already contains ID: " + id);
//		}
//		registeredIds.Add (id);
//	}
//	
//	public static void Deregister(string id)
//	{
//		if(registeredIds.Contains(id))
//			registeredIds.Remove (id);
//	}
//	
//	public static bool Contains(string id)
//	{
//		return registeredIds.Contains (id);
//	}
//}