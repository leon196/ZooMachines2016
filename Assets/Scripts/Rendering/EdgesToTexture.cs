using UnityEngine;
using System.Collections;

public class EdgesToTexture : MonoBehaviour {

	private Mesh mesh;
	private FloatTexture floatTexture;
	private Texture2D colorTexture;
	public Texture texture;
	public Vector3[] vectorArray;

	// Use this for initialization
	public void Init () {
		mesh = GetComponent<MeshFilter>().sharedMesh;
		vectorArray = Draw.GetEdgePointsFromMesh(mesh, 0f);
		floatTexture = new FloatTexture(vectorArray);		
		texture = floatTexture.texture;

		colorTexture = new Texture2D(floatTexture.resolution, floatTexture.resolution);
		colorTexture.filterMode = FilterMode.Point;
		Color[] colorArray = new Color[floatTexture.resolution * floatTexture.resolution];
		Draw.Edge[] edgeArray = Draw.GetEdges(mesh);
		Vector2[] uvs = mesh.uv;
		for (int i = 0; i < edgeArray.Length; ++i) {
			int index = edgeArray[i].i1;
			// colorArray[i] = meshTexture.GetPixelBilinear(uvs[index].x, uvs[index].y);
		}
		colorTexture.SetPixels(colorArray);
		colorTexture.Apply();
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < vectorArray.Length; ++i) {
			Vector3 p = transform.TransformPoint(vectorArray[i]);
			floatTexture.colorArray[i].r = p.x;
			floatTexture.colorArray[i].g = p.y;
			floatTexture.colorArray[i].b = p.z;
		}
		floatTexture.texture.SetPixels(floatTexture.colorArray);
		floatTexture.texture.Apply();

		Shader.SetGlobalTexture("_EdgeTexture", floatTexture.texture);
		Shader.SetGlobalTexture("_ColorTexture", colorTexture);
		Shader.SetGlobalVector("_ResolutionEdge", floatTexture.dimension);
	}
}
