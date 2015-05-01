using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PowerGridController : MonoBehaviour {
	public PlayerController1 playerShip;
	public Slider reactorPower;
	public Slider enginePower;
	public Slider phaser1;
	public Slider phaser2;
	public PhaserWeapon1 phaserWeapon1;
	public PhaserWeapon1 phaserWeapon2;

	void Start()
	{
		playerShip = playerShip == null ? GameObject.FindWithTag ("Player").GetComponent<PlayerController1>() : playerShip;
		if (playerShip == null)
			throw new UnityException ("Could not find Player GameObject (Start() in PowerGrid)");

		Rect rect = reactorPower.GetComponent<RectTransform> ().rect;

		enginePower.GetComponent<RectTransform> ().sizeDelta = new Vector2(rect.width * (playerShip.systems.EngineCapacity / playerShip.systems.PowerCapacity), rect.height);
		phaser1.GetComponent<RectTransform> ().sizeDelta = new Vector2(rect.width  * (phaserWeapon1.powerCost / playerShip.systems.PowerCapacity), rect.height);
		phaser2.GetComponent<RectTransform> ().sizeDelta = new Vector2(rect.width  * (phaserWeapon2.powerCost / playerShip.systems.PowerCapacity), rect.height);
	}

	void Update()
	{
		if(playerShip != null && playerShip.systems != null)
			reactorPower.value = (playerShip.systems.CurrentPowerLevel - playerShip.EngineCharge) / playerShip.systems.PowerCapacity;
		if(playerShip != null && playerShip.systems != null)
			enginePower.value = (playerShip.systems.CurrentEngineUse + playerShip.EngineCharge) / playerShip.systems.EngineCapacity; 
		if(phaserWeapon1 != null)
			phaser1.value = 1f - ((float)phaserWeapon1.ChargesLeft / (float)phaserWeapon1.chargesPerTurn);
		if(phaserWeapon2 != null)
			phaser2.value = 1f - ((float)phaserWeapon2.ChargesLeft / (float)phaserWeapon2.chargesPerTurn);
	}

}
