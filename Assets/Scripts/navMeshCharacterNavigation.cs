using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class navMeshCharacterNavigation : MonoBehaviour
{
    // public inspector fields
    public NavMeshAgent agent;
    public GameObject agentObject; // used to get material
    [Space(20)]
    public int patrolToStation = 0; //set to 0 so the engineer can choose starting Station
    public GameObject patrolStation1;
    public GameObject patrolStation2;
    public GameObject patrolStation3;
    public GameObject patrolStation4;
    [Space(10)]
    public GameObject chaseObject;
    [Space(10)]
    public Material displayPatrolling;
    public Material displayChasing;
    public Material displaySearching;
    public Material displayAttacking;
    public Material displayRetreating;

    // private fields
    private Material displayState;
    private Vector3 lastSeen; // holds the position when the player was last seen
    private Vector3 lastBeen; // holds the position from when the enemy left their patrol
    private bool startSearchOnce = true;
    private static STATE _state = STATE.PATROLLING;
    private enum STATE
    {
        PATROLLING, // to Chasing, from Retreating  /                   (set up patrol in square fashion with obstacles between points)
        CHASING,    // to Attacking or Searching, from Patrolling  /    (situational)
        SEARCHING,  // to Chasing or Retreating, from Chasing /         (situational)
        ATTACKING,  // to Chasing, from Chasing /                       (used to stop< and move away during an attack)
        RETREATING  // returns to Patrolling from Searching             
    }
    void Start()
    {
        agentObject.GetComponent<MeshRenderer>().material = displayState;
        lastSeen = this.transform.position;
        lastBeen = this.transform.position;
    }
    void Update()
    {
        Debug.Log("current state: "+_state);
        agentObject.GetComponent<MeshRenderer>().material = displayState;
        switch (_state)
        {
            case STATE.PATROLLING:
                displayState = displayPatrolling;
                if (patrolToStation == 1)
                {
                    agent.SetDestination(patrolStation1.transform.position);
                    if (Vector3.Distance(patrolStation1.transform.position, agent.transform.position) < 5)
                    {
                        patrolToStation = 2;
                    }
                }
                else if (patrolToStation == 2)
                {
                    agent.SetDestination(patrolStation2.transform.position);
                    if (Vector3.Distance(patrolStation2.transform.position, agent.transform.position) < 5)
                    {
                        patrolToStation = 3;
                    }
                }
                else if (patrolToStation == 3)
                {
                    agent.SetDestination(patrolStation3.transform.position);
                    if (Vector3.Distance(patrolStation3.transform.position, agent.transform.position) < 5)
                    {
                        patrolToStation = 4;
                    }
                }
                else if (patrolToStation == 4)
                {
                    agent.SetDestination(patrolStation4.transform.position);
                    if (Vector3.Distance(patrolStation4.transform.position, agent.transform.position) < 5)
                    {
                        patrolToStation = 1;
                    }
                }
                break;

            case STATE.CHASING:
                displayState = displayChasing;
                agent.SetDestination(chaseObject.transform.position);
                break;

            case STATE.SEARCHING:
                displayState = displaySearching;
                agent.SetDestination(lastSeen);
                if (Vector3.Distance(lastSeen, agent.transform.position) < 5)
                {
                    if (startSearchOnce)
                    {
                        StartCoroutine(SearchTime());
                        startSearchOnce = false;
                    }
                }
                break;

            case STATE.ATTACKING:
                displayState = displayAttacking;
                break;

            case STATE.RETREATING:
                displayState = displayRetreating;
                agent.SetDestination(lastBeen);
                if (Vector3.Distance(lastBeen, agent.transform.position) < 4)
                {
                    _state = STATE.PATROLLING;
                }
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            lastBeen = this.transform.position;
            _state = STATE.CHASING;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            lastSeen = chaseObject.transform.position;
            _state = STATE.SEARCHING;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Vector3.Distance(chaseObject.transform.position, agent.transform.position) < 3)
            {
                _state = STATE.ATTACKING;
            }
            else 
            {
                _state = STATE.CHASING;
            }
        }
    }
    IEnumerator SearchTime()
    {
        yield return new WaitForSeconds(5);
        _state = STATE.RETREATING;
        startSearchOnce = true;
    }
}

