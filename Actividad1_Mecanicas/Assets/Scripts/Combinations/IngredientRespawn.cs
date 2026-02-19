using System.Collections;
using UnityEngine;

public class IngredientRespawn : MonoBehaviour
{
    [Header("Respawn Settings")]
    public float respawnTime = 10f;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private bool respawnStarted = false;

    void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

   
    public void StartRespawnCountdown()
    {
        if (respawnStarted) return;

        respawnStarted = true;
        StartCoroutine(RespawnCoroutine());
    }

    IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnTime);

        GameObject newIngredient = Instantiate(gameObject, startPosition, startRotation);

        
        IngredientRespawn newRespawn = newIngredient.GetComponent<IngredientRespawn>();
        if (newRespawn != null)
        {
            newRespawn.respawnStarted = false;
        }
    }
}
