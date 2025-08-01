using csiimnida.CSILib.SoundManager.RunTime;
using UnityEngine;
using Work.Bakbak.Code.Shape;

public class ShapeGeter : MonoSingleton<ShapeGeter>
{
    private Sprite line;
    private Sprite hLine; 
    private Sprite underCheck; 
    private Sprite upperCheck;
    private Sprite star;
    private Sprite circle;
    private Sprite eleck;

    private bool loaded = false;
    public Sprite GetShape(ShapType type)
    {
        if(loaded == false)
        {
            line = Resources.Load<Sprite>("shapes/101_20250801172451");
            hLine = Resources.Load<Sprite>("shapes/101_20250801172414");
            underCheck = Resources.Load<Sprite>("shapes/101_20250801173303");
            upperCheck = Resources.Load<Sprite>("shapes/101_20250801173349");
            star = Resources.Load<Sprite>   ("shapes/101_20250801173029");
            circle = Resources.Load<Sprite>("shapes/100_20250801171814");
            eleck = Resources.Load<Sprite>("shapes/101_20250802010029");
        }
        switch (type)
        {
            case ShapType.Line:
                return line;
            case ShapType.HLine:
                return hLine;
            case ShapType.UnderCheck:
                return underCheck;
            case ShapType.UpperCheck:
                return upperCheck;
            case ShapType.Star:
                return star;
            case ShapType.Circle:
                return circle;
            case ShapType.Eleck:
                return eleck;
            default:
                return null;
        }
    }
}