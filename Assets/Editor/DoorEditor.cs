using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Door))]
public class DoorEditor : Editor
{
    private string[] signalOptions = new string[] { "0","1","2","3","4","5","6","7","8","9","10","11","12","13","14","15","16","17","18","19" };
    private string[] doorTypeOptions = new string[] { "위아래", "회전" };
    private string[] rotateTypeOptions = new string[] { "바로 회전", "한칸 띄워서 회전" };
    private string[] updownOptions = new string[] { "1","-1"};
    private string[] xyTypeOptions = new string[] { "x", "y" };
    //private string[] rotateType2_Optinos = new string[] { "어떤 신호든 감지 시 회전", "켜질때만 회전" };

    public override void OnInspectorGUI()
    {
        Door script = (Door)target;
        
        int signalSelectedIndex = script.SignalType;
        int doorTypeIndex = script.DoorType=="UpDown"?0:1;
        int rotateTypeIndex = script.RotateType;
        int updownIndex = script.updown == 1 ? 0 : 1;
        int xyTypeIndex = script.xyType;
        
        // 기본 인스펙터 그리기
        DrawDefaultInspector();

        // 드롭다운 리스트 추가
        signalSelectedIndex = EditorGUILayout.Popup("Select Signal", signalSelectedIndex, signalOptions);
        doorTypeIndex = EditorGUILayout.Popup("Select Door Option", doorTypeIndex, doorTypeOptions);
        rotateTypeIndex = EditorGUILayout.Popup("Select Rotate Option", rotateTypeIndex, rotateTypeOptions);
        updownIndex = EditorGUILayout.Popup("Select Updown", updownIndex, updownOptions);
        xyTypeIndex = EditorGUILayout.Popup("X,Y option", xyTypeIndex, xyTypeOptions);
        
        // 변경사항 적용
        if (GUI.changed)
        {
            EditorUtility.SetDirty(script);
        }
        script.SignalType = int.Parse(signalOptions[signalSelectedIndex]);
        script.DoorType = doorTypeIndex == 0 ? "UpDown" : "Rotate";
        script.RotateType = rotateTypeIndex;
        script.updown = updownIndex == 0 ? 1 : -1;
        script.xyType = xyTypeIndex;
    }
}