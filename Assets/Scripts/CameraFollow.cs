using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float offsetDistance = 5f;
    public float deadZoneRadius = 1.5f;
    public float offsetSmoothSpeed = 8f;
    public float cameraSmoothSpeed = 5f;
    [SerializeField] private LayerMask cameraTargetLayer;

    [SerializeField] private Vector2 currentOffset;

    private void Update()
    {
       // Ray raydb = Camera.main.ScreenPointToRay(Input.mousePosition);
       // Debug.DrawRay(raydb.origin, raydb.direction * 100f, Color.cyan);
    }
    void LateUpdate()
    {
        if (player == null) return;

        Vector2 targetOffset = Vector2.zero;

        // Raycast from mouse to ground
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //if (Physics.Raycast(ray, out RaycastHit hit, 100f, cameraTargetLayer))
        //{

        // Direction from player to mouse hit point
       // Vector2 toMouse = hit.point - player.position;
       //toMouse.y = 0f; // flatten on ground plane

       // float distance = toMouse.magnitude; // the length of how far the point is.

      //  if (distance > deadZoneRadius)//camera
        //{

        //   // Vector2 direction = toMouse.normalized;
        //   // float adjustedDistance = Mathf.Min(offsetDistance, distance - deadZoneRadius);
        //    targetOffset = direction * adjustedDistance;

        //    // Only smooth if mouse is outside the dead zone
        //    currentOffset = Vector2.Lerp(currentOffset, targetOffset, Time.deltaTime * offsetSmoothSpeed);
        //}
        //else
        //{
        //    // Snap to zero inside dead zone (prevents jitter)
        //    currentOffset = Vector2.zero;
        //}

    //}
  

        // Final camera position based on player + offset
        Vector2 targetPosition = new Vector2(player.position.x, player.position.y) + currentOffset;
        targetPosition.y = transform.position.y; // keep camera height fixed

        transform.position = Vector2.Lerp(transform.position, targetPosition, Time.deltaTime * cameraSmoothSpeed);
    }
}
