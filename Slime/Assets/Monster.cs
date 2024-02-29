using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public string monsterName;
    public bool playerInRange;

    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;


    [Header("Sounds")]
    [SerializeField] AudioSource soundChannel;
    [SerializeField] AudioClip mosterHit;
    [SerializeField] AudioClip mosterDie;

    private Animator monster;
    public bool isDead;

    private void Start()
    {
        currentHealth = maxHealth;

        monster = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead == false)
        {
            currentHealth -= damage;
            Debug.Log(currentHealth);
            if (currentHealth <= 0)
            {
                soundChannel.PlayOneShot(mosterDie);
                Destroy(gameObject);

                monster.SetTrigger("DIE");
                GetComponent<AI_Movement>().enabled = false;

                isDead = true;
            }
            else
            {
                soundChannel.PlayOneShot(mosterHit);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("อยู่");
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("ไม่อยู่");
            playerInRange = false;
        }
    }
}
