using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimationController : MonoBehaviour
{
    Animator anim;
    NavMeshAgent agent;
    Robot robot;

    bool isDead = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        robot = GetComponent<Robot>();
    }

    void Update()
    {
        if (anim == null)
            return;

        // Death check
        if (robot != null &&
            robot.GetHealthPercent() <= 0f &&
            !isDead)
        {
            isDead = true;

            anim.SetBool("IsDead", true);

            if (agent != null &&
                agent.enabled)
            {
                agent.isStopped = true;
                agent.ResetPath();
                agent.enabled = false;
            }

            return;
        }

        if (isDead)
            return;

        if (agent == null ||
            !agent.enabled)
        {
            return;
        }

        float speed = agent.velocity.magnitude;

        anim.SetFloat(
            "Speed",
            speed,
            0.15f,
            Time.deltaTime
        );
    }
}