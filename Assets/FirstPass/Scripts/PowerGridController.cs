using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PowerGridController : MonoBehaviour {
	public PlayerController playerShip;
	public Slider reactorPower;
	public Slider enginePower;
	public Slider phaser1;
	public Slider phaser2;
	public PhaserWeapon phaserWeapon1;
	public PhaserWeapon phaserWeapon2;

	void Start()
	{
		playerShip = playerShip == null ? GameObject.FindWithTag ("Player").GetComponent<PlayerController>() : playerShip;
		if (playerShip == null)
			throw new UnityException ("Could not find Player GameObject (Start() in PowerGrid)");

		Rect rect = reactorPower.GetComponent<RectTransform> ().rect;

		enginePower.GetComponent<RectTransform> ().sizeDelta = new Vector2(rect.width * (playerShip.systems.EngineCapacity / playerShip.systems.PowerCapacity), rect.height);
		phaser1.GetComponent<RectTransform> ().sizeDelta = new Vector2(rect.width  * (phaserWeapon1.powerCost / playerShip.systems.PowerCapacity), rect.height);
		phaser2.GetComponent<RectTransform> ().sizeDelta = new Vector2(rect.width  * (phaserWeapon2.powerCost / playerShip.systems.PowerCapacity), rect.height);
	}

	void Update()
	{
		reactorPower.value = (playerShip.systems.CurrentPowerLevel - playerShip.EngineCharge) / playerShip.systems.PowerCapacity;
		enginePower.value = (playerShip.systems.CurrentEngineUse + playerShip.EngineCharge) / playerShip.systems.EngineCapacity; 
		phaser1.value = 1f - ((float)phaserWeapon1.ChargesLeft / (float)phaserWeapon1.chargesPerTurn);
		phaser2.value = 1f - ((float)phaserWeapon2.ChargesLeft / (float)phaserWeapon2.chargesPerTurn);
	}

}
