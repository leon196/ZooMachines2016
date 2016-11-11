using UnityEngine;
using System;
using System.Collections.Generic;

public static class Utils
{
	public static T[] RandomizeArray<T> (T[] array)
	{
		for (int i = array.Length - 1; i > 0; --i) {
		int r = UnityEngine.Random.Range(0,i);
		T tmp = array[i];
		array[i] = array[r];
		array[r] = tmp;
		}
		return array;
	}

	public static List<GameObject> CreateParticles (int total, Material material)
	{
		int verticesMax = 65000;
		List<GameObject> meshList = new List<GameObject>();
		int index = 0;
		// int total = points.Length;
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
				vertices[i] = Utils.RandomVector(-10f, 10f);
				normals[i] = Vector3.up;
				colors[i] = Color.black;
				indices[i] = i;
				++index;
			}

			Mesh mesh = new Mesh();
			mesh.vertices = vertices;
			mesh.normals = normals;
			mesh.colors = colors;
			mesh.SetIndices(indices, MeshTopology.LineStrip, 0);
			mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 500f);

			GameObject meshGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
			GameObject.Destroy(meshGameObject.GetComponent<Collider>());
			meshGameObject.GetComponent<MeshFilter>().mesh = mesh;
			
			Renderer renderer = meshGameObject.GetComponent<Renderer>();
			renderer.sharedMaterial = material;
			meshList.Add(meshGameObject);
		}
		return meshList;
	}

	public static Vector3 RandomVector (float min, float max)
	{
		return new Vector3(UnityEngine.Random.Range(min, max), UnityEngine.Random.Range(min, max), UnityEngine.Random.Range(min, max));
	}

	public static float TriangleArea (Vector3 a, Vector3 b, Vector3 c)
	{
		return Vector3.Cross(a - b, a - c).magnitude / 2f;
	}
	
	public static Vector3 Vec3Lerp (Vector3 a, Vector3 b, Vector3 t)
	{
		return new Vector3(Mathf.Lerp(a.x, b.x, t.x), Mathf.Lerp(a.y, b.y, t.y), Mathf.Lerp(a.z, b.z, t.z));
	}
	
	public static Vector3 Vec3InverseLerp (Vector3 a, Vector3 b, Vector3 t)
	{
		return new Vector3(Mathf.InverseLerp(a.x, b.x, t.x), Mathf.InverseLerp(a.y, b.y, t.y), Mathf.InverseLerp(a.z, b.z, t.z));
	}

	// http://stackoverflow.com/questions/466204/rounding-up-to-nearest-power-of-2
	public static float GetNearestPowerOfTwo (float x)
	{
		return Mathf.Pow(2f, Mathf.Ceil(Mathf.Log(x) / Mathf.Log(2f)));
	}

	// Thank to Tomas Akenine-Möller
	// For sharing his Triangle Box Overlaping algorithm
	// http://fileadmin.cs.lth.se/cs/Personal/Tomas_Akenine-Moller/code/tribox3.txt

	public static int PlaneBoxOverlap (Vector3 normal, Vector3 vert, Vector3 maxbox)
	{
		Vector3 vmin = Vector3.zero;
		Vector3 vmax = Vector3.zero;
		if (normal.x > 0) { vmin.x = -maxbox.x - vert.x; vmax.x = maxbox.x - vert.x;	}
		else { vmin.x = maxbox.x - vert.x; vmax.x = -maxbox.x - vert.x; }
		if (normal.y > 0) { vmin.y = -maxbox.y - vert.y; vmax.y = maxbox.y - vert.y;	}
		else { vmin.y = maxbox.y - vert.y; vmax.y = -maxbox.y - vert.y; }
		if (normal.z > 0) { vmin.z = -maxbox.z - vert.z; vmax.z = maxbox.z - vert.z;	}
		else { vmin.z = maxbox.z - vert.z; vmax.z = -maxbox.z - vert.z; }
		Vector3 min = new Vector3(normal.x, normal.y, normal.z);
		Vector3 max = new Vector3(normal.x, normal.y, normal.z);
		if (Vector3.Dot(min, vmin) > 0) return 0;	
		if (Vector3.Dot(max, vmax) >= 0) return 1;	
		return 0;
	}

	public static int TriBoxOverlap (Vector3 boxcenter, Vector3 boxhalfsize, Vector3 a, Vector3 b, Vector3 c)
	{
		Vector3 v0 = Vector3.zero;
		Vector3 v1 = Vector3.zero;
		Vector3 v2 = Vector3.zero;
		float min, max, p0, p1, p2, rad, fex, fey, fez;
		Vector3 normal = Vector3.zero;
		Vector3 e0 = Vector3.zero;
		Vector3 e1 = Vector3.zero;
		Vector3 e2 = Vector3.zero;

		/* This is the fastest branch on Sun */
		/* move everything so that the boxcenter is in (0,0,0) */
		v0.x = a.x - boxcenter.x; v0.y = a.y - boxcenter.y; v0.z = a.z - boxcenter.z;
		v1.x = b.x - boxcenter.x; v1.y = b.y - boxcenter.y; v1.z = b.z - boxcenter.z;
		v2.x = c.x - boxcenter.x; v2.y = c.y - boxcenter.y; v2.z = c.z - boxcenter.z;
		/* compute triangle edges */
		e0.x = v1.x - v0.x; e0.y = v1.y - v0.y; e0.z = v1.z - v0.z;
		e1.x = v2.x - v1.x; e1.y = v2.y - v1.y; e1.z = v2.z - v1.z;
		e2.x = v0.x - v2.x; e2.y = v0.y - v2.y; e2.z = v0.z - v2.z;

		/* Bullet 3:  */
		/*  test the 9 tests first (this was faster) */
		fex = Mathf.Abs(e0.x);
		fey = Mathf.Abs(e0.y);
		fez = Mathf.Abs(e0.z);
		//
		p0 = e0.z * v0.y - e0.y * v0.z;
		p2 = e0.z * v2.y - e0.y * v2.z;
		if (p0 < p2) { min = p0; max = p2; } else { min = p2; max = p0; }  
		rad = fez * boxhalfsize.y + fey * boxhalfsize.z;
		if (min > rad || max < -rad) return 0;
		//
		p0 = -e0.z * v0.x + e0.x * v0.z;
		p2 = -e0.z * v2.x + e0.x * v2.z;	
	    if(p0 < p2) { min = p0; max = p2; } else { min = p2; max = p0; }
		rad = fez * boxhalfsize.x + fex * boxhalfsize.z;
		if (min > rad || max < -rad) return 0;
		//
		p1 = e0.y * v1.x - e0.x * v1.y;
		p2 = e0.y * v2.x - e0.x * v2.y;
	    if (p2 < p1) { min = p2; max = p1; } else { min = p1; max = p2; }
		rad = fey * boxhalfsize.x + fex * boxhalfsize.y;
		if (min > rad || max < -rad) return 0;
		//
		fex = Mathf.Abs(e1.x);
		fey = Mathf.Abs(e1.y);
		fez = Mathf.Abs(e1.z);
		//
		p0 = e1.z * v0.y - e1.y * v0.z;
		p2 = e1.z * v2.y - e1.y * v2.z;
		if (p0 < p2) { min = p0; max = p2; } else { min = p2; max = p0; }  
		rad = fez * boxhalfsize.y + fey * boxhalfsize.z;
		if (min > rad || max < -rad) return 0;
		//
		p0 = -e1.z * v0.x + e1.x * v0.z;
		p2 = -e1.z * v2.x + e1.x * v2.z;
	    if (p0 < p2) { min = p0; max = p2; } else { min = p2; max = p0; }
		rad = fez * boxhalfsize.x + fex * boxhalfsize.z;
		if (min > rad || max < -rad) return 0;
		//	
		p0 = e1.y * v0.x - e1.x * v0.y;
		p1 = e1.y * v1.x - e1.x * v1.y;
		if (p0 < p1) { min = p0; max = p1; } else { min = p1; max = p0; }
		rad = fey * boxhalfsize.x + fex * boxhalfsize.y;
		if (min > rad || max < -rad) return 0;
		//
		fex = Mathf.Abs(e2.x);
		fey = Mathf.Abs(e2.y);
		fez = Mathf.Abs(e2.z);
		//
		p0 = e2.z * v0.y - e2.y * v0.z;
		p1 = e2.z * v1.y - e2.y * v1.z;
	    if (p0 < p1) { min = p0; max = p1; } else { min = p1; max = p0; }
		rad = fez * boxhalfsize.y + fey * boxhalfsize.z;
		if (min > rad || max < -rad) return 0;
		//
		p0 = -e2.z * v0.x + e2.x * v0.z;
		p1 = -e2.z * v1.x + e2.x * v1.z;
		if (p0 < p1) { min = p0; max = p1; } else { min = p1; max = p0; }
		rad = fez * boxhalfsize.x + fex * boxhalfsize.z;
		if (min > rad || max < -rad) return 0;
		//
		p1 = e2.y * v1.x - e2.x * v1.y;
		p2 = e2.y * v2.x - e2.x * v2.y;
		if (p2 < p1) { min = p2; max = p1; } else { min = p1; max = p2;}
		rad = fey * boxhalfsize.x + fex * boxhalfsize.y;
		if (min > rad || max < -rad) return 0;

		/* Bullet 1: */
		/*  first test overlap in the {x,y,z}-directions */
		/*  find min, max of the triangle each direction, and test for overlap in */
		/*  that direction -- this is equivalent to testing a minimal AABB around */
		/*  the triangle against the AABB */
		/* test in X-direction */
		min = Mathf.Min(v0.x, Mathf.Min(v1.x, v2.x));
		max = Mathf.Max(v0.x, Mathf.Max(v1.x, v2.x));
		if (min > boxhalfsize.x || max < -boxhalfsize.x) return 0;
		/* test in Y-direction */
		min = Mathf.Min(v0.y, Mathf.Min(v1.y, v2.y));
		max = Mathf.Max(v0.y, Mathf.Max(v1.y, v2.y));
		if (min > boxhalfsize.y || max < -boxhalfsize.y) return 0;
		/* test in Z-direction */
		min = Mathf.Min(v0.z, Mathf.Min(v1.z, v2.z));
		max = Mathf.Max(v0.z, Mathf.Max(v1.z, v2.z));
		if (min > boxhalfsize.z || max < -boxhalfsize.z) return 0;
		/* Bullet 2: */
		/*  test if the box intersects the plane of the triangle */
		/*  compute plane equation of triangle: normal*x+d=0 */
		normal = Vector3.Cross(e0, e1);
		if ( 0 == Utils.PlaneBoxOverlap(normal, v0, boxhalfsize)) return 0;	
		return 1;   /* box and triangle overlaps */
	}
}