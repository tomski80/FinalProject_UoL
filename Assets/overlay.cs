using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class overlay : MonoBehaviour
{
    // Start is called before the first frame update

    public void Start()
    {

        if(PersistantData.overlayVisibility)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void ToggleOverlay()
    {
        PersistantData.overlayVisibility = !PersistantData.overlayVisibility;
        gameObject.SetActive(PersistantData.overlayVisibility);
    }
}
