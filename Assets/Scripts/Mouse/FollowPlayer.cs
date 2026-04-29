
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public enum EnemyState
{
    Patrol,
    Chase,
    Flee
}
public class FollowPlayer : MonoBehaviour
{
    public EnemyState currentState;

    NavMeshAgent agent;
    Rigidbody rb;
    [SerializeField] float rotationVelocity = 2.5f;

    public Transform player;
    public Transform[] patrolPoints ;
    int patrolIndex = 0;
    public float speed = 5f;
    public float detectionDistance = 10f;
    public float fleeDistance = 3f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        agent.updatePosition = false;
       // agent.updateRotation = false;

        //currentState = EnemyState.Chase;

    }

    void FixedUpdate() { 
        
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;

            case EnemyState.Chase:
                Chase();
                break;

            case EnemyState.Flee:
                Flee();
                break;
        }

        
    }
    void Move()
    {
        if (agent.hasPath)
        {
            Vector3 dir = (agent.nextPosition - transform.position).normalized;

            // ignorar inclinación vertical
            dir.y = 0;

            if (dir != Vector3.zero)
            {
                // rotación suave
                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationVelocity * Time.fixedDeltaTime // velocidad de giro
                );
            }

            // moverse SOLO hacia adelante (no lateral)
            rb.linearVelocity = transform.forward * speed + new Vector3(0, rb.linearVelocity.y, 0);

            agent.nextPosition = transform.position;
        }
    }

    void Flee()
    {
        
        if(player == null) return;
        Vector3 dir = (transform.position - player.position).normalized;
        Vector3 target = transform.position + dir * 5f;

        agent.SetDestination(target);
        Move();

        if (Vector3.Distance(transform.position, player.position) > detectionDistance)
        {
            currentState = EnemyState.Patrol;
        }
    }
    void Chase()
    {
        if(player == null) return;
        agent.SetDestination(player.position);
        Move();

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist < fleeDistance)
        {
           // currentState = EnemyState.Flee;
        }
    }

    void Patrol()
    {
        Transform target = patrolPoints[patrolIndex];

        agent.SetDestination(target.position);
        Move();

        if (Vector3.Distance(transform.position, target.position) < 1f)
        {
            patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
        }

        // cambio de estado
        if (player!=null && Vector3.Distance(transform.position, player.position) < detectionDistance)
        {
            currentState = EnemyState.Chase;
        }
    }
    void CheckJump()
    {
        Vector3 forward = transform.forward;

        // detecta pared
        if (Physics.Raycast(transform.position, forward, out RaycastHit hit, 1f))
        {
            // detecta espacio arriba
            Vector3 upper = transform.position + Vector3.up * 1.5f;

            if (!Physics.Raycast(upper, forward, 1f))
            {
                Jump();
            }
        }
    }
    void Jump()
    {
       // rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
    }
}
