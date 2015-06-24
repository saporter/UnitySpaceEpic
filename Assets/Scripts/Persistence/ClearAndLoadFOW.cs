using UnityEngine;
using System.Collections;
using System.IO;

/* This example script is placed on the FogOfWarCamera */
public class ClearAndLoadFOW : MonoBehaviour {
	[SerializeField] GameObject FOWLoader;	// This is a duplicated FogOfWarPlane from the video, but (1) raised higher on y-axis, (2) material removed and (3) Layer changed to the aperture layer from video
	[SerializeField] RenderTexture rt;	// The RenderTexture the FogOfWarCamera is using

	Camera c;

	void Awake () {
		Events.instance.AddListener<MapOpennedEvent> (MapOpenned);
	}

	// Use this for initialization
	void Start () {
		c = GetComponent<Camera> ();	// This script is on my FogOfWarCamera as an example
		
		/* (Assuming you have a saved FOW map from a previous run) */
		StartCoroutine (ClearAndLoad ());
	}
	
	void OnDestroy(){
		Events.instance.RemoveListener<MapOpennedEvent> (MapOpenned);
	}

	void MapOpenned(MapOpennedEvent e)
	{
		SaveMap ();
	}


	void SaveMap()
	{ /* Used this documentation for saving a Texture2D http://docs.unity3d.com/ScriptReference/Texture2D.EncodeToPNG.html */

		// Unlike the documentation, I want to use the RT my FOW camera is rendering to
		RenderTexture activeNow = RenderTexture.active;
		RenderTexture.active = rt;

		// Read pixels of our rt (ReadPixels reads from RenderTexture.active)
		Texture2D tex2d = new Texture2D (rt.width, rt.height, TextureFormat.ARGB32, false);
		tex2d.ReadPixels (new Rect (0f, 0f, rt.width, rt.height), 0, 0);
		tex2d.Apply ();

		// creates byte stream to save...
		byte[] bytes = tex2d.EncodeToPNG ();
		Object.Destroy (tex2d);

		// ...and write to a file.  Save however you like, and anywhere you like.  This is a simple example.
		File.WriteAllBytes (Application.dataPath + "/Pictures/SavedMap.png", bytes);

		// Resert to the original RenderTexture.active 
		RenderTexture.active = activeNow;
	}

	IEnumerator ClearAndLoad () {
		// clear the FOW texture, setting everything dark.
		c.clearFlags = CameraClearFlags.Color;
		c.Render ();	// Render camera to capture a cleared plane
		GetComponent<Camera> ().clearFlags = CameraClearFlags.Depth;

		// Load old map
		LoadMap ();
		yield return new WaitForEndOfFrame ();
	}

	void LoadMap () {
		// setup loading material using saved texture (saved Fog of War overlay)
		Texture2D tex = new Texture2D (2, 2); // size does not matter since LoadImage will replace with incoming image size
		tex.LoadImage (File.ReadAllBytes (Application.dataPath + "/Pictures/SavedMap.png")); // path to saved texture
		Material m = new Material(Shader.Find("Custom/TextureOnly")); // (TexutreOnly shader pasted below) Just need a shader unaffected by light, etc.
		m.mainTexture = tex;

		// Instantiate GameObject with exact dimensions of FOW plane to be seen by camera
		GameObject clone = Instantiate (FOWLoader) as GameObject;
		clone.GetComponent<MeshRenderer> ().material = m;

		// Render camera to capture texture and apply to current RT (current FOW)
		c.Render ();

		// Destroy clone to remove from Scene
		Destroy (clone);
	}
}

/*
 * 
Shader "Custom/TextureOnly" {
 
	 Properties {
	     _MainTex ("Texture", 2D) = ""
	 }
	 
	 SubShader {Pass {   // iPhone 3GS and later
	     GLSLPROGRAM
	     varying mediump vec2 uv;
	    
	     #ifdef VERTEX
	     void main() {
	         gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
	         uv = gl_MultiTexCoord0.xy;
	     }
	     #endif
	    
	     #ifdef FRAGMENT
	     uniform lowp sampler2D _MainTex;
	     void main() {
	         gl_FragColor = texture2D(_MainTex, uv);
	     }
	     #endif     
	     ENDGLSL
	 }}
	 
	 SubShader {Pass {   // pre-3GS devices, including the September 2009 8GB iPod touch
	     SetTexture[_MainTex]
	 }}
  
 }
 * 
 * */
