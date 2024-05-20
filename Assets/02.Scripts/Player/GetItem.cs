using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class GetItem : MonoBehaviour
{
    [SerializeField] private List<GameObject> collidersSet = new List<GameObject>();    // 아이템의 반경에 들어가고 나갈때마다 리스트에 추가하고 뺄 리스트
    private int setCount;               // 내가 휙득할려는 아이템이 주변에있는 아이템보다 넘게 하지않기위한 Idx값
    private bool isInputF;              // Update에서 F를 눌럿는지 확인하기 위한 bool변수
    private string itemTag = "Item";    // OnTrigger를 할때마다 부르는 아이템 Tag의 스트링
    private GameObject getItemPanel;    // 아이템주변에 갈때마다 나타날 UI
    public bool isContact;              // 아이템 주변에 있는 지 없는지 검사하기 위한 bool 변수
    private Animator animator;          // 플레이어가 아이템을 휙득할때 모션을 취할 애니메이터
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
        if(other.gameObject.CompareTag(itemTag))    // 아이템의 Colider에 닿았다면
        {
            getItemPanel.SetActive(true);           // 아이템 휙득 UI을 켜준다
            collidersSet.Add(other.gameObject);     // 콜라이더 리스트에 OnTrigger가 닿은 아이템을 추가한다
            isContact = true;                       // 아이템과 닿아있다는 걸 알린다
            StartCoroutine(GetItemPlayer());        // 아이템과 닿아있는 상태라면 닿은 아이템이 ItemType이 있는지 검사하는 스타트 코루틴을 실행 
            setCount = 0;                           // 아이템리스트의 Idx 값을 초기화시킨다
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isContact)   // 업데이트에서 주변에 아이템이 있다면 F를 눌렀는지 안눌렀는지 검사한다
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
            {   // F를 누르고 주변에 아이템이 있다면 아이템의 ItemInfo가 있는지 없는지 확인후에 GetItemF로 넘어간다
                var itemType = collidersSet[setCount].GetComponent<ItemInfo>()?.itemType;
                if (itemType != null)
                    GetItemF(itemType.Value);
            }
        }
    }
    private void GetItemF(ItemData.ItemType itemType)
    {
        if (isInputF)
        {   // 닿아있는 아이템을 꺼준다
            collidersSet[setCount].gameObject.SetActive(false);
            setCount++; // 콜라이더 리스트의 Idx를 올려준다
            isInputF = false;  

            if (setCount >= collidersSet.Count)
            {   // 콜라이더 리스트의 Idx값이 콜라이더의 숫자보다 높거나 같다면 즉 주변의 아이템을 다 휙득했다면
                getItemPanel.SetActive(false);  // 아이템 휙득 UI를 꺼준다
                collidersSet.Clear();   // 콜라이더 리스트를 클리어한다
                setCount = 0;           // 콜라이더 리스트 Idx 를 초기화 시킨다
                isContact = false;      // 아이템과 닿아있지 않은상태로 바꾼다
            }

            animator.SetTrigger("GetItemTrigger");  // 아이템 휙득 모션 
            GameManager.Instance.AddItem(itemType); // GameManager의 AddItem 으로 넘긴다
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(itemTag))
        {   // 아이템 범위에서 나간다면
            getItemPanel.SetActive(false);  // 아이템 휙득UI를 꺼준다
            isContact = false;              // 아이템과 닿지않은 상태로 바꾼다
            if(!isInputF)                   // F를 누른 상태가 아니라면
            collidersSet.Clear();           // 콜라이더를 리셋시킨다
        }
    }
}
