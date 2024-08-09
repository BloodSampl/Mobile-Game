using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class BallHandler : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivot;
    [SerializeField] private float delayTime;
    [SerializeField] private float respawnDelay;

    private Rigidbody2D currentBallRigidbody;
    private SpringJoint2D currentBallSpringJoint;
    private Camera mainCamera;

    private bool isDragging;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        SpawnNewBall();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBallRigidbody == null) return;

        if (IsPointerOverUIObject()) return;

        Vector2 pointerPosition;
        bool isPressed;
        
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            pointerPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            isPressed = Touchscreen.current.primaryTouch.press.isPressed;
        }
        else if (Mouse.current != null)
        {
            pointerPosition = Mouse.current.position.ReadValue();
            isPressed = Mouse.current.leftButton.isPressed;
        }
        else
        {
            return;
        }

        if (!isPressed)
        {
            if (isDragging)
            {
                LaunchBall();
            }

            isDragging = false;
            return;
        }

        isDragging = true;
        currentBallRigidbody.isKinematic = true;

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(pointerPosition);
        worldPosition.z = 0;

        currentBallRigidbody.position = worldPosition;
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);

        // Use the correct input method for UI check
        if (Touchscreen.current != null)
        {
            eventData.position = Touchscreen.current.primaryTouch.position.ReadValue();
        }
        else if (Mouse.current != null)
        {
            eventData.position = Mouse.current.position.ReadValue();
        }
        else
        {
            return false;
        }

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }

    private void SpawnNewBall()
    {
        GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity);

        currentBallRigidbody = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();

        currentBallSpringJoint.connectedBody = pivot;
    }

    private void LaunchBall()
    {
        currentBallRigidbody.isKinematic = false;
        currentBallRigidbody = null;

        Invoke(nameof(DetachBall), delayTime);
    }

    private void DetachBall()
    {
        currentBallSpringJoint.enabled = false;
        currentBallSpringJoint = null;

        Invoke(nameof(SpawnNewBall), respawnDelay);
    }
}
