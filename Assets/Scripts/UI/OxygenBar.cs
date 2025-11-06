using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI component that displays the player's oxygen level
/// </summary>
[RequireComponent(typeof(Slider))]
public class OxygenBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private OxygenSystem oxygenSystem;

    [Header("Visual Settings")]
    [SerializeField] private Image fillImage;
    [SerializeField] private Color normalColor = Color.cyan;
    [SerializeField] private Color lowOxygenColor = Color.red;
    [SerializeField] private bool animateLowOxygen = true;
    [SerializeField] private float pulseSpeed = 2f;

    private Slider slider;
    private bool isLowOxygen = false;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Start()
    {
        // Find oxygen system if not assigned
        if (oxygenSystem == null)
        {
            oxygenSystem = FindObjectOfType<OxygenSystem>();
            if (oxygenSystem == null)
            {
                Debug.LogError("OxygenBar: Could not find OxygenSystem in scene!");
                return;
            }
        }

        // Subscribe to oxygen events
        oxygenSystem.OnOxygenChanged += UpdateOxygenBar;
        oxygenSystem.OnLowOxygenWarning += HandleLowOxygenWarning;

        // Initialize UI
        UpdateOxygenBar(oxygenSystem.CurrentOxygen, oxygenSystem.MaxOxygen);
    }

    void OnDestroy()
    {
        // Unsubscribe from events
        if (oxygenSystem != null)
        {
            oxygenSystem.OnOxygenChanged -= UpdateOxygenBar;
            oxygenSystem.OnLowOxygenWarning -= HandleLowOxygenWarning;
        }
    }

    void Update()
    {
        // Animate low oxygen warning
        if (isLowOxygen && animateLowOxygen && fillImage != null)
        {
            float pulse = Mathf.PingPong(Time.time * pulseSpeed, 1f);
            fillImage.color = Color.Lerp(lowOxygenColor, normalColor, pulse * 0.3f);
        }
    }

    void UpdateOxygenBar(float current, float max)
    {
        if (slider != null)
        {
            slider.maxValue = max;
            slider.value = current;
        }
    }

    void HandleLowOxygenWarning(bool isLow)
    {
        isLowOxygen = isLow;

        // Reset color when oxygen is no longer low
        if (fillImage != null)
        {
            if (!isLow)
            {
                fillImage.color = normalColor;
            }
            else if (!animateLowOxygen)
            {
                fillImage.color = lowOxygenColor;
            }
        }
    }
}
