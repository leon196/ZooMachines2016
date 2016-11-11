using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Osciyo : MonoBehaviour
{
	public int lod = 1;
	public Material material;
	private Pass position;
	private Pass velocity;
	private Pass element;
	private EdgesToTexture edgesToTexture;

	void Start ()
	{
		Vector3[] points = GetComponent<MeshFilter>().sharedMesh.vertices;
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
		edgesToTexture = GameObject.FindObjectOfType<EdgesToTexture>();
		edgesToTexture.Init();
		Vector3[] edges = edgesToTexture.vectorArray;
		Vector3[] vectorArray = new Vector3[element.resolution * element.resolution];
		for (int i = 0; i < vectorArray.Length; ++i) {
			vectorArray[i].x = i % edges.Length;
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
		position.material.SetTexture("_VertexInitialTexture", position.texture.texture);
		velocity.material.SetTexture("_VertexInitialTexture", position.texture.texture);
		material.SetTexture("_VertexInitialTexture", position.texture.texture);
		Shader.SetGlobalVector("_Resolution", position.dimension);

		// velocity
		velocity.Update();
		velocity.material.SetTexture("_VelocityTexture", velocity.result);
		velocity.material.SetTexture("_VertexTexture", position.result);
		velocity.material.SetTexture("_ElementTexture", element.result);
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
