﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MidiJack;

public class TetrisGrid : MonoBehaviour 
{	

	[SerializeField]
	private Material		particleMaterial		= null;

	[SerializeField]
	private TetrisCube		_tetrisCubePrefab		= null;

	[SerializeField]
	private float			_fallDelay				= 0f;
	[SerializeField]
	private TetrisGame		_tetrisGame				= null;

	private TetrisCube[][]	_grid		  			= null;
	private TetrisShape		_currentShape 			= null;
	private Coord			_spawnPosition			= null;

	private int				_width  				= 0;
	private int				_height 				= 0;

	private Coord			_currentShapePosition	= null;

	private float			_timeToFall				= 0f;

	private bool			_gameOver				= false;

	private float			_currentHorizontalKnob	= 0f;

	private PlayerID		_playerId				= 0;

	private int[]			_horizontalMoveKey		= new int[]{(int)0x39, (int)0x40};
	private int[]			_forceFallKey 			= new int[]{(int)0x18, (int)0x1F};

	private int				_malusLine				= 0;

	private TetrisGrid		_opponentGrid			= null;

	private Osciyo osciyo;
	private bool			_cleanMidiDelegate		= false;

	public bool 			isInit
	{
		get { return _grid != null; }
	}

	private bool gameIsStop = false;

	public void CreateNewGrid (PlayerID playerId, TetrisColorPalette palette, int width, int height)
	{
		Debug.Log ("create new grid");

		// Create Grid Array
		_grid = new TetrisCube[width][];

		for (int i = 0; i < _grid.Length; i++) 
			_grid [i] = new TetrisCube[height];

		// Create Grid Data
		_spawnPosition	= new Coord (width / 2, height - 1);
		_width			= width;
		_height			= height;
		_playerId 		= playerId;

		gameObject.AddComponent<Osciyo>();
		osciyo = gameObject.GetComponent<Osciyo>();
		int osciyoIndex = 0;

		List<Vector3> points = new List<Vector3>();

		// Populate Grid with Cube prefab
		for (int i = 0; i < width; i++) 
		{
			for (int j = 0; j < height; j++) 
			{
				TetrisCube tetrisCube	= GameObject.Instantiate (_tetrisCubePrefab);
				tetrisCube.name			= "block_" + i + "_" + j;

				tetrisCube.transform.parent			= transform;
				tetrisCube.transform.localPosition	= new Vector3 (i, j, 0);
				tetrisCube.GetComponent<Renderer>().enabled = false;

				Vector3 origin = transform.position;
				transform.position = Vector3.zero;
				// List<Vector3> tmp = new List<Vector3>();
				Vector3[] edges = Draw.GetEdgePointsFromMesh(tetrisCube.GetComponent<MeshFilter>().sharedMesh, 0f);
				int[] osciyoIndices = new int[edges.Length];
				for (int e = 0; e < edges.Length; ++e) {
					Vector3 p = tetrisCube.transform.TransformPoint(edges[e]);
					// if (tmp.IndexOf(p) == -1) {
					// 	tmp.Add(p);
						points.Add(p);
						osciyoIndices[e] = osciyoIndex;
						++osciyoIndex;
					// }
				}
				transform.position = origin;

				tetrisCube.SetPalette (palette);
				tetrisCube.osciyo = osciyo;
				tetrisCube.osciyoColorIndices = osciyoIndices;

				_grid [i][j] = tetrisCube;
			}
		}

		osciyo.material = particleMaterial;
		osciyo.Init();

		//
		_timeToFall = Time.time + _fallDelay;

		//
		AddShape ();
	}

	public void RestartGrid ()
	{
		_timeToFall = Time.time + _fallDelay;	
		gameIsStop = false;
		AddShape ();
	}

	public void SetOpponentGrid (TetrisGrid opponentGrid)
	{
		_opponentGrid = opponentGrid;
	}

	public void AddShape ()
	{
		TetrisShape shape = null;

		if (gameIsStop == true)
			return;
		
		if (_malusLine > 0) 
		{
			AddMalusLine (_malusLine);
			_malusLine = 0;
			Redraw ();
		}

		int rand = Random.Range (0, 7);

		switch (rand) 
		{
			case 0:	shape = new TetrisShapeT ();	break;

			case 1:	shape = new TetrisShapeI ();	break;

			case 2:	shape = new TetrisShapeO ();	break;
			
			case 3:	shape = new TetrisShapeL ();	break;

			case 4:	shape = new TetrisShapeJ ();	break;

			case 5:	shape = new TetrisShapeZ ();	break;

			case 6:	shape = new TetrisShapeS ();	break;
		}

		_currentShape			= shape;

		Debug.Log ("shape on " + name + " is set to " + _currentShape);

		_currentShapePosition	= _spawnPosition;

		if (CheckGameOver () == true) 
		{
			_gameOver = true;
			_tetrisGame.GameOver ((_playerId == PlayerID.PlayerOne ? PlayerID.PlayerTwo : PlayerID.PlayerOne));
			Debug.Log ("Game Over");
		}

		Redraw ();
	}
	bool _magickey = false;

	public void Update ()
	{
		_magickey = MidiMaster.GetKeyDown ((int)0x46);

		if (_gameOver == true || gameIsStop == true)
			return;
		
		if (_currentShape != null) {
			Fall ();
			CheckShapeInput ();
		} else 
		{
			//Debug.Log ("shape is null on update " + name);
		}

		_magickey = false;
	}

