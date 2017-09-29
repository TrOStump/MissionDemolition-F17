using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {
    static public FollowCam S; // a FollowCam Singleton

    //fields set in the Unity Inspector pane
    public float easing = 0.05f;
    public Vector2 minXY;
    public bool __________________________;

    //fields set Dynamically
    public GameObject poi; // point of interest
    public float camZ; // the desired Z pos of the camera

    void Awake()
    {
        S = this;
        camZ = this.transform.position.z;
        // limit the x and y to minumum values
    }
    void FixedUpdate()
    {
        Vector3 destination;
        if (poi == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            //get the position poi
            destination = poi.transform.position;
            //if poi is a projectile,  check to see if it's at rest
            if (poi.tag == "Projectile")
            {
                //is it sleeping (not moving)
                if (poi.GetComponent<Rigidbody>().IsSleeping())
                {
                    //return to default view
                    poi = null;
                    // in the next update
                    return;
                }


            }
        }
        
        // limit the x and y to minumum values
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        //Interpolate from the current Camera position towards destination
        destination = Vector3.Lerp(transform.position, destination, easing);
        //retain a destination.z of camZ
        destination.z = camZ;
        //set the camera to the destination
        transform.position = destination;
        //set the orthographicsize of the camera t keep the ground in view
        this.GetComponent<Camera>().orthographicSize = destination.y + 10;
    }


}
