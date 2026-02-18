using UnityEngine;

public class ObjectPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public float pickupDistance = 3f;
    public float moveForce = 150f;
    public float damping = 10f;

    private Camera playerCamera;
    private Rigidbody heldObject;
    private Transform holdPoint;

    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        holdPoint = playerCamera.transform.Find("HoldPoint");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryPickup();
            Debug.Log("Estoy cogiendo el objeto");
        }

        if (Input.GetMouseButtonUp(0))
        {
            DropObject();
            Debug.Log("Estoy dejando el objeto");
        }
    }

    void FixedUpdate()
    {
        if (heldObject != null)
        {
            MoveObject();
        }
    }

    void TryPickup()
    {
        if (heldObject != null) return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupDistance))
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();

            if (rb != null)
            {
                heldObject = rb;
                heldObject.useGravity = false;
                heldObject.drag = damping;
                heldObject.angularDrag = damping;
            }
        }
    }

    void MoveObject()
    {
        Vector3 direction = holdPoint.position - heldObject.position;
        heldObject.AddForce(direction * moveForce, ForceMode.Force);
    }

    void DropObject()
    {
        if (heldObject == null) return;

        heldObject.useGravity = true;
        heldObject.drag = 0;
        heldObject.angularDrag = 0.05f;
        heldObject = null;
    }
}
