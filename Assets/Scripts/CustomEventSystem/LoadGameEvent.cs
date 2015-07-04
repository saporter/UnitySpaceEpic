using UnityEngine;
using System.Collections;

public class LoadGameEvent : GameEvent {
	private string _file;
	public string File { get { return _file; } }
	
	
	public LoadGameEvent(string file)
	{
		_file = file;
	}
}
