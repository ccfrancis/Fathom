using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Displays current currency on UI
/// </summary>
public class CurrencyDisplay : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private Text legacyText; // Fallback for regular Text

    [Header("Format")]
    [SerializeField] private string prefix = "$";
    [SerializeField] private string suffix = "";

    void Start()
    {
        // Subscribe to currency changes
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCurrencyChanged += UpdateDisplay;
            UpdateDisplay(CurrencyManager.Instance.CurrentCurrency, 0);
        }
        else
        {
            Debug.LogWarning("CurrencyDisplay: CurrencyManager not found!");
        }
    }

    void OnDestroy()
    {
        // Unsubscribe
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCurrencyChanged -= UpdateDisplay;
        }
    }

    void UpdateDisplay(int newAmount, int changeAmount)
    {
        string displayText = $"{prefix}{newAmount}{suffix}";

        if (currencyText != null)
        {
            currencyText.text = displayText;
        }
        else if (legacyText != null)
        {
            legacyText.text = displayText;
        }
    }
}
