using UnityEngine;

public class BodyPickup : MonoBehaviour
{
    public Transform holdPosition;
    public float pickupRange = 2f;

    private GameObject carriedBody;
    private Rigidbody bodyRb;
    private Collider[] bodyColliders;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (carriedBody == null)
            {
                PickUpBody();
            }
            else
            {
                DropBody();
            }
        }
    }

    void PickUpBody()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position, pickupRange
        );

        foreach (Collider hit in hits)
        {
            if (hit.transform.root.CompareTag("DeadBody"))
            {
                carriedBody = hit.transform.root.gameObject;

                bodyRb = carriedBody.GetComponent<Rigidbody>();
                bodyColliders = carriedBody.GetComponentsInChildren<Collider>();

                // Stop physics
                if (bodyRb != null)
                {
                    bodyRb.isKinematic = true;
                    bodyRb.linearVelocity = Vector3.zero;
                }

                // Disable body colliders
                foreach (Collider col in bodyColliders)
                {
                    col.enabled = false;
                }

                // Attach body to player
                carriedBody.transform.SetParent(holdPosition);
                carriedBody.transform.localPosition = Vector3.zero;
                carriedBody.transform.localRotation = Quaternion.identity;

                Debug.Log("Body picked up!");
                return;
            }
        }

        Debug.Log("No dead body nearby!");
    }

    void DropBody()
    {
        carriedBody.transform.SetParent(null);

        if (bodyRb != null)
        {
            bodyRb.isKinematic = false;
            bodyRb.useGravity = true;
        }

        foreach (Collider col in bodyColliders)
        {
            col.enabled = true;
        }

        carriedBody = null;
        bodyRb = null;
        bodyColliders = null;

        Debug.Log("Body dropped!");
    }
}
