using UnityEngine;
using System.Collections;

public interface ICameraInputListenable {

	IEnumerator ListenForInput (Camera c, Transform gameMenu, Transform mapMenu, float zoomSpeed, float minSize, float maxSize);
}
