using System.Collections.Generic;

public class TetrisShapeZ : TetrisShape
{
	public TetrisShapeZ()
	{
		_p1RotationKey = (int)0x39;
		_p2RotationKey = (int)0x40;

		_shapeEnum	   = TetrisShapeEnum.ShapeZ;

		_rotations = new List<int[][]> () {
			new int[][] {
				new int[]{ 1, 1, 0 },
				new int[]{ 0, 1, 1 },
				new int[]{ 0, 0, 0 },
			},

			new int [][] {
				new int[]{ 0, 0, 1 },
				new int[]{ 0, 1, 1 },
				new int[]{ 0, 1, 0 },
			}
		};

		SetColor ();
	}
}

