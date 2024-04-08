using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndSecneManager : MonoBehaviour
{
    [SerializeField] private GameObject[] textParent;
    [SerializeField] private Text[] scoreText;
    void Start()
    {
        scoreText = GameObject.Find("Panel-Score").GetComponentsInChildren<Text>();

    }
}