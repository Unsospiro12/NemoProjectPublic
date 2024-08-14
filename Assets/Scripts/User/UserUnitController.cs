using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// 유닛 컨트롤의 전반을 다루는 클래스
/// 유닛 선택, 이동, 공격 등을 명령하는 기능을 담당
/// </summary>
public class UserUnitController : MonoBehaviour
{
    #region Private Field
    private InputAction mouseLeftClick;
    private InputAction mouseRightClick;
    private InputAction mouseDrag;
    private InputAction hold;
    private InputAction readyToAttack;
    private InputAction mouseDoubleClick;
    private InputAction readyToMove;

    private bool isMouseLeftDown, isDragging, isReadyToAttack, isReadyToMove;
    private Vector2 MouseStartPos;
    private Camera mainCamera;
    private List<UserUnit> selectedUserUnits;

    private int userInt = 1 << 7;
    private int enemyInt = 1 << 6;
    #endregion
    #region Serilize Field
    [SerializeField] private SelectionArea selectArea;
    [SerializeField] private UserInputManager userInputManager;
    [SerializeField] private TargetPositionMarker targetPositionMarker;
    [SerializeField] private AttackPositionMarker attackPositionMarker;

    [SerializeField] private Button unitAttackButton;
    [SerializeField] private Button unitMoveButton;
    [SerializeField] private Button unitHoldButton;
    #endregion
    #region Public Properties
    public List<GameObject> SelectEnemy;
    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;
    #endregion
    #region MonoBehaviour Callbacks
    private void Awake()
    {
        selectedUserUnits = new List<UserUnit>();
        SelectEnemy = new List<GameObject>();
        mainCamera = Camera.main;
        ActiveUnitControlUI(false);
    }
    private void Start()
    {
        mouseLeftClick = userInputManager.MouseLeftClick;
        mouseRightClick = userInputManager.MouseRightClick;
        mouseDrag = userInputManager.MouseDrag;
        hold = userInputManager.Hold;
        readyToAttack = userInputManager.ReadyToAttack;
        mouseDoubleClick = userInputManager.MouseDoubleClick;
        readyToMove = userInputManager.ReadyToMove;

        mouseDoubleClick.performed += OnMouseDoubleClick;
        mouseLeftClick.performed += OnMouseLeftClick;
        mouseDrag.performed += OnMouseDragging;
        mouseLeftClick.canceled += OnMouseLeftUp;
        mouseRightClick.performed += OnMouseRightClick;
        hold.performed += OnHold;
        readyToAttack.performed += OnReadyToAttack;
        readyToMove.performed += OnReadyToMove;
    }
    #endregion
    #region Public Methods
    #region Input Callbacks
    /// <summary>
    /// 좌클릭시 동작
    /// 유닛을 선택하거나,
    /// A를 누른채로 좌클릭시 그 자리로 공격을 실행
    /// </summary>
    /// <param name="context"></param>
    public void OnMouseLeftClick(InputAction.CallbackContext context)
    {
        MouseStartPos = mouseDrag.ReadValue<Vector2>();
        Vector2 worldMousePos = mainCamera.ScreenToWorldPoint(MouseStartPos);

        // 클릭한 부분이 UI인지 먼저 확인한 후 UI 라면 유닛정보를 삭제하지 않고 가지고 있는다
        Vector2 screenMousePos = Camera.main.WorldToScreenPoint(worldMousePos);

        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = screenMousePos;

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerEventData, results);

