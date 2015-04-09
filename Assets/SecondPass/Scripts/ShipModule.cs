using UnityEngine;
using System.Collections;

public class ShipModule : MonoBehaviour, IDisplayableOnGUI {
	[SerializeField]
	private GraphicData _selector;
	private RectTransform rect;

	public GraphicData selector {get { return _selector; }}

	void Start()
	{
		if(selector == null)
			Debug.LogError ("Error: selector was null.  Add one as child.");

		selector.UIElement = Instantiate (selector.UIElement);
		selector.UIElement.transform.SetParent (GameObject.FindWithTag ("UI").transform);
		selector.UIElement.gameObject.GetComponent<ISelectable>().Selected = OnMouseClick;
		rect = selector.UIElement.GetComponent<RectTransform> ();
		Hide ();

	}

	public void Show()
	{
		UpdatePosition ();
		selector.UIElement.SetActive (true);
	}

	public void Hide()
	{
		selector.UIElement.SetActive (false);
	}

	public void UpdatePosition()
	{
		rect.rotation = Quaternion.identity;
		rect.position = Camera.main.WorldToScreenPoint (transform.position) + (Vector3)selector.offset;
	}

	public void ShowOnPlayerMovement(bool show)
	{
		if (show) {
			Events.instance.AddListener<PlacingMovementMarkerEvent> (ShowSelectorIfVisibleWhen);
			Events.instance.AddListener<MovementMarkerPlacedEvent> (HideSelectorWhen);
		} else {
			Events.instance.RemoveListener<PlacingMovementMarkerEvent> (ShowSelectorIfVisibleWhen);
			Events.instance.RemoveListener<MovementMarkerPlacedEvent> (HideSelectorWhen);
		}

	}

	void ShowSelectorIfVisibleWhen(PlacingMovementMarkerEvent e)
	{
		Ray ray = new Ray (e.position, transform.position - e.position); 
		RaycastHit moduleSelected;

		// Need to check raycast with base 2 int, not 0,1,2... layers as I read them in Unity Editor
		if (Physics.Raycast (ray, out moduleSelected, 100f, (1<<gameObject.layer))) {

			if(moduleSelected.collider.gameObject == this.gameObject){ 
				StopCoroutine(FadeSelectorOff());
				if(selector.UIElement.activeInHierarchy){
					rect.rotation = Quaternion.identity;
					rect.position = Vector3.MoveTowards(rect.position, 
					                                    Vector3.MoveTowards(Camera.main.WorldToScreenPoint (transform.position), Camera.main.WorldToScreenPoint (e.position), selector.followRadius),
					                                    selector.lerpSpeed * Time.deltaTime) + (Vector3)selector.offset;
				}else
				{
					Show ();
				}
			}else{
				if(selector.UIElement.activeInHierarchy)
					StartCoroutine (FadeSelectorOff());
			}
		}
	}

	void HideSelectorWhen(MovementMarkerPlacedEvent e)
	{
		Hide ();
	}

	IEnumerator FadeSelectorOff()
	{
		float time = 0f;
		float timeStep = .1f;
		float maxTime = 1f;
		while (Vector3.Distance(rect.position, Camera.main.WorldToScreenPoint (transform.position)) > 1f && time < maxTime) 
		{
			rect.rotation = Quaternion.identity;
			rect.position = Vector3.MoveTowards(rect.position, 
			                                    Camera.main.WorldToScreenPoint (transform.position),
			                                    selector.lerpSpeed * Time.deltaTime) + (Vector3)selector.offset;
			yield return new WaitForSeconds(timeStep);
			if(time > maxTime)
				Debug.Log("Runnaway coroutine Offender: " + gameObject.name);
			time += timeStep;

		}
		UpdatePosition ();
		Hide ();
	}

	void OnMouseClick()
	{
		Debug.Log (gameObject.name + " selected");
	}
}

