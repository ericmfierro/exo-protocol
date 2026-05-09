using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimationController : MonoBehaviour
{
    public float sprintSpeed = 3f;

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
        if (anim == null) return;

        // Check if dead
        if (robot != null && robot.GetCurrentHealth() <= 0 && !isDead)
        {
            isDead = true;
            anim.SetBool("IsDead", true);
            anim.SetInteger("DeathType", 0);

            if (agent != null)
            {
                agent.enabled = false;
            }
            return;
        }

        if (isDead) return;

        // Update movement parameters
        if (agent != null)
        {
            float speed = agent.velocity.magnitude;
            Vector3 localVelocity = transform.InverseTransformDirection(agent.velocity);

            anim.SetFloat("Speed", speed);
            anim.SetFloat("MoveX", localVelocity.x);
            anim.SetFloat("MoveY", localVelocity.z);

            if (speed > sprintSpeed)
            {
                anim.SetBool("IsSprinting", true);
            }
            else
            {
                anim.SetBool("IsSprinting", false);
            }
        }
    }
}
