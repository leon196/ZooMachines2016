using UnityEngine;
using System.Collections;

public class RenderWith : MonoBehaviour {

	public Shader shader;

	void Start ()
	{
		GetComponent<Camera>().SetReplacementShader(shader, "");
	}
}
