using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAnimation : MonoBehaviour
{
    public static ItemAnimation instance;

    private Vector3 randomIdleAnimationVec;

    private void Start()
    {
        instance = GetComponent<ItemAnimation>();
        randomIdleAnimationVec = GetIdleAnimationVec();
    }

    void Update()
    {
        // set idle random slowly animation
        gameObject.transform.Rotate(randomIdleAnimationVec);
    }

    private Vector3 GetIdleAnimationVec()
    {
        return new Vector3(Random.Range(1, 2), Random.Range(0, 2), Random.Range(0, 2));
    }

    private void OnMouseEnter()
    {
        StartCoroutine(IncreaseSize());
    }

    private void OnMouseExit()
    {
        StartCoroutine(DecreaseSize());
    }

    public IEnumerator IncreaseSize()
    {
        for (float scaleVar = 1; scaleVar <= 1.4f; scaleVar += Time.deltaTime)
        {
            transform.localScale = new Vector3(scaleVar, scaleVar, scaleVar);
            yield return null;
        }
    }

    public IEnumerator DecreaseSize()
    {
        for (float scaleVar = 1.4f; scaleVar >= 1; scaleVar -= Time.deltaTime)
        {
            transform.localScale = new Vector3(scaleVar, scaleVar, scaleVar);
            yield return null;
        }
    }
}
