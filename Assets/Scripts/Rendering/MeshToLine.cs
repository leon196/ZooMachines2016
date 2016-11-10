using UnityEngine;
using System.Collections;

public class MeshToLine : MonoBehaviour {

	private Material material;
	private Mesh mesh;
	private Vector3[] edges;

	void Start () {
		mesh = GetComponent<MeshFilter>().sharedMesh;
		material = GetComponent<Renderer>().sharedMaterial;
		edges = Draw.GetEdgePointsFromMesh(mesh, 0f);
		// Camera.onPostRender += onPostRender;
	}
	
	void OnRenderObject () {
	// public void onPostRender (Camera cam) {
		material.SetPass(0);
		Draw.color = Color.white;
		Draw.Wireframe(transform, edges);
	}
}
