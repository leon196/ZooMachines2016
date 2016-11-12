using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MidiJack;

public class TetrisSpecialGrid : MonoBehaviour 
{	
	[SerializeField]
	private TetrisCube		_tetrisCubePrefab		= null;

	[SerializeField]
	private float			_fallDelay				= 0f;

	private TetrisCube[][]	_grid		  			= null;

	private int				_width  				= 0;
	private int				_height 				= 0;

	private Coord[]			_shapePositions			= null;
	private TetrisShape[]   _currentShapes			= null;

	private float			_timeToFall				= 0f;

	private bool			_gameOver				= false;

	private float[]			_currentHorizontalKnob	= null;

	private PlayerID		_playerId				= 0;

	private int[][]			_horizontalMoveKey		= new int[][] {
		new int[]{ (int)0x0, (int)0x1, (int)0x2, (int)0x3 },
		new int[]{ (int)0x4, (int)0x5, (int)0x6, (int)0x7 },
	};

	//private int[]			_forceFallKey 			= new int[]{(int)0x18, (int)0x1F};

	private int					_malusLine				= 0;

	private TetrisSpecialGrid	_opponentGrid			= null;

	private bool				_cleanMidiDelegate		= false;

	private int					_specialGameId			= -1;

	public void CreateNewGrid (PlayerID playerId, TetrisColorPalette palette, int width, int height, int gameId)
	{
		// Create Grid Data
		_width			= width;
		_height			= height;
		_playerId 		= playerId;
		_specialGameId	= gameId;
		_gameOver = false;

		// Create Grid Array
		if (_grid == null) {
			_grid = new TetrisCube[width][];

			for (int i = 0; i < _grid.Length; i++)
				_grid [i] = new TetrisCube[height];


			// Populate Grid with Cube prefab
			for (int i = 0; i < width; i++) {
				for (int j = 0; j < height; j++) {
					TetrisCube tetrisCube	= GameObject.Instantiate (_tetrisCubePrefab);
					tetrisCube.name = "block_" + i + "_" + j;

					tetrisCube.transform.parent = transform;
					tetrisCube.transform.localPosition	= new Vector3 (i, j, 0);

					tetrisCube.SetPalette (palette);

					_grid [i] [j] = tetrisCube;
				}
			}
		}			

		int goodLine = ( (Random.Range (0, 200) % 2) == 0 ? 12 : 3);

		_grid [3] [goodLine].fill  = 10;
		_grid [3] [15].fill = 10;
		_grid [3] [16].fill = 10;
		_grid [3] [0].fill	= 10;
		_grid [2] [0].fill	= 10;

		_grid [6] [goodLine].fill  = 10;
		_grid [6] [15].fill = 10;

		_grid [8] [16].fill = 10;
		_grid [8] [17].fill = 10;
		_grid [8] [18].fill = 10;
		_grid [8] [19].fill = 10;

		_shapePositions = new Coord[] 
		{
			new Coord(0,Random.Range(4,16)),
			new Coord(4,Random.Range(4,18)),
			new Coord(6,4),
			new Coord(8,Random.Range(4,16))
		};

		_currentShapes = new TetrisShape[4];

		_currentShapes[0] = AddShape (_shapePositions [0], TetrisShapeEnum.ShapeL);

		_currentShapes[1] = AddShape (_shapePositions [1], TetrisShapeEnum.ShapeS);
		_currentShapes [1].ApplyRotation ();

		_currentShapes[2] = AddShape (_shapePositions [2], TetrisShapeEnum.ShapeJ);
		_currentShapes [2].ApplyRotation ();

		_currentShapes[3] = AddShape (_shapePositions [3], TetrisShapeEnum.ShapeI);

		_currentHorizontalKnob = new float[4];

		for (int i = 0; i < 4; i++) {
			MoveInputDetected (MidiMaster.GetKnob (_horizontalMoveKey [(int)playerId] [i]), i);
		}

		MidiMaster.knobDelegate += knobChanged;
		_cleanMidiDelegate = true;
	}

	public void SetOpponentGrid (TetrisSpecialGrid opponentGrid)
	{
		_opponentGrid = opponentGrid;
	}

	public TetrisShape AddShape (Coord spawnPosition , TetrisShapeEnum shapeEnum)
	{
		TetrisShape shape = null;

		switch (shapeEnum) 
		{
		case TetrisShapeEnum.ShapeT:	shape = new TetrisShapeT ();	break;

		case TetrisShapeEnum.ShapeI:	shape = new TetrisShapeI ();	break;

		case TetrisShapeEnum.ShapeO:	shape = new TetrisShapeO ();	break;

		case TetrisShapeEnum.ShapeL:	shape = new TetrisShapeL ();	break;

		case TetrisShapeEnum.ShapeJ:	shape = new TetrisShapeJ ();	break;

		case TetrisShapeEnum.ShapeZ:	shape = new TetrisShapeZ ();	break;

		case TetrisShapeEnum.ShapeS:	shape = new TetrisShapeS ();	break;
		}

		return shape;

		Redraw ();
	}

