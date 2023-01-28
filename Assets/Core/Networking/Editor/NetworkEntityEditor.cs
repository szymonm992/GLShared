using UnityEditor;
using GLShared.Networking.Components;

namespace GLShared.Networking
{
    #if UNITY_EDITOR
    [CustomEditor(typeof(NetworkEntity), true)]
    [CanEditMultipleObjects]
    public class NetworkEntityEditor : Editor
    {
        private SerializedProperty isSenderProperty, syncRateProperty;

        protected void OnEnable()
        {
            isSenderProperty = serializedObject.FindProperty("isSender");
            syncRateProperty = serializedObject.FindProperty("syncRate");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            if (isSenderProperty.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(syncRateProperty);
                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
    #endif
}
