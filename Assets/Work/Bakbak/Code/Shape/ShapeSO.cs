using UnityEngine;

namespace Work.Bakbak.Code.Shape
{
    public enum ShapType
    {
        Line,
        HLine,
        UnderCheck,
        UpperCheck,
        Star,
        Circle,
    }
    [CreateAssetMenu(fileName = "ShapeSO", menuName = "SO/ShapeSO")]
    public class ShapeSO : ScriptableObject
    {
        public ShapType Shape => shape;
        [SerializeField]
        private ShapType shape;
        public Color Color => color;
        [SerializeField]
        private Color color;
        
        
        
    }
}
