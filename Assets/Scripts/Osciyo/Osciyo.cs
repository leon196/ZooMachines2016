using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Osciyo : MonoBehaviour
{
	public int lod = 1;
	public Material material;
	public float speed = 0.05f;
	public float speedNoise = 0.001f;
	[Range(0,1)] public float damping = 0.7f;
	private Pass position;
	private Pass velocity;
	private Pass element;
	private FloatTexture edgeTexture;
	private FloatTexture colorTexture;
	private Vector3[] edgePositionArray;
	private Color[] colorArray;
	private int edgeCount;
	private float timeStart = 0f;
	private float timeDelay = 2f;

	void Start ()
	{
		// Init();
	}

	void OnEnable ()
	{
		timeStart = Time.time;
	}

	public void Init ()
	{
		List<Vector3> list = new List<Vector3>();
		List<Color> colorList = new List<Color>();
		MeshFilter[] meshFilterArray = GetComponentsInChildren<MeshFilter>();
		int r = 0;
		foreach (MeshFilter meshFilter in meshFilterArray) {
			Mesh mesh = meshFilter.mesh;
			Renderer renderer = meshFilter.GetComponent<Renderer>();
			Transform t = meshFilter.transform;
			// List<Vector3> tmp = new List<Vector3>();
			Vector3[] edges = Draw.GetEdgePointsFromMesh(mesh, 0f);
			// Debug.Log(edges.Length);
			foreach (Vector3 point in edges) {
				Vector3 p = t.TransformPoint(point);
				// if (tmp.IndexOf(p) == -1) {
					// tmp.Add(p);
					list.Add(p);
					Color color = renderer.material.color;
					colorList.Add(color);
				// }
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
		edgeCount = edgePositionArray.Length;
		edgeTexture = new FloatTexture(edgePositionArray);		
		colorTexture = new FloatTexture(colorArray);
		colorTexture.PrintColor(colorArray);

		List<GameObject> particles = Utils.CreateParticles(list.Count * lod, material);
		Mesh[] meshArray = new Mesh[particles.Count];
		for (int i = 0; i < particles.Count; ++i) {
			meshArray[i] = particles[i].GetComponent<MeshFilter>().mesh;
			particles[i].transform.parent = transform;
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
			vectorArray[i].x = i % edgeCount;
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
				
			float ratio = Mathf.Sin(Mathf.Clamp01((Time.time - timeStart) / timeDelay) * Mathf.PI);
			speedNoise = 0.1f * ratio;
			speed = 0.05f + ratio * 0.2f;

			position.material.SetVector("_Resolution", position.dimension);
			position.material.SetVector("_ResolutionEdge", edgeTexture.dimension);
			position.material.SetTexture("_EdgeTexture", edgeTexture.texture);
			position.material.SetFloat("_EdgeCount", edgeCount);
			position.material.SetVector("_ResolutionColor", colorTexture.dimension);
			position.material.SetTexture("_ColorTexture", colorTexture.texture);
			position.material.SetTexture("_VertexInitialTexture", position.texture.texture);
			
			element.material.SetVector("_Resolution", element.dimension);
			element.material.SetVector("_ResolutionEdge", edgeTexture.dimension);
			element.material.SetTexture("_EdgeTexture", edgeTexture.texture);
			element.material.SetFloat("_EdgeCount", edgeCount);
			element.material.SetVector("_ResolutionColor", colorTexture.dimension);
			element.material.SetTexture("_ColorTexture", colorTexture.texture);

			velocity.material.SetVector("_Resolution", velocity.dimension);
			velocity.material.SetVector("_ResolutionEdge", edgeTexture.dimension);
			velocity.material.SetTexture("_EdgeTexture", edgeTexture.texture);
			velocity.material.SetFloat("_EdgeCount", edgeCount);
			velocity.material.SetVector("_ResolutionColor", colorTexture.dimension);
			velocity.material.SetTexture("_ColorTexture", colorTexture.texture);
			velocity.material.SetTexture("_VertexInitialTexture", position.texture.texture);
			
			material.SetVector("_ResolutionEdge", edgeTexture.dimension);
			material.SetTexture("_EdgeTexture", edgeTexture.texture);
			material.SetFloat("_EdgeCount", edgeCount);
			material.SetVector("_ResolutionColor", colorTexture.dimension);
			material.SetTexture("_ColorTexture", colorTexture.texture);
			material.SetTexture("_VertexInitialTexture", position.texture.texture);

			// element
			element.material.SetTexture("_VertexTexture", position.result);
			element.material.SetTexture("_VelocityTexture", velocity.result);
			element.material.SetTexture("_ElementTexture", element.result);
			material.SetTexture("_ElementTexture", element.result);

			// velocity
			velocity.material.SetTexture("_VertexTexture", position.result);
			velocity.material.SetTexture("_ElementTexture", element.result);
			velocity.material.SetFloat("_GlobalSpeed", speed);
			velocity.material.SetFloat("_SpeedNoise", speedNoise);
			velocity.material.SetFloat("_Fade", damping);
			velocity.material.SetTexture("_VelocityTexture", velocity.result);
			material.SetTexture("_VelocityTexture", velocity.result);

			// position
			position.material.SetTexture("_VelocityTexture", velocity.result);
			position.material.SetTexture("_ElementTexture", element.result);
			position.material.SetTexture("_VertexTexture", position.result);
			material.SetTexture("_VertexTexture", position.result);
			
			velocity.Update();
			position.Update();
			element.Update();
		}
	}

	public void SetColor (int[] indices, Color color)
	{
		foreach (int i in indices) {
			colorArray[i] = color;
		}
	}

	public void UpdateColor ()
	{
		colorTexture.PrintColor(colorArray);
	}
}
