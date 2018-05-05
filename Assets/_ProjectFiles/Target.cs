using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {
    public GameObject shadowObj;
    public LayerMask groundLayer;

    private void Update()
    {
        UpdateShadow();
    }
    void UpdateShadow()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit,
            500, groundLayer))
        {
            shadowObj.transform.position = hit.point;
            shadowObj.transform.eulerAngles = new Vector3(-90, transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }
}
