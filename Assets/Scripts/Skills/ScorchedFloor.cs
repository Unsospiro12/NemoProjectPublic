using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorchedFloor : MonoBehaviour
{
    private List<Enemy> enemies = new List<Enemy>();
    private UnitType type = UnitType.Magic;
    private WaitForSeconds waitForSeconds;

    private SkillEffect effectType = SkillEffect.ScorchedFloor;

    [SerializeField] private int damage;
    [SerializeField] private float period;
    [SerializeField] private float duration;

    [SerializeField] private AudioClip BustSound;

    private void OnEnable()
    {
        waitForSeconds = new WaitForSeconds(period);
        StartCoroutine(periodicDamage());
        StartCoroutine(DisableEffect());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.Tags.Enemy))
        {
            enemies.Add(collision.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.Tags.Enemy))
        {
            enemies.Remove(collision.GetComponent<Enemy>());
        }
    }

    private IEnumerator periodicDamage()
    {
        while(true)
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.TakeDamage(damage, type);
            }
            yield return waitForSeconds;
        }
    }

    private IEnumerator DisableEffect()
    {
        yield return new WaitForSeconds(duration);
        UserData.Instance.SkillObjectPool.ReturnObject(effectType, this.gameObject);
    }
}
