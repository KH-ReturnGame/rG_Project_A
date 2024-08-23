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
    public RectTransform ArrowUI;

    public void Update()
    {
        switch (pm.ControlMode)
        {
            case "Head":
                HeadUI.localScale = Vector3.Slerp(HeadUI.localScale, new Vector3(1, 1, 1), 10 * Time.unscaledDeltaTime);
                BodyUI.localScale = Vector3.Slerp(BodyUI.localScale, new Vector3(0.7f, 0.7f, 0.7f), 10 * Time.unscaledDeltaTime);
                ArrowUI.localScale = Vector3.Slerp(ArrowUI.localScale, new Vector3(0.7f, 0.7f, 0.7f), 10 * Time.unscaledDeltaTime);
                break;
            case "Body":
                HeadUI.localScale = Vector3.Slerp(HeadUI.localScale, new Vector3(0.7f, 0.7f, 0.7f), 10 * Time.unscaledDeltaTime);
                BodyUI.localScale = Vector3.Slerp(BodyUI.localScale, new Vector3(1, 1, 1), 10 * Time.unscaledDeltaTime);
                ArrowUI.localScale = Vector3.Slerp(ArrowUI.localScale, new Vector3(0.7f, 0.7f, 0.7f), 10 * Time.unscaledDeltaTime);
                break;
            case "Arrow":
                HeadUI.localScale = Vector3.Slerp(HeadUI.localScale, new Vector3(0.7f, 0.7f, 0.7f), 10 * Time.unscaledDeltaTime);
                BodyUI.localScale = Vector3.Slerp(BodyUI.localScale, new Vector3(0.7f, 0.7f, 0.7f), 10 * Time.unscaledDeltaTime);
                ArrowUI.localScale = Vector3.Slerp(ArrowUI.localScale, new Vector3(1, 1, 1), 10 * Time.unscaledDeltaTime);
                break;
        }
    }
}