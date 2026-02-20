using UnityEngine;

public class ObjectPickup : MonoBehaviour
{
    public float pickupDistance = 3f;
    public float moveForce = 150f;
    public float damping = 10f;

    [Header("Scroll Settings")]
    public float scrollSpeed = 2f;
    public float minHoldDistance = 1f;
    public float maxHoldDistance = 4f;


    private Camera playerCamera;
    private Rigidbody heldObject;
    private Transform holdPoint;
    private IngredientRespawn heldRespawn;
    private float currentHoldDistance;

    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        holdPoint = playerCamera.transform.Find("HoldPoint");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TryPickup();

        if (Input.GetMouseButtonUp(0))
            DropObject();

        HandleScroll();
    }

    void FixedUpdate()
    {
        if (heldObject != null)
            MoveObject();
    }

    void TryPickup()
    {
        if (heldObject != null) return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, pickupDistance))
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb == null) return;

            heldObject = rb;

            heldRespawn = rb.GetComponent<IngredientRespawn>();
            if (heldRespawn != null)
                heldRespawn.canRespawn = false;

            rb.isKinematic = false;
            rb.useGravity = false;
            rb.drag = damping;
            rb.angularDrag = damping;
        }
    }

    void MoveObject()
    {
        Vector3 dir = holdPoint.position - heldObject.position;
        heldObject.AddForce(dir * moveForce, ForceMode.Force);
    }

    void DropObject()
    {
        if (heldObject == null) return;

        heldObject.useGravity = true;
        heldObject.drag = 0;
        heldObject.angularDrag = 0.05f;

       
        if (heldRespawn != null)
            heldRespawn.canRespawn = true;

        heldObject = null;
        heldRespawn = null;
    }

    void HandleScroll()
    {
        if (heldObject == null) return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) < 0.01f) return;

        currentHoldDistance += scroll * scrollSpeed;
        currentHoldDistance = Mathf.Clamp(currentHoldDistance, minHoldDistance, maxHoldDistance);

        holdPoint.localPosition = new Vector3(
            holdPoint.localPosition.x,
            holdPoint.localPosition.y,
            currentHoldDistance
        );
    }
}