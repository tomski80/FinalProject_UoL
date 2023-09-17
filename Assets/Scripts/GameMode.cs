using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
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


    //TODO: this could be better, maybe Array but I am runnign out of time
    public TMP_Text[] score; 

    public TMP_Text[] scoreNames;

    public TMP_InputField[] names;
    public GameObject[] namesGameObject;

    public int editIndex = -1;

    private bool scoreUpdated = false;

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
            UpdateScore();
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

    public void OnRestartButton()
    {
        //if (finish && Input.GetButtonDown("Fire1"))
        {
            // need first clear the level
            finish = false;
            distanceTravelled = 0.0f;
            craftScript.CraftBuild.Clear();
            DestroyImmediate(craft);
            SceneManager.LoadScene("DesignScene", LoadSceneMode.Single);
        }
    }

    public void OnNameEndEdit(string nameUpdated)
    {
        PlayerPrefs.SetString("scoreName" + (editIndex + 1), nameUpdated);
        PlayerPrefs.Save();
        scoreNames[editIndex].text = PlayerPrefs.GetString("scoreName" + (editIndex + 1));
        namesGameObject[editIndex].SetActive(false);
    }

    public void UpdateScore()
    {
        if (!scoreUpdated)
        {
            scoreUpdated = true;
            // replace highscore
            float[] scores = new float[4];
            int scoreIndex = -1;
            for (int i = 3; i >= 0; i--)
            {
                if (!PlayerPrefs.HasKey("scoreName" + (i + 1)))
                {
                    PlayerPrefs.SetString("scoreName" + (i + 1), "Tom ");
                }

                if (!PlayerPrefs.HasKey("score" + (i + 1)))
                {
                    PlayerPrefs.SetFloat("score" + (i + 1), 5.0f);

                }
                PlayerPrefs.Save();

                scores[i] = PlayerPrefs.GetFloat("score" + (i + 1));
                if (distanceTravelled > scores[i])
                {
                    scoreIndex = i;
                }
            }


            //replace score
            if (scoreIndex != -1)
            {
                List<float> scoresL = new List<float>();
                List<string> namesL = new List<string>();

                for (int j = 1; j < 5; j++)
                {
                    scoresL.Add(PlayerPrefs.GetFloat("score" + j.ToString()));
                    namesL.Add(PlayerPrefs.GetString("scoreName" + j.ToString()));
                }
                //names[scoreIndex].
                namesGameObject[scoreIndex].SetActive(true);

                
                TMP_InputField inputField = names[scoreIndex];
                editIndex = scoreIndex;
                PlayerPrefs.SetString("scoreName" + (scoreIndex + 1), "");
                inputField.ActivateInputField();
                inputField.onEndEdit.AddListener(OnNameEndEdit);
                scoresL.Insert(scoreIndex,distanceTravelled);
                namesL.Insert(scoreIndex, "");

                PlayerPrefs.SetFloat("score1" , scoresL[0]);
                PlayerPrefs.SetFloat("score2", scoresL[1]);
                PlayerPrefs.SetFloat("score3", scoresL[2]);
                PlayerPrefs.SetFloat("score4", scoresL[3]);

                PlayerPrefs.SetString("scoreName1", namesL[0]);
                PlayerPrefs.SetString("scoreName2", namesL[1]);
                PlayerPrefs.SetString("scoreName3", namesL[2]);
                PlayerPrefs.SetString("scoreName4", namesL[3]);

            }
            PlayerPrefs.Save();

            //update text
            score[0].text = PlayerPrefs.GetFloat("score1").ToString("F1");
            score[1].text = PlayerPrefs.GetFloat("score2").ToString("F1");
            score[2].text = PlayerPrefs.GetFloat("score3").ToString("F1");
            score[3].text = PlayerPrefs.GetFloat("score4").ToString("F1");

            scoreNames[0].text = PlayerPrefs.GetString("scoreName1");
            scoreNames[1].text = PlayerPrefs.GetString("scoreName2");
            scoreNames[2].text = PlayerPrefs.GetString("scoreName3");
            scoreNames[3].text = PlayerPrefs.GetString("scoreName4");
        }
    }
}
