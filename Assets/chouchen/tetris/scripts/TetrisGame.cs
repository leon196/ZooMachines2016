using UnityEngine;
using System.Collections;

public class TetrisGame : MonoBehaviour 
{
	[SerializeField]
	private TetrisGrid _playerOneGrid;
	[SerializeField]
	private TetrisGrid _playerTwoGrid;

	[SerializeField]
	private int 	   _width	= 0;
	[SerializeField]
	private int		   _height	= 0;

	[SerializeField]
	private TetrisColorPalette	_palette = null;

	// Use this for initialization
	void Start ()
	{
		InitializeGame ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	private void InitializeGame ()
	{
		_playerOneGrid.CreateNewGrid (PlayerID.PlayerOne, _palette, _width, _height);
		_playerTwoGrid.CreateNewGrid (PlayerID.PlayerTwo, _palette, _width, _height);
	}
}
