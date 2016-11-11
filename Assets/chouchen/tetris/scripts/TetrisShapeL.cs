using System.Collections.Generic;

public class TetrisShapeL : TetrisShape
{
	public TetrisShapeL()
	{
		_p1RotationKey = (int)0x35;
		_p2RotationKey = (int)0x43;

		_shapeEnum 	   = TetrisShapeEnum.ShapeL;

		_rotations = new List<int[][]> () {
			new int[][] {
				new int[]{ 1, 1, 1 },
				new int[]{ 1, 0, 0 },
				new int[]{ 0, 0, 0 },
			},

			new int [][] {
				new int[]{ 0, 1, 1 },
				new int[]{ 0, 0, 1 },
				new int[]{ 0, 0, 1 },
			},

			new int[][] {
				new int[]{ 0, 0, 0 },
				new int[]{ 0, 0, 1 },
				new int[]{ 1, 1, 1 },
			},

			new int [][] {
				new int[]{ 1, 0, 0 },
				new int[]{ 1, 0, 0 },
				new int[]{ 1, 1, 0 },
			}
		};

		SetColor ();
	}
}

