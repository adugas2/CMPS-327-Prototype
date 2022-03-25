using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    public enum State { Wander, Chase, Shoot };


    [SerializeField] private Transform player;
    [SerializeField] private State state;
    private NavMeshAgent ai;

    [SerializeField] private float visionAngle = 50;

    [Header("Wander Settings")]
    [SerializeField] private float circleDist = 5;
    [SerializeField] private float circleRad = 3;
    [SerializeField] private float angle;
    [SerializeField] private Vector2 wanderRange = new Vector2(-1, 3);

    [Header("Shoot Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform gunTip;
    [SerializeField] private float bulletForce = 20;

    [SerializeField] private float runSpeed = 4;

    [Header("Transition Settings")]
    [SerializeField] private float inRangeDist = 10;

    private Dictionary<State, System.Action> enter;
    private Dictionary<State, System.Action> exit;
    private Dictionary<State, System.Action> execute;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        ai = GetComponent<NavMeshAgent>();
        enter = new Dictionary<State, System.Action>() {
            { State.Chase, ChaseEnter },
            { State.Wander, WanderEnter },
            { State.Shoot, ShootEnter }
        };
        exit = new Dictionary<State, System.Action>() {
            { State.Chase, ChaseExit },
            { State.Wander, WanderExit },
            { State.Shoot, ShootExit }
        };
        execute = new Dictionary<State, System.Action>() {
            { State.Chase, ChaseExecute },
            { State.Wander, WanderExecute },
            { State.Shoot, ShootExecute }
        };

        Transition(State.Wander);
    }

    // Update is called once per frame
    void Update()
    {
        execute[state]();
    }

    private void Transition(State nextState)
    {
        exit[state]();
        state = nextState;
        enter[state]();
    }

    private bool CanSee()
    {
        Vector2 toPlayer = player.position - transform.position;
        float angToPlayer = Vector3.Angle(transform.forward, toPlayer);
        return angToPlayer < visionAngle && Vector3.Distance(transform.position, player.position) < 30;
    }

    private bool InRange()
    {
        return Vector2.Distance(transform.position, player.position) < inRangeDist;
    }

    void WanderEnter()
    {
        ai.speed = 1;
    }
    void WanderExit()
    {

    }
    void WanderExecute()
    {
        angle += UnityEngine.Random.Range(wanderRange.x, wanderRange.y);
        Vector3 posOnCircle = new Vector3(Mathf.Sin(angle) * circleRad, 0, Mathf.Cos(angle) * circleRad);
        Vector3 target = transform.position + transform.forward * circleDist + posOnCircle;

        ai.destination = target;

        // Check transition
        if (CanSee())
        {
            Transition(State.Chase);
        }
    }
    void ChaseEnter()
    {
        ai.speed = runSpeed;
    }
    void ChaseExit()
    {

    }
    void ChaseExecute()
    {
        ai.destination = player.position;

        if (InRange())
            Transition(State.Shoot);
    }
    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = gunTip.position;
        bullet.transform.rotation = gunTip.rotation;

        Vector3 force = (player.position - gunTip.position).normalized * bulletForce;

        bullet.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
    }
    void ChangeShootAngle()
    {
        shootAngle += UnityEngine.Random.Range(-10, 20);
    }
    void ShootEnter()
    {
        InvokeRepeating("Shoot", 0, 1);
        InvokeRepeating("ChangeShootAngle", 0, 3);
    }
    void ShootExit()
    {
        CancelInvoke();
    }
    float shootAngle;
    void ShootExecute()
    {
        Vector3 posOnCircle = new Vector3(Mathf.Sin(shootAngle) * inRangeDist, 0, Mathf.Cos(shootAngle) * inRangeDist);
        Vector3 target = player.position + posOnCircle;

        ai.destination = target;
        transform.LookAt(player);

        if (!CanSee())
            Transition(State.Wander);
    }
}
