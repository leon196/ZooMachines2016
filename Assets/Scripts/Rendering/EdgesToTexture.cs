using UnityEngine;
using System.Collections;

public class EdgesToTexture : MonoBehaviour {

	public Mesh mesh;
	public Texture2D meshTexture;
	FloatTexture floatTexture;
	Texture2D colorTexture;
	public Texture texture;
	public Vector3[] vectorArray;

	// Use this for initialization
	public void Init () {
		vectorArray = Draw.GetEdgePointsFromMesh(mesh, 0f);
		// Vector3[] vectorArray = new Vector3[128];
		// for (int i = 0; i < vectorArray.Length; ++i) {
		// 	vectorArray[i] *= 5f;
		// 	vectorArray[i] = Utils.RandomVector(-10f, 10f);
			// float angle = ((float)i / (float)vectorArray.Length) * 2f * Mathf.PI;
			// float radius = 20f;
			// vectorArray[i] = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle * 10f), Mathf.Sin(angle) * radius);
		// }
		floatTexture = new FloatTexture(vectorArray);		
		// floatTexture.texture.filterMode = FilterMode.Bilinear;
		colorTexture = new Texture2D(floatTexture.resolution, floatTexture.resolution);
		colorTexture.filterMode = FilterMode.Point;
		Color[] colorArray = new Color[floatTexture.resolution * floatTexture.resolution];
		Draw.Edge[] edgeArray = Draw.GetEdges(mesh);
		Vector2[] uvs = mesh.uv;
		for (int i = 0; i < edgeArray.Length; ++i) {
			int index = edgeArray[i].i1;
			colorArray[i] = meshTexture.GetPixelBilinear(uvs[index].x, uvs[index].y);
		}
		colorTexture.SetPixels(colorArray);
		colorTexture.Apply();
		texture = floatTexture.texture;
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
