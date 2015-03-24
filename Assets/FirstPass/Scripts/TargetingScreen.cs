using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TargetingScreen : MonoBehaviour {
	public Camera targetingCamera;
	public Slider healthBar;
	public RawImage screen;

	private TargetingSystem weaponSystem;
	private RectTransform rectTransform;
	private ShipModule selectedModule;

	void Start () {
		rectTransform = GetComponent<RectTransform> ();
		weaponSystem = targetingCamera.GetComponent<TargetingSystem> ();
		healthBar.gameObject.SetActive(false);

		Events.instance.AddListener<TransitionToPlayerTurnEvent> (ActivateComputer);
		Events.instance.AddListener<TransitionToEnemyTurnEvent> (DeactivateComputer);
	}

	void Update()
	{
		if (selectedModule != null) {
			healthBar.gameObject.SetActive (true);
			DrawHealth (selectedModule);
		} else {
			healthBar.gameObject.SetActive(false);
		}
	}

	void ActivateComputer(TransitionToPlayerTurnEvent e)
	{
		screen.enabled = true;
	}

	void DeactivateComputer(TransitionToEnemyTurnEvent e)
	{
		screen.enabled = false;
		selectedModule = null;
	}

	public void Clicked()
	{
		GameObject selectedObject = weaponSystem.ScreenSelection (targetingCamera.ScreenPointToRay (GetLocalMousePosition ()));
		if(selectedObject){
			selectedModule = selectedObject.GetComponent<ShipModule>();
			DrawHealth (selectedModule);
		}
	}

	Vector3 GetLocalMousePosition()
	{
		Vector3 inverse = rectTransform.InverseTransformPoint(Input.mousePosition);	// Will return negative values for x and y if anchor in top right
		Vector3 local = new Vector3 ((inverse.x + rectTransform.rect.width), (inverse.y + rectTransform.rect.height), 0f);
		return local;
	}

	void DrawHealth(ShipModule module)
	{
		if (module == null) 
		{
			Debug.Log("Module selected is null");
			return;
		}
		healthBar.value = module.currentHealth / module.maxHealth;
	}

}
