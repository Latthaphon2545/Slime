using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public string monsterName;
    public bool playerInRange;

    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;

    public List<GameObject> itemPrefabs;


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

                int ran = Random.Range(0, itemPrefabs.Count());
                Vector3 tranformPosition = transform.position;
                Instantiate(itemPrefabs[ran], new Vector3(tranformPosition.x, tranformPosition.y, tranformPosition.z), Quaternion.identity);



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
