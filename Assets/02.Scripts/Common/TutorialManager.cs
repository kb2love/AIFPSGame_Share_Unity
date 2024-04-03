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
    private GameObject player;
    void Start()
    {
        player = GameObject.FindWithTag("Player").gameObject;
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
            tutoText.text = "2. F�� �������� �׵��ϰ� Tab������\r\n    �������� ����� �� �ֽ��ϴ�.";
        }
        else if(page == 2)
        {
            imagetuto2.SetActive(false);
            imagetuto3.SetActive(true);
            tutoText.text = "3.1,2,3�� Ȥ�� �κ��丮�������â��\r\n    �����ؼ� ���⸦ ��ü�Ҽ��ֽ��ϴ�.";
        }
        else if( page == 3)
        {
            tutoUI.SetActive(false);
            StartGame();
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
            tutoText.text = "1. WASD�� �����ϼ� �ֽ��ϴ�.";
        }
        else if (page == 1)
        {
            imagetuto1.SetActive(false);
            imagetuto2.SetActive(true);
            imagetuto3.SetActive(false);
            tutoText.text = "2. F�� �������� �׵��ϰ� Tab������\r\n    �������� ����� �� �ֽ��ϴ�";
        }
    }
    private void StartGame()
    {
        GameObject.Find("Canvas_ui").transform.GetChild(2).gameObject.SetActive(true);
        GameObject.Find("Canvas_ui").transform.GetChild(3).gameObject.SetActive(true);
        GameManager.Instance.gameObject.SetActive(true);
        ObjectPoolingManager.objPooling.GetComponent<ObjectPoolingManager>().enabled = true;
        GameManager.Instance.GetComponent<GameManager>().enabled = true;
        GameManager.Instance.GetComponent<LoopSpawn>().enabled = true;
        GameObject.Find("Rweaponholder").transform.GetChild(0).gameObject.SetActive(true);
        GameObject.Find("Rweaponholder").transform.GetChild(0).gameObject.SetActive(true);
        player.GetComponent<PlayerMove>().enabled = true;
        player.GetComponent<FireCtrl>().enabled = true;
        player.GetComponent<PlayerDamage>().enabled = true;
        player.GetComponent<CameraDirection>().enabled = true;
        player.GetComponent<GetItem>().enabled = true;
    }
}
