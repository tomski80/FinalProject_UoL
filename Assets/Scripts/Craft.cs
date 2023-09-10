using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class Craft : MonoBehaviour
{
    public Stack<GameObject> CraftBuild = new Stack<GameObject>();
    public Rigidbody craftjointsParent = null;

    public bool commited = false;
    public float rotationSpeed = 1.0f;
    public GameObject viewCenter;

    private GameObject tempCraftPart;
    private MenuButtonsManager menuButtonsManager;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        menuButtonsManager = FindFirstObjectByType<MenuButtonsManager>();
    }
    void Update()
    {
        if(SceneManager.GetActiveScene().name == "DesignScene") // this should be only done in design scene 
        {
            if (!commited)
            {
                //Rotation
                if (Input.GetButton("RotateElementRight"))
                {
                    if (CraftBuild.TryPeek(out tempCraftPart))
                    {
                        tempCraftPart.transform.Rotate(0, -rotationSpeed, 0, 0);
                    }
                }

                if (Input.GetButton("RotateElementLeft"))
                {
                    if (CraftBuild.TryPeek(out tempCraftPart))
                    {
                        tempCraftPart.transform.Rotate(0, rotationSpeed, 0, 0);
                    }
                }

                Vector3 pos = viewCenter.transform.position;

                //Move
                if (Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
                {
                    if (CraftBuild.TryPeek(out tempCraftPart))
                    {
                        Vector3 partCenter = viewCenter.transform.position;
                        tempCraftPart.transform.position = partCenter;// (0, translationVertical, 0);
                    }
                }

               
                if (Input.GetButtonDown("Fire1"))
                {
                    if (CraftBuild.TryPeek(out tempCraftPart))
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;
                        // we want to commit to object when we click on any part of the scene... but not when there is nothing in the scene under/
                        // also if there is no base and we trying to commit something else, that is an exception and we shouldn't allow this
                        if (Physics.Raycast(ray, out hit, 1000))
                        {
                            {
                                Debug.Log("Commiting");
                                CraftPart craftPart = tempCraftPart.GetComponent<CraftPart>();
                                craftPart.CommitInitialize(transform.root, craftjointsParent);
                                commited = true;
                                menuButtonsManager.OnCommited();
                            }
                        }
                    }
                }
            }    
        }  
    }
}
