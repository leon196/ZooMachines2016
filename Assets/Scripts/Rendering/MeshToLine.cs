using UnityEngine;
using System.Collections;

public class MeshToLine : MonoBehaviour {

	public Mesh mesh;
	private Mesh meshLine;

	void Start () {
		meshLine = new Mesh();
		Vector3[] positions = mesh.vertices;
		int[] triangles = mesh.triangles;
		int[] edges = new int[triangles.Length * 3 * 2];
		int index = 0;
		for (int i = 0; i < triangles.Length; i += 3) {
			edges[index] = triangles[i];
			edges[index + 1] = triangles[i + 1]; 
			edges[index + 2] = triangles[i + 1];
			edges[index + 3] = triangles[i + 2];
			edges[index + 4] = triangles[i + 2];
			edges[index + 5] = triangles[i];
			index += 6;
		}
		meshLine.vertices = positions;
	}
	
	void Update () {
	
	}
}
