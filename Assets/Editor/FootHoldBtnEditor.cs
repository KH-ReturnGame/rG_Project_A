using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FootHoldButton))]
public class FootHoldBtnEditor : Editor
{
    private string[] xyoptions = new string[] { "X", "Y" };
    
    private string[] signalOptions = new string[] { "0","1","2","3","4","5","6","7","8","9","10","11","12","13","14","15","16","17","18","19" };

    private string[] buttonFlipOtions = new string[] { "-1", "1" };

    private string[] oneUseOptions = new string[] { "한번만 사용 가능", "여러번 사용 가능" };

    private string[] oneUseOptinos2 = new string[] { "켜지는 것만 가능", "켜지고 꺼지는 것까지 가능" };
    
    private string[] canClickArrow = new string[] { "화살 누를 수 있어", "화살 못 눌러" };

    public override void OnInspectorGUI()
    {
        FootHoldButton script = (FootHoldButton)target;
        
        
        int xyselectedIndex = script.downDirection=="X"?0:1;
        int signalSelectedIndex = script.SignalType;
        int buttonFlipIndex = script.flip==-1?0:1;
        int oneUseIndex = script.oneUse ? 0 : 1;
        int oneUse2Index = script.onOnly ? 0 : 1;
        int cnaClickArrowIndex = script.canClickArrow ? 0 : 1;
        
        
        // 기본 인스펙터 그리기
        DrawDefaultInspector();

        // 드롭다운 리스트 추가
        xyselectedIndex = EditorGUILayout.Popup("Select XY Option", xyselectedIndex, xyoptions);
        signalSelectedIndex = EditorGUILayout.Popup("Select Signal", signalSelectedIndex, signalOptions);
        buttonFlipIndex = EditorGUILayout.Popup("Select Flip", buttonFlipIndex, buttonFlipOtions);
        oneUseIndex = EditorGUILayout.Popup("Select One Use", oneUseIndex, oneUseOptions);
        oneUse2Index = EditorGUILayout.Popup("Select On Only", oneUse2Index, oneUseOptinos2);
        cnaClickArrowIndex = EditorGUILayout.Popup("Can Click Arrow", cnaClickArrowIndex, canClickArrow);
        
        // 변경사항 적용
        if (GUI.changed)
        {
            EditorUtility.SetDirty(script);
        }

        script.downDirection = xyoptions[xyselectedIndex];
        script.SignalType = int.Parse(signalOptions[signalSelectedIndex]);
        script.flip = (buttonFlipIndex == 0) ? -1 : 1;
        script.oneUse = oneUseIndex == 0;
        script.onOnly = oneUse2Index == 0;
        script.canClickArrow = cnaClickArrowIndex == 0;
    }
}