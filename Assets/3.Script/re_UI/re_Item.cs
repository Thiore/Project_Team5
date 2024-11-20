using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class re_Item
{


    // 다시 하면서 구현? 생각해야할 목록들 

    // 1. for문 아끼기 (return break 확실히)
    // 2. Destroy 타이밍 잡기 애매하니 지양 
    // 3. Update 지양
    // 4. 버튼이나 상호작용은 코루틴? (러프이미지에 쓰긴함)

    // Item >> 이 새끼는 그냥 우선 DataManager에서 정리한 데이터의 ID만 가져오게끔 하는게 베스트 같음 

    // 그럼 필드에 있는 아이템은 어떻게 확인 할건데? 
    // 거긴 더미 Component로 ID만 넣어주는건? 어쩌피 거기서는 뭐 그런것도 없는데 


    // 그럼 모노비를 상속받지 않는 Item을 싹 정리해두고 데이터 매니저가 가지고 있고 

    // 아이템 자체 데이터만 있고 
    // 데이터 테이블 복사하고 수정해주는 방향? 
    // >> 이렇게 되면 테이블은 언제 업데이트 해줌? 
    // 이땐 세이브 타이밍에 껴넣으면 될라나? 복사한 데이터 테이블에 껴넣어주고 이거 저장 하는 방식? 

    // 그럼 먹으면 >>  ID 랑 스프라이트만 받아서 표시할까? 아이디만 있어도 인포메이션은 할 수 있으니까 
    // 그럼 조합은 ?
    // 각각 기능 자체는 같은데 세세한 구현이 다르니 아예 구분?
    // invenslot ondrag 
    // quick drag   enum 이 quick 이면 복사랄까 하나 더 해준다는 방향으로? 


    // slot list > 넣어줄때 Item id 쑤셔 넣고 sprite 가져와서 조지고 


    // 생성자에서 받아온다고 ? 

    // 테이블자체에 spritename이 있지 
    // 역직렬화 할때 Sprite 자체를 





    // 먹었을때 인벤에 넣고 
    // 1개라도 있으면 인포 띄우고 마지막 아이템 기준 인포 띄우기 

    public int id;
    public string name;
    public eItemType eItemType;
    public int elementindex;
    public int combineindex;
    public string tableName;
    public bool isused;
    public int usecount;
    public bool isfix;
    public Sprite sprite;


    public re_Item(int id, string name, int eItemType, int elementindex, 
                    int combineindex, string tableName, bool isused, int usecount, bool isfix, string spritename)
    {
        this.id = id;
        this.name = name;
        this.eItemType = (eItemType)eItemType;
        this.elementindex = elementindex;
        this.combineindex = combineindex;
        this.tableName = tableName;
        this.isused = isused;
        this.usecount = usecount;
        this.isfix = isfix;
        this.sprite = Resources.Load<Sprite>($"UI/Item/{spritename}");
        Debug.Log(this.sprite);
    }



}
