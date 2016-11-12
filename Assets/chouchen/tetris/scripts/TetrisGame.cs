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
	private TetrisColorPalette	_palette     		= null;
	[SerializeField]
	private GameObject			_gridRoot	 		= null;
	[SerializeField]
	private TetrisTutorial		_tutorial	 		= null;
	[SerializeField]
	private TetrisCube			_tetrisCubePrefab	= null;

	// Use this for initialization
	void Start ()
	{
		InitializeTutorial ();
		_tutorial.LaunchTutorial ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void InitializeTutorial ()
	{
		_tutorial.ConstructMidiController ();

		foreach (int i in System.Enum.GetValues (typeof(TetrisShapeEnum)))
		{
			_tutorial.ConstructTetrisShape (PlayerID.PlayerOne, (TetrisShapeEnum)i, _tetrisCubePrefab, _palette);
			_tutorial.ConstructTetrisShape (PlayerID.PlayerTwo, (TetrisShapeEnum)i, _tetrisCubePrefab, _palette);
		}
	}

	private void InitializeGame ()
	{
		_playerOneGrid.CreateNewGrid (PlayerID.PlayerOne, _palette, _width, _height);
		_playerTwoGrid.CreateNewGrid (PlayerID.PlayerTwo, _palette, _width, _height);

		_playerOneGrid.SetOpponentGrid (_playerTwoGrid);
		_playerTwoGrid.SetOpponentGrid (_playerOneGrid);
	}

	public void TutorialIsOver ()
	{
		_tutorial.gameObject.SetActive (false);
		InitializeGame ();
	}
}
