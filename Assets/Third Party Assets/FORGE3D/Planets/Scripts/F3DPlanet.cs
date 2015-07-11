using UnityEngine;
using System.Collections;

public class F3DPlanet : MonoBehaviour
{
	public bool FlattenOrbit;

    public float RotationRate;
    public float OrbitRate;
    public Transform OrbitPoint;

    public bool ShowOrbit;

    float orbitRadius;
    Vector3 orbitAxis;
	float timeAsleep;

    // Use this for initialization
    void Awake ()
    {
		Events.instance.AddListener<LoadGameEvent> (NewGameLoaded);

        if (OrbitPoint)
        {
            orbitRadius = Vector3.Distance(transform.position, OrbitPoint.position);
			orbitAxis = FlattenOrbit ? Vector3.up : transform.up;
        }
		timeAsleep = Time.timeSinceLevelLoad;
		if (GetComponent<Rigidbody> () == null) {
			Rigidbody r = gameObject.AddComponent<Rigidbody>();
			r.isKinematic = true;
		}
    }

	void OnDestroy()
	{
		Events.instance.RemoveListener<LoadGameEvent> (NewGameLoaded);
	}

	void OnEnable()
	{
		timeAsleep = Mathf.Abs (timeAsleep - Time.timeSinceLevelLoad);
		OrbitAroundPoint (timeAsleep);
	}

	void OnDisable()
	{
		timeAsleep = Time.timeSinceLevelLoad;
	}
    
    void FixedUpdate ()
    {
        transform.rotation *= Quaternion.Euler(0f, RotationRate * Time.deltaTime, 0f);

        if (OrbitPoint)
        {
			OrbitAroundPoint(Time.fixedDeltaTime);
        }
    }

	public void OrbitAroundPoint(float time)
	{
		transform.RotateAround(OrbitPoint.position, orbitAxis, ((OrbitRate / Mathf.Deg2Rad) / orbitRadius) * time);//OrbitRate * Time.deltaTime);
	}

	void NewGameLoaded (LoadGameEvent e)
	{
		OrbitAroundPoint (GameManager.GM.GameTime);
	}
}
