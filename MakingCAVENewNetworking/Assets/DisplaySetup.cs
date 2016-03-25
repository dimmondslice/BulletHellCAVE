using UnityEngine;
using System.Collections;

public class DisplaySetup : MonoBehaviour 
{
    void Awake()
    {
        //active the projects displays
        Display.displays[1].Activate();
        Display.displays[2].Activate();
    }
}
