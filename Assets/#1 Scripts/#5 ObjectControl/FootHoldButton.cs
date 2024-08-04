using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using ColorUtility = UnityEngine.ColorUtility;

public class FootHoldButton : MonoBehaviour
{
    public bool isToggle = true;
    [HideInInspector]
    public string downDirection = "X";
    [HideInInspector]
    public int flip = 1;

    public int SignalType = 0;
    public bool signal = false;

    public List<Collider2D> downObj;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Head") || other.CompareTag("Body") || other.CompareTag("Arrow"))
        {
            downObj.Add(other);
        }
        else
        {
            return;
        }
        
        if (downObj.Count == 1 && !signal && !isToggle)
        {
            //신호 보내기 넣어야함
            signal = true;
            moveButton("enter");
        }

        if (downObj.Count == 1 && isToggle)
        {
            signal = !signal;
            if (signal)
            {
                moveButton("enter");
            }
            else
            {
                moveButton("exit");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Head") || other.CompareTag("Body") || other.CompareTag("Arrow"))
        {
            downObj.Remove(other);
        }
        else
        {
            return;
        }
        
        if (downObj.Count == 0 && signal && !isToggle)
        {
            signal = false;
            moveButton("exit");
        }
    }

    private void moveButton(string str)
    {
        int f = str == "enter" ? 1 : -1;
        if (downDirection == "Y")
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f*flip*f,
                transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x - 0.2f*flip*f, transform.position.y,
                transform.position.z);
        }
    }

    public void Update()
    {
        //919191,96FF7F
        Color color;
        if (signal)
        {
            ColorUtility.TryParseHtmlString("#96FF7F",out color);
            GetComponent<SpriteRenderer>().color = color;
        }
        else
        {
            ColorUtility.TryParseHtmlString("#919191",out color);
            GetComponent<SpriteRenderer>().color = color;
        }
    }
}
