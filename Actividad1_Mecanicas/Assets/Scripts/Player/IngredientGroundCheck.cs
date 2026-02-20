using UnityEngine;

public class IngredientGroundCheck : MonoBehaviour
{
    private IngredientRespawn respawn;
    private Rigidbody rb;

    void Awake()
    {
        respawn = GetComponent<IngredientRespawn>();
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            // Solo si NO está en la mano (o sea, con gravedad)
            if (!rb.isKinematic)
            {
                respawn.TriggerRespawn();
            }
        }
    }
}