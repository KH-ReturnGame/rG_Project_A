using System;
using System.Collections;
using System.Collections.Generic;
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
    private bool signal = false;

    public List<Collider2D> downObj;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Head") || other.CompareTag("Body") || other.CompareTag("Arrow"))
        {
            downObj.Add(other);
        }

        if (downObj.Count == 1 && !signal)
        {
            if (isToggle)
            {
                signal = !signal;
            }
            else
            {
                //신호 보내기 넣어야함
                signal = true;
                if (downDirection == "Y")
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f*flip,
                        transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(transform.position.x - 0.2f*flip, transform.position.y,
                        transform.position.z);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Head") || other.CompareTag("Body") || other.CompareTag("Arrow"))
        {
            downObj.Remove(other);
        }
        if (downObj.Count == 0 && signal)
        {
            if (isToggle)
            {
                
            }
            else
            {
                //신호 보내기 넣어야함
                signal = false;
                if (downDirection == "Y")
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f*flip,
                        transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(transform.position.x + 0.2f*flip, transform.position.y,
                        transform.position.z);
                }
            }
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
