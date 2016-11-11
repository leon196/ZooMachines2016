using System.Collections.Generic;

public class TetrisShapeS : TetrisShape
{
	public TetrisShapeS()
	{
		_p1RotationKey = (int)0x3B;
		_p2RotationKey = (int)0x3E;

		_shapeEnum	   = TetrisShapeEnum.ShapeS;

		_rotations = new List<int[][]> () {
			new int[][] {
				new int[]{ 0, 1, 1 },
				new int[]{ 1, 1, 0 },
				new int[]{ 0, 0, 0 },
			},

			new int [][] {
				new int[]{ 1, 0, 0 },
				new int[]{ 1, 1, 0 },
				new int[]{ 0, 1, 0 },
			}
		};

		SetColor ();
	}
}

