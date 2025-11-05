using UnityEngine;

/// <summary>
/// Base class for fish behavior
/// This is a basic implementation that will be expanded later
/// </summary>
public class Fish : MonoBehaviour
{
    [Header("Fish Info")]
    [SerializeField] private string fishName = "Basic Fish";
    [SerializeField] private int value = 10;

    /// <summary>
    /// Called when this fish is hit by a spear
    /// </summary>
    public void OnHitBySpear(Spear spear)
    {
        // TODO: Implement fish catching mechanic
        // For now, just log and destroy
        Debug.Log($"{fishName} was hit by spear!");

        // Destroy the fish
        Destroy(gameObject);
    }

    /// <summary>
    /// Get the value of this fish (for selling)
    /// </summary>
    public int GetValue()
    {
        return value;
    }
}
