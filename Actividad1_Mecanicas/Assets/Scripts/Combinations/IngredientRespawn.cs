using System.Collections;
using UnityEngine;

public class IngredientRespawn : MonoBehaviour
{
    public float respawnTime = 3f;
    public Transform respawnPoint; // 👈 único por ingrediente

    private bool respawnRunning = false;
    public bool canRespawn = true;

    private Rigidbody rb;
    private Collider col;
    private Renderer rend;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        rend = GetComponent<Renderer>();
    }

    public void TriggerRespawn()
    {
        if (!canRespawn || respawnRunning) return;
        StartCoroutine(RespawnCoroutine());
    }

    IEnumerator RespawnCoroutine()
    {
        respawnRunning = true;

        yield return new WaitForSeconds(respawnTime);

        // Congelar física antes de mover
        rb.isKinematic = true;
        rb.useGravity = false;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Teleport seguro
        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;

        col.enabled = true;
        rend.enabled = true;

        // Esperar 1 frame para evitar glitches
        yield return null;

        rb.isKinematic = false;
        rb.useGravity = true;

        respawnRunning = false;
    }
}