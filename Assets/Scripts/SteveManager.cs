using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteveManager : MonoBehaviour
{
    public Dictionary<string, string> helpTexts = new() { { "Start", "Hello I'm Steve... I am here to guide you. First choose the base of the craft, you can find the list at the top left corner." },
        {"NewElement" , "You can move with WSAD and rotate using Q and E key. Click on the element to appove it!" },
        {"Commited" , "Excellent! Now you have the base, you can build rest of the craft - add some wheels and wings. Have fun!" }
    
    };

}
