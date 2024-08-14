using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class CameraControl : MonoBehaviour
{
    // 유저 인풋 매니저에 합치기
    public CinemachineVirtualCamera virtualCamera;
    public float edgeThreshold = 10.0f;
    public float moveSpeed = 0;
    public float zoomSpeed = 0.006f;
    public float minSize; 
    public float maxSize;

    private Vector2 moveInput;

    [Header("Camera Control")]
    public UserInputManager inputManager;
    public InputAction mouseDrag;
    public InputAction cameraLock;
    public InputAction cameraReset;
    public InputAction cameraZoom;
    public InputAction cameraMove;
    public bool isLock;

    //헤더
    [Header("Camera Position")]
    public float minX; // 카메라 x 위치의 최소값
    public float maxX;  // 카메라 x 위치의 최대값
    public float minY; // 카메라 y 위치의 최소값
    public float maxY;  // 카메라 y 위치의 최대값
    public float baseSize = 8.1f;

    private void Start()
    {
        inputManager = GetComponent<UserInputManager>();
        mouseDrag = inputManager.MouseDrag;
        cameraLock = inputManager.CameraLock;
        cameraReset = inputManager.CameraReset;
        cameraZoom = inputManager.MouseWheel;
        cameraMove = inputManager.CameraMove;

        isLock = false;

        cameraLock.performed += LockCamera;
        cameraReset.performed += ResetCamera;
        cameraZoom.performed += ZoomCamera;
        cameraMove.performed += OnMoveCameraByKey;
        cameraMove.canceled += OnMoveCameraByKey;
    }

    void FixedUpdate()
    {
        moveSpeed = DataManager.Instance.CameraMovementSpeed;
        if (!isLock)
        {
            MoveCamera();
            MoveCameraByKey();
        }
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        cameraLock.performed -= LockCamera;
        cameraReset.performed -= ResetCamera;
        cameraZoom.performed -= ZoomCamera;
    }

    // 카메라 이동 제한
    public void LockCamera(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isLock = !isLock;
        }
    }

    // 카메라 위치 초기화
    public void ResetCamera(InputAction.CallbackContext context)
    {
        Vector3 basePos = new Vector3(0, 0, virtualCamera.transform.position.z);
        virtualCamera.transform.position = basePos;
    }

    
    // 스크린 크기와 마우스 위치를 체크해서 스크린 가장자리에 있다면 카메라를 이동시킨다
    // edgeThreshold : 마우스가 화면 가장자리에 있다고 인식하는 거리
    private void MoveCamera()
    {
        // 카메라 락 되어 있다면 리턴
        if (isLock) return;

        Vector3 moveDirection = Vector3.zero; ;

        Vector2 mousePos = mouseDrag.ReadValue<Vector2>();

        if (mousePos.x >= Screen.width - edgeThreshold)
        {
            moveDirection.x += 1;
        }
        if (mousePos.x <= edgeThreshold)
        {
            moveDirection.x -= 1;
        }
        if (mousePos.y >= Screen.height - edgeThreshold)
        {
            moveDirection.y += 1;
        }
        if (mousePos.y <= edgeThreshold)
        {
            moveDirection.y -= 1;
        }

        // Cinemachine 카메라의 위치를 조정
        Vector3 newPosition = virtualCamera.transform.position + moveDirection * moveSpeed * Time.deltaTime;

        // 카메라의 x와 y 위치가 특정 범위 내에서만 변하도록 제한
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        virtualCamera.transform.position = newPosition;
    }

    private void OnMoveCameraByKey(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCameraByKeyCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    private void MoveCameraByKey()
    {
        if (isLock) return;

        Vector3 moveDirection = new Vector3(moveInput.x, moveInput.y, 0);
        Vector3 newPosition = virtualCamera.transform.position + moveDirection * moveSpeed * Time.deltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        virtualCamera.transform.position = newPosition;
    }

    // 마우스 휠 입력을 사용하여 줌 조정
    // 스크롤 값이 120과 -120만 받아오기 때문에 zoomSpeed를 곱하여 낮춘다.
    private void ZoomCamera(InputAction.CallbackContext context)
    {
        if (UIManager.Instance.isZoomLocked)
        {
            return;
        }
        float zoom = context.ReadValue<float>();
        virtualCamera.m_Lens.OrthographicSize -= zoom * zoomSpeed;
        virtualCamera.m_Lens.OrthographicSize = Mathf.Clamp(virtualCamera.m_Lens.OrthographicSize, minSize, maxSize);
    }

    public void SetInitialCameraPos()
    {
        Vector3 basePos = new Vector3(0, 0, virtualCamera.transform.position.z);
        virtualCamera.transform.position = basePos;
        virtualCamera.m_Lens.OrthographicSize = baseSize;
        isLock = true;
}
}