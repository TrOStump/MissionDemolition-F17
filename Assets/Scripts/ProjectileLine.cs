using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour {
    static public ProjectileLine S;
    //fields set in the unity game inspector
    public float minDist = 0.1f;
    public bool _________________;
    //fields set dynamically
    public LineRenderer line;
    private GameObject _poi;
    public List<Vector3> points;

    void Awake() {
        S = this; //set the singleton
        //get a reference to lineRenderer
        line = GetComponent<LineRenderer>();
        //disable the LineRenderer until its needed
        line.enabled = false;
        //initialize the points list 
        points = new List<Vector3>();
    }

    //this is a property (that is a method masquerading as a field
    public GameObject poi
    {
        get
        {
            return (_poi);
        }
        set
        {
            _poi = value;
            if (_poi != null)
            {
                //when _poi is set to something new, it resets everything
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    //this can be used to clear the line directly
    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    public void AddPoint()
    {
        //this is called to add a point to the line
        Vector3 pt = _poi.transform.position;
        if ( points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            //if the point isnt far enough from the last point, it returns
            return;
        }
        if ( points.Count == 0)
        {
            //if this is the launch point
            Vector3 launchPos = Slingshot.S.launchPoint.transform.position;
            Vector3 launchPosDif = pt - launchPos;
            //it adds an extra bit of line to aid aiming later
            points.Add(pt + launchPosDif);
            points.Add(pt);
            line.positionCount = 2;
            //sets the first 2 points
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            //enables the linerenderer
            line.enabled = true;
        }
        else
        {
            //normal behavior of adding a point
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }
    //Returns the location of the most recently added point
    public Vector3 lastPoint
    {
        get
        {
            if (points == null)
            {
                //if there are no points, return vector3.zero
                return (Vector3.zero);
            }
            return (points[points.Count - 1]);
        }
    }
    void FixedUpdate()
    {
        if (poi == null)
        {
            //there is no poi search for one
            if (FollowCam.S.poi != null)
            {
                if (FollowCam.S.poi.tag == "Projectile")
                {
                    poi = FollowCam.S.poi;
                }
                else
                {
                    return; // return if we didn't find a poi
                }
            }
            else
            {
                return; // return if we didn't find a poi
            }
        }

        AddPoint();
        if (poi.GetComponent<Rigidbody>().IsSleeping())
        {
            //once the poi is sleeping, it is cleared
            poi = null;
        }
        
    }
}
