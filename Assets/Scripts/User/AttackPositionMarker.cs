using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackPositionMarker : MonoBehaviour
{
    #region Private Field
    private InputAction mousePos;
    private Vector2 mousePosition;
    private Vector3 followPosition = new Vector3();
    private Color baseColor = Color.white;
    private Color onEnemyColor = Color.red;
    private int enemyInt = 1 << 6;
    private Camera mainCamera;
    private Vector3 offset = new Vector2(0.1f, 0.3f);
    #endregion

    #region Serilize Field
    [SerializeField] private UserInputManager userInputManager;
    [SerializeField] private SpriteRenderer spriteRenderer;
    #endregion

    #region MonoBehaviour Callbacks
    private void OnEnable()
    {
        mousePos = userInputManager.MouseDrag;
        spriteRenderer.color = baseColor;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // 매 프레임마다 마우스 위치를 업데이트
        mousePosition = mousePos.ReadValue<Vector2>();
        Vector2 worldMousePos = mainCamera.ScreenToWorldPoint(mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldMousePos, Vector2.zero, float.MaxValue, enemyInt);

        if (hit.collider != null)
        {
            spriteRenderer.color = onEnemyColor;
        }
        else
        {
            spriteRenderer.color = baseColor;
        }
    }

    private void FixedUpdate()
    {
        // 물리 업데이트에서 오브젝트 위치를 갱신
        followPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        followPosition.z = 0f;
        transform.position = followPosition + offset;
    }
    private void OnDisable()
    {
        spriteRenderer.color = baseColor;
    }
    #endregion
}
