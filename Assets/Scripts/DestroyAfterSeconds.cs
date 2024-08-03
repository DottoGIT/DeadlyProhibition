using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    [SerializeField] float lifeTime;

    void Start()
    {
        StartCoroutine(DestroyCounter());
    }

    IEnumerator DestroyCounter()
    {
        yield return new WaitForSeconds(lifeTime);
    }
}
