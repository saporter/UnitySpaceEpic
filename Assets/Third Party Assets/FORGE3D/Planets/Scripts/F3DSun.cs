using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class F3DSun : MonoBehaviour
{
    public F3DPlanet[] Planets;

    public float radius;
    public float shadowStrength;

    int ShadowCasters = 20;

    public bool EnableSoftShadow;

    public float RotationRate;

    public int PlanetLayer;

    public Color sunColor;

    int sunRadiusRef, sunPosRef, shadowStrengthRef;
    int[] shadowCasterRef;

    Vector4 dummyShadowPos = new Vector4(20000f, 20000f, 20000f, 0f);
    Vector4[] shadowCasters;

    // Use this for initialization
    void Awake()
    {
        shadowCasterRef = new int[ShadowCasters];
        sunRadiusRef = Shader.PropertyToID("_SunRadius");
        sunPosRef = Shader.PropertyToID("_SunPos");
        shadowStrengthRef = Shader.PropertyToID("_ShadowStrength");

        for (int n = 1; n < ShadowCasters + 1; n++)
            shadowCasterRef[n - 1] = Shader.PropertyToID("_ShadowCasterPos_" + n.ToString());

    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.Euler(0f, RotationRate * Time.deltaTime, 0f);

        if (Planets != null)
        {
            for (int i = 0; i < Planets.Length; i++)
            {
                if (Planets[i] == null)
                {
                    Debug.LogWarning("F3DSun : Planet script has been removed from one of the objects in the scene. Please refresh.");
                    Planets = null;
                    return;
                }

                if (EnableSoftShadow)
                {
                    RaycastHit[] hits = Physics.SphereCastAll(transform.position, Mathf.Max(Planets[i].transform.localScale.x, radius), Planets[i].transform.position - transform.position, Vector3.Distance(transform.position, Planets[i].transform.position), 1 << PlanetLayer);

                    shadowCasters = new Vector4[ShadowCasters];

                    for (int n = 0; n < shadowCasters.Length; n++)
                        shadowCasters[n] = dummyShadowPos;

                    for (int n = 0; n < hits.Length && n < ShadowCasters; n++)
                    {
                        if (hits[n].transform != Planets[i].transform)
                        {
                            shadowCasters[n] = hits[n].collider.transform.position;
                            shadowCasters[n].w = hits[n].collider.transform.localScale.x / 2.0f;
                        }
                        else
                            shadowCasters[n] = dummyShadowPos;
                    }
                }
                
                Renderer[] planetRenderers = Planets[i].GetComponentsInChildren<Renderer>();

                for (int m = 0; m < planetRenderers.Length; m++)
                {
                    planetRenderers[m].sharedMaterial.SetVector(sunPosRef, transform.position);                    

                    if (EnableSoftShadow)
                    {
                        planetRenderers[m].sharedMaterial.SetFloat(sunRadiusRef, radius);

                        for (int n = 0; n < ShadowCasters; n++)
                        {
                            planetRenderers[m].sharedMaterial.SetVector(shadowCasterRef[n], shadowCasters[n]);
                            planetRenderers[m].sharedMaterial.SetFloat(shadowStrengthRef, shadowStrength);
                        }
                    }
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, transform.localScale.x * 0.55f);
    }
}
