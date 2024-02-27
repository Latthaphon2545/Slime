using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; set; }

    // Health
    public float currentHealth;
    public float maxHealth;

    // Stamina
    public float currentStamina;
    public float maxStamina;

    float distanceTravalled = 0;
    Vector3 lastPosition;

    public GameObject playerBody;

    private void Awake()
    {
        if(Instance != null && Instance !=this)
        {
            Destroy(gameObject);
        }
        else{
            Instance = this;
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {

        distanceTravalled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;

        if (distanceTravalled >= 5)
        {
            distanceTravalled = 0;
            currentStamina -= 0.5f;
        }


        if(Input.GetKeyDown(KeyCode.N))
        {
            currentHealth -= 10;
        }
    }

    public void setHealth(float newHealth)
    {
        currentHealth = newHealth;
    }

    public void setStamina(float newStamina)
    {
        currentStamina = newStamina;
    }
}
