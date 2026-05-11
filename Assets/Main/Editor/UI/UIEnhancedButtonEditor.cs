using Main.Scripts.UI.Base;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace Main.Editor.UI
{
    [CustomEditor(typeof(UIEnhancedButton), true)]
    [CanEditMultipleObjects]
    public class UIEnhancedButtonEditor : ButtonEditor
    {
        private SerializedProperty _highlightedSound;
        private SerializedProperty _clickSound;

        protected override void OnEnable()
        {
            base.OnEnable();
            _highlightedSound = serializedObject.FindProperty("highlightedSound");
            _clickSound = serializedObject.FindProperty("clickSound");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();
            serializedObject.Update();
            EditorGUILayout.PropertyField(_highlightedSound);
            EditorGUILayout.PropertyField(_clickSound);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