	public void Update ()
	{
		if (_gameOver == true)
			return;
		/*
		if (_currentShape != null) 
		{
			Fall ();
			CheckShapeInput ();
		}*/
	}

	public void Fall ()
	{
		/*
		if (Time.time >= _timeToFall) 
		{
			Coord newCoord = new  Coord (_currentShapePosition.x, _currentShapePosition.y-1);
			if (CheckMove (newCoord) == true) 
			{
				_currentShapePosition = newCoord;
				Redraw ();
			} 
			else 
			{
				LockShape ();
				_currentShape = null;
				RemoveLine ();
				Redraw ();
				AddShape ();
			}

			_timeToFall = Time.time + _fallDelay;
		}
		*/
	}

	public void ForceFall ()
	{
		/*
		Coord newCoord = new Coord (_currentShapePosition.x, _currentShapePosition.y);
		for (int j = _currentShapePosition.y; j >= 0; j--) 
		{
			newCoord.y--;
			if (CheckMove (newCoord) == false) 
			{
				newCoord.y++;
				_currentShapePosition = newCoord;
				Redraw ();
				_timeToFall = 0f;
				break;		
			}
		}
		*/
	}

	public void knobChanged (MidiChannel channel, int  knobNumber, float knobValue)
	{
		for (int i = 0; i < _horizontalMoveKey [(int)_playerId].Length; i++)
		{
			if (knobNumber == _horizontalMoveKey [(int)_playerId] [i])
				MoveInputDetected (knobValue, i);
		}
	}

	public void MoveInputDetected (float knobValue, int index)
	{
		if (index == 0 || index == 2)
			knobValue = 1f - knobValue;
		
		int y = (int)(_height * knobValue);

		if (CheckMove (new Coord (_shapePositions [index].x, y), index)==true) {

			_shapePositions [index].y = y;
		}

		Redraw ();
	}

	public bool CheckGameOver ()
	{
		bool onelinecomplete = false;
		for (int j = 0; j < _height; j++) 
		{
			onelinecomplete = true;
			for (int i = 0; i < _width; i++) 
			{
				if (_grid [i] [j].fill == 0) 
				{
					onelinecomplete = false;
					break;
				}
			}

			if (onelinecomplete == true)
				return true;
		}

		return false;
	}

	public void Redraw ()
	{
		// Reset Grid Graphics Buffer
		for (int i = 0; i < _width; i++) 
		{
			for (int j = 0; j < _height; j++) 
			{
				if (_grid [i][j].fill < 10)
					_grid [i][j].fill = 0;
			}
		}

		//
		for (int shapeIndex = 0; shapeIndex < _currentShapes.Length; shapeIndex++)
		{
			TetrisShape currentShape = _currentShapes[shapeIndex];

			for (int i = 0; i < currentShape.rotation.Length; i++) 
			{
				for (int j = 0; j < currentShape.rotation [i].Length; j++) 
				{
					if (currentShape.rotation[i][j] == 0)
						continue;

					int yi = _shapePositions[shapeIndex].y - i;
					int xj = _shapePositions[shapeIndex].x + j;

					_grid [xj][yi].fill = currentShape.rotation[i][j];
				}
			}
		}

		//
		if (_gameOver == false && CheckGameOver () == true) 
		{
			_gameOver = true;
			GameObject.FindObjectOfType<TetrisGame> ().SpecialGameEnd (_playerId, _specialGameId);
		}
	}

	public void Reset ()
	{
		for (int i = 0; i < _width; i++)
			for (int j = 0; j < _height; j++)
				_grid [i] [j].fill = 0;

		if (_cleanMidiDelegate == true) 
		{
			_cleanMidiDelegate = false;
			MidiMaster.knobDelegate -= knobChanged;
		}
	}

	public void LockShape ()
	{
		/*
		for (int i = 0; i < _currentShape.rotation.Length; i++) 
		{
			for (int j = 0; j < _currentShape.rotation [i].Length; j++) 
			{
				if (_currentShape.rotation[i][j] == 0)
					continue;

				int yi = _currentShapePosition.y - i;
				int xj = _currentShapePosition.x + j;

				_grid [xj] [yi].fill = 10 + _currentShape.rotation[i][j];
			}
		}
		*/
	}

