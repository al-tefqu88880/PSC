using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    int maxLeft = -10;
    int maxRight = 10;
    int maxUp = 10;
    int maxDown = -10;
    int maxIn = 3;
    int maxOut = -5;
    int x_move = 0;
    int y_move = 0;
    int z_move = 0;

    void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)& (x_move > maxLeft)) 
        {
            transform.Translate(-1, 0, 0);
            x_move--;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) & x_move < maxRight) 
        {
            transform.Translate(1, 0, 0);
            x_move++;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) & y_move < maxUp) 
        {
            transform.Translate(0, 1, 0);
            y_move++;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)& y_move > maxDown) 
        {
            transform.Translate(0, -1, 0);
            y_move--;
        }
        if (Input.mouseScrollDelta.y > 0 & z_move < maxIn) 
        {
            transform.Translate(0, 0, 1);
            z_move++;
        }
        if(Input.mouseScrollDelta.y < 0 & z_move > maxOut) 
        {
            transform.Translate(0, 0, -1);
            z_move--;
        }
    }
}
