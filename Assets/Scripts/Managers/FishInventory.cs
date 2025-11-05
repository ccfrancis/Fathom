using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Manages player's caught fish inventory
/// Singleton pattern for easy global access
/// </summary>
public class FishInventory : MonoBehaviour
{
    public static FishInventory Instance { get; private set; }

    [Header("Inventory")]
    [SerializeField] private List<CaughtFish> caughtFish = new List<CaughtFish>();
    [SerializeField] private int totalFishCaught = 0;
    [SerializeField] private int totalValue = 0;

    // Events
    public event Action<CaughtFish> OnFishAdded;
    public event Action OnInventoryCleared;

    // Properties
    public int FishCount => caughtFish.Count;
    public int TotalValue => totalValue;
    public List<CaughtFish> AllFish => new List<CaughtFish>(caughtFish); // Return copy

    void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Subscribe to fish caught events
        // Find all fish in scene and subscribe (for existing fish)
        SubscribeToExistingFish();
    }

    void SubscribeToExistingFish()
    {
        Fish[] allFish = FindObjectsOfType<Fish>();
        foreach (Fish fish in allFish)
        {
            fish.OnCaught += HandleFishCaught;
        }
    }

    /// <summary>
    /// Called when a fish is caught
    /// </summary>
    public void HandleFishCaught(Fish fish)
    {
        CaughtFish caughtFishData = new CaughtFish
        {
            name = fish.FishName,
            value = fish.Value,
            timeCaught = Time.time
        };

        caughtFish.Add(caughtFishData);
        totalFishCaught++;
        totalValue += fish.Value;

        OnFishAdded?.Invoke(caughtFishData);

        Debug.Log($"Added {fish.FishName} to inventory. Total fish: {caughtFish.Count}, Total value: {totalValue}");
    }

    /// <summary>
    /// Sell all fish in inventory
    /// </summary>
    public int SellAllFish()
    {
        int soldValue = totalValue;

        if (soldValue > 0)
        {
            // Add money to currency
            if (CurrencyManager.Instance != null)
            {
                CurrencyManager.Instance.AddCurrency(soldValue);
            }

            // Clear inventory
            caughtFish.Clear();
            totalValue = 0;

            OnInventoryCleared?.Invoke();

            Debug.Log($"Sold all fish for {soldValue} coins!");
        }

        return soldValue;
    }

    /// <summary>
    /// Get count of specific fish type
    /// </summary>
    public int GetFishCount(string fishName)
    {
        int count = 0;
        foreach (var fish in caughtFish)
        {
            if (fish.name == fishName)
            {
                count++;
            }
        }
        return count;
    }

    /// <summary>
    /// Subscribe new fish to the inventory system
    /// Call this when spawning new fish
    /// </summary>
    public void RegisterFish(Fish fish)
    {
        if (fish != null)
        {
            fish.OnCaught += HandleFishCaught;
        }
    }
}

/// <summary>
/// Data structure for caught fish
/// </summary>
[System.Serializable]
public class CaughtFish
{
    public string name;
    public int value;
    public float timeCaught;
}
