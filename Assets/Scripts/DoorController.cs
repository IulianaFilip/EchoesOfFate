using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;



// --- DoorController.cs ---
// Simple script to control a door's state (open/close) in response to a puzzle being solved.
// It subscribes to the PuzzleManager's OnPuzzleSolved event.
public class DoorController : MonoBehaviour
{
    public GameObject doorGameObject; // Assign the actual door GameObject in the Inspector
                                      // This object will be disabled/enabled to simulate opening/closing.

    // OnEnable is called when the object becomes enabled and active.
    void OnEnable()
    {
        // Subscribe to the puzzle solved event.
        PuzzleManager.OnPuzzleSolved += OpenDoor;
    }

    // OnDisable is called when the behaviour becomes disabled or inactive.
    void OnDisable()
    {
        // Unsubscribe from the event to prevent memory leaks.
        PuzzleManager.OnPuzzleSolved -= OpenDoor;
    }

    // Method to open the door. Called when OnPuzzleSolved event is triggered.
    private void OpenDoor()
    {
        if (doorGameObject != null)
        {
            doorGameObject.SetActive(false); // Make the door disappear (or play an animation)
            UnityEngine.Debug.Log("Door Controller: Door opened in response to puzzle solved.");
        }
        else
        {
            UnityEngine.Debug.LogWarning("Door Controller: No doorGameObject assigned to open!", this);
        }
    }

    public void CloseDoor()
    {
        if (doorGameObject != null)
        {
            doorGameObject.SetActive(true); // Make the door reappear
            UnityEngine.Debug.Log("Door Controller: Door closed.");
        }
    }
}
