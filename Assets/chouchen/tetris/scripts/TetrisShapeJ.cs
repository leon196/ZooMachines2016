using System.Collections.Generic;

public class TetrisShapeJ : TetrisShape
{
	public TetrisShapeJ()
	{
		_p1RotationKey = (int)0x37;
		_p2RotationKey = (int)0x41;

		_shapeEnum	   = TetrisShapeEnum.ShapeJ;

		_rotations = new List<int[][]> () {
			new int[][] {
				new int[]{ 1, 1, 1 },
				new int[]{ 0, 0, 1 },
				new int[]{ 0, 0, 0 },
			},

			new int [][] {
				new int[]{ 0, 0, 1 },
				new int[]{ 0, 0, 1 },
				new int[]{ 0, 1, 1 },
			},

			new int[][] {
				new int[]{ 0, 0, 0 },
				new int[]{ 1, 0, 0 },
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

