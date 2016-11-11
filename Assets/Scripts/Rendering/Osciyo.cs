using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Osciyo : MonoBehaviour
{
	public int lod = 1;
	public Material material;
	public float speed = 0.02f;
	[Range(0,1)] public float damping = 0.9f;
	private Pass position;
	private Pass velocity;
	private Pass element;
	private FloatTexture edgeTexture;
	private Vector3[] edgePositionArray;

	void Start ()
	{
		Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
		edgePositionArray = Draw.GetEdgePointsFromMesh(mesh, 0f);
		edgeTexture = new FloatTexture(edgePositionArray);		

		Vector3[] points = mesh.vertices;
		GetComponent<Renderer>().enabled = false;
		List<GameObject> particles = Utils.CreateParticles(points.Length * lod, material);
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
		material.SetTexture("_VertexTexture", position.result);
		material.SetTexture("_VertexInitialTexture", position.result);
		material.SetTexture("_VelocityTexture", velocity.result);
	}
	
	void Update ()
	{
		material.SetMatrix("_Matrix", transform.localToWorldMatrix);

		position.material.SetVector("_Resolution", position.dimension);
		velocity.material.SetVector("_Resolution", velocity.dimension);
		element.material.SetVector("_Resolution", element.dimension);
		position.material.SetVector("_ResolutionEdge", edgeTexture.dimension);
		velocity.material.SetVector("_ResolutionEdge", edgeTexture.dimension);
		element.material.SetVector("_ResolutionEdge", edgeTexture.dimension);

		position.material.SetTexture("_EdgeTexture", edgeTexture.texture);
		velocity.material.SetTexture("_EdgeTexture", edgeTexture.texture);
		element.material.SetTexture("_EdgeTexture", edgeTexture.texture);

		position.material.SetTexture("_VertexInitialTexture", position.texture.texture);
		velocity.material.SetTexture("_VertexInitialTexture", position.texture.texture);
		material.SetTexture("_VertexInitialTexture", position.texture.texture);

		// velocity
		velocity.Update();
		velocity.material.SetTexture("_VelocityTexture", velocity.result);
		velocity.material.SetTexture("_VertexTexture", position.result);
		velocity.material.SetTexture("_ElementTexture", element.result);
		velocity.material.SetFloat("_GlobalSpeed", speed);
		velocity.material.SetFloat("_Fade", damping);
		material.SetTexture("_VelocityTexture", velocity.result);

		// position
		position.Update();
		position.material.SetTexture("_VertexTexture", position.result);
		position.material.SetTexture("_VelocityTexture", velocity.result);
		position.material.SetTexture("_ElementTexture", element.result);
		material.SetTexture("_VertexTexture", position.result);

		// element
		element.Update();
		element.material.SetTexture("_VertexTexture", position.result);
		element.material.SetTexture("_ElementTexture", element.result);
		element.material.SetTexture("_VelocityTexture", velocity.result);
		material.SetTexture("_ElementTexture", element.result);
	}
}
