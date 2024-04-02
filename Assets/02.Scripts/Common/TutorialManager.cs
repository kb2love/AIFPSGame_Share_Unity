using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TutorialManager : MonoBehaviour
{
    public int page;
    private GameObject tutoUI;
    private GameObject imagetuto1;
    private GameObject imagetuto2;
    private GameObject imagetuto3;
    private Text tutoText;
    private GameObject buttontuto;
    void Start()
    {
        page = 0;
        tutoUI = GameObject.Find("Tutorial-Panel").gameObject;
        imagetuto1 = tutoUI.transform.GetChild(0).gameObject;
        imagetuto2 = tutoUI.transform.GetChild(1).gameObject;
        imagetuto3 = tutoUI.transform.GetChild(2).gameObject;
        tutoText = tutoUI.transform.GetChild(3).GetComponent<Text>();
        buttontuto = tutoUI.transform.GetChild(5).gameObject;
    }
    public void NextPage()
    {
        page++;
        if(page == 1)
        {
            imagetuto1.SetActive(false);
            imagetuto2.SetActive(true);
            buttontuto.SetActive(true);
            tutoText.text = "2. F�� �������� �׵��ϰ� Tab������\r\n �������� ����� �� �ֽ��ϴ�";
        }
        else if(page >= 2)
        {
            imagetuto2.SetActive(false);
            imagetuto3.SetActive(true);
            page = 0;
        }
    }
    public void BackPage()
    {
        page--;
        if (page < 0)
            page = 0;
        else if (page == 0)
        {
            imagetuto1.SetActive(true);
            imagetuto2.SetActive(false);
            buttontuto.SetActive(false);
        }
        else if (page == 1)
        {
            imagetuto1.SetActive(false);
            imagetuto2.SetActive(true);
            imagetuto3.SetActive(false);
            buttontuto.SetActive(true);
        }
    }
}
