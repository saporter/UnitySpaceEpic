using UnityEngine;
using System.Collections;

public class TargetingSystem : MonoBehaviour {

	// One selection
	public PhaserWeapon[] phasers;
	private BoundBoxes_BoundBox selector;


	private int enemyMask;
	private float camRayLength;
	private Vector3 hitPoint;
	private PlayerController playerShip;
	private GameObject target;
	
	void Start () {
		enemyMask = LayerMask.GetMask ("Enemy");
		camRayLength = 300.0f;
		playerShip = playerShip == null ? GameObject.FindWithTag ("Player").GetComponent<PlayerController>() : playerShip;
		if (playerShip == null)
			throw new UnityException ("Could not find Player GameObject");

		Events.instance.AddListener<ShipSelectedEvent> (OnEnemyTargeted);
		Events.instance.AddListener<FireWeaponKeyPressEvent> (OnWeaponFire);
		Events.instance.AddListener<TransitionToEnemyTurnEvent> (ClearTargeted);
	}

	void Update () {
		if (target != null) {
			Vector3 rot = (target.transform.position - transform.position).normalized;
			Quaternion newRot = Quaternion.LookRotation (new Vector3 (rot.x, 0f, rot.z));
			if(newRot != transform.rotation){
				if(selector != null) selector.enabled = false;
				transform.rotation = newRot;
			}
		}
	}

	public GameObject ScreenSelection(Ray ray)
	{
		RaycastHit enemySelected;
		GameObject enemyToReturn = null;
		if (Physics.Raycast (ray, out enemySelected, camRayLength, enemyMask)) 
		{
			BoundBoxes_BoundBox newSelector = enemySelected.collider.GetComponent<BoundBoxes_BoundBox>();
			if(selector != null){
				selector.enabled = false;
			}

			selector = newSelector;

			if(selector != null){
				selector.enabled = true;
				hitPoint = enemySelected.point;
			}

			enemyToReturn = enemySelected.collider.gameObject;
		}

		return enemyToReturn;
	}

	void OnEnemyTargeted(ShipSelectedEvent e)
	{

		if (LayerMask.GetMask(e.selected.tag) == enemyMask) { 
			target = e.selected;
			Vector3 rot = (target.transform.position - transform.position).normalized;
			transform.rotation = Quaternion.LookRotation (new Vector3 (rot.x, 0f, rot.z));
		}
	}

	void OnWeaponFire(FireWeaponKeyPressEvent e) 
	{
		if(selector && selector.enabled){
			foreach(PhaserWeapon p in phasers)
			{
				if(p.CanFire(hitPoint) && playerShip.PowerLevel >= p.powerCost)
				{
					playerShip.FireWeapon(p, selector.gameObject, hitPoint);
				}
			}
		}
	}

	void ClearTargeted(TransitionToEnemyTurnEvent e)
	{
		if(selector != null) selector.enabled = false;
	}
}
