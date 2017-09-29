using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour {
    //fields set in the unity inspector pane
    public GameObject prefabProjectile;
    public float velocityMult = 4f;
    public bool _______________________;
    //fields set dynamically
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;
    void Awake()
    {
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }
    void Update()
    {
        //if Slingshot is not in aiming mode, dont't run this code
        if (!aimingMode) return;
        //get current mouse position in 2d screen coordinates
        Vector3 mousePos2D = Input.mousePosition;
        //convert the mouse position to 3d world coordinates
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3d = Camera.main.ScreenToWorldPoint(mousePos2D);
        //find the delta from the launchPos to the mousePos3d
        Vector3 mouseDelta = mousePos3d - launchPos;
        //limit mouse delta to the radius of the slingshot spherecollider
        float maxmagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxmagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxmagnitude;
        }
        //move the object to this new position 
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        if (Input.GetMouseButtonUp(0))
        {
            //the mouse has been released
            aimingMode = false;
            projectile.GetComponent<Rigidbody>().isKinematic = false;
            projectile.GetComponent<Rigidbody>().velocity = -mouseDelta * velocityMult;
            FollowCam.S.poi = projectile;
            projectile = null;
        }
        
    }
    void OnMouseEnter ()
    {
        //print("Slingshot:OnMouseEnter() ");
        launchPoint.SetActive(true);
	}
	
	void OnMouseExit ()
    {
        //print("Slingshot:OnMouseExit() ");
        launchPoint.SetActive(false);
	}
    void OnMouseDown()
    {
        //the player has pressed the mouse button down while over the slingshot 
        aimingMode = true;
        //instantiate a projectile
        projectile = Instantiate(prefabProjectile) as GameObject;
        //start it at launch point
        projectile.transform.position = launchPos;
        //set it to isKinematic for now
        projectile.GetComponent<Rigidbody>().isKinematic = true; 
    }
}
