using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Simple shop UI that shows fish count, sell button, and upgrade button
/// </summary>
public class ShopUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private TextMeshProUGUI fishCountText;
    [SerializeField] private TextMeshProUGUI fishValueText;
    [SerializeField] private Button sellButton;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI upgradeNameText;
    [SerializeField] private TextMeshProUGUI upgradeCostText;
    [SerializeField] private TextMeshProUGUI instructionText;

    [Header("References")]
    [SerializeField] private Shop shop;

    private ShopUpgrade currentUpgrade;

    void Start()
    {
        // Find shop if not assigned
        if (shop == null)
        {
            shop = FindObjectOfType<Shop>();
        }

        // Subscribe to shop events
        if (shop != null)
        {
            shop.OnShopOpened += ShowShop;
            shop.OnShopClosed += HideShop;
        }

        // Setup buttons
        if (sellButton != null)
        {
            sellButton.onClick.AddListener(OnSellButtonClicked);
            Debug.Log("Sell button listener added");
        }
        else
        {
            Debug.LogError("ShopUI: Sell button reference is missing!");
        }

        if (upgradeButton != null)
        {
            upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
            Debug.Log("Upgrade button listener added");
        }
        else
        {
            Debug.LogError("ShopUI: Upgrade button reference is missing!");
        }

        HideShop();
    }

    void OnDestroy()
    {
        // Unsubscribe
        if (shop != null)
        {
            shop.OnShopOpened -= ShowShop;
            shop.OnShopClosed -= HideShop;
        }
    }

    void ShowShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(true);
        }

        // Load available upgrades when shop opens
        if (shop != null)
        {
            var upgrades = shop.GetAvailableUpgrades();
            if (upgrades != null && upgrades.Count > 0)
            {
                currentUpgrade = upgrades[0];
                Debug.Log($"Loaded upgrade: {currentUpgrade.name}");
            }
            else
            {
                Debug.LogWarning("No upgrades available in shop!");
            }
        }

        UpdateUI();
    }

    void HideShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
        }
    }

    void UpdateUI()
    {
        // Update fish count
        if (FishInventory.Instance != null)
        {
            int fishCount = FishInventory.Instance.FishCount;
            int totalValue = FishInventory.Instance.TotalValue;

            if (fishCountText != null)
            {
                fishCountText.text = $"Fish: {fishCount}";
            }

            if (fishValueText != null)
            {
                fishValueText.text = $"Value: ${totalValue}";
            }

            // Enable/disable sell button
            if (sellButton != null)
            {
                sellButton.interactable = fishCount > 0;
            }
        }

        // Update upgrade info
        if (currentUpgrade != null)
        {
            if (upgradeNameText != null)
            {
                upgradeNameText.text = currentUpgrade.name;
            }

            if (upgradeCostText != null)
            {
                upgradeCostText.text = $"${currentUpgrade.cost}";
            }

            // Enable/disable upgrade button based on currency
            if (upgradeButton != null && CurrencyManager.Instance != null)
            {
                upgradeButton.interactable = CurrencyManager.Instance.CanAfford(currentUpgrade.cost);
            }
        }

        // Update instructions
        if (instructionText != null)
        {
            instructionText.text = "Press E to close shop";
        }
    }

    void OnSellButtonClicked()
    {
        if (shop != null)
        {
            shop.SellFish();
            UpdateUI();
        }
    }

    void OnUpgradeButtonClicked()
    {
        Debug.Log("Upgrade button clicked!");

        if (shop == null)
        {
            Debug.LogError("Shop reference is null!");
            return;
        }

        if (currentUpgrade == null)
        {
            Debug.LogError("Current upgrade is null!");
            return;
        }

        Debug.Log($"Attempting to buy {currentUpgrade.name} for {currentUpgrade.cost} coins");

        if (shop.BuyUpgrade(currentUpgrade))
        {
            Debug.Log("Upgrade purchased successfully!");
            UpdateUI();
        }
        else
        {
            Debug.Log("Failed to purchase upgrade");
        }
    }

    void Update()
    {
        // Update UI continuously while shop is open (for currency changes)
        if (shop != null && shop.IsOpen)
        {
            UpdateUI();
        }
    }
}
