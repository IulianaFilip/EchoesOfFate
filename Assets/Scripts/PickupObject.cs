using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


// --- PickupObject.cs ---
// Allows the player to pick up and drop GameObjects with the "Pickup" tag.
// Requires a Rigidbody on the picked-up object.
public class PickupObject : MonoBehaviour
{
    public float pickupDistance = 3f; // Max distance to pick up an object
    public Transform holdPosition; // An empty GameObject child of the camera/player where the object will be held

    private GameObject heldObject; // The currently held object
    private Rigidbody heldObjectRigidbody; // Rigidbody of the held object

    // Update is called once per frame.
    void Update()
    {
        // Check for 'E' key press
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null)
            {
                TryPickup();
            }
            else
            {
                DropObject();
            }
        }

        // Keep the held object in place relative to the player's view
        if (heldObject != null)
        {
            heldObject.transform.position = holdPosition.position;
            heldObject.transform.rotation = holdPosition.rotation; // Optionally align rotation
        }
    }

    // Attempts to pick up an object in front of the player.
    void TryPickup()
    {
        // Cast a ray from the center of the camera forward
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupDistance))
        {
            // Check if the hit object has the "Pickup" tag
            if (hit.collider.CompareTag("Pickup"))
            {
                heldObject = hit.collider.gameObject;
                heldObjectRigidbody = heldObject.GetComponent<Rigidbody>();

                if (heldObjectRigidbody != null)
                {
                    // Disable physics simulation for the held object
                    heldObjectRigidbody.isKinematic = true;
                    // Prevent rotation from outside forces (e.g., collisions)
                    heldObjectRigidbody.useGravity = false;
                    // Parent the object to the hold position
                    heldObject.transform.SetParent(holdPosition);
                    // Reset local position and rotation relative to the hold position
                    heldObject.transform.localPosition = Vector3.zero;
                    heldObject.transform.localRotation = Quaternion.identity; // No local rotation
                    Debug.Log($"Picked up: {heldObject.name}");
                }
                else
                {
                    Debug.LogWarning($"Object '{heldObject.name}' tagged 'Pickup' but has no Rigidbody!", heldObject);
                    heldObject = null; // Don't hold if no Rigidbody
                }
            }
        }
    }

    // Drops the currently held object.
    void DropObject()
    {
        if (heldObject != null && heldObjectRigidbody != null)
        {
            // Re-enable physics simulation
            heldObjectRigidbody.isKinematic = false;
            heldObjectRigidbody.useGravity = true;
            // Unparent the object
            heldObject.transform.SetParent(null);

            // Apply a small forward force to "throw" the object if desired
            // heldObjectRigidbody.AddForce(Camera.main.transform.forward * 5f, ForceMode.Impulse);

            Debug.Log($"Dropped: {heldObject.name}");
            heldObject = null;
            heldObjectRigidbody = null;
        }
    }
}
