using UnityEngine;
using System.Collections;

public class ShapeDecoration : MonoBehaviour {

	public TetrisShapeEnum	tetrisShapeEnum	=	default(TetrisShapeEnum);

	public void Constuct (TetrisShape tetrisShape, TetrisCube cubePrefab, TetrisColorPalette palette)
	{
		for (int i = 0; i < tetrisShape.rotation.Length; i++)
		{
			for (int j = 0; j < tetrisShape.rotation [i].Length; j++) 
			{
				if (tetrisShape.rotation [i] [j] > 0) 
				{
					TetrisCube cube = GameObject.Instantiate<TetrisCube> (cubePrefab);
					cube.transform.parent = transform;
					cube.transform.localPosition = new Vector3 (i, j, 0);
					cube.SetPalette (palette);
					cube.fill = tetrisShape.rotation [i] [j];
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
