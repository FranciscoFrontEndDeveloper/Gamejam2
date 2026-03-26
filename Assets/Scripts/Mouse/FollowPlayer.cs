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

    public Transform player;
    public Transform[] patrolPoints;

    int patrolIndex = 0;
    public float speed = 5f;
    public float detectionDistance = 10f;
    public float fleeDistance = 3f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        agent.updatePosition = false;
        agent.updateRotation = false;

        currentState = EnemyState.Patrol;

        StartCoroutine(StateMachine());
    }

    IEnumerator StateMachine()
    {
        while (true)
        {
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

            yield return new WaitForFixedUpdate();
        }
    }
    void Move()
    {
        if (agent.hasPath)
        {
            Vector3 dir = (agent.nextPosition - transform.position).normalized;

            rb.linearVelocity = new Vector3(
                dir.x * speed,
                rb.linearVelocity.y,
                dir.z * speed
            );

            agent.nextPosition = transform.position;
        }
    }

    void Flee()
    {
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
        agent.SetDestination(player.position);
        Move();

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist < fleeDistance)
        {
            currentState = EnemyState.Flee;
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
        if (Vector3.Distance(transform.position, player.position) < detectionDistance)
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
        rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
    }
}
