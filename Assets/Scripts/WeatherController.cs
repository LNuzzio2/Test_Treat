using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
/// <summary>
/// Script that controls the weather change via the button
/// </summary>
public class WeatherController : MonoBehaviour
{
    //Weather base settings
    [System.Serializable]
    public struct WeatherConfig
    {
        public GameObject uiIcon;     
        public Sprite skySprite;      
        public Color skyColor;        
    }

    [Header("References")]
    [SerializeField] private SpriteRenderer backgroundRenderer; 
    [SerializeField] private List<Material> materialsToAffect;
    [SerializeField] private Button cycleButton;

    [Header("Configurations")]
    [SerializeField] private List<WeatherConfig> weatherStates;

    private int _currentIndex = 0;

    void Start()
    {
        if (cycleButton != null)
            cycleButton.onClick.AddListener(CycleWeather);

        UpdateWeatherState();
    }

    public void CycleWeather()
    {
        _currentIndex = (_currentIndex + 1) % weatherStates.Count;
        UpdateWeatherState();
    }

    private void UpdateWeatherState()
    {
        if (weatherStates.Count == 0) return;

        WeatherConfig currentField = weatherStates[_currentIndex];

        // Change weather image
        if (backgroundRenderer != null && currentField.skySprite != null)
        {
            backgroundRenderer.sprite = currentField.skySprite;
        }

        // Change weather UIIcon
        for (int i = 0; i < weatherStates.Count; i++)
        {
            if (weatherStates[i].uiIcon != null)
            {
                weatherStates[i].uiIcon.SetActive(i == _currentIndex);
            }
        }

        // Change the base colors of the materilas in the list
        foreach (Material mat in materialsToAffect)
        {
            if (mat != null)
            {
                mat.SetColor("_BaseColor", currentField.skyColor);
            }
        }
    }
}