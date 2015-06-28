using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class SaverScript : MonoBehaviour, IConfirmable, ISaver {
	[SerializeField] string toFile;
	[SerializeField] string displayName;
	[SerializeField] RenderTexture fogOfWarRT;
	[SerializeField] GameObject confirmWindow;

	string savePath = "";

	#region ISaver implementation

	public string FileName {
		get {
			if(savePath == "")
				savePath = Application.dataPath + toFile;
			return savePath;
		}
	}

	public string SaveName {
		get {
			return displayName;
		}
	}

	#endregion

	void Awake()
	{
		savePath = Application.dataPath + toFile;
		if (FileExists()) {
			GetComponent<Text>().text = SaveName;
		}
	}

	void OnEnable()
	{
		if (FileExists()) {
			GetComponent<Text>().text = SaveName;
		}
	}

	public void AttemptSave()
	{
		if (!File.Exists (savePath))
			Save ();
		else
			ConfirmSave ();
	}

	void ConfirmSave()
	{
		IConfirmer window = confirmWindow.GetComponent<IConfirmer> ();
		window.ToNotify = this;
		window.Message = "Are you sure you want to overwrite save?";
		confirmWindow.SetActive (true);
	}

	#region IConfirmable implementation
	public void Confirmed ()
	{
		Save ();
		OnEnable ();
	}
	#endregion


	void Save()
	{/* Used this documentation for saving a Texture2D http://docs.unity3d.com/ScriptReference/Texture2D.EncodeToPNG.html */
		
		// Unlike the documentation, I want to use the RT my FOW camera is rendering to
		RenderTexture activeNow = RenderTexture.active;
		RenderTexture.active = fogOfWarRT;
		
		// Read pixels of our rt (ReadPixels reads from RenderTexture.active)
		Texture2D tex2d = new Texture2D (fogOfWarRT.width, fogOfWarRT.height, TextureFormat.ARGB32, false);
		tex2d.ReadPixels (new Rect (0f, 0f, fogOfWarRT.width, fogOfWarRT.height), 0, 0);
		tex2d.Apply ();
		
		// creates byte stream to save...
		byte[] bytes = tex2d.EncodeToPNG ();
		Object.Destroy (tex2d);
		
		// ...and write to a file.  Save however you like, and anywhere you like.  This is a simple example.
		File.WriteAllBytes (savePath, bytes); // Change from dataPath to something else...
		
		// Resert to the original RenderTexture.active 
		RenderTexture.active = activeNow;
	}

	#region ISaver implementation

	public bool FileExists()
	{
		return File.Exists (FileName);
	}

	#endregion

}
