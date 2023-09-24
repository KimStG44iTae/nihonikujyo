using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JavascriptHook : MonoBehaviour
{
    float X_length;
    float Y_length;
    float pos_x;
    float pos_y;
    float X;
    float Y;
    // Start is called before the first frame update
    void Start()
    {
        pos_x = 0f;
        X_length = 1f;
        Y_length = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(pos_x, 0f, 0f);
        transform.localScale = new Vector3(X_length, Y_length, 1f);
    }

    public void Move(string RorL)
    {
        if (RorL.Equals("right"))
        {
            pos_x += 0.2f;
            Debug.Log("right move");
        }else if (RorL.Equals("left")) 
        {
            pos_x -= 0.2f;
            Debug.Log("left move");
        }
        else
        {
            Debug.Log("move err : " + RorL);
        }
    }
    public void length(string su)
    {
        string[] result;
        result = su.Split('@');
        X_length = int.Parse(result[0]);
        Y_length = int.Parse(result[1]);
        Debug.Log("length : " + su);
    }
}
