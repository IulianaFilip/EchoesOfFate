using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PickupObject : MonoBehaviour
{
    public float pickupDistance = 3f;
    public Transform holdPosition;
    private GameObject heldObject;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null)
                TryPickup();
            else
                DropObject();
        }
    }

    void TryPickup()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupDistance))
        {
            if (hit.collider.CompareTag("Pickup"))
            {
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true;
                heldObject.transform.SetParent(holdPosition);
                heldObject.transform.localPosition = Vector3.zero;
            }
        }
    }

    void DropObject()
    {
        if (heldObject != null)
        {
            heldObject.GetComponent<Rigidbody>().isKinematic = false;
            heldObject.transform.SetParent(null);
            heldObject = null;
        }
    }
}

