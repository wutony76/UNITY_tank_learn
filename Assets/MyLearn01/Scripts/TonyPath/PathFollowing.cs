using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PathFollowing : MonoBehaviour {
    public TonyPath path;      //PATH_ND
    public float speed = 5.0f; //following speed
    public float mass = 5.0f;  //this is for object mass for simulating the real car
    public bool isLooping = true;//the car will loop or not


    private float curSpeed;//Actual speed of the car
    private int curPathIndex;
    private float pathLength;
    private Vector3 targetPosition;
    private Vector3 curVelocity;
    bool isRotate = true;


    private float lerpSpeed = 0.08f;
    private float lerpTm = 0.0f;
    private Quaternion rawRotate;
    private Quaternion lookatRotate;
    //private 


    // Start is called before the first frame update
    void Start(){
        if (path) {
            pathLength = path.Length;
            curPathIndex = 0;
            curVelocity = transform.forward;
        }
    }


    // Update is called once per frame
    void Update(){
        //ObjRun();
    }


    public void ObjRun(){
        if (path == null) return;

        //Unify the speed
        curSpeed = speed * Time.deltaTime;
        targetPosition = path.GetPosition(curPathIndex);
        //If reach the radius within the path then move to next point in the path
        if (Vector3.Distance(transform.position, targetPosition) < path.Radius){
            //Don't move the vehicle if path is finished
            if (curPathIndex < pathLength - 1)
                curPathIndex++;

            else if (isLooping)
                curPathIndex = 0;
                
            else
                return;
        }

        //Calculate the acceleration towards the path
        curVelocity += Accelerate(targetPosition);

        //Move the car according to the velocity
        //transform.position += curVelocity;
        this.gameObject.transform.Translate(Vector3.forward * Time.deltaTime * speed);


        // 轉彎 Rotate the car towards the desired Velocity 
        Vector3 vec = targetPosition - this.gameObject.transform.position;
        Quaternion rotate = Quaternion.LookRotation(vec);
        this.gameObject.transform.localRotation = Quaternion.Slerp(this.gameObject.transform.localRotation, rotate, lerpSpeed);

        //transform.rotation = Quaternion.LookRotation(curVelocity);
        //print( "target =" + Quaternion.LookRotation(curVelocity)  +". obj = "+Tool.getRotationValue(this.gameObject.transform)  );
    }


    public void TargetRun( GameObject target ){
        print("TargetRun =" + target);
        curSpeed = speed * Time.deltaTime;
        targetPosition = target.transform.position; //path.GetPosition(curPathIndex);
        curVelocity += Accelerate(targetPosition);

        //Move the car according to the velocity
        //transform.position += curVelocity;
        this.gameObject.transform.Translate(Vector3.forward * Time.deltaTime * speed);


        // 轉彎 Rotate the car towards the desired Velocity 
        Vector3 vec = targetPosition - this.gameObject.transform.position;
        Quaternion rotate = Quaternion.LookRotation(vec);
        this.gameObject.transform.localRotation = Quaternion.Slerp(this.gameObject.transform.localRotation, rotate, lerpSpeed);
    }







    //Steering algorithm to steer the vector towards the target
    public Vector3 Accelerate(Vector3 target){
        //Calculate the directional vector from the current position towards the target point
        Vector3 desiredVelocity = target - transform.position;

        //Normalise the desired Velocity
        desiredVelocity.Normalize();
        desiredVelocity *= curSpeed;

        //Calculate the force Vector
        Vector3 steeringForce = desiredVelocity - curVelocity;
        Vector3 acceleration = steeringForce / mass;

        //return acceleration;
        return acceleration;
    }


    /*
    void initRotate() {
        rawRotate = this.gameObject.transform.rotation;
        this.gameObject.transform.LookAt(targetPosition);
        lookatRotate = this.gameObject.transform.rotation;
        this.gameObject.transform.rotation = rawRotate;

        float rotateAngle = Quaternion.Angle(rawRotate, lookatRotate);
        lerpSpeed =  1080.0f / rotateAngle;
        lerpTm = 0.0f;

        print("rotateAngle =" + rotateAngle);
    }

    void objRotate() {
        lerpSpeed += Time.deltaTime * lerpSpeed;
        this.gameObject.transform.rotation = Quaternion.Lerp(rawRotate, lookatRotate, lerpTm);
        if (lerpTm >= 1) this.gameObject.transform.rotation = lookatRotate;
    }
    */
}

