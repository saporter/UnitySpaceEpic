using UnityEngine;
using System.Collections;

public class SaveGameEvent : GameEvent {
	private string _file;
	public string File { get { return _file; } }


	public SaveGameEvent(string file)
	{
		_file = file;
	}
}
