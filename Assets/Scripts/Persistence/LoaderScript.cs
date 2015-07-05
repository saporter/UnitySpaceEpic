using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class LoaderScript : MonoBehaviour, IConfirmable {
	[SerializeField] GameObject saveSlot;
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
		PlayerPrefs.SetString ("Load", saver.FileName);
		Application.LoadLevel ("Main");
	}

	#endregion
}
