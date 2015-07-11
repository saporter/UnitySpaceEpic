using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO;

public class SlotSaverScript : MonoBehaviour, IConfirmable, ISaver {
	[SerializeField] string toFile;
	[SerializeField] string displayName;
	[SerializeField] GameObject confirmWindow;
	private bool awakeCalled = false;	// Need to know if I've updated my name yet.

	private static string saveFolder = "";

	#region ISaver implementation

	public string FileName {
		get {
			if(saveFolder == "")
				saveFolder = Application.persistentDataPath;
			return saveFolder + "/" + toFile + ".sp"; 
		}
	}

	public string SaveName {
		get {
			if(!awakeCalled && FileExists()) {
				displayName = ES2.Load<string>(FileName + "?tag=SaverScript_DisplayName");
			}
			return displayName;
		}
	}

	#endregion

	void Awake()
	{
		saveFolder = Application.persistentDataPath;
		if (FileExists()) {
			displayName = ES2.Load<string>(FileName + "?tag=SaverScript_DisplayName");
			GetComponent<Text>().text = displayName;
		}
		awakeCalled = true;
	}

	public void AttemptSave()
	{
		if (!FileExists())
			Confirmed (); // This saves the game
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
		TimeSpan timeSpan = TimeSpan.FromSeconds(GameManager.GM.GameTime);
		displayName = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
		ES2.Save<string> (displayName, FileName + "?tag=SaverScript_DisplayName");
		Events.instance.Raise(new SaveGameEvent(FileName));
		GetComponent<Text>().text = displayName;	
	}
	#endregion
	

	#region ISaver implementation

	public bool FileExists()
	{
		return ES2.Exists(FileName);
	}

	#endregion
}
