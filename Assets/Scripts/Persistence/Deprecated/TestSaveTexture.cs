using UnityEngine;
using System.Collections;
using System.IO;

public class TestSaveTexture : MonoBehaviour {
	[SerializeField] RenderTexture rt;

	// Use this for initialization

	
	void MapOpenned(MapOpennedEvent e)
	{
		Debug.Log ("Test saving map");
		RenderTexture activeNow = RenderTexture.active;
		RenderTexture.active = rt;

		Texture2D tex2d = new Texture2D (rt.width, rt.height, TextureFormat.ARGB32, false);
		tex2d.ReadPixels (new Rect (0f, 0f, rt.width, rt.height), 0, 0);
		tex2d.Apply ();

		byte[] bytes = tex2d.EncodeToPNG ();
		Object.Destroy (tex2d);

		File.WriteAllBytes (Application.dataPath + "/Pictures/SavedMap.png", bytes);
	
		RenderTexture.active = activeNow;
	}
}
