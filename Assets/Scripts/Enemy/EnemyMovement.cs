using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    
    private enum State {
            Patrol,
            Chase,
            Attack
        }

    private State currentState = State.Patrol;

    public Transform[] wayPoints;

    [Header("Movimiento")]
    [SerializeField] private float margin = 2f;
    [SerializeField] private float patrolSpeed = 3.5f;
    [SerializeField] private float chaseSpeed = 5f;

    [SerializeField] private float idleDuration = 2f;
    [SerializeField] private float attackDst = 2f;
    private int WPIndex = -1;

    private float idleTimer;

    private Transform PJ;
    private NavMeshAgent agent;
    [SerializeField] private Animator anim;

    private bool playerDetected;
    private bool isAttacking;
    private bool waitingAtWaypoint;
    private bool isWalkingTowardsDestiny = false;

    [Header("Last PJ Position")]
    private bool wasChasing;
    [SerializeField] private float chaseMemoryDuration = 4f;

    private float lastTimePlayerSeen;
    private bool hasSeenPlayer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            PJ = player.transform;
            SetPlayerDetected(true);

        }

        agent = GetComponent<NavMeshAgent>();
        if(wayPoints != null)
        {
            WPIndex = Random.Range(0, wayPoints.Length);
            agent.SetDestination(wayPoints[WPIndex].position);
        }

        
    }
    public void SetPlayerDetected(bool detected)
    {
        playerDetected = detected;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking)
        {
            return;
        }


        // Remember player last position
        if (playerDetected)
        {

            lastTimePlayerSeen = Time.time;
            hasSeenPlayer = true;
        }

        // Aun recordamos donde estaba el jugador, o no?
        bool shouldChase = hasSeenPlayer && Time.time - lastTimePlayerSeen < chaseMemoryDuration; //debo añadir algo para que el jugador no sea perseguido si no es accesible


        if (shouldChase)
        {

            float distance = Vector3.Distance(transform.position, PJ.position);

            if (distance <= attackDst && playerDetected)
            {
                StartAttack();
                return;
            }
            currentState = State.Chase;
            wasChasing = true;
        }

        else if (isWalkingTowardsDestiny == false)
        {

            hasSeenPlayer = false;

            // Reset Patrol

            WPIndex = Random.Range(0, wayPoints.Length);

            isWalkingTowardsDestiny = true;

            agent.isStopped = false;
            agent.speed = patrolSpeed;
            agent.SetDestination(wayPoints[WPIndex].position);

            currentState = State.Patrol;
        }
        switch (currentState)
        {

            case State.Patrol:
                Patrol();
                break;

            case State.Chase:
                Chase();
                break;
        }
    }

    private void Patrol()
    {

        if (waitingAtWaypoint)
        {

            agent.isStopped = true;

            SetWalking(false);

            idleTimer -= Time.deltaTime;

            if (idleTimer <= 0f)
            {

                waitingAtWaypoint = false;

                // Choose another point
                WPIndex = Random.Range(0, wayPoints.Length);

                bool destinationSet = agent.SetDestination(wayPoints[WPIndex].position);

                Debug.Log("Nuevo waypoint: " + WPIndex);
                Debug.Log("Destino establecido: " + destinationSet);
            }

            return;
        }

        agent.isStopped = false;
        agent.speed = patrolSpeed;

        SetWalking(true);
    }

    private void Chase() {

        waitingAtWaypoint = false;

        agent.isStopped = false;
        agent.speed = chaseSpeed;

        // Actualizar el destino solo si ve al jugador.
        // Si no lo ve, conserva el último destino establecido.

        if (playerDetected)
        {

            agent.SetDestination(PJ.position);
        }

        SetWalking(true);

    }
    private void StartAttack()
    {

        currentState = State.Attack;
        isAttacking = true;

        agent.isStopped = true;
        //agent.velocity = Vector3.zero;
        //agent.speed =0f;

        SetWalking(false);

        anim.ResetTrigger("Attack");
        anim.SetTrigger("Attack");

        Debug.Log("Atacando al jugador");
    }

    public void OnAttackAnimationFinished() {

        isAttacking = false;
        playerDetected = false;
        hasSeenPlayer = false;
        wasChasing = false;

        currentState = State.Patrol;
        waitingAtWaypoint = false;

        WPIndex = Random.Range(0, wayPoints.Length);

        agent.speed = patrolSpeed;
        agent.isStopped = false;
        agent.SetDestination(wayPoints[WPIndex].position);

        Debug.Log("Ataque terminado: volviendo a patrullar");
    }


    private void SetWalking(bool walking)
    {
        anim.SetBool("Walk", walking);
    }
}
