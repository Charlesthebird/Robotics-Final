using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public float speed = 2.0f;
    public float gravity = -.001f;
    SceneController sc;

    public LayerMask groundLayer;
    public LayerMask targetLayer;
    public LayerMask obstacleLayer;

    Vector3 extents;
    bool grounded = false;

	// Use this for initialization
	void Start () {
        sc = GameObject.Find("SceneController").GetComponent<SceneController>();
        extents = GetComponent<BoxCollider>().bounds.extents; // assumes length and width are same
	}

    // Update is called once per frame
    void Update() {
        UpdateShadow();
        GroundCheck();
        ObstaclesCheck();
        TargetCheck();
        MoveDownUpdate();
    }
    public GameObject shadowObj;
    void UpdateShadow()
    {
        if (grounded) return;
        // boxcast to see if colliding with anything
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit,
            500, targetLayer))
        {
            shadowObj.transform.position = hit.point;
            shadowObj.transform.eulerAngles = new Vector3(-90, transform.eulerAngles.y, 90 + transform.eulerAngles.z);
        }
        else if (Physics.Raycast(transform.position, -transform.up, out hit,
            500, groundLayer))
        {
            shadowObj.transform.position = hit.point;
            shadowObj.transform.eulerAngles = new Vector3(-90, transform.eulerAngles.y, 90 + transform.eulerAngles.z);
        }
    }
    void ObstaclesCheck()
    {
        if (grounded) return;
        // boxcast to see if colliding with anything
        RaycastHit hit;
        if (Physics.BoxCast(transform.position, extents, -transform.up, out hit, transform.rotation,
            Mathf.Abs(velocityY) * Time.deltaTime, obstacleLayer))
        {
            Debug.Log("HIT! " + hit.point);
        }
    }
    void GroundCheck()
    {
        if (grounded) return;
        // boxcast to see if colliding with anything
        RaycastHit hit;
        if (Physics.BoxCast(transform.position, extents, -transform.up, out hit, transform.rotation, 
            Mathf.Abs(velocityY) * Time.deltaTime, groundLayer))
        {
            Debug.Log("did not hit target...");
            grounded = true;

            sc.loseObj.SetActive(true);
        }
    }
    void TargetCheck()
    {
        if (grounded) return;
        // boxcast to see if colliding with anything
        RaycastHit hit;
        if (Physics.BoxCast(transform.position, extents, -transform.up, out hit, transform.rotation, 
            Mathf.Abs(velocityY) * Time.deltaTime, targetLayer))
        {
            var targetPos = new Vector2(hit.transform.position.x, hit.transform.position.z);
            var myPos = new Vector2(transform.position.x, transform.position.z);
            float dist = Vector2.Distance(myPos, targetPos);
            float totalPossibleDist = GetWidthQuick(hit.transform.GetComponent<MeshCollider>().sharedMesh.vertices);
            Debug.Log("HIT TARGET! " + hit.point);
            Debug.Log("\thit dist: " + dist);
            Debug.Log("\ttarget width: " + totalPossibleDist);
            Debug.Log("\taccuracy: " + ((dist / totalPossibleDist) * 100.0f));
            grounded = true;

            // make sure that the player is then fixed to the target 
            transform.SetParent(hit.transform);

            sc.winObj.SetActive(true);
            sc.winObj.transform.Find("TextInfo").GetComponent<Text>().text = "Accuracy: " + (100 - ((dist / totalPossibleDist) * 100.0f) + "%");
        }
    }
    /// <summary>
    /// gets the furthest vertex from the first vertex in verts that is at the same y position (works for nonrotated target obj only) in O(n) time
    /// </summary>
    float GetWidthQuick(Vector3[] verts)
    {
        var maxDist = 0f;
        var firstXZ = new Vector2(verts[0].x, verts[0].z);
        for(int i=1; i<verts.Length; i++)
        {
            // only compare if at the same level
            if(verts[0].y == verts[i].y)
            {
                var curXZ = new Vector2(verts[i].x, verts[i].z);
                var dist = Vector2.Distance(firstXZ, curXZ);
                if (dist > maxDist) maxDist = dist;
            }
        }
        return maxDist * transform.localScale.x;
    }

    public bool moveXPos = false;
    public bool moveXNeg = false;
    public bool moveZPos = false;
    public bool moveZNeg = false;
    public float velocityY = 0.0f;
    void MoveDownUpdate()
    {
        if (sc.upPressed) Debug.Log("PRESSING UP");
        if (grounded) return;
        var movement = Vector3.zero;
        if (moveXPos || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || sc.rightPressed) movement.x += speed;
        if (moveXNeg || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || sc.leftPressed) movement.x -= speed;
        if (moveZPos || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || sc.upPressed) movement.z += speed;
        if (moveZNeg || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || sc.downPressed) movement.z -= speed;
        velocityY = velocityY + (gravity * Time.deltaTime);
        movement.y = velocityY;
        movement *= Time.deltaTime;
        transform.localPosition = transform.localPosition + movement;
    }
}
