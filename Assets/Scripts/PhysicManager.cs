using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicManager : MonoBehaviour
{

    public GameObject myObject;
    public float m_Force = - 20000.0f;
    Rigidbody m_Rigidbody;
    bool applyForce = false;
    float m_period = 0;

    // Start is called before the first frame update
    void Start()
    {
       m_Rigidbody = myObject.GetComponent<Rigidbody>();
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 m_Direction = Vector3.forward;
        
        if (Input.GetButtonDown("Fire1"))
        {
           applyForce = true;
           
        }
        if(applyForce & m_period < 4.0f)    // 2.0 sec
        {
            Debug.Log("Apply Force!");
            m_Rigidbody.AddForce(m_Direction * m_Force);
            m_period += Time.deltaTime;
        }

    }
}
