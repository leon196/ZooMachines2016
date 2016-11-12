using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using MidiJack;

public class TetrisGame : MonoBehaviour 
{
	[SerializeField]
	private TetrisGrid _playerOneGrid;
	[SerializeField]
	private TetrisGrid _playerTwoGrid;

	//
	[SerializeField]
	private TetrisSpecialGrid	_playerOneSpecialGrid;
	[SerializeField]
	private TetrisSpecialGrid	_playerTwoSpecialGrid;


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
	private GameObject			_specialGridRoot	= null;

	[SerializeField]
	private TetrisCube			_tetrisCubePrefab	= null;

	[SerializeField]
	private GameObject				_gameOverText		= null;

	private Dictionary<int, int>	_specialGames	= new Dictionary<int, int> ();
	private int _uniqueSpecialGames					= 0;
	private int _lineCount							= 0;

	void Awake ()
	{
		_gameOverText.SetActive (false);
	}

	// Use this for initialization
	void Start ()
	{
		InitializeTutorial ();

		MidiMaster.knobDelegate += knobChanged;
	}

	public void knobChanged (MidiChannel channel, int  knobNumber, float knobValue)
	{
		if (_playerOneGrid != null)
			_playerOneGrid.knobChanged (channel, knobNumber, knobValue);

		if (_playerTwoGrid != null)
			_playerTwoGrid.knobChanged (channel, knobNumber, knobValue);

		if (_playerOneSpecialGrid != null && _specialGridRoot.activeSelf == true)
			_playerOneSpecialGrid.knobChanged (channel, knobNumber, knobValue);

		if (_playerTwoSpecialGrid != null && _specialGridRoot.activeSelf == true)
			_playerTwoSpecialGrid.knobChanged (channel, knobNumber, knobValue);
	}

	private void InitializeTutorial ()
	{
		if (_tutorial.isinit == false) {
			BuildTutorial ();
		}

		_tutorial.LaunchTutorial ();
	}

	private void BuildTutorial()
	{
		_tutorial.ConstructMidiController ();

		foreach (int i in System.Enum.GetValues (typeof(TetrisShapeEnum)))
		{
			_tutorial.ConstructTetrisShape (PlayerID.PlayerOne, (TetrisShapeEnum)i, _tetrisCubePrefab, _palette);
			_tutorial.ConstructTetrisShape (PlayerID.PlayerTwo, (TetrisShapeEnum)i, _tetrisCubePrefab, _palette);
		}

		_tutorial.isinit = true;
	}

	public void TutorialIsOver ()
	{
		_tutorial.gameObject.SetActive (false);
		Debug.Log ("tuto is over");
		InitializeGame ();
	}

	private void InitializeGame ()
	{
		if (_playerOneGrid.isInit == false) {
			_playerOneGrid.CreateNewGrid (PlayerID.PlayerOne, _palette, _width, _height);
			_playerTwoGrid.CreateNewGrid (PlayerID.PlayerTwo, _palette, _width, _height);

			_playerOneGrid.SetOpponentGrid (_playerTwoGrid);
			_playerTwoGrid.SetOpponentGrid (_playerOneGrid);
		} else {
			Debug.Log ("active game");
			_gridRoot.gameObject.SetActive (true);
			_playerOneGrid.RestartGrid ();
			_playerTwoGrid.RestartGrid ();
		}
	}

	public void LineRemove (int lineCount)
	{
		_lineCount++;

		if (_lineCount >= 1) 
		{
			_lineCount = 0;

			_playerOneGrid.StopGame ();
			_playerTwoGrid.StopGame ();

			_gridRoot.gameObject.SetActive (false);
			InitializeSpecialTutorial ();
		}
	}

	private void InitializeSpecialTutorial ()
	{
		if (_tutorial.isinit == false) {
			BuildTutorial ();
		}

		_tutorial.gameObject.SetActive (true);
		_tutorial.LaunchSpecialTutorial ();
	}

	public void SpecialTutorialIsOver ()
	{
		_tutorial.gameObject.SetActive (false);
		InitialzeSpecialGame();
	}

	private void InitialzeSpecialGame ()
	{
		_uniqueSpecialGames++;

		_specialGames [_uniqueSpecialGames] = -1;

		_playerOneSpecialGrid.CreateNewGrid (PlayerID.PlayerOne, _palette, _width, _height, _uniqueSpecialGames);
		_playerTwoSpecialGrid.CreateNewGrid (PlayerID.PlayerTwo, _palette, _width, _height, _uniqueSpecialGames);

		_playerOneSpecialGrid.SetOpponentGrid (_playerTwoSpecialGrid);
		_playerTwoSpecialGrid.SetOpponentGrid (_playerOneSpecialGrid);

		_specialGridRoot.gameObject.SetActive (true);
	}
	public void SpecialGameEnd(PlayerID player, int specialGameId)
	{
		if (_specialGames [specialGameId] == -1) 
		{
			_specialGames [specialGameId] = (int)player;
			_playerOneSpecialGrid.Reset ();
			_playerTwoSpecialGrid.Reset ();

			if (player == PlayerID.PlayerOne)
				_playerTwoGrid.SetMalusLines (2);
			else
				_playerOneGrid.SetMalusLines (2);

			_specialGridRoot.gameObject.SetActive (false);
			_gridRoot.gameObject.SetActive (false);
			_tutorial.gameObject.SetActive (true);

			InitializeTutorial ();
		}
	}

	public bool CheckKeyDown (int key)
	{
		if (MidiMaster.GetKeyDown (key))
			return true;
		return false;
	}

	void OnDestroy()
	{
		MidiMaster.knobDelegate = null;
	}
}
