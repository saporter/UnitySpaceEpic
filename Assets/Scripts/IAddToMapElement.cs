using UnityEngine;
using System.Collections;

public interface IAddToMapElement {
	bool ElementAdded { get; }
	string ElementName { get; }
	void AddElement();
}
