using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;



// --- RotatingTile.cs ---
// Controls the rotation of a single puzzle tile and checks if it's in the correct orientation.
// It reports its state changes back to the PuzzleManager via an event.
public class RotatingTile : MonoBehaviour
{
    public float correctAngle = 90f; // The Y-axis rotation angle that makes this tile 'correct'
    public float rotationAmount = 90f; // How much the tile rotates per click
    public bool isCorrect = false; // Public read-only, controlled internally

    // Event for when a tile is rotated. PuzzleManager subscribes to this.
    public static event Action OnTileRotated;

    // Visual feedback variables
    public Material correctMaterial;
    public Material incorrectMaterial;
    private Renderer tileRenderer;

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        tileRenderer = GetComponent<Renderer>();
        if (tileRenderer == null)
        {
            Debug.LogError("RotatingTile: No Renderer found on this GameObject.", this);
        }
        UpdateTileState(); // Initialize the visual state
    }

    // OnMouseDown is called when the user has pressed the mouse button while
    // over the Collider. Requires a Collider component on the GameObject.
    void OnMouseDown()
    {
        // Rotate the tile around its local Y-axis
        transform.Rotate(0, rotationAmount, 0, Space.Self);

        // Check the current rotation and update 'isCorrect'
        // Using Mathf.Round to handle floating point inaccuracies for angle comparisons.
        float currentYRotation = Mathf.Round(transform.localEulerAngles.y) % 360;
        isCorrect = Mathf.Abs(currentYRotation - correctAngle) < 1f ||
                    Mathf.Abs(currentYRotation - (correctAngle + 360)) < 1f ||
                    Mathf.Abs(currentYRotation - (correctAngle - 360)) < 1f;

        UpdateTileState(); // Update visual feedback
        OnTileRotated?.Invoke(); // Notify subscribers (e.g., PuzzleManager)
    }

    // Updates the material of the tile based on its 'isCorrect' state.
    private void UpdateTileState()
    {
        if (tileRenderer != null)
        {
            if (isCorrect && correctMaterial != null)
            {
                tileRenderer.material = correctMaterial;
            }
            else if (!isCorrect && incorrectMaterial != null)
            {
                tileRenderer.material = incorrectMaterial;
            }
          
        }
    }

  
    public void ResetRotation()
    {
        transform.localRotation = Quaternion.identity; // Resets to 0,0,0 rotation
        isCorrect = false;
        UpdateTileState();
    }
}
