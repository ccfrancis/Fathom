using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Surface shop for selling fish and buying upgrades
/// Only accessible when player is at surface
/// </summary>
public class Shop : MonoBehaviour
{
    [Header("Shop Settings")]
    [SerializeField] private bool isOpen = false;
    [SerializeField] private KeyCode openShopKey = KeyCode.E;

    [Header("Upgrades")]
    [SerializeField] private List<ShopUpgrade> availableUpgrades = new List<ShopUpgrade>();

    [Header("References")]
    [SerializeField] private PlayerZoneDetector playerZoneDetector;
    [SerializeField] private PlayerController playerController;

    // Events
    public event Action OnShopOpened;
    public event Action OnShopClosed;
    public event Action<int> OnFishSold; // amount earned
    public event Action<ShopUpgrade> OnUpgradePurchased;

    // Properties
    public bool IsOpen => isOpen;
    public bool CanOpenShop => playerZoneDetector != null && playerZoneDetector.IsAtSurface;

    void Start()
    {
        // Find player components if not assigned
        if (playerZoneDetector == null)
        {
            playerZoneDetector = FindObjectOfType<PlayerZoneDetector>();
        }

        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
        }

        // Initialize default upgrade (swim speed) if list is null or empty
        if (availableUpgrades == null || availableUpgrades.Count == 0)
        {
            if (availableUpgrades == null)
            {
                availableUpgrades = new List<ShopUpgrade>();
            }

            availableUpgrades.Add(new ShopUpgrade
            {
                name = "Faster Swim Fins",
                description = "Increases swim speed by 2 units/sec",
                cost = 50,
                upgradeType = UpgradeType.SwimSpeed,
                upgradeValue = 2f
            });
        }

        CloseShop();
    }

    void Update()
    {
        // Toggle shop with key when at surface
        if (Input.GetKeyDown(openShopKey) && CanOpenShop)
        {
            if (isOpen)
            {
                CloseShop();
            }
            else
            {
                OpenShop();
            }
        }
    }

    public void OpenShop()
    {
        if (!CanOpenShop)
        {
            Debug.Log("Can't open shop - not at surface!");
            return;
        }

        isOpen = true;
        Time.timeScale = 0f; // Pause game
        OnShopOpened?.Invoke();

        Debug.Log("Shop opened! Press E to close");
    }

    public void CloseShop()
    {
        isOpen = false;
        Time.timeScale = 1f; // Resume game
        OnShopClosed?.Invoke();
    }

    /// <summary>
    /// Sell all fish in inventory
    /// </summary>
    public void SellFish()
    {
        if (FishInventory.Instance == null)
        {
            Debug.LogError("FishInventory not found!");
            return;
        }

        int fishCount = FishInventory.Instance.FishCount;
        int totalValue = FishInventory.Instance.TotalValue;

        if (fishCount == 0)
        {
            Debug.Log("No fish to sell!");
            return;
        }

        // Sell all fish
        int earnedMoney = FishInventory.Instance.SellAllFish();
        OnFishSold?.Invoke(earnedMoney);

        Debug.Log($"Sold {fishCount} fish for {earnedMoney} coins!");
    }

    /// <summary>
    /// Purchase an upgrade
    /// </summary>
    public bool BuyUpgrade(ShopUpgrade upgrade)
    {
        if (CurrencyManager.Instance == null)
        {
            Debug.LogError("CurrencyManager not found!");
            return false;
        }

        // Check if can afford
        if (!CurrencyManager.Instance.CanAfford(upgrade.cost))
        {
            Debug.Log($"Can't afford {upgrade.name}. Need {upgrade.cost} coins.");
            return false;
        }

        // Spend currency
        if (!CurrencyManager.Instance.SpendCurrency(upgrade.cost))
        {
            return false;
        }

        // Apply upgrade
        ApplyUpgrade(upgrade);
        OnUpgradePurchased?.Invoke(upgrade);

        Debug.Log($"Purchased {upgrade.name}!");
        return true;
    }

    void ApplyUpgrade(ShopUpgrade upgrade)
    {
        if (playerController == null)
        {
            Debug.LogError("PlayerController not found!");
            return;
        }

        switch (upgrade.upgradeType)
        {
            case UpgradeType.SwimSpeed:
                // Increase player swim speed
                playerController.IncreaseMoveSpeed(upgrade.upgradeValue);
                break;

            // Future upgrade types can be added here
        }
    }

    public List<ShopUpgrade> GetAvailableUpgrades()
    {
        return new List<ShopUpgrade>(availableUpgrades);
    }
}

/// <summary>
/// Shop upgrade data
/// </summary>
[System.Serializable]
public class ShopUpgrade
{
    public string name;
    public string description;
    public int cost;
    public UpgradeType upgradeType;
    public float upgradeValue;
}

public enum UpgradeType
{
    SwimSpeed,
    OxygenCapacity,
    SpearSpeed,
    // Add more types as needed
}
