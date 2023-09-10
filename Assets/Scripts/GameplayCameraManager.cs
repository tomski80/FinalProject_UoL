using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayCameraManager : MonoBehaviour
{
    GameObject craft;
    Rigidbody craftRB;
    Craft craftScript;
    public float cameraOffset = -30.0f;

    // Start is called before the first frame update
    void Start()
    {
        //setup physics on new craft
        craft = GameObject.Find("CraftBuild");
        craftScript = craft.GetComponent<Craft>();
        craftRB = craftScript.craftjointsParent;
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position = craftRB.transform.position + new Vector3(cameraOffset, 0, -10);
    }
}

