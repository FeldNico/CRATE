using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(CrateConfig))]
    public class CrateConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var createConfig = target as CrateConfig;
            EditorGUILayout.LabelField("Vehicles");
            GUILayout.Space(15);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_minVehicleCount"));
            GUILayout.Space(5);
            var types = serializedObject.FindProperty("_vehicleTypes");
            EditorGUILayout.LabelField("Types");
            EditorGUI.indentLevel++;
            for (int i = 0; i < types.arraySize; i++)
            {
                EditorGUILayout.PropertyField(types.GetArrayElementAtIndex(i));
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("-",GUILayout.Width(150)))
                {
                    createConfig.VehicleTypes.RemoveAt(i);
                    serializedObject.ApplyModifiedProperties();
                    serializedObject.Update();
                }
                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("+",GUILayout.Width(650)))
            {
                createConfig.VehicleTypes.Add(new VehicleType("No Name",1));
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel--;
            GUILayout.Space(5);
            EditorGUILayout.LabelField("Vehicle Count");
            EditorGUI.indentLevel++;
            var fleet = createConfig.GetFleet();
            foreach (var type in createConfig.VehicleTypes)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(type.VehicleName);
                EditorGUILayout.LabelField(fleet[type].ToString());
                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel--;

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.LabelField("Assignments");
            GUILayout.Space(15);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_minMaxDays"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_maxVehicleCountPerTypePerDay"));
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            serializedObject.ApplyModifiedProperties();
        }
    }
}