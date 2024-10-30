using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eItemType
{
    element = 0,
    clue,
    trigger,
    quick
}

public class ItemData
{
    public int id;
    public int nametableid; 
    public eItemType type;
    public int elementindex;
    public int combineindex;
    public bool isfix;
    public string imgname;
}


// 로컬제이션 전환할 변수하나 
// 스프라이트로 저장해둘 string 변수하나

public class Item : MonoBehaviour
{
    public int id;
    public int nametableid;
    public eItemType type;
    public int elementindex;
    public int combineindex;
    public bool isfix;
    public Sprite imgname;
}
