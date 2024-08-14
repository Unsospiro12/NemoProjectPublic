using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TargetPositionMarker : MonoBehaviour
{
    #region Private Field
    private InputAction mousePos;
    private Vector2 mousePosition;
    private Camera mainCamera;
    private Vector3 followPosition = new Vector3();
    private Vector3 offset = new Vector2(0.1f, 0.3f);
    #endregion
    #region Serilize Field
    [SerializeField] private UserInputManager userInputManager;
    #endregion
    #region MonoBehaviour Callbacks
    private void OnEnable()
    {
        mousePos = userInputManager.MouseDrag;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // 매 프레임마다 마우스 위치를 업데이트
        mousePosition = mousePos.ReadValue<Vector2>();
        Vector2 worldMousePos = mainCamera.ScreenToWorldPoint(mousePosition);
    }
    private void FixedUpdate()
    {
        // 물리 업데이트에서 오브젝트 위치를 갱신
        followPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        followPosition.z = 0f;
        transform.position = followPosition + offset;
    }
    #endregion
    #region Public Methods
    public void InvokeTurnOff()
    {
        Invoke("TurnOff", 0.2f);
    }
    #endregion
    #region Private Methods
    private void TurnOff()
    {
        gameObject.SetActive(false);
    }
    #endregion
}
