using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField] private Vector3 moveDir = Vector3.zero;
    private EnemyStat enemyStat;
    public float moveSpeed;
    private void Awake()
    {
        enemyStat = GetComponent<EnemyStat>();
    }
    private void Update()
    {
        if (moveDir != Vector3.zero)
            transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
    public void Move(Vector3 direction)
    {
        moveSpeed = enemyStat.MovementSpeed.BaseValue;
        moveDir = direction;
    }
}
