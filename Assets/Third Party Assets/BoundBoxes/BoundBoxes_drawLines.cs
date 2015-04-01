using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BoundBoxes_drawLines : MonoBehaviour {
	static Material lineMaterial;
	public Color lColor;
	List<Vector3[,]> outlines;
	public List<Color> colors;
	// Use this for initialization

	void Awake () {
		outlines = new List<Vector3[,]>();
		colors = new List<Color>();
		CreateLineMaterial();
	}
	
	void Start () {
//		outlines = new List<Vector3[,]>();
//		colors = new List<Color>();
//		CreateLineMaterial();
	}

	static void CreateLineMaterial()
{
    if( !lineMaterial ) {
        lineMaterial = new Material( "Shader \"Lines/Colored Blended\" {" +
            "SubShader { Pass { " +
            "    Blend SrcAlpha OneMinusSrcAlpha " +
            "    ZWrite Off Cull Off Fog { Mode Off } " +
            "    BindChannels {" +
            "      Bind \"vertex\", vertex Bind \"color\", color }" +
            "} } }" );
        lineMaterial.hideFlags = HideFlags.HideAndDontSave;
        lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
    }
}

	void OnPostRender() {
		if(outlines==null) return;
	    CreateLineMaterial();
	    lineMaterial.SetPass( 0 );
	    GL.Begin( GL.LINES );
		for (int j=0; j<outlines.Count; j++) {
			GL.Color(colors[j]);
			for (int i=0; i<outlines[j].GetLength(0); i++) {
				GL.Vertex(outlines[j][i,0]);
				GL.Vertex(outlines[j][i,1]);
			}
		}
		GL.End();
	}
		
	public void setOutlines(Vector3[,] newOutlines, Color newcolor) {
		if(newOutlines.GetLength(0)>0)	{
			outlines.Add(newOutlines);
			//Debug.Log ("no "+newOutlines.GetLength(0).ToString());
			colors.Add(newcolor);
		}
	}	
	
	// Update is called once per frame
	void Update () {
		outlines = new List<Vector3[,]>();
		colors = new List<Color>();
	}
}

public class CameraSingleton
{
	private BoundBoxes_drawLines staticCamera;
	public BoundBoxes_drawLines StaticCamera
	{
		get { return staticCamera; }
		set
		{
			if(staticCamera != null)
			{
				throw new UnityException("StaticCamera was assigned twice.  BoundBox_BoundBox currently only works with one camera.");
			}
			staticCamera = value;
		}
	}
}
