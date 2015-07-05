using UnityEngine;
using System.Collections;
using System.IO;

public class SaveLoadFogofWar : MonoBehaviour {
	[SerializeField] GameObject FOWLoader;
	RenderTexture fogOfWarRT;
	Camera c;

	void Awake()
	{
		c = GetComponent<Camera> ();
		fogOfWarRT = c.targetTexture;
		Events.instance.AddListener<SaveGameEvent> (SaveGame);
		Events.instance.AddListener<LoadGameEvent> (LoadGame);
	}

	void OnDestroy()
	{
		Events.instance.RemoveListener<SaveGameEvent> (SaveGame);
		Events.instance.RemoveListener<LoadGameEvent> (LoadGame);
	}

	void SaveGame(SaveGameEvent e)
	{
		// Unlike the documentation, I want to use the RT my FOW camera is rendering to
		RenderTexture activeNow = RenderTexture.active;
		RenderTexture.active = fogOfWarRT;
		
		// Read pixels of our rt (ReadPixels reads from RenderTexture.active)
		Texture2D tex2d = new Texture2D (fogOfWarRT.width, fogOfWarRT.height, TextureFormat.ARGB32, false);
		tex2d.ReadPixels (new Rect (0f, 0f, fogOfWarRT.width, fogOfWarRT.height), 0, 0);
		tex2d.Apply ();

		string imagePath = Path.GetDirectoryName (e.File) + "/FOWMap/" + Path.GetFileNameWithoutExtension (e.File) + ".png";
		ES2.SaveImage (tex2d, imagePath);
		ES2.Save<string> (imagePath, e.File + "?tag=SaveFogofWar_FOWFilePath");
		
		// Resert to the original RenderTexture.active 
		RenderTexture.active = activeNow;
	}

	void LoadGame(LoadGameEvent e)
	{
		// clear the FOW texture, setting everything dark.
		c.clearFlags = CameraClearFlags.Color;
		c.Render ();	// Render camera to capture a cleared plane
		c.clearFlags = CameraClearFlags.Depth;
		
		// setup loading material using saved texture (saved Fog of War overlay)
		Texture2D tex = ES2.LoadImage (ES2.Load<string>(e.File + "?tag=SaveFogofWar_FOWFilePath")); 
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
