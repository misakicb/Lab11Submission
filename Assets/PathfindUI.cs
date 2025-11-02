using UnityEngine;
using TMPro; // Required for TMP
using UnityEngine.UI;

public class PathfindingUI : MonoBehaviour
{
    public APathfinding pathfinding;

    public TMP_InputField startXInput;
    public TMP_InputField startYInput;
    public TMP_InputField goalXInput;
    public TMP_InputField goalYInput;
    public TMP_InputField widthInput;
    public TMP_InputField heightInput;
    public TMP_InputField obstacleProbabilityInput;

    // Set Start
    public void OnSetStart()
    {
        if (int.TryParse(startXInput.text, out int x) && int.TryParse(startYInput.text, out int y))
        {
            pathfinding.start = new Vector2Int(x, y);
            Debug.Log("Start set to: " + pathfinding.start);
        }
        else Debug.LogError("Invalid Start Input!");
    }

    // Set Goal
    public void OnSetGoal()
    {
        if (int.TryParse(goalXInput.text, out int x) && int.TryParse(goalYInput.text, out int y))
        {
            pathfinding.goal = new Vector2Int(x, y);
            Debug.Log("Goal set to: " + pathfinding.goal);
        }
        else Debug.LogError("Invalid Goal Input!");
    }

    // Generate Random Grid and Find Path
    public void OnGenerateRandomGrid()
    {
        if (int.TryParse(widthInput.text, out int w) &&
            int.TryParse(heightInput.text, out int h) &&
            float.TryParse(obstacleProbabilityInput.text, out float prob))
        {
            pathfinding.GenerateRandomGrid(w, h, prob);
            pathfinding.FindPath(pathfinding.start, pathfinding.goal);
        }
        else Debug.LogError("Invalid Grid Input!");
    }
}