        bool isUI = false;
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.layer == Constants.Layers.UI)
            {
                isUI = true;
                break;
            }
        }

        // 이동 버튼으로 이동할 경우
        if (isReadyToMove)
        {
            MoveToPosition(worldMousePos);
            ClearReadyForActions();
            return;
        }

        // 클릭한 부분이 UI가 아니라면 가지고 있던 유닛정보를 삭제하고 이후 로직을 진행한다.
        if (!isUI)
        {
            if (isReadyToAttack)
            {
                HandleAttackClick(worldMousePos);
            }
            else
            {
                HandleSelectionClick(worldMousePos);
            }
            ClearReadyForActions();
        }
    }
    /// <summary>
    /// 더블 클릭시 위치에 유닛이 있으면 처음 클릭한 유닛과 같은 유닛들만 단체로 선택하게 해줍니다.
    /// </summary>
    /// <param name="context"></param>
    public void OnMouseDoubleClick(InputAction.CallbackContext context)
    {
        MouseStartPos = mouseDrag.ReadValue<Vector2>();
        Vector2 worldMousePos = mainCamera.ScreenToWorldPoint(MouseStartPos);

        RemoveAllSelectedUnit();
        RemoveAllSelectEnemy();

        RaycastHit2D hit = Physics2D.Raycast(worldMousePos, Vector2.zero, float.MaxValue, userInt);
        if (hit.collider != null)
        {
            UserUnit selectedUnit = hit.collider.GetComponent<UserUnit>();
            if (selectedUnit != null)
            {
                int id = selectedUnit.Stat.ID;

                Collider2D[] colliders = Physics2D.OverlapCircleAll(worldMousePos, 3);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].CompareTag("Player"))
                    {
                        UserUnit unitInRange = colliders[i].GetComponent<UserUnit>();
                        if (unitInRange.Stat.ID == id)
                        {
                            AddSelectedUnit(unitInRange);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 유닛을 선택한 상태로 마우스 우클릭시 유닛을 그 위치로 이동시키는 기능
    /// </summary>
    /// <param name="context"></param>
    public void OnMouseRightClick(InputAction.CallbackContext context)
    {
        MouseStartPos = mouseDrag.ReadValue<Vector2>();
        Vector2 worldMousePos = mainCamera.ScreenToWorldPoint(MouseStartPos);
        MoveToPosition(worldMousePos);
        ClearReadyForActions();
    }
    /// <summary>
    /// 마우스를 드래그할 때 시작시 드래그 할 것을 세팅하고
    /// 계속 진행하면 유닛 다중 선택 상자의 크기를 조절하여 유닛을 다중 선택할 수 있게 해준다.
    /// 물론 좌클릭일 때만 작동한다.
    /// </summary>
    /// <param name="context"></param>
    public void OnMouseDragging(InputAction.CallbackContext context)
    {
        if (isMouseLeftDown)
        {
            Vector2 currentMousePos = context.ReadValue<Vector2>();
            selectArea.gameObject.SetActive(true);
            selectArea.IsSelecting = true;
            Vector2 worldStartPos = mainCamera.ScreenToWorldPoint(MouseStartPos);
            Vector2 worldCurrentPos = mainCamera.ScreenToWorldPoint(currentMousePos);
            selectArea.SetSize(worldStartPos, worldCurrentPos);
        }
    }
    /// <summary>
    /// 마우스 좌클릭에서 손을 땠을 경우
    /// 드래그 중이던 드래그 범위 상자를 없애고 새로운 다중 선택을 준비한다.
    /// </summary>
    /// <param name="context"></param>
    public void OnMouseLeftUp(InputAction.CallbackContext context)
    {
        isMouseLeftDown = false;
        isDragging = false;
        selectArea.IsSelecting = false;

        selectArea.SetSize(Vector2.zero, Vector2.zero);
        selectArea.gameObject.SetActive(false);
    }
    /// <summary>
    /// H 키 클릭시 선택된 유닛들은 정지 상태가 된다.
    /// </summary>
    /// <param name="context"></param>
    public void OnHold(InputAction.CallbackContext context)
    {
        unitHoldButton.Select();
        OnClickHold();
    }
    /// <summary>
    /// A 키를 누르면 공격 표시 마커가 뜨면서 좌클릭을 하면 해당 지점으로 어택땅을 하거나 적을 누르면 추격한다.
    /// 이 때 선택한 유닛이 공격 가능해야한다.
    /// </summary>
    /// <param name="context"></param>
    public void OnReadyToAttack(InputAction.CallbackContext context)
    {
        OnClickReadyToAttack();
    }
    public void OnReadyToMove(InputAction.CallbackContext context)
    {
        OnClickReadyToMove();
    }
    #endregion
    #region Normal Methods
    // 선택한 유닛을 GameObject리스트에 넣는 작업
    // 선택한 유닛의 이름을 UI로 넘겨줌
    public void AddSelectedUnit(UserUnit newUnit)
    {
        if (!selectedUserUnits.Contains(newUnit))
        {
            newUnit.SelectThis();
            selectedUserUnits.Add(newUnit);
        }
        if (selectedUserUnits.Count > 0)
        {
            ActiveUnitControlUI(true);
        }
        CheckUnitCount();
    }
    public void AddSelectEnemy(GameObject newUnit)
    {
        newUnit.GetComponent<ISelectable>().SelectThis();
        SelectEnemy.Add(newUnit);
        EnemyStatUI();
    }
    // 선택한 유닛의 개수에따라 유닛정보를 표현하는 방법을 다르게 넘김
    void CheckUnitCount()
    {
        if (selectedUserUnits.Count == 1)
        {
            //유닛의 스텟 전체 출력
            SingleUnit();
        }
        else
        {
            //아이콘만 출력
            StatUI.Instance.SetToggles(selectedUserUnits.Count);
            StatUI.Instance.SetUnitIcon(selectedUserUnits);
        }
    }
    //단일 유닛 선택시 호출되는 함수
    void SingleUnit()
    {
        UserUnitStat stat = selectedUserUnits[0].Stat;
        int count = selectedUserUnits.Count;
        StatUI.Instance.UserUnitStatSetting(stat, count);
    }
    //적 유닛 선택시 적 유닛의 데이터를 넘겨줌
    void EnemyStatUI()
    {
        if (SelectEnemy.Count != 0)
        {
            EnemyStat stat = SelectEnemy[0].GetComponent<EnemyStat>();
            int count = selectedUserUnits.Count;
            StatUI.Instance.EnemyStatSetting(stat, count);
        }

    }
    // 선택해제한 유닛을 GameObject리스트에서 삭제하는 작업
    //선택해제한 유닛의 이름을 UI에서 제거하는 작업
    public void RemoveSelectedUnit(UserUnit newUnit)
    {
        selectedUserUnits.Remove(newUnit);
        newUnit.DeSelectThis();

        if (selectedUserUnits.Count <= 0)
        {
            ActiveUnitControlUI(false);
        }
    }
    public void RemoveSelectedEnemy(GameObject newUnit)
    {
        newUnit.GetComponent<ISelectable>().DeSelectThis();
        SelectEnemy.Remove(newUnit);
    }
    void RemoveSelectUnitUI(int count)
    {
        if (count == 1)
        {
            StatUI.Instance.DeleteUnitName();
            StatUI.Instance.DeleteUnitIcon();
            StatUI.Instance.DeleteUnitStat();
        }
        else
        {
            for (int i = 0; i < 14; i++)
            {
                StatUI.Instance.DeleteUnitIcon(i);
            }
            StatUI.Instance.DeActiveUnitIcon();
        }
    }
    /// <summary>
    /// 선택한 유닛 리스트를 초기화한다.
    /// </summary>
    public void RemoveAllSelectedUnit()
    {
        int count = selectedUserUnits.Count;
        for (int i = count - 1; i >= 0; i--)
        {
            RemoveSelectedUnit(selectedUserUnits[i]);
        }
    }
    public void RemoveAllSelectEnemy()
    {
        SelectEnemy.RemoveAll(item => item == null);
        RemoveSelectUnitUI(SelectEnemy.Count);        
        for (int i = SelectEnemy.Count - 1; i >= 0; i--)
        {
            SelectEnemy[i].GetComponent<ISelectable>().DeSelectThis();
        }
        SelectEnemy.Clear();
    }
    /// <summary>
    /// hit.collider에 적이 있었으면 이를 추적하고, 없었으면 어택땅이므로 해당 지점으로 이동하면서 적을 공격한다.
    /// </summary>
    /// <param name="worldMousePos"></param>
    public void HandleAttackClick(Vector2 worldMousePos)
    {
        attackPositionMarker.gameObject.SetActive(false);

        RaycastHit2D hit = Physics2D.Raycast(worldMousePos, Vector2.zero, float.MaxValue, enemyInt);

        if (hit.collider != null)
        {
            ISelectable selectable = hit.collider.GetComponent<ISelectable>();
            if (selectable != null && selectable.SelectType() == SelectableType.Enemy)
            {
                foreach (var unit in selectedUserUnits)
                {
                    unit.SetTarget((Enemy)selectable);
                }
            }
        }
        else
        {
            MoveUnitsToAttackPosition(worldMousePos);
        }
    }
    /// <summary>
    /// 일반 선택 기능이다.
    /// 현재 선택했던 유닛들을 초기화하고, 드래그할 준비를 하며 현재 좌클릭한 자리에 아군 유닛이 있으면 선택 목록에 일단 포함시킨다.
    /// </summary>
    /// <param name="hit"></param>
    public void HandleSelectionClick(Vector2 worldMousePos)
    {
        isMouseLeftDown = true;
        selectArea.SetStartPos(worldMousePos);
        selectArea.gameObject.SetActive(true);

        RemoveAllSelectedUnit();
        RemoveAllSelectEnemy();

        RaycastHit2D hit = Physics2D.Raycast(worldMousePos, Vector2.zero, float.MaxValue, userInt);
        if (hit.collider != null)
        {
            UserUnit selectable = hit.collider.GetComponent<UserUnit>();
            if (selectable != null)
            {
                if (selectable.SelectType() == SelectableType.UserUnit)
                {
                    AddSelectedUnit(selectable);
                }                
            }
        }
    }
    /// <summary>
    /// 이동하면서 적을 공격하는 기능을 호출 할 함수이다.
    /// 해당 지점으로 이동할 때 뜨는 마커를 활성화해서 이동 지점을 잠깐 표시해주고,
    /// 선택된 유닛들을 해당 지점으로 이동시키며 공격시킨다.
    /// </summary>
    /// <param name="worldMousePos"></param>
    private void MoveUnitsToAttackPosition(Vector2 worldMousePos)
    {
        int count = selectedUserUnits.Count;
        if (count > 0)
        {
            targetPositionMarker.transform.position = worldMousePos;
            targetPositionMarker.gameObject.SetActive(true);
            targetPositionMarker.InvokeTurnOff();

            if (count <= 1)
            {
                ((IAttackable)selectedUserUnits[0])?.AttackWhileMoving(GridGenerator.Instance.Grid.WorldPositionInBound(worldMousePos));
            }
            else
            {
                LinkedList<Vector2> destinationList = new LinkedList<Vector2>();
                // 배치 위치 찾기
                for (int i = 0; i < count; i++)
                {
                    destinationList.AddLast(FindDestination(worldMousePos));
                }
                LinkedListNode<Vector2> node = destinationList.First;
                // 유닛별로 위치 할당
                foreach (var unit in selectedUserUnits)
                {
                    unit.AttackWhileMoving(node.Value);
                    node = node.Next;
                }
                foreach (Vector2 n in destinationList)
                {
                    GridGenerator.Instance.Grid.SetGridValue(n, true);
                }
            }
        }
    }
    public void SetReadyToAttack(bool isReady)
    {
        isReadyToAttack = isReady;
        attackPositionMarker.gameObject.SetActive(isReady);
    }
    /// <summary>
    /// A키 또는 공격 준비 버튼을 눌렀을 때 다음 클릭으로 공격할 준비를 함
    /// </summary>
    public void OnClickReadyToAttack()
    {
        if (selectedUserUnits.Count > 0)
        {
            ClearReadyForActions();
            targetPositionMarker.gameObject.SetActive(false);
            SetReadyToAttack(true);
        }
    }
    public void OnClickReadyToMove()
    {
        if (selectedUserUnits.Count > 0)
        {
            ClearReadyForActions();
            targetPositionMarker.gameObject.SetActive(true);
            isReadyToMove = true;
        }
    }
    public void OnClickHold()
    {
        if (selectedUserUnits.Count > 0)
        {
            foreach (UserUnit unit in selectedUserUnits)
            {
                unit.Hold();
            }
            ClearReadyForActions();
        }
    }
    #endregion
    #endregion
    #region Private Methods
    private Vector2 FindDestination(Vector2 worldPos)
    {
        Vector2 result = GridGenerator.Instance.Grid.Grid2World(GridGenerator.Instance.FindNearestValidGridPosition(GridGenerator.Instance.Grid.World2Grid(worldPos)));
        GridGenerator.Instance.Grid.SetGridValue(result, false);
        return result;
    }
    private bool IsPositionInField(Vector2 worldPosition)
    {
        if (worldPosition.x > 8f || worldPosition.y > 4f || worldPosition.x < -8f || worldPosition.y < -4f)
        {
            return false;
        }
        return true;
    }
    private void MoveToPosition(Vector2 worldMousePos)
    {
        int count = selectedUserUnits.Count;
        if (count > 0)
        {
            targetPositionMarker.transform.position = worldMousePos;
            targetPositionMarker.gameObject.SetActive(true);
            targetPositionMarker.InvokeTurnOff();

            if (count <= 1)
            {
                selectedUserUnits[0]?.Move(GridGenerator.Instance.Grid.WorldPositionInBound(worldMousePos));
            }
            else
            {
                LinkedList<Vector2> destinationList = new LinkedList<Vector2>();
                // 배치 위치 찾기
                for (int i = 0; i < count; i++)
                {
                    destinationList.AddLast(FindDestination(worldMousePos));
                }
                LinkedListNode<Vector2> node = destinationList.First;
                // 유닛별로 위치 할당
                foreach (UserUnit unit in selectedUserUnits)
                {
                    unit.Move(node.Value);
                    node = node.Next;
                }
                foreach (Vector2 n in destinationList)
                {
                    GridGenerator.Instance.Grid.SetGridValue(n, true);
                }
            }
            unitMoveButton.Select();
        }
    }
    private void ClearReadyForActions()
    {
        SetReadyToAttack(false);
        isReadyToMove = false;
    }
    private void ActiveUnitControlUI(bool val)
    {
        unitAttackButton.interactable = val;
        unitMoveButton.interactable = val;
        unitHoldButton.interactable = val;
    }
    #endregion
}
