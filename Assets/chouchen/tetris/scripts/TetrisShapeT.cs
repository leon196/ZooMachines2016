using System.Collections.Generic;

public class TetrisShapeT : TetrisShape
{
	public TetrisShapeT()
	{
		_p1RotationKey = (int)0x30;
		_p2RotationKey = (int)0x48;

		_shapeEnum     = TetrisShapeEnum.ShapeT;

		_rotations = new List<int[][]> () {
			new int[][] {
				new int[]{ 1, 1, 1 },
				new int[]{ 0, 1, 0 },
				new int[]{ 0, 0, 0 },
			},

			new int [][] {
				new int[]{ 0, 0, 1 },
				new int[]{ 0, 1, 1 },
				new int[]{ 0, 0, 1 },
			},

			new int[][] {
				new int[]{ 0, 0, 0 },
				new int[]{ 0, 1, 0 },
				new int[]{ 1, 1, 1 },
			},

			new int [][] {
				new int[]{ 1, 0, 0 },
				new int[]{ 1, 1, 0 },
				new int[]{ 1, 0, 0 },
			}
		};

		SetColor ();
	}
}

