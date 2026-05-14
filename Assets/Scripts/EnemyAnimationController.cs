using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimationController : MonoBehaviour
{
    public float sprintSpeedThreshold = 2.5f;
    public float damping = 0.15f;

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
        if (robot != null && robot.GetHealthPercent() <= 0 && !isDead)
        {
            isDead = true;
            if (agent != null) agent.enabled = false;
            return;
        }

        if (isDead) return;
        if (agent == null) return;

        float speed = agent.velocity.magnitude;

        // If agent is stopped, snap to idle
        if (agent.isStopped || speed < 0.1f)
        {
            anim.SetFloat("Speed", 0f, damping, Time.deltaTime);
            anim.SetFloat("MoveX", 0f, damping, Time.deltaTime);
            anim.SetFloat("MoveY", 0f, damping, Time.deltaTime);
            anim.SetBool("IsSprinting", false);
            return;
        }

        // Compute local velocity for direction
        Vector3 localVelocity = transform.InverseTransformDirection(agent.velocity);
        localVelocity = localVelocity.normalized;

        // Snap small strafe values so blend tree picks forward cleanly
        if (Mathf.Abs(localVelocity.x) < 0.5f)
        {
            localVelocity.x = 0f;
        }

        // Force MoveY to positive — enemies should never play "walking backward"
        if (localVelocity.z < 0f)
        {
            localVelocity.z = Mathf.Abs(localVelocity.z);
        }

        anim.SetFloat("Speed", speed, damping, Time.deltaTime);
        anim.SetFloat("MoveX", localVelocity.x, damping, Time.deltaTime);
        anim.SetFloat("MoveY", localVelocity.z, damping, Time.deltaTime);
        anim.SetBool("IsSprinting", speed > sprintSpeedThreshold);
    }
}
