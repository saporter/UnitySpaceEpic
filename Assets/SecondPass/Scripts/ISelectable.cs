using UnityEngine;
using System.Collections;

public interface ISelectable {
	SelectableFunction Selected { get; set; }
}

public delegate void SelectableFunction();