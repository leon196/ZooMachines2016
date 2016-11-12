using UnityEngine;
using System.Collections;

public class LineFont : ScriptableObject
{
    [System.Serializable]
    public class LineFontCharacter
    {
        public string name;
        public Vector2[] linePoints;
        public float width;
    }

    public LineFontCharacter[] characters;
    public bool isGridable = true;
    public int gridHeight = 5;
    public float lineSpacing = 1;
}
