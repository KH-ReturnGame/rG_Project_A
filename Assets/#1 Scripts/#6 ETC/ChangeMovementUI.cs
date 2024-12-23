using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class ChangeMovementUI : MonoBehaviour
{
    public PlayerMovement pm;

    public RectTransform HeadUI;
    public RectTransform BodyUI;
    //public RectTransform ArrowUI;

    public void Update()
    {
        float dt = ((!GameManager.Instance.hasFocus) ? Time.unscaledDeltaTime : Time.deltaTime);
        switch (pm.controlMode)
        {
            case "Head":
                HeadUI.localScale = Vector3.Slerp(HeadUI.localScale, new Vector3(1, 1, 1), 10 * dt);
                BodyUI.localScale = Vector3.Slerp(BodyUI.localScale, new Vector3(0.7f, 0.7f, 0.7f), 10 * dt);
                //ArrowUI.localScale = Vector3.Slerp(ArrowUI.localScale, new Vector3(0.7f, 0.7f, 0.7f), 10 * dt);
                break;
            case "Body":
                HeadUI.localScale = Vector3.Slerp(HeadUI.localScale, new Vector3(0.7f, 0.7f, 0.7f), 10 * dt);
                BodyUI.localScale = Vector3.Slerp(BodyUI.localScale, new Vector3(1, 1, 1), 10 * dt);
                //ArrowUI.localScale = Vector3.Slerp(ArrowUI.localScale, new Vector3(0.7f, 0.7f, 0.7f), 10 * dt);
                break;
            case "Arrow":
                HeadUI.localScale = Vector3.Slerp(HeadUI.localScale, new Vector3(0.7f, 0.7f, 0.7f), 10 * dt);
                BodyUI.localScale = Vector3.Slerp(BodyUI.localScale, new Vector3(0.7f, 0.7f, 0.7f), 10 * dt);
                //ArrowUI.localScale = Vector3.Slerp(ArrowUI.localScale, new Vector3(1, 1, 1), 10 * dt);
                break;
        }

        var headUILocalPosition = HeadUI.anchoredPosition3D;
        var bodyUILocalPosition = BodyUI.anchoredPosition3D;

        headUILocalPosition.x = -60;
        bodyUILocalPosition.x = 60;

        HeadUI.anchoredPosition = headUILocalPosition;
        BodyUI.anchoredPosition = bodyUILocalPosition;

        //ArrowUI.gameObject.SetActive(GameManager.Instance.useArrow);

        
        
    }
}
