using UnityEditor;
using GLShared.Networking.Components;

namespace GLShared.Networking
{
    #if UNITY_EDITOR
    [CustomEditor(typeof(NetworkEntity), true)]
    [CanEditMultipleObjects]
    public class NetworkEntityEditor : Editor
    {
        private SerializedProperty isSenderProperty, tickRateProperty;

        protected void OnEnable()
        {
            isSenderProperty = serializedObject.FindProperty("isSender");
            tickRateProperty = serializedObject.FindProperty("tickRate");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            if (isSenderProperty.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(tickRateProperty);
                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
    #endif
}
