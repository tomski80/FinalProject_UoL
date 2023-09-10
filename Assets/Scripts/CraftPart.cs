using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftPart : MonoBehaviour
{
    public Material ghostMaterial;
    public Material defaultMaterial;

    public virtual void Initialize()
    {
        Debug.Log("Default Initialize");
        Debug.Log(ghostMaterial);
        SetMaterial(ghostMaterial);
    }

    public virtual void CommitInitialize(Transform newTransform, Rigidbody jointConnectedBody)
    {
        Debug.Log("Default Initialize");
        Debug.Log(defaultMaterial);

        SetMaterial(defaultMaterial);
        gameObject.transform.parent = newTransform;

        //prevent connecting to itself
        if (jointConnectedBody.gameObject == gameObject)
        {
            Debug.Log("Removing Joints from parent base");
            HingeJoint hJoint = gameObject.GetComponentInChildren<HingeJoint>();
            Destroy(hJoint);
            FixedJoint fJoint = gameObject.GetComponentInChildren<FixedJoint>();
            Destroy(fJoint);
        }

        if(jointConnectedBody != null && jointConnectedBody.gameObject != gameObject)
        {
            HingeJoint hJoint = gameObject.GetComponentInChildren<HingeJoint>();
            if (hJoint != null)
            {
                Debug.Log("Adding joint parent");
                hJoint.connectedBody = jointConnectedBody;
            }

            FixedJoint fJoint = gameObject.GetComponentInChildren<FixedJoint>();
            if (fJoint != null)
            {
                Debug.Log("Adding joint parent");
                fJoint.connectedBody = jointConnectedBody;
            }
        }
        //
    }

    public void SetMaterial(Material mat)
    {
        Renderer[] childRenderers = gameObject.GetComponentsInChildren<Renderer>();
        Renderer[] renderers = gameObject.GetComponents<Renderer>();
        //GetComponents<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            if (renderer)
            {
                renderer.material = mat;
            }
        }

        foreach (Renderer renderer in childRenderers)
        {
            if (renderer)
            {
                renderer.material = mat;
            }
        }

    }
}
