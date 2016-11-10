using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShaderPass : MonoBehaviour
{
	public Material materialShader;
	public string uniformName = "_ShaderPassTexture";
	private FrameBuffer frameBuffer;
	private RenderTexture output;

	public void Setup (int width, int height, Texture2D input)
	{
		Camera.onPreRender += onPreRender;
		frameBuffer = new FrameBuffer(width, height, 2, RenderTextureFormat.ARGBFloat, FilterMode.Point);
		Print(input);
	}

	public void Print (Texture2D input)
	{
		Graphics.Blit(input, frameBuffer.Get());
		frameBuffer.Swap();
		Graphics.Blit(input, frameBuffer.Get());
		output = frameBuffer.Get();
	}

	public void onPreRender (Camera camera)
	{
		if (materialShader) {
			Graphics.Blit(frameBuffer.Get(), output, materialShader);
			output = frameBuffer.Get();
			frameBuffer.Swap();
			Shader.SetGlobalTexture(uniformName, output);
		}
	}

	// public void Save ()
	// {
	// 	RenderTexture last = RenderTexture.active;
	// 	RenderTexture.active = output;
	// 	Texture2D texture2D = new Texture2D(output.width, output.height, TextureFormat.RGBAFloat, false);
	// 	texture2D.ReadPixels(new Rect(0, 0, output.width, output.height), 0, 0);
	// 	texture2D.Apply();
	// 	Shader.SetGlobalTexture("_Morph" + textureMorphArray.Count, texture2D);
	// 	textureMorphArray.Add(texture2D);
	// 	RenderTexture.active = last;
	// }
}
