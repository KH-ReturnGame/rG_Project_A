using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Door))]
public class DoorEditor : Editor
{
    private string[] signalOptions = new string[] { "0","1","2","3","4","5","6","7","8","9" };
    private string[] doorTypeOptions = new string[] { "위아래", "회전" };
    private string[] rotateTypeOptions = new string[] { "바로 회전", "한칸 띄워서 회전" };

    public override void OnInspectorGUI()
    {
        Door script = (Door)target;
        
        int signalSelectedIndex = script.SignalType;
        int doorTypeIndex = script.DoorType=="UpDown"?0:1;
        int rotateTypeIndex = script.RotateType;
        
        // 기본 인스펙터 그리기
        DrawDefaultInspector();

        // 드롭다운 리스트 추가
        signalSelectedIndex = EditorGUILayout.Popup("Select Signal", signalSelectedIndex, signalOptions);
        doorTypeIndex = EditorGUILayout.Popup("Select Door Option", doorTypeIndex, doorTypeOptions);
        rotateTypeIndex = EditorGUILayout.Popup("Select Rotate Option", rotateTypeIndex, rotateTypeOptions);
        
        // 변경사항 적용
        if (GUI.changed)
        {
            EditorUtility.SetDirty(script);
        }
        script.SignalType = int.Parse(signalOptions[signalSelectedIndex]);
        script.DoorType = doorTypeIndex == 0 ? "UpDown" : "Rotate";
        script.RotateType = rotateTypeIndex;
    }
}