using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineFontDrawer : MonoBehaviour
{

    public LineFont lineFont;

    [Multiline()]
    public string textToPreview;

    public float scale = 0.01f;

    [Range(0.6f, 20)]
    public float perlinHeight = 1;

    void DrawFontTest()
    {
        Draw.color = Color.white;

        Vector2[] fontLines = GetFontLines(textToPreview, lineFont);

        Vector2 start = Vector2.up * 0.3f + Vector2.right * 0.2f;

        for (int i = 0; i < fontLines.Length; i += 2)
        {
            if (i >= fontLines.Length || i + 1 >= fontLines.Length)
                return;

            Vector2 p1 = start + Woblify(fontLines[i], 2, perlinHeight) * scale;
            Vector2 p2 = start + Woblify(fontLines[i + 1], 2, perlinHeight) * scale;

            Draw.Line(p1, p2);
        }
    }

    Vector2 Woblify(Vector2 point, float frequency, float scale)
    {
        float x = (-0.5f + Mathf.PerlinNoise(frequency * Time.time + point.x, 1045.4f + frequency * Time.time + point.y)) * scale;
        float y = (-0.5f + Mathf.PerlinNoise(frequency * Time.time + point.y, frequency * Time.time + point.x + 234.345f)) * scale;
        return point + new Vector2(x, y);

    }

    void DrawFontGizmosPreview()
    {
        Vector2[] fontLines = GetFontLines(textToPreview, lineFont);

        for (int i = 0; i < fontLines.Length; i += 2)
        {
            if (i >= fontLines.Length || i + 1 >= fontLines.Length)
                return;

            Gizmos.DrawLine(fontLines[i], fontLines[i + 1]);
        }
    }

    Vector2[] GetFontLines(string text, LineFont font)
    {
        if (font == null) return null;

        if (text.Length == 0 || string.IsNullOrEmpty(text)) return null;

        List<Vector2> lines = new List<Vector2>();

        float hPos = 0;
        int line = 0;

        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == '\n')
            {
                line++;
                hPos = 0;
                continue;
            }

            foreach (var fontCharacter in font.characters)
            {
                if (string.IsNullOrEmpty(fontCharacter.name) || fontCharacter.name.Length < 1) continue;

                if (fontCharacter.name[0] == text[i])
                {
                    Vector2 right = Vector2.right * hPos;
                    Vector2 up = -Vector2.up * (line * (font.gridHeight + font.lineSpacing));

                    lines.AddRange(GetCharacter(right + up, fontCharacter));

                    hPos += fontCharacter.width + 1;
                }
            }
        }

        return lines.ToArray();
    }

    Vector2[] GetCharacter(Vector2 at, LineFont.LineFontCharacter lineFontCharacter)
    {
        Vector2[] charLines = new Vector2[lineFontCharacter.linePoints.Length];

        for (int i = 0; i < charLines.Length; i++)
            charLines[i] = lineFontCharacter.linePoints[i] + at;

        return charLines;
    }

    void OnDrawGizmos()
    {
        DrawFontGizmosPreview();

        /*
        if (lineFont == null) return;

        if (textToPreview.Length == 0 || string.IsNullOrEmpty(textToPreview)) return;

        float curPos = 0;

        int line = 0;

        for (int i = 0; i < textToPreview.Length; i++)
        {
            if (textToPreview[i] == '\n')
            {
                line++;
                curPos = 0;
                continue;
            }

            foreach (var fontCharacter in lineFont.characters)
            {
                if (string.IsNullOrEmpty(fontCharacter.name) || fontCharacter.name.Length < 1) continue;

                if (fontCharacter.name[0] == textToPreview[i])
                {
                    Vector2 right = Vector2.right * curPos;
                    Vector2 up = -Vector2.up * (line * (lineFont.gridHeight + lineFont.lineSpacing));
                    DrawCharacter(right + up, fontCharacter);
                    curPos += fontCharacter.width + 1;
                }
            }
        }*/
    }

    void DrawCharacter(Vector2 at, LineFont.LineFontCharacter lineFontCharacter)
    {
        for (int i = 0; i < lineFontCharacter.linePoints.Length; i += 2)
        {
            if (i >= lineFontCharacter.linePoints.Length || i + 1 >= lineFontCharacter.linePoints.Length)
                return;

            Gizmos.DrawLine(at + lineFontCharacter.linePoints[i], at + lineFontCharacter.linePoints[i + 1]);
            //Draw.Line(at + lineFontCharacter.linePoints[i], at + lineFontCharacter.linePoints[i + 1]);
        }
    }

    void Start()
    {

    }

    public Material lineMaterial;

    void OnRenderObject()
    {
        lineMaterial.SetPass(0);
        // GL.PushMatrix();
        GL.Begin(GL.LINES);

        DrawFontTest();

        GL.End();
        // GL.PopMatrix();
    }
}
