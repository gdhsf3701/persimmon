using csiimnida.CSILib.SoundManager.RunTime;
using UnityEngine;
using Work.Bakbak.Code.Shape;

public class ShapeGeter : MonoSingleton<ShapeGeter>
{
    public Sprite GetShape(ShapType type)
    {
        switch (type)
        {
            case ShapType.Line:
                return null;
            case ShapType.HLine:
                return null;
            case ShapType.UnderCheck:
                return null;
            case ShapType.UpperCheck:
                return null;
            case ShapType.Star:
                return null;
            case ShapType.Circle:
                return null;
            default:
                return null;
        }
    }
}