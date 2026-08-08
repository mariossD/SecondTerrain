using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class zombieBoss : MonoBehaviour
{
    Animator anim;
    public GameObject target;

    NavMeshAgent agent;

    enum STATE { IDLE, WANDER, CHASE, ATTACK, DEAD }
    STATE currentState = STATE.IDLE;


    void Start()
    {
        anim = this.GetComponent<Animator>();
        agent = this.GetComponent<NavMeshAgent>();

        anim.SetBool("isZombieWalking", true);
    }

    void TunrOffTriggers()
    {
        anim.SetBool("isZombieWalking", false);
        anim.SetBool("isZombieRunning", false);
        anim.SetBool("isZombieAttacking", false);
        anim.SetBool("isZombieDead", false);
    }

    void Update()
    {
        agent.SetDestination(target.transform.position);

        if (agent.remainingDistance < agent.stoppingDistance)
        {
            anim.SetBool("isZombieWalking", false);
            anim.SetBool("isZombieAttacking", true);
        }
        else
        {
            anim.SetBool("isZombieWalking", true);
            anim.SetBool("isZombieAttacking", false);
        }
        // if(Input.GetKey(KeyCode.W))
        // {
        //     anim.SetBool("isZombieWalking",true);
        // }else{
        //       anim.SetBool("isZombieWalking",false);
        // }

        //  if(Input.GetMouseButtonDown(0))
        // {
        //     anim.SetBool("isZombieAttacking",true);
        // }else{
        //       anim.SetBool("isZombieAttacking",false);
        // }

        //  if(Input.GetKey(KeyCode.D))
        // {
        //     anim.SetBool("isZombieDead",true);
        // }
    }
}
