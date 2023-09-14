using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour
{
    Rigidbody[] craftParts;
    Craft craftScript;
    Rigidbody baseRB;
    float countDownToForce; // temporary before we made it interactive

    public GameObject craft;
    public Transform spawnPoint;
    public TMP_Text distanceText;

    public void Start()
    {
        //pause game before we setup craft fully
        Time.timeScale = 0.0f;

        //setup physics on new craft - all RigidBodies are kinematic in designer 
        craft = GameObject.Find("CraftBuild");
        craftParts = craft.GetComponentsInChildren<Rigidbody>();
        craftScript = craft.GetComponent<Craft>();
        if(craftScript != null)
        {
            baseRB = craftScript.craftjointsParent.GetComponent<Rigidbody>();    // get the base rigid body component, so we can push it 
        }
        
        foreach (Rigidbody craftPart in craftParts)
        {
            craftPart.isKinematic = false;
            craftPart.useGravity = true;
        }

        craft.transform.position = spawnPoint.position;
        craft.transform.rotation = spawnPoint.rotation;
    }

    public void Update()
    {
        // start simulation
        if (Input.GetButtonDown("Fire1"))
        {
            Time.timeScale = 1.0f;
        }

        UpdateDistanceTravelled();
    }

    public void FixedUpdate()
    {
        countDownToForce += Time.deltaTime;
        if(countDownToForce > 2.0f && countDownToForce < 5.0f)
        {
            //we want to add constant force to the craft first
            Vector3 m_Direction = Vector3.forward;
            baseRB.AddForce(m_Direction * -2000);
        }
    }

    public void UpdateDistanceTravelled()
    {
        float distanceTravelled = spawnPoint.position.z - baseRB.transform.position.z;
        distanceTravelled = Mathf.Max(0, distanceTravelled);
        distanceText.text = "Distance: " + distanceTravelled.ToString("F1") + "m";
    }
}
