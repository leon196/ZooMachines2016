using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Osciyo : MonoBehaviour
{
	public int lod = 2;
	public Material material;
	public float speed = 0.8f;
	public float speedNoise = 0.0f;
	[Range(0,1)] public float damping = 0.1f;
	private Pass position;
	private Pass velocity;
	private Pass element;
	private FloatTexture edgeTexture;
	private FloatTexture colorTexture;
	private Vector3[] edgePositionArray;
	private Color[] colorArray;

	public void Init ()
	{
		List<Vector3> list = new List<Vector3>();
		List<Color> colorList = new List<Color>();
		MeshFilter[] meshFilterArray = GetComponentsInChildren<MeshFilter>();
		Renderer[] rendererArray = GetComponentsInChildren<Renderer>();
		int r = 0;
		foreach (MeshFilter meshFilter in meshFilterArray) {
			Mesh mesh = meshFilter.sharedMesh;
			Renderer renderer = rendererArray[r];
			Transform t = meshFilter.transform;
			Vector3[] edges = Draw.GetEdgePointsFromMesh(mesh, 0f);
			// Draw.Edge[] indices = Draw.GetEdges(mesh);
			// int index = 0;
			foreach (Vector3 point in edges) {
				list.Add(t.TransformPoint(point));
				colorList.Add(rendererArray[r].material.color);
				// colorList.Add(new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f)));
        // ++index;
			}
			renderer.enabled = false;
			++r;
		}
		colorArray = colorList.ToArray();
		Init(list);
	}

	public void Init (List<Vector3> list)
	{
		edgePositionArray = list.ToArray();
		edgeTexture = new FloatTexture(edgePositionArray);		
		colorTexture = new FloatTexture(edgeTexture.resolution);
		colorTexture.PrintColor(colorArray);

		List<GameObject> particles = Utils.CreateParticles(list.Count * lod, material);
		Mesh[] meshArray = new Mesh[particles.Count];
		for (int i = 0; i < particles.Count; ++i) {
			meshArray[i] = particles[i].GetComponent<MeshFilter>().sharedMesh;
		}
		Init(meshArray);
	}

	public void Init (Mesh[] meshArray)
	{
		position = new Pass("Filters/Position", meshArray);
		position.Print(meshArray);

		velocity = new Pass("Filters/Velocity", meshArray);

		element = new Pass("Filters/Health", meshArray);
		Vector3[] vectorArray = new Vector3[element.resolution * element.resolution];
		for (int i = 0; i < vectorArray.Length; ++i) {
			vectorArray[i].x = i % edgePositionArray.Length;
		}
		element.Print(vectorArray);

		position.material.SetTexture("_VertexInitialTexture", position.result);
		position.material.SetTexture("_VelocityTexture", velocity.result);
		position.material.SetTexture("_ElementTexture", element.result);
		velocity.material.SetTexture("_VertexInitialTexture", position.result);
		velocity.material.SetTexture("_VertexTexture", position.result);
		velocity.material.SetTexture("_ElementTexture", element.result);
		element.material.SetTexture("_VertexInitialTexture", position.result);
		element.material.SetTexture("_VertexTexture", position.result);
		element.material.SetTexture("_ElementTexture", element.result);
		material.SetTexture("_VertexTexture", position.result);
		material.SetTexture("_VertexInitialTexture", position.result);
		material.SetTexture("_VelocityTexture", velocity.result);
		material.SetTexture("_ElementTexture", element.result);
	}
	
	void Update ()
	{
		// material.SetMatrix("_Matrix", transform.localToWorldMatrix);

		if (position != null) {
			position.material.SetVector("_Resolution", position.dimension);
			velocity.material.SetVector("_Resolution", velocity.dimension);
			element.material.SetVector("_Resolution", element.dimension);
			position.material.SetVector("_ResolutionEdge", edgeTexture.dimension);
			velocity.material.SetVector("_ResolutionEdge", edgeTexture.dimension);
			element.material.SetVector("_ResolutionEdge", edgeTexture.dimension);
			material.SetVector("_ResolutionEdge", edgeTexture.dimension);

			position.material.SetTexture("_EdgeTexture", edgeTexture.texture);
			velocity.material.SetTexture("_EdgeTexture", edgeTexture.texture);
			element.material.SetTexture("_EdgeTexture", edgeTexture.texture);
			material.SetTexture("_EdgeTexture", edgeTexture.texture);

			position.material.SetTexture("_ColorTexture", colorTexture.texture);
			velocity.material.SetTexture("_ColorTexture", colorTexture.texture);
			element.material.SetTexture("_ColorTexture", colorTexture.texture);
			material.SetTexture("_ColorTexture", colorTexture.texture);

			position.material.SetTexture("_VertexInitialTexture", position.texture.texture);
			velocity.material.SetTexture("_VertexInitialTexture", position.texture.texture);
			material.SetTexture("_VertexInitialTexture", position.texture.texture);
			// material.SetTexture("_ColorTexture", colorTexture.texture);

			// velocity
			velocity.material.SetTexture("_VertexTexture", position.result);
			velocity.material.SetTexture("_ElementTexture", element.result);
			velocity.material.SetFloat("_GlobalSpeed", speed);
			velocity.material.SetFloat("_SpeedNoise", speedNoise);
			velocity.material.SetFloat("_Fade", damping);
			velocity.Update();
			velocity.material.SetTexture("_VelocityTexture", velocity.result);
			material.SetTexture("_VelocityTexture", velocity.result);

			// position
			position.material.SetTexture("_VelocityTexture", velocity.result);
			position.material.SetTexture("_ElementTexture", element.result);
			position.Update();
			position.material.SetTexture("_VertexTexture", position.result);
			material.SetTexture("_VertexTexture", position.result);

			// element
			element.material.SetTexture("_VertexTexture", position.result);
			element.material.SetTexture("_VelocityTexture", velocity.result);
			element.Update();
			element.material.SetTexture("_ElementTexture", element.result);
			material.SetTexture("_ElementTexture", element.result);
		}
	}
}
