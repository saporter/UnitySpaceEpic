using UnityEngine;
using System.Collections;

public interface ISaver { 
	bool FileExists();
	string FileName { get; }
	string SaveName { get; }
}
