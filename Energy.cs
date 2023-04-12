using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Energy : MonoBehaviour
{

    public TextMeshProUGUI text;
    public static int value = 0;
    public static void Add(int n)
    {
        value+= n;
    }

    public static bool Retrieve(int n)
    {
        if(n>value) return false;
        value-=n; return true;
    }

    public static int Get()
    {
        return value;
    }
    public static void Set(int  n)
    {
        value= n;
    }

    private void Update()
    {
        text.SetText("Energy :" + value.ToString());

    }
}
