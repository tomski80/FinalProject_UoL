using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    //objects to work with

    public Camera myCamera;
    Transform myCameraTransform;
    public Transform centerXTransform;
    public Transform centerYTransform;

    public float zoomSpeed = 100.0f;
    public float moveSpeed = 10.0f;
    public float rotateSpeed = 1.0f;

    Vector3 lastMousePosition = Vector3.zero;
    Vector3 defaultCameraPosition;
    Vector3 defaultCenterXPosition;
    Vector3 defaultCenterYPosition;

    // Start is called before the first frame update
    void Start()
    {
        myCameraTransform = myCamera.transform;
        defaultCameraPosition = myCameraTransform.position;
        defaultCenterXPosition = centerXTransform.position;
        defaultCenterYPosition = centerYTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Rotate Camera
        if (Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire2"))
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetButton("Fire2"))
        {
            float deltaX = (lastMousePosition.x - Input.mousePosition.x) * rotateSpeed;
            float deltaY = (lastMousePosition.y - Input.mousePosition.y ) * rotateSpeed;
            //left - right rotation
            centerXTransform.Rotate(Vector3.up, deltaX);
            //up - down roatation
            centerYTransform.Rotate(Vector3.left, deltaY);
            lastMousePosition = Input.mousePosition;
        }
        //************************************* //

        //Zoom Camera
        if (Input.GetAxis("Mouse ScrollWheel") != 0){
            float zoomDelta = Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomSpeed;
            myCameraTransform.Translate(0,0,zoomDelta);
        }

        if (Input.GetButton("Fire3"))
        {
            //Debug.Log("Fire3");
            //Debug.Log(Input.GetAxis("Mouse Y"));
            float zoomDelta = Input.GetAxis("Mouse Y") * zoomSpeed * Time.deltaTime;
            myCameraTransform.Translate(0, 0, zoomDelta);
        }
        //************************************* //

        //Move Camera Up/Down
        if (Input.GetButton("Vertical"))
        {
            float translationVertical = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
            //Debug.Log(translationVertical);
            centerXTransform.Translate(0, translationVertical, 0);
        }
        //************************************* //
        //Move Camera Horizontal
        if (Input.GetButton("Horizontal"))
        {
            float translationHorizontal = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
            //Debug.Log(translationHorizontal);
            centerXTransform.Translate(translationHorizontal,0, 0);
        }

    }

    public void ResetCamera()
    {
       // Debug.Log("reset camera");

        Quaternion zeroRotation = new Quaternion(0, 0, 0, 1);

        centerXTransform.rotation = zeroRotation;
        centerYTransform.rotation = zeroRotation;
        myCameraTransform.rotation = zeroRotation;

       
        centerXTransform.position = defaultCenterXPosition;
        centerYTransform.position = defaultCenterYPosition;
        myCameraTransform.position = defaultCameraPosition;

    }
}

