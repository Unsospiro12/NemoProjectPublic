using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

/// <summary>
/// 캐릭터 대량 선택을 위한 표시 범위
/// 스타나 워크래프트에 나오듯이 초록색 상자가 마우스 드래그를 따라 표시되며 범위안에 존재하는 제어 가능한 유닛들을 동시에 선택하게 해준다.
/// </summary>
public class SelectionArea : MonoBehaviour
{
    #region Private Field
    private BoxCollider2D boxCollider;
    private bool isSelecting;
    #endregion
    #region Serilize Field
    [SerializeField] private UserUnitController userUnitController;
    #endregion
    #region Public Properties
    public bool IsSelecting
    {
        get
        {
            return isSelecting;
        }
        set
        {
            isSelecting = value;
        }
    }
    #endregion
    #region MonoBehaviour Callbacks
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        isSelecting = false;
    }
    #endregion
    #region Public Methods
    // 선택 상자 범위 조절. 콜라이더 범위도 조절한다.
    public void SetSize(Vector2 MouseStartPos, Vector2 endPoint)
    {
        float areaWidth = endPoint.x - MouseStartPos.x;
        float areaHeight = endPoint.y - MouseStartPos.y;

        transform.localScale = new Vector2(Mathf.Abs(areaWidth), Mathf.Abs(areaHeight));
        transform.position = (MouseStartPos + endPoint) / 2;
    }
    public void SetStartPos(Vector2 MouseStartPos)
    {
        transform.localScale = Vector2.zero;
        transform.position = MouseStartPos ;
    }
    #endregion
    #region Private Methods
    // 콜라이더를 이용하여 범위안에 유닛이 들어오면 플레이어가 컨트롤 하는 유닛의 리스트에 넣어준다.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        UserUnit selectedUnit = collision.gameObject.GetComponent<UserUnit>();
        ISelectable selectEnemy = collision.gameObject.GetComponent<ISelectable>();
        if (selectedUnit != null && isSelecting)
        {
            if(selectedUnit.SelectType() == SelectableType.UserUnit)
            {
                userUnitController.AddSelectedUnit(selectedUnit);
            }
        }

        if(selectEnemy != null && isSelecting)
        {
            if(selectEnemy.SelectType() == SelectableType.Enemy)
            {
                userUnitController.AddSelectEnemy(collision.gameObject);
            }
        }
    }
    // 아직 드래그를 끝내지 않은 상태에서 범위 밖으로 유닛이 나가게 하면 해당 유닛의 선택은 취소된다.
    private void OnTriggerExit2D(Collider2D collision)
    {
        UserUnit selectedUnit = collision.gameObject.GetComponent<UserUnit>();
        ISelectable selectEnemy = collision.gameObject.GetComponent<ISelectable>();
        if (selectedUnit != null && isSelecting)
        {
            if (selectedUnit.SelectType() == SelectableType.UserUnit)
            {
                userUnitController.RemoveSelectedUnit(selectedUnit);
            }
        }
        if (selectEnemy != null && isSelecting)
        {
            if (selectEnemy.SelectType() == SelectableType.Enemy)
            {
                userUnitController.RemoveSelectedEnemy(collision.gameObject);
            }
        }
    }
    #endregion
}
