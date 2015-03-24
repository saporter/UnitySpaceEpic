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

		enginePower.GetComponent<RectTransform> ().sizeDelta = new Vector2(rect.width * (playerShip.EngineCapacity / playerShip.PowerCapacity), rect.height);
		phaser1.GetComponent<RectTransform> ().sizeDelta = new Vector2(rect.width  * (phaserWeapon1.powerCost / playerShip.PowerCapacity), rect.height);
		phaser2.GetComponent<RectTransform> ().sizeDelta = new Vector2(rect.width  * (phaserWeapon2.powerCost / playerShip.PowerCapacity), rect.height);
	}

	void Update()
	{
		reactorPower.value = playerShip.PowerLevel / playerShip.PowerCapacity;
		enginePower.value = playerShip.EngineUse / playerShip.EngineCapacity; 
		phaser1.value = 1f - ((float)phaserWeapon1.ChargesLeft / (float)phaserWeapon1.chargesPerTurn);
		phaser2.value = 1f - ((float)phaserWeapon2.ChargesLeft / (float)phaserWeapon2.chargesPerTurn);
	}

}
