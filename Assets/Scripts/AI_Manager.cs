
using UnityEngine;
using System.Collections.Generic; // Required for List
using System; // Required for Action events


// This script acts as a central hub for managing game difficulty and AI adaptation.
// It tracks player performance in puzzles and can be extended to influence NPC behavior.
// It uses Unity's PlayerPrefs for simple data persistence, allowing game state to influence
// adaptive difficulty across sessions.
public class AI_Manager : MonoBehaviour
{
    // Singleton pattern to ensure only one instance of AI_Manager exists.
    public static AI_Manager Instance { get; private set; }

    // Constants for PlayerPrefs keys
    private const string PUZZLE_SUCCESS_KEY = "PuzzleSuccessCount";
    private const string PUZZLE_FAILURE_KEY = "PuzzleFailureCount";
    private const string CURRENT_PUZZLE_COMPLEXITY_KEY = "CurrentPuzzleComplexity";
    private const string NPC_CHALLENGE_KEY = "NPCChallengeLevel";

    // Internal counters for tracking performance
    private int successfulPuzzles;
    private int failedPuzzles;
    private int currentPuzzleComplexity; // Example: influences tile count or rotation speed
    private int currentNPCChallengeLevel; // Example: influences NPC speed, detection range, etc.

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        // Implement singleton pattern:
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scene loads
            LoadGameData(); // Load data when the manager is initialized
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    // Loads game-related data from PlayerPrefs.
    private void LoadGameData()
    {
        successfulPuzzles = PlayerPrefs.GetInt(PUZZLE_SUCCESS_KEY, 0);
        failedPuzzles = PlayerPrefs.GetInt(PUZZLE_FAILURE_KEY, 0);
        currentPuzzleComplexity = PlayerPrefs.GetInt(CURRENT_PUZZLE_COMPLEXITY_KEY, 3); // Default complexity
        currentNPCChallengeLevel = PlayerPrefs.GetInt(NPC_CHALLENGE_KEY, 1); // Default NPC challenge
        UnityEngine.Debug.Log($"AI_Manager Loaded: Success={successfulPuzzles}, Failed={failedPuzzles}, Complexity={currentPuzzleComplexity}, NPC Challenge={currentNPCChallengeLevel}");
    }

    // Saves game-related data to PlayerPrefs.
    private void SaveGameData()
    {
        PlayerPrefs.SetInt(PUZZLE_SUCCESS_KEY, successfulPuzzles);
        PlayerPrefs.SetInt(PUZZLE_FAILURE_KEY, failedPuzzles);
        PlayerPrefs.SetInt(CURRENT_PUZZLE_COMPLEXITY_KEY, currentPuzzleComplexity);
        PlayerPrefs.SetInt(NPC_CHALLENGE_KEY, currentNPCChallengeLevel);
        PlayerPrefs.Save(); // Ensure data is written to disk
        UnityEngine.Debug.Log("AI_Manager Data Saved.");
    }

    // Records the outcome of a puzzle attempt.
    public void RecordPuzzleOutcome(bool success)
    {
        if (success)
        {
            successfulPuzzles++;
            UnityEngine.Debug.Log($"Puzzle Solved! Total Successes: {successfulPuzzles}");
        }
        else
        {
            failedPuzzles++;
            UnityEngine.Debug.Log($"Puzzle Failed! Total Failures: {failedPuzzles}");
        }
        SaveGameData(); // Save data after recording
        AdjustPuzzleDifficulty(); // Adjust difficulty based on new data
    }

    // Adjusts the puzzle difficulty based on player performance.
    // This is a simple rule-based adaptation. For more complex systems,
    // this would interact with ML-Agents or more sophisticated AI.
    private void AdjustPuzzleDifficulty()
    {
        // Example logic: if player is doing too well, increase complexity.
        // If player is struggling, decrease complexity.
        int netPerformance = successfulPuzzles - failedPuzzles;

        if (netPerformance > 3 && currentPuzzleComplexity < 5) // If significantly more successes
        {
            currentPuzzleComplexity++;
            UnityEngine.Debug.Log($"Increasing puzzle complexity to: {currentPuzzleComplexity}");
            // Reset counters to make future adjustments more sensitive
            successfulPuzzles = 0;
            failedPuzzles = 0;
        }
        else if (netPerformance < -2 && currentPuzzleComplexity > 1) // If significantly more failures
        {
            currentPuzzleComplexity--;
            UnityEngine.Debug.Log($"Decreasing puzzle complexity to: {currentPuzzleComplexity}");
            // Reset counters
            successfulPuzzles = 0;
            failedPuzzles = 0;
        }
        SaveGameData(); // Save the adjusted complexity
        // You would then use currentPuzzleComplexity to affect actual puzzle generation
        // (e.g., number of tiles, speed of rotation, number of required correct positions).
    }

    // Public getter for current puzzle complexity. Puzzles can query this.
    public int GetCurrentPuzzleComplexity()
    {
        return currentPuzzleComplexity;
    }

    // Example method to adjust NPC challenge. This would be called based on
    // combat performance, stealth success, or specific game events.
    public void AdjustNPCChallenge(int adjustment)
    {
        currentNPCChallengeLevel = Mathf.Clamp(currentNPCChallengeLevel + adjustment, 1, 10); // Clamp between 1 and 10
        SaveGameData();
        UnityEngine.Debug.Log($"NPC Challenge adjusted to: {currentNPCChallengeLevel}");
        // NPCs would then query this level to adjust their patrol speed, attack strength, etc.
    }

    // Public getter for current NPC challenge level. NPCs can query this.
    public int GetCurrentNPCChallengeLevel()
    {
        return currentNPCChallengeLevel;
    }

    // Clear all stored PlayerPrefs data for debugging or new game starts.
    public void ClearAllAIData()
    {
        PlayerPrefs.DeleteKey(PUZZLE_SUCCESS_KEY);
        PlayerPrefs.DeleteKey(PUZZLE_FAILURE_KEY);
        PlayerPrefs.DeleteKey(CURRENT_PUZZLE_COMPLEXITY_KEY);
        PlayerPrefs.DeleteKey(NPC_CHALLENGE_KEY);
        PlayerPrefs.Save();
        LoadGameData(); // Reload defaults
        UnityEngine.Debug.Log("AI_Manager data cleared.");
    }
}

