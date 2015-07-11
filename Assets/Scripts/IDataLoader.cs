using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// For leading data from a file
/// </summary>
public interface IDataLoader {
	Dictionary<string, float> FloatVars { get; }
}
