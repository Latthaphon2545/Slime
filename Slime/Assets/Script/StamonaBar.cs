using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StamonaBar : MonoBehaviour
{
    private Slider slider;
    public TMP_Text staminaCounter;

    public GameObject playerState;

    private float currentStamina, maxStamina;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        currentStamina = playerState.GetComponent<PlayerState>().currentStamina;
        maxStamina = playerState.GetComponent<PlayerState>().maxStamina;

        float fillValur = currentStamina / maxStamina;
        slider.value = fillValur;

        staminaCounter.text = currentStamina + "/" + maxStamina;
    }
}
