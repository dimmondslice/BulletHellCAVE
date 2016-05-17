using UnityEngine;
using System.Collections;

public class DisplaySetup : MonoBehaviour 
{
	//
	//
	//this script is just used on start up to activate our extra displays
	//
	//
    void Awake()
    {
        //active the projects displays
        if(Display.displays.Length > 1)
        {
            Display.displays[1].Activate();
        }
        if (Display.displays.Length > 2)
        {
            Display.displays[2].Activate();
        }
    }
}
