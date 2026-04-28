using UnityEngine;

public class Civilian : MonoBehaviour
{
    public bool isCaptured = false;

    public void Capture(Transform holder)
    {
        isCaptured = true;

        // Attach civilian to enemy (like being held)
        transform.SetParent(holder);
        transform.localPosition = new Vector3(0, 1.5f, 0); // adjust height
    }

    public void Release()
    {
        isCaptured = false;
        transform.SetParent(null);
    }

    public void Die()
    {
        Debug.Log("Civilian killed!");

        // Trigger psychosis later here
        Destroy(gameObject);
    }
}