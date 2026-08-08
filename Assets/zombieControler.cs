using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class zombieControler : MonoBehaviour
{
    Animator anim;
    public GameObject target;
    public AudioSource splash;
    NavMeshAgent agent;
    public float damageAmount = 5;
    public float walkSpeed;
    public float runSpeed;

    enum STATE { IDLE, WANDER, CHASE, ATTACK, DEAD }
    STATE currentState = STATE.IDLE;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
        anim.SetBool("isZombieWalking", true);
    }

    void TurnOffTriggers()
    {
        anim.SetBool("isZombieWalking", false);
        anim.SetBool("isZombieRunning", false);
        anim.SetBool("isZombieAttacking", false);
        anim.SetBool("isZombieDead", false);
    }

    float DistanceFromPlayer()
    {
        if (GameStats.gameOver) return Mathf.Infinity;

        return Vector3.Distance(this.transform.position, target.transform.position);
    }

    bool IsTargetInChaseRange()
    {
        if (DistanceFromPlayer() < 10)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool ForgetPlayer()
    {
        if (DistanceFromPlayer() > 20)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DamagePlayer()
    {
        if (target != null && GameStats.gameOver == false)
        {
            target.GetComponent<MainCharacterController>().TakeHit(damageAmount);
            PlaySplat();
        }
    }


    public void DeadZombie()
    {
        TurnOffTriggers();
        anim.SetBool("isZombieDead", true);
        currentState = STATE.DEAD;
    }

    void PlaySplat()
    {
        splash.Play();
    }

    void Update()
    {
        if (target == null && GameStats.gameOver == false)
        {
            target = GameObject.FindGameObjectWithTag("Player");
            return;
        }

        switch (currentState)
        {
            case STATE.IDLE:
                if (IsTargetInChaseRange())
                {
                    currentState = STATE.CHASE;
                }
                else if (Random.Range(0, 5000) < 5)
                {
                    currentState = STATE.WANDER;
                }
                break;
            case STATE.WANDER:
                if (!agent.hasPath)
                {
                    float nexW = this.transform.position.x + Random.Range(-5, 5);
                    float nextZ = this.transform.position.z + Random.Range(-5, 5);
                    float nextY = Terrain.activeTerrain.SampleHeight(new Vector3(nexW, 0, nextZ));
                    Vector3 nextDestination = new Vector3(nexW, nextY, nextZ);
                    agent.SetDestination(nextDestination);
                    agent.stoppingDistance = 0;
                    TurnOffTriggers();
                    agent.speed = walkSpeed;
                    anim.SetBool("isZombieWalking", true);
                }


                if (IsTargetInChaseRange())
                {
                    currentState = STATE.CHASE;
                }
                else if (Random.Range(0, 5000) < 5)
                {
                    TurnOffTriggers();
                    currentState = STATE.IDLE;
                    agent.ResetPath();
                }
                break;
            case STATE.CHASE:
                if (GameStats.gameOver)
                {
                    TurnOffTriggers();
                    currentState = STATE.WANDER;
                    return;
                }
                agent.SetDestination(target.transform.position);
                agent.stoppingDistance = 3;
                TurnOffTriggers();
                agent.speed = runSpeed;
                anim.SetBool("isZombieRunning", true);

                if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                {
                    currentState = STATE.ATTACK;
                }

                if (ForgetPlayer())
                {
                    currentState = STATE.WANDER;
                    agent.ResetPath();
                }
                break;
            case STATE.ATTACK:
                if (GameStats.gameOver)
                {
                    TurnOffTriggers();
                    currentState = STATE.WANDER;
                    break;
                }
                TurnOffTriggers();
                anim.SetBool("isZombieAttacking", true);
                this.transform.LookAt(target.transform.position);
                if (DistanceFromPlayer() > agent.stoppingDistance + 2)
                {
                    currentState = STATE.CHASE;
                }
                break;
            case STATE.DEAD:
                Destroy(agent);
                this.GetComponent<Sink>().StartSink();
                break;
        }
    }
}
