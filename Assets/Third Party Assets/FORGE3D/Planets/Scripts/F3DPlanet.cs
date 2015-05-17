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
    //Vector3 pointToPlanetDir;

    // Use this for initialization
    void Awake ()
    {
        if (OrbitPoint)
        {
            orbitRadius = Vector3.Distance(transform.position, OrbitPoint.position);
			orbitAxis = FlattenOrbit ? Vector3.up : transform.up;
            //pointToPlanetDir = Vector3.Normalize(transform.position - OrbitPoint.position);
        }
		timeAsleep = Time.timeSinceLevelLoad;
		if (GetComponent<Rigidbody> () == null) {
			Rigidbody r = gameObject.AddComponent<Rigidbody>();
			r.isKinematic = true;
		}
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
    
    // Update is called once per frame
    void FixedUpdate ()
    {
        transform.rotation *= Quaternion.Euler(0f, RotationRate * Time.deltaTime, 0f);

        if (OrbitPoint)
        {
//            pointToPlanetDir = Quaternion.AngleAxis(OrbitRate * Time.deltaTime, orbitAxis) * pointToPlanetDir;
//            transform.position = OrbitPoint.position + pointToPlanetDir * distToOrbitPoint;
			//transform.RotateAround(OrbitPoint.position, orbitAxis, ((OrbitRate / Mathf.Deg2Rad) / orbitRadius) * Time.deltaTime);//OrbitRate * Time.deltaTime);
			OrbitAroundPoint(Time.fixedDeltaTime);
        }
    }

	public void OrbitAroundPoint(float time)
	{
		transform.RotateAround(OrbitPoint.position, orbitAxis, ((OrbitRate / Mathf.Deg2Rad) / orbitRadius) * time);//OrbitRate * Time.deltaTime);
	}
}
