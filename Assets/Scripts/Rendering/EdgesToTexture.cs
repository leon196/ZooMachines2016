using UnityEngine;
using System.Collections;

public class EdgesToTexture : MonoBehaviour {

	public Mesh mesh;
	FloatTexture floatTexture;

	// Use this for initialization
	void Start () {
		Vector3[] vectorArray = Draw.GetEdgePointsFromMesh(mesh, 0f);
		// Vector3[] vectorArray = new Vector3[128];
		// for (int i = 0; i < vectorArray.Length; ++i) {
		// 	vectorArray[i] *= 5f;
		// 	vectorArray[i] = Utils.RandomVector(-10f, 10f);
			// float angle = ((float)i / (float)vectorArray.Length) * 2f * Mathf.PI;
			// float radius = 20f;
			// vectorArray[i] = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle * 10f), Mathf.Sin(angle) * radius);
		// }
		floatTexture = new FloatTexture(vectorArray);		
	}
	
	// Update is called once per frame
	void Update () {
		Shader.SetGlobalTexture("_EdgeTexture", floatTexture.texture);
		Shader.SetGlobalVector("_ResolutionEdge", floatTexture.dimension);
	}
}
