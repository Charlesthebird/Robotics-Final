using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveControl : MonoBehaviour
{
    public bool moveXPos = false;
    public bool moveXNeg = false;
    public bool moveZPos = false;
    public bool moveZNeg = false;

    bool mouseDown = false;
    Player player;
    // Use this for initialization
    void Awake() {
        player = GameObject.Find("Player").GetComponent<Player>();
        oldColor = this.GetComponent<MeshRenderer>().sharedMaterial.color;
    }


    private void OnMouseDown()
    {
        if (moveXPos) player.moveXPos = true;
        if (moveXNeg) player.moveXNeg = true;
        if (moveZPos) player.moveZPos = true;
        if (moveZNeg) player.moveZNeg = true;
    }
    private void OnMouseUp()
    {
        if (moveXPos) player.moveXPos = false;
        if (moveXNeg) player.moveXNeg = false;
        if (moveZPos) player.moveZPos = false;
        if (moveZNeg) player.moveZNeg = false;
    }

    public LayerMask controlLayer;
    Color oldColor;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, controlLayer))
            {
                if(hit.transform.gameObject == this.gameObject)
                {
                    oldColor = this.GetComponent<MeshRenderer>().sharedMaterial.color;
                    this.GetComponent<MeshRenderer>().sharedMaterial.color = Color.white;
                    OnMouseDown();
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            this.GetComponent<MeshRenderer>().sharedMaterial.color = oldColor;
            OnMouseUp();
        }
    }
}
