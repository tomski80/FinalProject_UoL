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
    public GameObject finishText;

    float stationaryTimer = 0.0f;
    bool finish = false;
    bool start = false;
    float distanceTravelled = 0.0f;
    float lastDistanceTravelled = 0.0f;

    public float startingForce = -3200.0f;


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

            // check if we have wheel, if not we don't push 
            // to avoid situation that we get better score without wheels 
            // couldn't tweak physics to avoid it 
            GameObject[] craftWheels = craftScript.CraftBuild.ToArray();
            bool hasWheels = false;
            foreach(GameObject craftWheel in craftWheels)
            {
                if(craftWheel.GetComponent<CraftPartWheel>())
                {
                    hasWheels = true;
                }
            }
            if(!(hasWheels))
            {
                startingForce = startingForce / 5;
                Debug.Log("No Wheels!");
            }
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
            start = true;
            Time.timeScale = 1.0f;
        }

        if (finish && Input.GetButtonDown("Fire1"))
        {
            // need first clear the level
            finish = false;
            distanceTravelled = 0.0f;
            craftScript.CraftBuild.Clear();
            DestroyImmediate(craft);
            SceneManager.LoadScene("DesignScene", LoadSceneMode.Single);
        }

    }

    public void FixedUpdate()
    {
        countDownToForce += Time.deltaTime;
        if(countDownToForce > 2.0f && countDownToForce < 5.0f)
        {
            //we want to add constant force to the craft first
            Vector3 m_Direction = Vector3.forward;
            baseRB.AddForce(m_Direction * startingForce);
        }

        // need to do in fixed update because of finish detecting 
        UpdateDistanceTravelled();
        if (finish)
        {
            Debug.Log("FINISHED!");
            finishText.SetActive(true);

        }

    }

    public void UpdateDistanceTravelled()
    {
        distanceTravelled = spawnPoint.position.z - baseRB.transform.position.z;
        distanceTravelled = Mathf.Max(0, distanceTravelled);
        distanceText.text = "Distance: " + distanceTravelled.ToString("F1") + "m";
        if(start)
        {
            finish = detectCraftStopped();
        }
        lastDistanceTravelled = distanceTravelled;
        Debug.Log("LastDistance: " + lastDistanceTravelled);
        Debug.Log("Distance: " + distanceTravelled);
    }

    public bool detectCraftStopped()
    { 
        if((distanceTravelled - lastDistanceTravelled < 0.01))
        {
            stationaryTimer += Time.deltaTime;
        }
        return (stationaryTimer > 5.0f);
    }

}
