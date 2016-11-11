using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Particles : MonoBehaviour
{
	public int count = 1000;
	public Material particleMaterial;
	public Material vertexPass;
	public Material velocityPass;
	public Material elementPass;

	private FloatTexture textureVertex;
	private FloatTexture textureVelocity;
	private FloatTexture textureElement;
	private FloatTexture textureNormal;
	private FrameBuffer bufferVertex;
	private FrameBuffer bufferVelocity;
	private FrameBuffer bufferElement;
	private RenderTexture resultVertex;
	private RenderTexture resultVelocity;
	private RenderTexture resultElement;

	void Start ()
	{
		List<GameObject> particles = CreateParticles(count);
		Mesh[] meshArray = new Mesh[particles.Count];
		for (int i = 0; i < particles.Count; ++i) {
			meshArray[i] = particles[i].GetComponent<MeshFilter>().sharedMesh;
		}
		Init(meshArray);
	}

	public List<GameObject> CreateParticles (int total, float spawnRange = 100f)
	{
		int verticesMax = 65000;
		List<GameObject> meshList = new List<GameObject>();
		int index = 0;
		while (index < total)
		{
			int count = verticesMax;
			if (total < verticesMax) {
				count = total;
			} else if (total > verticesMax && Mathf.Floor(total / verticesMax) == Mathf.Floor(index / verticesMax)) {
				count = total % verticesMax;
			}

			Vector3[] vertices = new Vector3[count];
			Vector3[] normals = new Vector3[count];
			Color[] colors = new Color[count];
			int[] indices = new int[count];

			for (int i = 0; i < count && index < total; ++i) {
				vertices[i] = Utils.RandomVector(-spawnRange, spawnRange);
				normals[i] = Vector3.up;
				colors[i] = Color.black;
				indices[i] = i;
				++index;
			}

			Mesh mesh = new Mesh();
			mesh.vertices = vertices;
			mesh.normals = normals;
			mesh.colors = colors;
			mesh.SetIndices(indices, MeshTopology.Points, 0);
			mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 500f);

			GameObject meshGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
			GameObject.Destroy(meshGameObject.GetComponent<Collider>());
			meshGameObject.GetComponent<MeshFilter>().mesh = mesh;
			
			Renderer renderer = meshGameObject.GetComponent<Renderer>();
			renderer.sharedMaterial = particleMaterial;
			meshList.Add(meshGameObject);
		}
		return meshList;
	}

	public void Init (Mesh[] meshArray)
	{
		// position
		textureVertex = new FloatTexture(meshArray);
		textureVertex.PrintPosition(meshArray);
		bufferVertex = new FrameBuffer(textureVertex);

		// normal
		textureNormal = new FloatTexture(meshArray);
		textureNormal.PrintNormal(meshArray);

		// velocity
		textureVelocity = new FloatTexture(meshArray);
		textureVelocity.PrintEmpty();
		bufferVelocity = new FrameBuffer(textureVelocity);

		// element
		textureElement = new FloatTexture(meshArray);
		textureElement.PrintNoise();
		// textureElement.PrintNoiseInt(0, 10000);
		bufferElement = new FrameBuffer(textureElement);

		vertexPass.SetTexture("_VertexInitialTexture", textureVertex.texture);
		vertexPass.SetTexture("_VelocityTexture", textureVelocity.texture);
		vertexPass.SetTexture("_ElementTexture", textureElement.texture);
		velocityPass.SetTexture("_VertexInitialTexture", textureVertex.texture);
		velocityPass.SetTexture("_VertexTexture", textureVertex.texture);
		velocityPass.SetTexture("_ElementTexture", textureElement.texture);
		particleMaterial.SetTexture("_VertexTexture", textureVertex.texture);
		particleMaterial.SetTexture("_VertexInitialTexture", textureVertex.texture);
		particleMaterial.SetTexture("_VelocityTexture", textureVelocity.texture);

		resultVelocity = bufferVelocity.Apply(velocityPass);
		resultVertex = bufferVertex.Apply(vertexPass);
		resultElement = bufferElement.Apply(elementPass);
		
		Camera.onPreRender += onPreRender;
	}
	
	public void onPreRender (Camera camera)
	{
		// for editing shader live
		if (textureVertex != null) {
			vertexPass.SetTexture("_VertexInitialTexture", textureVertex.texture);
			velocityPass.SetTexture("_VertexInitialTexture", textureVertex.texture);
			particleMaterial.SetTexture("_VertexInitialTexture", textureVertex.texture);
			vertexPass.SetTexture("_NormalTexture", textureNormal.texture);
			velocityPass.SetTexture("_NormalTexture", textureNormal.texture);
			particleMaterial.SetTexture("_NormalTexture", textureNormal.texture);
			Shader.SetGlobalVector("_Resolution", textureVertex.dimension);
		}

		// velocity
		if (bufferVelocity != null && velocityPass != null) {
			resultVelocity = bufferVelocity.Apply(velocityPass);
			velocityPass.SetTexture("_VelocityTexture", resultVelocity);
			velocityPass.SetTexture("_VertexTexture", resultVertex);
			velocityPass.SetTexture("_ElementTexture", resultElement);
			particleMaterial.SetTexture("_VelocityTexture", resultVelocity);
		}

		// position
		if (bufferVertex != null && vertexPass != null) {
			resultVertex = bufferVertex.Apply(vertexPass);
			vertexPass.SetTexture("_VertexTexture", resultVertex);
			vertexPass.SetTexture("_VelocityTexture", resultVelocity);
			vertexPass.SetTexture("_ElementTexture", resultElement);
			particleMaterial.SetTexture("_VertexTexture", resultVertex);
		}

		// element
		if (bufferElement != null && elementPass != null) {
			resultElement = bufferElement.Apply(elementPass);
			elementPass.SetTexture("_VertexTexture", resultVertex);
			elementPass.SetTexture("_ElementTexture", resultElement);
			elementPass.SetTexture("_VelocityTexture", resultVelocity);
			particleMaterial.SetTexture("_ElementTexture", resultElement);
		}
	}
}
