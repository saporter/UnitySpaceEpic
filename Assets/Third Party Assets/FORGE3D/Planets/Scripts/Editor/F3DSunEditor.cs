using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(F3DSun))]
public class F3DSunEditor : Editor
{
    [MenuItem("FORGE3D/Planets/Add F3DSun")]
    static void AddSunScript()
    {
        Selection.activeGameObject.AddComponent<F3DSun>();
    }

    [MenuItem("FORGE3D/Planets/Add F3DSun", true, 0)]
    static bool ValidateSunScript()
    {
        if (Selection.activeGameObject != null && !Selection.activeGameObject.GetComponent<F3DSun>())
            return true;
        else return false;
    }

    public override void OnInspectorGUI()
    {
        F3DSun myTarget = (F3DSun)target;

        EditorGUILayout.BeginVertical(); // BEGIN

        //
        GUIStyle smallFont = new GUIStyle();
        smallFont.fontSize = 9;
        smallFont.wordWrap = true;

        smallFont.normal.textColor = new Color(0.7f, 0.7f, 0.7f);

        GUIStyle headerFont = new GUIStyle();
        headerFont.fontSize = 11;
        headerFont.fontStyle = FontStyle.Bold;
        headerFont.normal.textColor = new Color(0.75f, 0.75f, 0.75f);

        GUIStyle subHeaderFont = new GUIStyle();
        subHeaderFont.fontSize = 10;
        subHeaderFont.fontStyle = FontStyle.Bold;
        subHeaderFont.margin = new RectOffset(1, 0, 0, 0);
        subHeaderFont.padding = new RectOffset(1, 0, 3, 0);
        subHeaderFont.normal.textColor = new Color(0.70f, 0.70f, 0.70f);
        //

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Sun lighting:", headerFont);

        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.BeginVertical("Box");
        
        if(myTarget.Planets != null)
            EditorGUILayout.LabelField("Soft shadowed objects in scene: " + myTarget.Planets.Length, smallFont);
        else
            EditorGUILayout.LabelField("No objects. Try to refresh.", smallFont);

        myTarget.PlanetLayer = EditorGUILayout.LayerField("Planet layer:", myTarget.PlanetLayer);

        if (GUILayout.Button("Refresh"))
        {
            myTarget.Planets = FindObjectsOfType<F3DPlanet>();
            Debug.Log("F3DSun: Updated " + myTarget.Planets.Length + " objects.");
        }
        EditorGUILayout.EndVertical();

        myTarget.EnableSoftShadow = EditorGUILayout.Toggle("Enable soft shadows", myTarget.EnableSoftShadow);        
        myTarget.shadowStrength = EditorGUILayout.Slider("Shadow strength:", myTarget.shadowStrength, 0.0f, 1.0f);
        myTarget.radius = EditorGUILayout.Slider("Sun radius:", myTarget.radius, 0.1f, 5f);
		myTarget.RotationRate = EditorGUILayout.FloatField("Rotation Rate:", myTarget.RotationRate);


        EditorGUILayout.EndVertical();
        EditorGUILayout.EndVertical(); // END

        EditorUtility.SetDirty(myTarget);
    }	
}
