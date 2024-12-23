using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ArrowCreate))]
public class ArrowCreateEditor: Editor
{
    private string[] signalOptions = new string[] { "0","1","2","3","4","5","6","7","8","9","10","11","12","13","14","15","16","17","18","19" };
    
    public override void OnInspectorGUI()
    {
        ArrowCreate script = (ArrowCreate)target;
        int signalSelectedIndex = script.SignalType;
        
        
        // 기본 인스펙터 그리기
        DrawDefaultInspector();

        // 드롭다운 리스트 추가
        signalSelectedIndex = EditorGUILayout.Popup("Select Signal", signalSelectedIndex, signalOptions);
        
        // 변경사항 적용
        if (GUI.changed)
        {
            EditorUtility.SetDirty(script);
        }
        script.SignalType = int.Parse(signalOptions[signalSelectedIndex]);
    }
}