	public void Fall ()
	{
		if (gameIsStop == true)
			return;
		
		if (Time.time >= _timeToFall) 
		{
			//Debug.Log ("fall on " + name);

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
				//Debug.Log ("shape set to null at " + name);
				RemoveLine ();
				Redraw ();
				AddShape ();
			}

			_timeToFall = Time.time + _fallDelay;
		}
	}

	public void ForceFall ()
	{
		if (_currentShape == null || gameIsStop == true || _gameOver == true)
			return;
		
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
	}

	public void knobChanged (MidiChannel channel, int  knobNumber, float knobValue)
	{
		if (knobNumber == _horizontalMoveKey [(int)_playerId])
			MoveInputDetected (knobValue);
		else if (knobNumber == _forceFallKey [(int)_playerId] && knobValue >= 1f)
			ForceFall ();
	}

	public void MoveInputDetected (float knobValue)
	{
		if (_currentShape == null)
			return;
		
		if (_currentHorizontalKnob < 0f) 
		{
			_currentHorizontalKnob = knobValue;
		}
		else if (knobValue <= 0f || knobValue < _currentHorizontalKnob) 
		{
			//Move Left
			Coord newCoord = new  Coord (_currentShapePosition.x - 1, _currentShapePosition.y);
			if (CheckMove (newCoord) == true) 
			{
				_currentShapePosition = newCoord;
				Redraw ();
			}

			_currentHorizontalKnob = knobValue;
		} 
		else if (knobValue >= 1f || knobValue > _currentHorizontalKnob) 
		{
			//Move Right
			Coord newCoord = new  Coord (_currentShapePosition.x + 1, _currentShapePosition.y);
			if (CheckMove (newCoord) == true) 
			{
				_currentShapePosition = newCoord;
				Redraw ();
			}

			_currentHorizontalKnob = knobValue;
		}		
	}

	public void CheckShapeInput ()
	{
		if (_currentShape == null)
			return;

		if (pressRotation) 
		{
			pressRotation = false;

			if (CheckRotation () == true) 
			{
				_currentShape.ApplyRotation ();
				Redraw ();
			}
		}
	}

	bool pressRotation = false;
	public void noteOn(MidiChannel channel, int note, float velocity)
	{
		if (_currentShape == null || gameIsStop == true || _gameOver == true)
			return;

		if (note == _currentShape.rotationKey (_playerId))
			pressRotation = true;
	}

	public bool CheckGameOver ()
	{
		for (int i = 0; i < _currentShape.rotation.Length; i++) 
		{
			for (int j = 0; j < _currentShape.rotation [i].Length; j++) {
				if (_currentShape.rotation [i] [j] == 0)
					continue;

				int yi = _currentShapePosition.y - i;
				int xj = _currentShapePosition.x + j;

				if (_grid [xj] [yi].fill > 0) 
				{
					Debug.Log ("colide on " + _grid [xj] [yi]);
					return true;
				}
			}
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

		// Draw Current Shape
		if (_currentShape != null) 
		{
			for (int i = 0; i < _currentShape.rotation.Length; i++) 
			{
				for (int j = 0; j < _currentShape.rotation [i].Length; j++) 
				{
					if (_currentShape.rotation[i][j] == 0)
						continue;
					
					int yi = _currentShapePosition.y - i;
					int xj = _currentShapePosition.x + j;

					_grid [xj][yi].fill = _currentShape.rotation[i][j];
				}
			}
		}

		osciyo.UpdateColor();
	}

	public void LockShape ()
	{
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
		osciyo.UpdateColor();
	}

	public void RemoveLine()
	{
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

				GameObject.FindObjectOfType<TetrisGame> ().LineRemove (lineToDelete.Count);
			}
		}
	}

	public bool CheckRotation ()
	{
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

		return true;
	}

	public bool CheckMove (Coord newCoord)
	{
		if (_currentShape == null)
			return false;
		
		for (int i = 0; i < _currentShape.rotation.Length; i++) 
		{
			for (int j = 0; j < _currentShape.rotation[i].Length; j++) 
			{
				if (_currentShape.rotation [i] [j] == 0)
					continue;
				
				int yi = newCoord.y - i;
				int xj = newCoord.x + j;

				if (yi < 0) { return false; }
				if (xj < 0) { return false; }
				if (xj >= _width)  { return false; }
				if (_grid [xj] [yi].fill >= 10)  { return false; }
			}
		}

		return true;		
	}

	private void AddMalusLine (int lineCount)
	{
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
	}

	public void SetMalusLines(int malusLineCount)
	{
		_malusLine = malusLineCount;
	}

	private void OnDestroy()
	{
		if (_cleanMidiDelegate == true)
			MidiMaster.knobDelegate = null;
	}

	private bool IsLineEmpty (int line)
	{
		for (int i = 0; i < _width; i++)
			if (_grid [i] [line].fill > 0)
				return false;
		return true;
	}

	public void StopGame ()
	{
		Debug.Log ("Stop Game on " + name);

		gameIsStop = true;

		if (_currentShape != null) {
			for (int i = 0; i < _currentShape.rotation.Length; i++) {
				for (int j = 0; j < _currentShape.rotation [i].Length; j++) {
					if (_currentShape.rotation [i] [j] == 0)
						continue;

					int yi = _currentShapePosition.y - i;
					int xj = _currentShapePosition.x + j;

					_grid [xj] [yi].fill = 0;
				}
			}
		}

		_currentShape = null;
	}
}
