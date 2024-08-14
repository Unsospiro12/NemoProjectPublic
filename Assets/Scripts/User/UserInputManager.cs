using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 유저의 input에 따른 event함수들에 담아줄 행동들을 구현한 클래스
/// </summary>
public class UserInputManager : MonoBehaviour
{
    #region Private Field
    private PlayerInputActions playerInputActions;
    #endregion
    #region Public Properties
    public InputAction MouseLeftClick { get; private set; }
    public InputAction MouseRightClick { get; private set; }
    public InputAction MouseDrag { get; private set; }
    public InputAction Hold { get; private set; }
    public InputAction ReadyToAttack { get; private set; }
    public InputAction CameraLock { get; private set; }
    public InputAction CameraReset { get; private set; }
    public InputAction MouseWheel { get; private set; }
    public InputAction MouseDoubleClick { get; private set; }
    public InputAction ReadyToMove { get; private set; }
    public InputAction CameraMove { get; private set; }
    #endregion
    #region MonoBehaviour Callbacks
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }
    // 이벤트들을 실행 시켜주고, 함수를 할당해준다.
    private void OnEnable()
    {
        MouseLeftClick = playerInputActions.User.MouseLeftClick;
        MouseRightClick = playerInputActions.User.MouseRightClick;
        MouseDrag = playerInputActions.User.MouseDrag;
        Hold = playerInputActions.User.Hold;
        ReadyToAttack = playerInputActions.User.ReadyToAttack;
        CameraLock = playerInputActions.User.CameraLock;
        CameraReset = playerInputActions.User.CameraReset;
        MouseWheel = playerInputActions.User.MouseWheel;
        MouseDoubleClick = playerInputActions.User.MouseDoubleClick;
        ReadyToMove = playerInputActions.User.ReadyToMove;
        CameraMove = playerInputActions.User.CameraMove;

        MouseLeftClick.Enable();
        MouseRightClick.Enable();
        MouseDrag.Enable();
        Hold.Enable();
        ReadyToAttack.Enable();
        CameraLock.Enable();
        CameraReset.Enable();
        MouseWheel.Enable();
        MouseDoubleClick.Enable();
        ReadyToMove.Enable();
        CameraMove.Enable();
    }
    private void OnDisable()
    {
        MouseLeftClick.Disable();
        MouseRightClick.Disable();
        MouseDrag.Disable();
        Hold.Disable();
        ReadyToAttack.Disable();
        CameraLock.Disable();
        CameraReset.Disable();
        MouseWheel.Disable();
        MouseDoubleClick.Disable();
        ReadyToMove.Disable();
        CameraMove.Disable();
    }
    #endregion
}
