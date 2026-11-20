using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static Unity.Collections.Unicode;

public class BasicEnemyMovement : MonoBehaviour
{
    public static event Action<GameObject> OnEnemySpawn;
    public static event Action<Transform> OnPlayerAttacked;
    public static event Action<GameObject> OnExorcited;
    [Header("Distances")]
    [SerializeField] private float MaxDistanceFromPlayer;
    [SerializeField] private float AttackDistance;
    [Header("Movement")]
    [SerializeField] private float speed;

    [Header("Dead")]
    [SerializeField] private float goDown;
    [SerializeField] private float deathTime;

    [Header("Debug")]
    Transform playerT;
    NavMeshAgent agent;
    [SerializeField] private Animator anim;
    [SerializeField] bool move= true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        playerT = GameObject.FindGameObjectWithTag("Player").transform;
        OnPlayerAttacked += KillMyself;
        OnEnemySpawn?.Invoke(gameObject);
    }
    private void SetMove()
    {
        move = anim.GetCurrentAnimatorStateInfo(0).IsName("run1");
        if (agent.enabled)
        {
            agent.isStopped = !move;
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        SetMove();

        float distanceToPlayer = Vector3.Distance(playerT.position, transform.position);
        if (move)
        {

            if (distanceToPlayer > MaxDistanceFromPlayer)
            {
                Die(false);
            }
            else if (distanceToPlayer <= AttackDistance)
            {
                Attack();
            }
            else
            {
                if (agent.enabled )
                {
                    agent.SetDestination(playerT.position);
                }
            }
        }
        
        
        
    }
    private void OnDestroy()
    {
        OnPlayerAttacked -= KillMyself;
    }
    private void Attack()
    {
        OnPlayerAttacked?.Invoke(transform);
        anim.SetTrigger("Attack");

    }

    private void KillMyself(Transform t)
    {
        Die(false, 3f);
    }
    Coroutine dieCoroutineSave;
    private void Die(bool withAnimation)
    {

        Die(withAnimation, 0);


    }
    private void Die(bool withAnimation, float delay)
    {

        if (withAnimation)
        {

            if (dieCoroutineSave == null)
            {
                dieCoroutineSave = StartCoroutine(DieCoroutine(delay));
            }

        }
        else
        {
            if(dieCoroutineSave == null)
            {
                dieCoroutineSave = StartCoroutine(DestroyWithDelay(delay));
            }
            
        }

    }
    IEnumerator DestroyWithDelay(float delay)
    {
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }
        Destroy(gameObject);
    }

    IEnumerator DieCoroutine(float delay)
    {
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }
        anim.SetTrigger("Kill");
        OnExorcited.Invoke(gameObject);
        Collider collider = GetComponent<Collider>();

        if (collider!= null)
        {
            collider.enabled = false;
        }
        agent.enabled = false;
        float timer = 0f;
        while(timer < deathTime)
        {
            yield return null;
            timer += Time.deltaTime;
            transform.position -= Vector3.up * goDown * (Time.deltaTime/deathTime);
        }
        dieCoroutineSave = null; // se que no es necesario pero por si acaso
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, MaxDistanceFromPlayer);
    }
    public void Exorcite()
    {
        if (move)
        {
            Die(true);
        }
        
    }
    public float getMaxDistance()
    {
        return MaxDistanceFromPlayer;
    }
}
