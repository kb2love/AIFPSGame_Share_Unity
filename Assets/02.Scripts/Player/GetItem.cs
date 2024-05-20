using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class GetItem : MonoBehaviour
{
    [SerializeField] private List<GameObject> collidersSet = new List<GameObject>();    // �������� �ݰ濡 ���� ���������� ����Ʈ�� �߰��ϰ� �� ����Ʈ
    private int setCount;               // ���� �׵��ҷ��� �������� �ֺ����ִ� �����ۺ��� �Ѱ� �����ʱ����� Idx��
    private bool isInputF;              // Update���� F�� �������� Ȯ���ϱ� ���� bool����
    private string itemTag = "Item";    // OnTrigger�� �Ҷ����� �θ��� ������ Tag�� ��Ʈ��
    private GameObject getItemPanel;    // �������ֺ��� �������� ��Ÿ�� UI
    public bool isContact;              // ������ �ֺ��� �ִ� �� ������ �˻��ϱ� ���� bool ����
    private Animator animator;          // �÷��̾ �������� �׵��Ҷ� ����� ���� �ִϸ�����
    void OnEnable()
    {
        getItemPanel = GameObject.Find("Canvas_ui").transform.GetChild(1).gameObject;
        animator = transform.GetChild(0).GetComponent<Animator>();
        isContact = false;
        isInputF = false;
        setCount = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(itemTag))    // �������� Colider�� ��Ҵٸ�
        {
            getItemPanel.SetActive(true);           // ������ �׵� UI�� ���ش�
            collidersSet.Add(other.gameObject);     // �ݶ��̴� ����Ʈ�� OnTrigger�� ���� �������� �߰��Ѵ�
            isContact = true;                       // �����۰� ����ִٴ� �� �˸���
            StartCoroutine(GetItemPlayer());        // �����۰� ����ִ� ���¶�� ���� �������� ItemType�� �ִ��� �˻��ϴ� ��ŸƮ �ڷ�ƾ�� ���� 
            setCount = 0;                           // �����۸���Ʈ�� Idx ���� �ʱ�ȭ��Ų��
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isContact)   // ������Ʈ���� �ֺ��� �������� �ִٸ� F�� �������� �ȴ������� �˻��Ѵ�
        {
            isInputF = true;
        }
    }
    IEnumerator GetItemPlayer()
    {
        while (isContact)
        {
            yield return new WaitForSeconds(0.2f);

            if (isInputF && collidersSet.Count > 0)
            {   // F�� ������ �ֺ��� �������� �ִٸ� �������� ItemInfo�� �ִ��� ������ Ȯ���Ŀ� GetItemF�� �Ѿ��
                var itemType = collidersSet[setCount].GetComponent<ItemInfo>()?.itemType;
                if (itemType != null)
                    GetItemF(itemType.Value);
            }
        }
    }
    private void GetItemF(ItemData.ItemType itemType)
    {
        if (isInputF)
        {   // ����ִ� �������� ���ش�
            collidersSet[setCount].gameObject.SetActive(false);
            setCount++; // �ݶ��̴� ����Ʈ�� Idx�� �÷��ش�
            isInputF = false;  

            if (setCount >= collidersSet.Count)
            {   // �ݶ��̴� ����Ʈ�� Idx���� �ݶ��̴��� ���ں��� ���ų� ���ٸ� �� �ֺ��� �������� �� �׵��ߴٸ�
                getItemPanel.SetActive(false);  // ������ �׵� UI�� ���ش�
                collidersSet.Clear();   // �ݶ��̴� ����Ʈ�� Ŭ�����Ѵ�
                setCount = 0;           // �ݶ��̴� ����Ʈ Idx �� �ʱ�ȭ ��Ų��
                isContact = false;      // �����۰� ������� �������·� �ٲ۴�
            }

            animator.SetTrigger("GetItemTrigger");  // ������ �׵� ��� 
            GameManager.Instance.AddItem(itemType); // GameManager�� AddItem ���� �ѱ��
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(itemTag))
        {   // ������ �������� �����ٸ�
            getItemPanel.SetActive(false);  // ������ �׵�UI�� ���ش�
            isContact = false;              // �����۰� �������� ���·� �ٲ۴�
            if(!isInputF)                   // F�� ���� ���°� �ƴ϶��
            collidersSet.Clear();           // �ݶ��̴��� ���½�Ų��
        }
    }
}
