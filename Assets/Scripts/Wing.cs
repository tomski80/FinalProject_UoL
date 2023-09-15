using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wing : MonoBehaviour
{
    Rigidbody rb;
   // public  Rigidbody airframe;
    public AnimationCurve aoACurve;
    public AnimationCurve inducedDragCurve;
    public float liftPower;                //approximation of area 
    public float maxLift;

    public float velocityScale = 10.0f;         //need to scale velocity as we not really have realistic scale or speeds

    bool WingBroke = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //respect if body is kinematic, so we don't add this force 
        if(!rb.isKinematic)
        {
            Vector2 AoAoY = CalculateAngleOfAttack();
            Vector3 liftForce = CalculateLift(AoAoY.x);
            //Vector3 yawForce = CalculateLift(AoAoY.y);

            rb.AddForce(liftForce);
        }
        
    }


    public void OnJointBreak(float breakForce)
    {
        Debug.Log("Wing Broke off?");
        WingBroke=true; 
    }

    Vector2 CalculateAngleOfAttack()
    {
        //Lift also depends on Angle of Attack (AOA), which is the angle between the direction the plane’s velocity is pointing, and the direction the plane’s nose is pointing.
        Vector3 CraftVelocity = rb.velocity * velocityScale;

        Vector3 localVelocity = rb.transform.InverseTransformDirection(CraftVelocity);
        
        Vector3 localAngularVelocity = rb.transform.InverseTransformDirection(rb.angularVelocity);

        //AngleOfAttack is the AOA measured on the pitch axis.
        //AngleOfAttackYaw is the AOA measured on the yaw axis.
        //float angleOfAttack = Mathf.Atan2( -localVelocity.y, localVelocity.z);
        float angleOfAttackYaw = Mathf.Atan2(localVelocity.x, localVelocity.z);

        
        // Calculate the angle of attack of the wings
        Vector3 velocity = rb.velocity * velocityScale;
        Debug.DrawLine((velocity * 0.1f) + transform.position, transform.position, Color.green);
        Vector3 relativeVelocity = (velocity - transform.forward) * Vector3.Dot(velocity, transform.forward);
        Vector3 chordLine = transform.forward;
        Debug.DrawLine(chordLine + transform.position, transform.position, Color.red);
        Debug.DrawLine(relativeVelocity + transform.position, transform.position, Color.green);
        float angleOfAttack = Vector3.SignedAngle(relativeVelocity, chordLine, transform.right);
       
        

        //Debug.Log(angleOfAttack);
        return new Vector2(angleOfAttack, angleOfAttackYaw);
    }

    void CalculateDrag()
    {
        // Drag
        // Drag is a bit more complicated. This is the basic drag formula:
        // (https://www1.grc.nasa.gov/beginners-guide-to-aeronautics/drag-equation/)

        // D =\frac12\times\rho\times v^2\times A\times C_d
        // D is the resulting drag force
        // ρ(rho) is the air density
        // v is the velocity
        // A is the surface area
        // Cd is the coefficient of drag
    }


    Vector3 CalculateLift(float angleOfAttack)
    {
        // Lift and Induced Drag
        // This is the lift equation:

        // L = A/2 * rho * C_L * v^2
        // L is the resulting lift force
        // ρ(rho) is the air density
        // v is the velocity
        // A is the surface area
        // CL is the coefficient of lift
        //Debug.Log("Angle of Attack Degrees = "+ (180 - angleOfAttack * Mathf.Rad2Deg));
        // simplified lift = V^2 * LiftCoeefficient * liftPower;
        Vector3 CraftVelocity = rb.velocity * velocityScale;

        Vector3 liftVelocity =  Vector3.ProjectOnPlane(rb.transform.InverseTransformDirection(CraftVelocity),transform.up);
        //Debug.DrawLine(liftVelocity + transform.position, transform.position);
        float liftVelocity2 = liftVelocity.sqrMagnitude;
        float liftCoefficient = aoACurve.Evaluate(angleOfAttack);
        float liftForce = liftVelocity2 * liftCoefficient * liftPower;
       // Debug.Log("LiftForce = " + liftForce);
        // lift direction is penperdicular to velocity 
        //Vector3 liftDirection = Vector3.Cross(liftVelocity.normalized, -Vector3.right);
        // lift is always up 
        
        //Debug.Log("Lift Direction" + liftDirection);
        Vector3 lift = Vector3.up * liftForce;

        // need to limit lift, it is all approximation 
        if(lift.magnitude > maxLift)
        {
            //lift = lift.normalized * maxLift;
        }

        // induced drag
        float dragForce = liftCoefficient * liftCoefficient;
        Vector3 dragDirection = -liftVelocity.normalized;
        Vector3 inducedDrag = dragDirection * liftVelocity2 * dragForce;
        inducedDrag = inducedDrag * inducedDragCurve.Evaluate(Mathf.Max(0, liftVelocity.z));
        //Debug.Log("Induced Draf" + inducedDrag);
        Debug.DrawLine((inducedDrag * 0.001f) + transform.position, transform.position, Color.magenta);
        Debug.DrawLine((lift) + transform.position, transform.position, Color.blue);

        if(WingBroke)
        {
            return Vector3.zero;
        }
        return lift;// + inducedDrag;
    }
}
