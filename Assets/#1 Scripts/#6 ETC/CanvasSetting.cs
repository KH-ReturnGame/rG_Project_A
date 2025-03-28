using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSetting : MonoBehaviour
{
    void Update()
    {
        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null)
        {
            //canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.Normal;
        }
    }
}
