using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class GranadeCtrl : MonoBehaviour
{
    [SerializeField]
    private GranadeData granadeData;
    Vector3 targetPosition;
    void OnEnable()
    {
        targetPosition = transform.position + transform.forward * 5f;
        transform.DOJump(targetPosition, 1f, 1, granadeData.arriveTime).SetEase(Ease.InOutSine).OnComplete(Bomb);
    }
    private void Bomb()
    {
        if(gameObject.activeSelf)
        StartCoroutine(Explozion());
    }
    private IEnumerator Explozion()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);

    }
}
