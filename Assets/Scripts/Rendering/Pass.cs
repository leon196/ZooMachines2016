using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pass
{
	public Material material;
	public FloatTexture texture;
	public FrameBuffer buffer;
	public RenderTexture result;
	public int resolution;
	public Vector2 dimension;

	public Pass (string shaderName, Mesh[] meshArray)
	{
		material = new Material(Shader.Find(shaderName));
		texture = new FloatTexture(meshArray);
		texture.PrintEmpty();
		buffer = new FrameBuffer(texture);
		resolution = texture.resolution;
		dimension = texture.dimension;
		Update();
	}

	public void Print (Mesh[] meshArray)
	{
		texture.PrintPosition(meshArray);
		buffer.Print(texture.texture);
	}

	public void Print (Vector3[] array)
	{
		texture.PrintVectorArray(array);
		buffer.Print(texture.texture);
	}

	public void Update ()
	{
		result = buffer.Apply(material);
	}
}