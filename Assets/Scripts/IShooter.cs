using UnityEngine;
using System.Collections;

public interface IShooter {
	IWeapon Primary { get; set; }
	IWeapon Secondary { get; set; }
}
