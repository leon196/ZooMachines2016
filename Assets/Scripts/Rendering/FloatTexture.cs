using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class FloatTexture
{
	[ThreadStatic]

	public Color[] colorArray;
	public Texture2D texture;
	public int resolution;
	
	public FloatTexture (int resolution_)
	{
		resolution = resolution_;	
		texture = new Texture2D(resolution, resolution, TextureFormat.RGBAFloat, false);
		texture.filterMode = FilterMode.Point;
		colorArray = new Color[resolution * resolution];
		PrintEmpty();
	}
	
	public FloatTexture (Mesh[] meshArray)
	{
		int vertexCount = 0;
		for (int index = 0; index < meshArray.Length; ++index) {
			vertexCount += meshArray[index].vertices.Length;
		}
		resolution = (int)Utils.GetNearestPowerOfTwo(Mathf.Sqrt(vertexCount));
		texture = new Texture2D(resolution, resolution, TextureFormat.RGBAFloat, false);
		texture.filterMode = FilterMode.Point;
		colorArray = new Color[resolution * resolution];
		SetupUV(meshArray);
	}

	public void PrintPosition (Mesh[] meshArray)
	{
		if (texture != null) {
			int vertexIndex = 0;
			int meshCount = meshArray.Length;
			for (int meshIndex = 0; meshIndex < meshCount; ++meshIndex) {
				Mesh mesh = meshArray[meshIndex];
				Vector3 p;
				Vector3[] vertices = mesh.vertices;
				for (int i = 0; i < vertices.Length; ++i) {
					p = vertices[i];
					colorArray[vertexIndex].r = p.x;
					colorArray[vertexIndex].g = p.y;
					colorArray[vertexIndex].b = p.z;
					++vertexIndex;
				}
			}
			texture.SetPixels(colorArray);
			texture.Apply();
		}
	}

	public void PrintNormal (Mesh[] meshArray)
	{
		if (texture != null) {
			int vertexIndex = 0;
			int meshCount = meshArray.Length;
			for (int meshIndex = 0; meshIndex < meshCount; ++meshIndex) {
				Mesh mesh = meshArray[meshIndex];
				Vector3 n;
				Vector3[] normals = mesh.normals;
				for (int i = 0; i < normals.Length; ++i) {
					n = normals[i];
					colorArray[vertexIndex].r = n.x;
					colorArray[vertexIndex].g = n.y;
					colorArray[vertexIndex].b = n.z;
					++vertexIndex;
				}
			}
			texture.SetPixels(colorArray);
			texture.Apply();
		}
	}

	public void PrintVertexColor (Mesh[] meshArray)
	{
		if (texture != null) {
			int vertexIndex = 0;
			int meshCount = meshArray.Length;
			for (int meshIndex = 0; meshIndex < meshCount; ++meshIndex) {
				Mesh mesh = meshArray[meshIndex];
				Color[] colors = mesh.colors;
				for (int i = 0; i < colors.Length; ++i) {
					colorArray[vertexIndex].r = colors[i].r;
					colorArray[vertexIndex].g = colors[i].g;
					colorArray[vertexIndex].b = colors[i].b;
					++vertexIndex;
				}
			}
			texture.SetPixels(colorArray);
			texture.Apply();
		}
	}

	public void PrintEmpty ()
	{
		if (texture != null) {
			for (int i = 0; i < colorArray.Length; ++i) {
				colorArray[i].r = 0f;
				colorArray[i].g = 0f;
				colorArray[i].b = 0f;
			}
			texture.SetPixels(colorArray);
			texture.Apply();
		}
	}

	public void PrintNoise ()
	{
		if (texture != null) {
			for (int i = 0; i < colorArray.Length; ++i) {
				colorArray[i].r = UnityEngine.Random.Range(0f, 1f);
				colorArray[i].g = UnityEngine.Random.Range(0f, 1f);
				colorArray[i].b = UnityEngine.Random.Range(0f, 1f);
				colorArray[i].a = UnityEngine.Random.Range(0f, 1f);
			}
			texture.SetPixels(colorArray);
			texture.Apply();
		}
	}

	public void SetupUV (Mesh[] meshArray)
	{
		if (texture != null) {
			int vertexIndex = 0;
			int meshCount = meshArray.Length;
			for (int meshIndex = 0; meshIndex < meshCount; ++meshIndex) {
				Mesh mesh = meshArray[meshIndex];
				Vector2[] uvs2 = new Vector2[mesh.vertices.Length];
				for (int i = 0; i < uvs2.Length; ++i) {
					float x = vertexIndex % resolution;
					float y = Mathf.Floor(vertexIndex / (float)resolution);
					uvs2[i] = new Vector2(x, y) / (float)resolution;
					++vertexIndex;
				}
				mesh.uv2 = uvs2;
			}
		}
	}
}