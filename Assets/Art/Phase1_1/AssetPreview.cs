using UnityEngine;

public class AssetPreview : MonoBehaviour
{
    [Header("Asset Preview Info")]
    public string assetName;
    public string assetType;
    public Vector3 scale;
    public int estimatedPolyCount;
    public string moodDescription;

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 200));
        GUILayout.Label($"Asset: {assetName}");
        GUILayout.Label($"Type: {assetType}");
        GUILayout.Label($"Scale: {scale.x}x{scale.y}x{scale.z}");
        GUILayout.Label($"Poly Count: ~{estimatedPolyCount}");
        GUILayout.Label($"Mood: {moodDescription}");
        GUILayout.EndArea();
    }
}