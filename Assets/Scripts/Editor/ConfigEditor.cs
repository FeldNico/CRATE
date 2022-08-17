using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(CrateConfig))]
    public class CrateConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
        }
    }
}