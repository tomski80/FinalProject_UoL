using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayCameraManager : MonoBehaviour
{
    GameObject craft;
    Rigidbody craftRB;
    Craft craftScript;
    public float cameraOffsetX = -30.0f;
    public float cameraOffsetY = 3.0f;
    public float cameraOffsetZ = -5.0f;

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
        
        transform.position = craftRB.transform.position + new Vector3(cameraOffsetX, cameraOffsetY , cameraOffsetZ);
    }
}

