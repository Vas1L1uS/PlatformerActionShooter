using System.Collections;
using UnityEngine;

public class DestroyAfterSpawn : MonoBehaviour
{
    [SerializeField] private float _timeToDestroy;
    private void Awake()
    {
        StartCoroutine(TimerToDestroy(_timeToDestroy));
    }

    private IEnumerator TimerToDestroy(float timeToDestroy)
    {
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(this.gameObject);
    }
}
