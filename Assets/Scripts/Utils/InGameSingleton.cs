using UnityEngine;

/// <summary>
/// MonoBehaviour를 상속 받는 제너릭 싱글톤 패턴
/// 기존 싱글톤과 다르게 게임 진행중에만 사용하는 싱글톤들을 위해 특별히 제작 되었으며
/// 게임을 나가면 (씬 전환이 되면) 파괴되도록 설계하였다.
/// 이를 통해 중복 코드를 줄일 수 있다.
/// 보면 알겠지만 T 자리에 자기자신의 클래스가 들어가면 된다.
/// </summary>
/// <typeparam name="T"></typeparam>

public class InGameSingleton<T> : MonoBehaviour where T : InGameSingleton<T>
{
    protected static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject gameObject = new GameObject();
                gameObject.name = $"{typeof(T).Name}";
                instance = gameObject.AddComponent<T>();
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
