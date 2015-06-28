using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class LoaderScript : MonoBehaviour, IConfirmable {
	[SerializeField] GameObject saveSlot;
	[SerializeField] GameObject FOWLoader;
	[SerializeField] Camera FOWCamera;
	[SerializeField] GameObject confirmWindow;

	ISaver saver;

	void Awake()
	{
		if (saveSlot != null) {
			saver = saveSlot.GetComponent<ISaver>();
		}
		if (saver == null) {
			Debug.LogError ("SaveSlot is null or ISaver Component not attached to SaveSlot GameObject in " + gameObject.name);
		}

		if (saver.FileExists ()) {
			GetComponent<Text> ().text = saver.SaveName;
		} else {
			GetComponent<Text> ().text = "";
		}
	}

	void OnEnable()
	{
		if (saver.FileExists ()) {
			GetComponent<Text> ().text = saver.SaveName;
		} else {
			GetComponent<Text> ().text = "";
		}
	}

	public void AttemptLoad()
	{
		IConfirmer window = confirmWindow.GetComponent<IConfirmer> ();
		window.ToNotify = this;
		window.Message = "Unsaved progress will be lost.  Continue with load?";
		confirmWindow.SetActive (true);
	}

	#region IConfirmable implementation

	public void Confirmed ()
	{
		Load ();
		GameManager.Instance.NewGameLoaded ();
	}

	#endregion

	public void Load()
	{
		// clear the FOW texture, setting everything dark.
		FOWCamera.clearFlags = CameraClearFlags.Color;
		FOWCamera.Render ();	// Render camera to capture a cleared plane
		FOWCamera.clearFlags = CameraClearFlags.Depth;

		// setup loading material using saved texture (saved Fog of War overlay)
		Texture2D tex = new Texture2D (2, 2); // size does not matter since LoadImage will replace with incoming image size
		tex.LoadImage (File.ReadAllBytes (saver.FileName)); // path to saved texture
		Material m = new Material(Shader.Find("Custom/TextureOnly")); // (TexutreOnly shader pasted below) Just need a shader unaffected by light, etc.
		m.mainTexture = tex;
		
		// Instantiate GameObject with exact dimensions of FOW plane to be seen by camera
		GameObject clone = Instantiate (FOWLoader) as GameObject;
		clone.GetComponent<MeshRenderer> ().material = m;
		
		// Render camera to capture texture and apply to current RT (current FOW)
		FOWCamera.Render ();
		
		// Destroy clone to remove from Scene
		Destroy (clone);
	}
}