	public void RemoveLine()
	{
		/*
		List<int> lineToDelete = new List<int> ();

		for (int j = 0; j < _height; j++)
		{
			bool isToDelete = true;

			for (int i = 0; i < _width; i++) 
			{
				if (_grid [i][j].fill == 0) 
				{
					isToDelete = false;
					break;
				}
			}

			if (isToDelete == true)
				lineToDelete.Add (j);
		}

		if (lineToDelete.Count > 0)
		{
			for (int j = 0; j < lineToDelete.Count; j++) 
			{
				for (int i = 0; i < _width; i++) 
				{
					_grid [i] [lineToDelete[j]].fill = 0;
				}
			}

			//Better remove lines
			for (int j = 0; j < (_height-1); j++) 
			{
				int gap = 0;

				if (IsLineEmpty (j) == true) 
				{
					gap = 1;

					for (int topJ = j + 1; topJ < _height; topJ++)
					{
						if (IsLineEmpty (topJ) == true)
							gap++;
						else
							break;
					}

					int line = j + gap;
					if (line >= _height) 
					{
						line = _height - 1;
					}

					for (int i = 0; i < _width; i++) 
					{
						_grid [i] [j].fill = _grid [i] [line].fill;
						_grid [i] [line].fill = 0;
					}
				}
			}

			if (_opponentGrid != null) 
			{
				if (lineToDelete.Count == 4) 
					_opponentGrid.SetMalusLines (4);	
				else if (lineToDelete.Count == 3)
					_opponentGrid.SetMalusLines (3);
				else if (lineToDelete.Count == 2) 
					_opponentGrid.SetMalusLines (2);
			}
		}
		*/
	}

	public bool CheckRotation ()
	{
		/*
		for (int i = 0; i < _currentShape.nextRotation.Length; i++) 
		{
			for (int j = 0; j < _currentShape.nextRotation[i].Length; j++) 
			{
				if (_currentShape.nextRotation [i] [j] == 0)
					continue;

				int yi = _currentShapePosition.y - i;
				int xj = _currentShapePosition.x + j;

				if (yi < 0) { return false; }
				if (xj < 0) { return false; }
				if (xj >= _width)  { return false; }
				if (_grid [xj] [yi].fill >= 10)  { return false; }
			}
		}
		*/
		return true;
	}

	public bool CheckMove (Coord newCoord, int index)
	{
		for (int i = 0; i < _currentShapes[index].rotation.Length; i++) 
		{
			for (int j = 0; j < _currentShapes[index].rotation[i].Length; j++) 
			{
				if (_currentShapes[index].rotation [i] [j] == 0)
					continue;

				int yi = newCoord.y - i;
				int xj = newCoord.x + j;

				if (yi < 0) { return false; }
				if (xj < 0) { return false; }
				if (xj >= _width)  { return false; }
				if (yi >= _height) { return false; }

				if (_grid [xj] [yi].fill >= 10)  { return false; }

				if (_grid [xj] [yi].fill > 0 &&
					_grid [xj] [yi].fill != (int)_currentShapes [index].rotation [i] [j]) 
				{
					return false;
				}
			}
		}

		return true;		
	}

	private void AddMalusLine (int lineCount)
	{
		/*
		for (int k = 0; k < lineCount; k++)
		{
			for (int j = (_height - 1); j > 0; j--) 
			{
				for (int i = 0; i < _width; i++) {
					_grid [i] [j].fill = _grid [i] [j - 1].fill;
				}	
			}
		}

		List<int> randomHoles = new List<int> ();
		for (int i = 0; i < _width; i++)
			randomHoles.Add (i);


		int hole = Random.Range (0, randomHoles.Count);
		randomHoles.RemoveAt (hole);

		for (int k = 0; k < lineCount; k++) 
		{
			for (int i = 0; i < _width; i++)
				if (i != hole)
					_grid [i] [k].fill = 20;
				else
					_grid [i] [k].fill = 0;

			if (k == 2)
			{
				hole = Random.Range (0, randomHoles.Count);
				randomHoles.RemoveAt (hole);
			}
		}
		*/
	}

	public void SetMalusLines(int malusLineCount)
	{
		//_malusLine = malusLineCount;
	}

	private void OnDestroy()
	{
		if (_cleanMidiDelegate == true) 
		{
			_cleanMidiDelegate = false;
			MidiMaster.knobDelegate -= knobChanged;
		}
	}

	private bool IsLineEmpty (int line)
	{
		for (int i = 0; i < _width; i++)
			if (_grid [i] [line].fill > 0)
				return false;
		return true;
	}
}
