using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CheckPlayerAround : MonoBehaviour
{
    public float sonarRange = 15f;
    public TextMeshProUGUI humanCountText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sonarPulse();
    }

    public void sonarPulse()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, sonarRange);
        int count = colliders.Count();
        count = count - 2;

        foreach (Collider collider in colliders)
        {

            //Debug.Log("SonarTarget found: " + collider.name);

            if (collider.CompareTag("Human"))
            {
                //Debug.Log("Human around: " + count);
                humanCountText.text = count.ToString();

                Renderer[] renderers = collider.GetComponentsInChildren<Renderer>();

                foreach (Renderer rend in renderers)
                {
                    rend.enabled = true;
                }
            }
            else
            {
                humanCountText.text = count.ToString();
            }
        }
    }
}
