using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
//using System.Numerics;

public class MenuButtonsManager : MonoBehaviour
{
    public GameObject database;
    public GameObject partButton;

    public GameObject[] scrollViews;
    public Transform[] contentContainters;
    public GameObject craftManager;

    public TMP_Text steveTextBubble;
    public SteveManager steveManager;


    //populate menu with Craft Parts Elements (buttons)
    public void OnEnable()
    {
        UpdateSteveText("Start");

        RectTransform buttonSize = partButton.GetComponent<RectTransform>();
        Vector3 buttonOffset = new Vector3(0.0f, -buttonSize.rect.width, 0.0f);

        CraftElementsDatabase myData = database.GetComponent<CraftElementsDatabase>();
        ScriptableObject[][] CraftPartsArray = { myData.CraftBase, myData.CraftWheels, myData.CraftWings, myData.CraftMisc };

        for (int i = 0; i < contentContainters.Length; i++)
        {
            int buttonCounter = 0;
            ContentSizeFitter contentContainer = contentContainters[i].GetComponent<ContentSizeFitter>();
            foreach (CraftPartInfo craftPartInfo in CraftPartsArray[i])
            {
                Debug.Log(craftPartInfo);
                GameObject buttonUI = Instantiate(partButton, contentContainer.transform);
                //buttonUI.transform.position += buttonCounter * buttonOffset;
                Button button = buttonUI.GetComponent<Button>();
                Image buttonImage = buttonUI.transform.GetChild(0).GetComponent<Image>();
                button.onClick.AddListener(() => OnClickCraftPart(button));
                CraftPartToSpawn craftPartToSpawn = button.GetComponent<CraftPartToSpawn>();
                craftPartToSpawn.craftPartPrefab = craftPartInfo.prefab;
                craftPartToSpawn.partTag = craftPartInfo.tag;
                buttonImage.sprite = craftPartInfo.Image;
                TMP_Text buttonText = buttonUI.GetComponentInChildren<TMP_Text>();
                buttonText.text = craftPartInfo.prefabName;
                buttonCounter++;
            }
        }
    }

    public void OnClickBasesButton()
    {
        HideScrollViews();
        scrollViews[0].SetActive(true);
    }

    public void OnClickWheelsButton()
    {
        HideScrollViews();
        scrollViews[1].SetActive(true);
    }

    public void OnClickWingsButton()
    {
        HideScrollViews();
        scrollViews[2].SetActive(true);
    }

    public void HideScrollViews()
    {
        foreach (GameObject scrollView in scrollViews)
        {
            scrollView.SetActive(false);
        }
    }

    public void OnClickCraftPart(Button button)
    {
        Craft craft = craftManager.GetComponent<Craft>();

        UpdateSteveText("NewElement");

        if (craft.commited == false)
        {
            Debug.Log("Disappear you are not commited!");
            //remove Part that we didn't commit to
            GameObject craftPart = craft.CraftBuild.Pop();
            Destroy(craftPart);
            craft.commited = true;
        }
        // ... after we removed not commited elements we still need to spawn the new one 

        craft.commited = false;
        GameObject cursor3d = GameObject.Find("CenterX");
        CraftPartToSpawn craftPartToSpawn = button.GetComponent<CraftPartToSpawn>();

        Vector3 spawnPos = cursor3d.transform.position;
        Quaternion spawnRotation = Quaternion.identity;// cursor3d.transform.rotation;

        GameObject spawnedObject = Instantiate(craftPartToSpawn.craftPartPrefab, spawnPos, spawnRotation);
        if (craft.CraftBuild.Count == 0)
        {
            craft.craftjointsParent = spawnedObject.GetComponent<Rigidbody>();
            spawnedObject.GetComponent<CraftPart>().Initialize();
        }
        else 
            spawnedObject.GetComponent<CraftPart>().Initialize();


        craft.CraftBuild.Push(spawnedObject);

    }
    
    // let's have a chance to do something when craft part is commited 
    public void OnCommited()
    {
        Debug.Log("OnCommited!");
        UpdateSteveText("Commited");
    }

    public void UpdateSteveText(string key)
    {
        string helpText = string.Empty;
        steveManager.helpTexts.TryGetValue(key,out helpText);
        steveTextBubble.text = helpText;
    }

}


