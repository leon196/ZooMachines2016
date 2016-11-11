using UnityEngine;

[System.Serializable]
public class TetrisColorPalette
{
	[SerializeField]
	private Color _shapeIColor	= default(Color);
	[SerializeField]
	private Color _shapeJColor  = default(Color);
	[SerializeField]
	private Color _shapeLColor	= default(Color);
	[SerializeField]
	private Color _shapeOColor	= default(Color);
	[SerializeField]
	private Color _shapeSColor	= default(Color);
	[SerializeField]
	private Color _shapeZColor	= default(Color);
	[SerializeField]
	private Color _shapeTColor	= default(Color);
	[SerializeField]
	private Color _emptyColor	= default(Color);
	[SerializeField]
	private Color _malusColor	= default(Color);

	public Color GetShapeColor (int shapeColor)
	{
		if (shapeColor == 0)
			return _emptyColor;

		if (shapeColor == 20)
			return _malusColor;

		if (shapeColor == (int)TetrisShapeEnum.ShapeI ||
		    shapeColor == (int)TetrisShapeEnum.ShapeI + 10) 
		{
			return _shapeIColor;
		}

		if (shapeColor == (int)TetrisShapeEnum.ShapeJ ||
			shapeColor == (int)TetrisShapeEnum.ShapeJ + 10) 
		{
			return _shapeJColor;
		}

		if (shapeColor == (int)TetrisShapeEnum.ShapeL ||
			shapeColor == (int)TetrisShapeEnum.ShapeL + 10) 
		{
			return _shapeLColor;
		}

		if (shapeColor == (int)TetrisShapeEnum.ShapeO ||
			shapeColor == (int)TetrisShapeEnum.ShapeO + 10) 
		{
			return _shapeOColor;
		}


		if (shapeColor == (int)TetrisShapeEnum.ShapeS ||
			shapeColor == (int)TetrisShapeEnum.ShapeS + 10) 
		{
			return _shapeSColor;
		}

		if (shapeColor == (int)TetrisShapeEnum.ShapeT ||
			shapeColor == (int)TetrisShapeEnum.ShapeT + 10) 
		{
			return _shapeTColor;
		}

		if (shapeColor == (int)TetrisShapeEnum.ShapeZ ||
			shapeColor == (int)TetrisShapeEnum.ShapeZ + 10) 
		{
			return _shapeZColor;
		}

		return Color.magenta;
	}
}

