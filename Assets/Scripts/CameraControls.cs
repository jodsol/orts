using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraControls : MonoBehaviour
{
    public float keyboardSpeed, dragSpeed, screenEdgeSpeed, screenEdgeBorderSize, mouseRotationSpeed, followMoveSpeed, followRotationSpeed,
        minHeight, maxHeight, zoomSensitivity, zoomSmoothing, mapLimitSmoothing;

    public Vector2 mapLimits, rotationLimits;
    public Vector3 followOffset;

    Transform targetToFollow;

    float zoomAmout = 1, yaw, pitch;
    KeyCode dragKey = KeyCode.Mouse2;
    KeyCode rotationKey = KeyCode.Mouse1;
    private Transform mainTransform;
    LayerMask groundMask;

    // Start is called before the first frame update
    void Start()
    {
        mainTransform = transform;
        groundMask = LayerMask.GetMask("Ground");
        pitch = mainTransform.eulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (!targetToFollow) { Move(); } else { FollowTarget(); }

        Rotation();
        HeightCalculation();
        LimitPosition();

        if (Input.GetKey(KeyCode.Escape)) { ResetTarget(); }
    }
    void Move()
    {
        if(Input.GetKey(dragKey))
        {
            // Click Drag
            Vector3 desiredDragMove = new Vector3(-Input.GetAxis("Mouse X"), 0, -Input.GetAxis("Mouse Y")) * dragSpeed;
            desiredDragMove = Quaternion.Euler(new Vector3(0, mainTransform.eulerAngles.y, 0)) * desiredDragMove * Time.deltaTime;
            desiredDragMove = mainTransform.InverseTransformDirection(desiredDragMove);

            mainTransform.Translate(desiredDragMove, Space.Self);
        }
        else
        {
            //KeyBoard Move
            Vector3 desiredMove = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            desiredMove *= keyboardSpeed;
            desiredMove *= Time.deltaTime;
            desiredMove = Quaternion.Euler(new Vector3(0, mainTransform.eulerAngles.y, 0)) * desiredMove;
            desiredMove = mainTransform.InverseTransformDirection(desiredMove);

            mainTransform.Translate(desiredMove, Space.Self);

            //Screen Edge Move 
            Vector3 desiredEdgeMove = new Vector3();
            Vector3 mousePos = Input.mousePosition;

            Rect leftRect = new Rect(0, 0, screenEdgeBorderSize, Screen.height);
            Rect rightRect = new Rect(Screen.width - screenEdgeBorderSize, 0, screenEdgeBorderSize, Screen.height);
            Rect upRect = new Rect(0, Screen.height, screenEdgeBorderSize, Screen.height);
            Rect downRect = new Rect(0, 0, Screen.width, screenEdgeBorderSize);

            desiredEdgeMove.x = leftRect.Contains(mousePos) ? -1 : rightRect.Contains(mousePos) ? 1 : 0;
            desiredEdgeMove.z = upRect.Contains(mousePos) ? -1 : downRect.Contains(mousePos) ? 1 : 0;

            desiredEdgeMove *= screenEdgeSpeed;
            desiredEdgeMove *= Time.deltaTime;
            desiredEdgeMove = Quaternion.Euler(new Vector3(0, mainTransform.eulerAngles.y, 0)) * desiredEdgeMove;
            desiredEdgeMove = mainTransform.InverseTransformDirection(desiredEdgeMove);

            mainTransform.Translate(desiredEdgeMove, Space.Self);
        }
    }

    void Rotation()
    {
        if (Input.GetKey(rotationKey))
        {
            yaw += mouseRotationSpeed * Input.GetAxis("Mouse X");
            pitch += mouseRotationSpeed * Input.GetAxis("Mouse Y");

            pitch = Mathf.Clamp(pitch, rotationLimits.x, rotationLimits.y);

            mainTransform.eulerAngles = new Vector3(pitch, yaw, 0);
        }

    }

    void LimitPosition()
    {
        mainTransform.position = Vector3.Lerp(mainTransform.position, new Vector3(
            Mathf.Clamp(mainTransform.position.x, -mapLimits.x, mapLimits.x), mainTransform.position.y,
            Mathf.Clamp(mainTransform.position.z, -mapLimits.y, mapLimits.y)), Time.deltaTime * mapLimitSmoothing);
    }

    void HeightCalculation()
    {
        zoomAmout += -Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomSensitivity;
        zoomAmout = Mathf.Clamp01(zoomAmout);

        float distanceToGround = DistanceToGround();
        float targetHeight = Mathf.Lerp(minHeight, maxHeight, zoomAmout);

        mainTransform.position = Vector3.Lerp(mainTransform.position, new Vector3(mainTransform.position.x, targetHeight + distanceToGround, mainTransform.position.z), Time.deltaTime * zoomSmoothing);
    }

    private float DistanceToGround()
    {
        Ray ray = new Ray(mainTransform.position, Vector3.down);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask)) { return hit.point.y; }
        return 0;
    }

    void FollowTarget()
    {
        Vector3 targetPos = new Vector3(targetToFollow.position.x, mainTransform.position.y, targetToFollow.position.z) + followOffset;
        mainTransform.position = Vector3.MoveTowards(mainTransform.position, targetPos, Time.deltaTime * followMoveSpeed);

        if(followRotationSpeed > 0 && !Input.GetKey(rotationKey))
        {
            Vector3 targetDirection = (targetToFollow.position - mainTransform.position).normalized;
            Quaternion targetRotation = Quaternion.Lerp(mainTransform.rotation, Quaternion.LookRotation(targetDirection), followRotationSpeed * Time.deltaTime);
            mainTransform.rotation = targetRotation;

            pitch = mainTransform.eulerAngles.x;
            yaw = mainTransform.eulerAngles.y;
        }
    }

    public void SetTarget(Transform target)
    {
        targetToFollow = target;
    }

    public void ResetTarget()
    {
        targetToFollow = null;
    }
}
