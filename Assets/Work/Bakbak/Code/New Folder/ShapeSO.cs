using UnityEngine;

namespace Work.Bakbak.Code.New_Folder
{
    public enum ShapType
    {
        Line,
        HLine,
        UnderCheck,
        UpperCheck,
        star,
        circle,
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
