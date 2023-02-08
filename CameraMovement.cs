using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public int maxLeft = -10;
    public int maxRight = 10;
    public int maxUp = 10;
    public int maxDown = -10;
    int maxIn = 0;
    int maxOut = 8;

    double x_move = 0;
    double y_move = 0;
    int z_move = 2;

    static int[] camera_position = { -2, -5, -7, -10, -15, -25, -35, -45, -55 };
    static double[] camera_speed = { 0.05, 0.07, 0.1, 0.15, 0.25, 0.5, 0.7, 1, 1.5 };


    void Update()
    {
        if ((Input.GetKey(KeyCode.LeftArrow) | Input.GetKey(KeyCode.Q)) & (x_move > maxLeft)) 
        {
            var move = (-1)*camera_speed[z_move];
            transform.Translate((float)move, 0, 0);
            x_move+=move;
        }
        if ((Input.GetKey(KeyCode.RightArrow) | Input.GetKey(KeyCode.D)) & x_move < maxRight) 
        {
            var move = camera_speed[z_move];
            transform.Translate((float)move, 0, 0);
            x_move += move;
        }
        if ((Input.GetKey(KeyCode.UpArrow) | Input.GetKey(KeyCode.Z)) & y_move < maxUp) 
        {
            var move = camera_speed[z_move];
            transform.Translate(0, (float)move, 0);
            y_move += move;
        }
        if ((Input.GetKey(KeyCode.DownArrow) | Input.GetKey(KeyCode.S)) & y_move > maxDown) 
        {
            var move = (-1) * camera_speed[z_move];
            transform.Translate(0, (float)move, 0);
            y_move += move;
        }
        if (Input.mouseScrollDelta.y > 0 & z_move > maxIn) 
        {
            z_move--;
            var new_pos = transform.position;
            new_pos.z = camera_position[z_move];
            transform.position=new_pos;
        }
        if(Input.mouseScrollDelta.y < 0 & z_move < maxOut) 
        {
            z_move++;
            var new_pos = transform.position;
            new_pos.z = camera_position[z_move];
            transform.position = new_pos;
        }
    }
}
