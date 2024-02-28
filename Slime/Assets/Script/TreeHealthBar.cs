using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeHealthBar : MonoBehaviour
{
    private Slider slider;
    private float currentHealth, maxHealth;

    public GameObject globalState;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }


    private void Update()
    {
        currentHealth = globalState.GetComponent<GlobalState>().resourceHealth;
        maxHealth = globalState.GetComponent<GlobalState>().resourceMaxHealth;

        float fillValur = currentHealth / maxHealth;
        slider.value = fillValur;

    }
}
