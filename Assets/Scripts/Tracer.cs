using UnityEngine;

public class Tracer : MonoBehaviour
{
    public float lifeTime = 0.05f;

    private LineRenderer line;

    void Awake()
    {
        line = GetComponent<LineRenderer>();

        if (line != null)
        {
            line.useWorldSpace = true;
        }
    }

    public void Setup(Vector3 start, Vector3 end)
    {
        if (line == null)
            return;

        line.positionCount = 2;

        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}