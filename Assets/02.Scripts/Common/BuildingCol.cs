using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCol : MonoBehaviour
{
    private string bulletTag = "Bullet";
    private Collider[] colliders;
    private List<Collider> collidersList = new List<Collider>();
    void Start()
    {
        colliders = GetComponentsInChildren<Collider>();
        for(int i = 0; i < colliders.Length; i++)
        {
            collidersList.Add(colliders[i]);
        }
        collidersList.RemoveAt(0);
    }
    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag(bulletTag))
        {
            col.gameObject.SetActive(false);
        }
    }
}
