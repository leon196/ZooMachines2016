using System.Collections.Generic;
using UnityEngine;

public class TetrisShape
{
	protected List<int[][]> 	_rotations		= null;
	protected int				_rotationIndex	= 0;

	protected int				_p1RotationKey	= 0;
	protected int				_p2RotationKey 	= 0;

	protected TetrisShapeEnum	_shapeEnum		= default(TetrisShapeEnum);

	public void SetColor()
	{
		foreach (int[][] array in _rotations) 
		{
			for (int i = 0; i < array.Length; i++)
				for (int j = 0; j < array.Length; j++)
					array [i] [j] *= (int)_shapeEnum;
		}
	}

	public int rotationKey(PlayerID playerId)
	{
		if (playerId == PlayerID.PlayerOne)
			return _p1RotationKey;
		else
			return _p2RotationKey;
	}

	public int[][] rotation
	{
		get { return _rotations [_rotationIndex]; }
	}

	public int[][] nextRotation
	{
		get { return _rotations [((_rotationIndex < _rotations.Count - 1) ? _rotationIndex + 1 : 0)]; }
	}

	public void ApplyRotation()
	{
		_rotationIndex = ((_rotationIndex < _rotations.Count - 1) ? _rotationIndex + 1 : 0);
	}
}
