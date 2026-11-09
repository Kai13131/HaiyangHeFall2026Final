using UnityEngine;

public class HumanNPC : MonoBehaviour
{
    public bool isAlive = true;

    public void Die()
    {
        if (!isAlive)
        {
            return;
        }
        isAlive = false;

        Rigidbody humanRb = GetComponent<Rigidbody>();

        if (humanRb != null)
        {
            humanRb.constraints = RigidbodyConstraints.None;
            humanRb.linearVelocity = Vector3.zero;
            humanRb.angularVelocity = Vector3.zero;
            humanRb.isKinematic = false;
        }

        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {
            if(script != this)
            {
                script.enabled = false;
            }
        }
        gameObject.tag = "DeadBody";
    }
}
