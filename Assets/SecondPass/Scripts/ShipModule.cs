using UnityEngine;
using System.Collections;
using System.Linq;

public class ShipModule : MonoBehaviour, IDisplayableOnGUI, IDamageable {
	[SerializeField]
	private GraphicData _selector;
	private RectTransform rect;

	public GraphicData Selector {get { return _selector; } }
	public GameObject GameObj { get { return gameObject; } }
	public GameObject destroyedEffect;
	public float health = 10f;


	void Start()
	{
		if(Selector == null)
			Debug.LogError ("Error: selector was null.  Add one as child.");

		Selector.UIElement = Instantiate (Selector.UIElement);
		Selector.UIElement.transform.SetParent (GameObject.FindWithTag ("UI").transform);
		Selector.UIElement.gameObject.GetComponent<ISelectable>().Selected = OnMouseClick;
		rect = Selector.UIElement.GetComponent<RectTransform> ();
		Hide ();

	}

	public void ApplyDamage(float damage)
	{
		health -= damage;

		if (health <= 0f) {
			GameObject effect = Instantiate(destroyedEffect, transform.position, Quaternion.identity) as GameObject;
			float destroyAfter = 3f;
			if(effect.GetComponent<ParticleSystem>())
				destroyAfter = effect.GetComponent<ParticleSystem>().duration + 1f;
			Destroy(effect, destroyAfter);
			DestroyMe();
		}
	}

	void DestroyMe()
	{
		Destroy(this.gameObject);
		Hide ();
		ShowOnPlayerMovement (false);
		Destroy (Selector.UIElement);
	}

	public void Show()
	{
		UpdatePosition ();
		Selector.UIElement.SetActive (true);
	}

	public void ShowIfVisibleFrom(Vector3 position)
	{
		Ray ray = new Ray (position, transform.position - position); 

		RaycastHit[] hitInfo = Physics.RaycastAll (ray, 100f, 1 << gameObject.layer).OrderBy(h => h.distance).ToArray();
		foreach (RaycastHit info in hitInfo) {
			if(info.collider.transform.parent.tag != "Player"){
				if(info.collider.gameObject == this.gameObject){ 
					StopCoroutine(FadeSelectorOff());
					if(Selector.UIElement.activeInHierarchy){
						rect.rotation = Quaternion.identity;
						rect.position = Vector3.MoveTowards(rect.position, 
						                                    Vector3.MoveTowards(Camera.main.WorldToScreenPoint (transform.position), Camera.main.WorldToScreenPoint (position), Selector.followRadius),
						                                    Selector.lerpSpeed * Time.deltaTime) + (Vector3)Selector.offset;
					}else
					{
						Show ();
					}
				}else{
					if(Selector.UIElement.activeInHierarchy)
						StartCoroutine (FadeSelectorOff());
				}
				return;
			}
		}

	}

	public void Hide()
	{
		Selector.UIElement.SetActive (false);
	}

	public void UpdatePosition()
	{
		rect.rotation = Quaternion.identity;
		rect.position = Camera.main.WorldToScreenPoint (transform.position) + (Vector3)Selector.offset;
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
		ShowIfVisibleFrom (e.position);
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
			                                    Selector.lerpSpeed * Time.deltaTime) + (Vector3)Selector.offset;
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
		Events.instance.Raise (new EnemyModuleSelectedEvent (this));
	}
}

