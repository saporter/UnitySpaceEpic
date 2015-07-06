using UnityEngine;
using System.Collections;

public interface ISaveLoadable {

	string Name { get; }
	void Save(string file);
	void Load(string file);
}
