using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private readonly string ANIM_WALK = "Zombie_Walk";
    private readonly string ANIM_EAT = "Zombie_Eating";

    [SerializeField] private int dame;
    [SerializeField] private int health;
    [SerializeField] private int range;
    [SerializeField] private bool isBoss = false;
    [SerializeField] private Slider healthBar;
    [SerializeField] private EnemyTarget enemyTargetPrefab;
    [SerializeField] private List<GameObject> itemPrefabs;
    
    private GameObject itemPrefab;
    private System.Random random = new System.Random();
    private GameObject targetGameObject;
    private string ownorID;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private string curentState;

    public string OwnorID { get => ownorID; set => ownorID = value; }
    public GameObject TargetGameObject { get => targetGameObject; set => targetGameObject = value; }
    public bool IsBoss { get => isBoss; set => isBoss = value; }

    private bool isDied;

    private PlayerController player;

    private void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        this.transform.tag = GameContans.ENEMY_TAG_NAME;
        OwnorID = RandomString(10);
        player = FindObjectOfType<PlayerController>();
        itemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Count)];

        healthBar.maxValue = health;
        healthBar.value = health;
    }


    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= range)
        {
            TargetGameObject = player.gameObject;
        }

        if(TargetGameObject == null)
            InitializedTarget();
        
        navMeshAgent.SetDestination(TargetGameObject.transform.position);
        ChangeAnimationState(ANIM_WALK);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == GameContans.PLAYER_TAG_NAME)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            TargetGameObject = player.gameObject;
            player.TakeDame(dame);
        }
    }

    private void InitializedTarget()
    {
        var clone = Instantiate(enemyTargetPrefab, transform.position, transform.rotation,transform.parent);
        TargetGameObject = clone.gameObject;
        clone.EnemyOwnorID = OwnorID;
    }

    private string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public void TakeDame(int amount)
    {
        health -= amount;
        healthBar.value = health;

        if(health <= 0 && !isDied)
        {
            Die();
        }
    }

    private void Die()
    {
        isDied = true;
        if (isBoss)
            GamePlayController.Instance.OnBossDie();
        else
            GamePlayController.Instance.OnEnemyDie();

        ChangeAnimationState(ANIM_EAT);
        Destroy(gameObject, 1.0f);
        SpawnItem();
    }

    private void SpawnItem()
    {
        
        Instantiate(itemPrefab, transform.position, transform.rotation);
        var random = Random.Range(-1, 100);
        if(isBoss || random  % 2 == 0)
        {
            Instantiate(itemPrefab, transform.position, transform.rotation);
        }
    }

    private void ChangeAnimationState(string newState)
    {
        if (string.Equals(curentState, newState))
            return;

        StartCoroutine(DeplayChangeAnimationState(newState));
    }

    private IEnumerator DeplayChangeAnimationState(string newState)
    {
        yield return new WaitForSeconds(0.5f);
        animator.Play(newState);
        curentState = newState;
    }
}
