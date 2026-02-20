using System.Collections;
using UnityEngine;

public class IngredientRespawn : MonoBehaviour
{
    public float respawnTime = 3f;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool respawnRunning = false;

    public bool canRespawn = true;   

    private Rigidbody rb;
    private Collider col;
    private Renderer rend;

    void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        rend = GetComponent<Renderer>();
    }

    public void TriggerRespawn()
    {
        if (!canRespawn) return;
        if (respawnRunning) return;

        StartCoroutine(RespawnCoroutine());
    }

    IEnumerator RespawnCoroutine()
    {
        respawnRunning = true;

        yield return new WaitForSeconds(respawnTime);

        // Reset total
        transform.position = startPosition;
        transform.rotation = startRotation;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.isKinematic = false;
        rb.useGravity = true;

        col.enabled = true;
        rend.enabled = true;

        respawnRunning = false;
    }
}