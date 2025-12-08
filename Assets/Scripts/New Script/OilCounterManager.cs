using UnityEngine;
using TMPro;
using System.Collections.Generic; // Required for List

public class OilCounterManager : MonoBehaviour
{
    public static OilCounterManager Instance; 

    [Header("Wall Display Setup")]
    // CHANGE: Use a List to hold ALL your counter text objects
    public List<TextMeshPro> counterTexts = new List<TextMeshPro>(); 

    private int oilCount = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateCounterUI();
    }

    public void IncrementOilCount()
    {
        oilCount++;
        UpdateCounterUI();
    }

    private void UpdateCounterUI()
    {
        // FIX: Loop through every text object in the list and update it
        foreach (TextMeshPro textDisplay in counterTexts)
        {
            if (textDisplay != null)
            {
                textDisplay.text = "Oil Spawned: " + oilCount.ToString();
            }
        }
    }
}