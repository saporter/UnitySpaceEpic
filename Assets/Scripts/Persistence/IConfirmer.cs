using UnityEngine;
using System.Collections;

public interface IConfirmer {
	IConfirmable ToNotify { get; set; } 
	string Message { get; set; }
}
