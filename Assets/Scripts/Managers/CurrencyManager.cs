using UnityEngine;
using System;

/// <summary>
/// Manages player's currency (money from selling fish)
/// Singleton pattern for easy global access
/// </summary>
public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [Header("Currency")]
    [SerializeField] private int currentCurrency = 0;
    [SerializeField] private int startingCurrency = 0;

    // Events
    public event Action<int, int> OnCurrencyChanged; // newAmount, changeAmount
    public event Action<int> OnPurchase; // amount spent
    public event Action<int> OnEarn; // amount earned

    // Properties
    public int CurrentCurrency => currentCurrency;

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

        // Initialize currency
        currentCurrency = startingCurrency;
    }

    /// <summary>
    /// Add currency (from selling fish)
    /// </summary>
    public void AddCurrency(int amount)
    {
        if (amount <= 0) return;

        currentCurrency += amount;
        OnEarn?.Invoke(amount);
        OnCurrencyChanged?.Invoke(currentCurrency, amount);

        Debug.Log($"Earned {amount} coins. Total: {currentCurrency}");
    }

    /// <summary>
    /// Remove currency (for purchases)
    /// </summary>
    public bool SpendCurrency(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning("Cannot spend negative or zero amount");
            return false;
        }

        if (currentCurrency < amount)
        {
            Debug.LogWarning($"Not enough currency. Need {amount}, have {currentCurrency}");
            return false;
        }

        currentCurrency -= amount;
        OnPurchase?.Invoke(amount);
        OnCurrencyChanged?.Invoke(currentCurrency, -amount);

        Debug.Log($"Spent {amount} coins. Remaining: {currentCurrency}");
        return true;
    }

    /// <summary>
    /// Check if player can afford something
    /// </summary>
    public bool CanAfford(int amount)
    {
        return currentCurrency >= amount;
    }

    /// <summary>
    /// Reset currency (for testing or new game)
    /// </summary>
    public void ResetCurrency()
    {
        int oldAmount = currentCurrency;
        currentCurrency = startingCurrency;
        OnCurrencyChanged?.Invoke(currentCurrency, -oldAmount);
    }
}
