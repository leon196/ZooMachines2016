using UnityEngine;
using System.Collections;

public class CameraToTexture : MonoBehaviour
{
	public string textureName = "_CameraTexture";
	public RenderTextureFormat format = RenderTextureFormat.ARGB32;
	public FilterMode filterMode = FilterMode.Bilinear;
	public TextureWrapMode wrapMode = TextureWrapMode.Clamp;
	
	private RenderTexture renderTexture;
	
	void Awake ()
	{
		renderTexture = new RenderTexture(Screen.width, Screen.height, 24, format);
		renderTexture.filterMode = filterMode;
		renderTexture.wrapMode = wrapMode;
		renderTexture.Create();
		GetComponent<Camera>().targetTexture = renderTexture;
	}

	void Update ()
	{
		Shader.SetGlobalTexture(textureName, renderTexture);
	}
}