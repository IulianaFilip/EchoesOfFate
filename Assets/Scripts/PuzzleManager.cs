using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Manages a set of interactive puzzle tiles, checking if they are all in the correct state
// and triggering events upon puzzle completion. It communicates with AI_Manager.
public class PuzzleManager : MonoBehaviour
{
    // Singleton pattern
    public static PuzzleManager Instance { get; private set; }

    public RotatingTile[] tiles; // Assign all RotatingTile GameObjects here in the Inspector
    public int requiredCorrectTiles = 3; // The number of tiles that need to be correct.
                                         // This could be dynamic based on AI_Manager.GetCurrentPuzzleComplexity()

    // Event that other scripts can subscribe to when the puzzle is solved.
    public static event Action OnPuzzleSolved;

    private int currentCorrectTiles = 0; // Tracks how many tiles are currently correct
    private bool isPuzzleSolved = false; // Prevents multiple triggerings

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure only one PuzzleManager
        }
    }

    // OnEnable is called when the object becomes enabled and active.
    void OnEnable()
    {
        // Subscribe to the OnTileRotated event from RotatingTile.
        // This makes the PuzzleManager react whenever a tile is rotated.
        RotatingTile.OnTileRotated += CheckPuzzleSolved;
    }

    // OnDisable is called when the behaviour becomes disabled or inactive.
    void OnDisable()
    {
     
        RotatingTile.OnTileRotated -= CheckPuzzleSolved;
    }


    public void InitializePuzzle()
    {
        isPuzzleSolved = false;
        currentCorrectTiles = 0;

        UnityEngine.Debug.Log($"Puzzle Initialized. Complexity Level: {AI_Manager.Instance.GetCurrentPuzzleComplexity()}");
    }

    public void CheckPuzzleSolved()
    {
        if (isPuzzleSolved) return; // Already solved, no need to re-check

        currentCorrectTiles = 0;
        foreach (var tile in tiles)
        {
            if (tile.isCorrect)
            {
                currentCorrectTiles++;
            }
        }

        // Check if all required tiles are correct
        if (currentCorrectTiles >= requiredCorrectTiles)
        {
            UnityEngine.Debug.Log("Puzzle Solved!");
            isPuzzleSolved = true;
            OnPuzzleSolved?.Invoke(); // Trigger the puzzle solved event
            AI_Manager.Instance?.RecordPuzzleOutcome(true); // Inform AI Manager of success
        }
        else
        {
            UnityEngine.Debug.Log($"Puzzle Progress: {currentCorrectTiles}/{requiredCorrectTiles} correct tiles.");
          
        }
    }

   
    public void ForcePuzzleFailure()
    {
        if (!isPuzzleSolved)
        {
            UnityEngine.Debug.Log("Puzzle Failed!");
            AI_Manager.Instance?.RecordPuzzleOutcome(false); // Inform AI Manager of failure
        }
    }
}
