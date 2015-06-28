using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConfirmWindow : MonoBehaviour, IConfirmer {
	#region IConfirmer implementation
	IConfirmable _notifyObject;
	string _message;

	public IConfirmable ToNotify {
		get {
			return _notifyObject;
		}
		set {
			_notifyObject = value;
		}
	}

	public string Message {
		get {
			return _message;
		}
		set {
			_message = value;
			transform.GetChild(0).gameObject.GetComponent<Text>().text = _message;
		}
	}

	#endregion

	public void Yes()
	{
		if (_notifyObject != null) {
			_notifyObject.Confirmed ();
			_notifyObject = null;
			gameObject.SetActive(false);
		}
	}


}
