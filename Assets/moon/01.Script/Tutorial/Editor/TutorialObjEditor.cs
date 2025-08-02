using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace moon._01.Script.Tutorial.Editor
{
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(TutorialObj))]
    public class TutorialObjDrawer : PropertyDrawer
    {
        const int TextAreaLines = 3;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var typeProp = property.FindPropertyRelative(nameof(TutorialObj.TutorialType));
            int subFieldCount = GetSubFieldCount(typeProp.enumValueIndex);

            float line = EditorGUIUtility.singleLineHeight;
            float space = EditorGUIUtility.standardVerticalSpacing;

            float totalLines = 1 + subFieldCount;
            float height = totalLines * line + (totalLines - 1) * space;

            if ((TutorialType)typeProp.enumValueIndex == TutorialType.Say)
            {
                if (FieldHasTextArea(nameof(TutorialObj.SayText)))
                {
                    height += (TextAreaLines - 1) * (line + space);
                }
            }

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            float line = EditorGUIUtility.singleLineHeight;
            float space = EditorGUIUtility.standardVerticalSpacing;
            float x = position.x;
            float y = position.y;
            float w = position.width;

            var typeProp = property.FindPropertyRelative(nameof(TutorialObj.TutorialType));
            EditorGUI.PropertyField(new Rect(x, y, w, line), typeProp);
            y += line + space;

            switch ((TutorialType)typeProp.enumValueIndex)
            {
                case TutorialType.Say:
                    DrawSayFields(ref y, x, w, line, space, property);
                    break;

                case TutorialType.Enemy:
                    DrawField(ref y, x, w, line, space, property, nameof(TutorialObj.EnemyPrefab));
                    DrawField(ref y, x, w, line, space, property, nameof(TutorialObj.SpawnPointInt));
                    DrawField(ref y, x, w, line, space, property, nameof(TutorialObj.EnemyStopTime));
                    break;
            }

            EditorGUI.EndProperty();
        }

        private void DrawSayFields(ref float y, float x, float w, float line, float space, SerializedProperty parent)
        {
            var sayProp = parent.FindPropertyRelative(nameof(TutorialObj.SayText));

            bool isTextArea = FieldHasTextArea(nameof(TutorialObj.SayText));
            float height = isTextArea
                ? TextAreaLines * line + (TextAreaLines - 1) * space
                : line;

            Rect sayRect = new Rect(x, y, w, height);
            if (isTextArea)
            {
                sayProp.stringValue = EditorGUI.TextArea(sayRect, sayProp.stringValue);
            }
            else
            {
                EditorGUI.PropertyField(sayRect, sayProp);
            }

            y += height + space;

            DrawField(ref y, x, w, line, space, parent, nameof(TutorialObj.TextTime));
            DrawField(ref y, x, w, line, space, parent, nameof(TutorialObj.TextDelayTime));
            DrawField(ref y, x, w, line, space, parent, nameof(TutorialObj.EnemyDieToText));
            DrawField(ref y, x, w, line, space, parent, nameof(TutorialObj.EndToDestroy));
        }

        private bool FieldHasTextArea(string fieldName)
        {
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var field = typeof(TutorialObj).GetField(fieldName, flags);
            if (field == null) return false;
            return field.GetCustomAttribute<TextAreaAttribute>() != null;
        }

        private int GetSubFieldCount(int enumIndex)
        {
            switch ((TutorialType)enumIndex)
            {
                case TutorialType.Say: return 5;
                case TutorialType.Enemy: return 3;
                default: return 0;
            }
        }

        private void DrawField(ref float y, float x, float width, float line, float space, SerializedProperty parent,
            string fieldName)
        {
            var prop = parent.FindPropertyRelative(fieldName);
            EditorGUI.PropertyField(new Rect(x, y, width, line), prop);
            y += line + space;
        }
    }
}