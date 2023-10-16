using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestContentsPool : MonoBehaviour
{
    [SerializeField] List<QuestContent> pool;
    [SerializeField] QuestContent origin;

    public QuestContent GetOne()    // 공부해올것
    {
        foreach (var item in pool)
        {
            pool.Remove(item);  // pool을 순회하며 QuestContent 오브젝트를 찾고 제거함
            return item;
        }
        GameObject newObject = Instantiate(origin.gameObject);  // orgin을 복제해 새로 만든 변수에 넣음
        if (!newObject.TryGetComponent(out QuestContent component)) // 새 게임 오브젝트에 tryget컴포넌트 함수를 통해 qusetcontent를 찾음
            component = newObject.AddComponent<QuestContent>(); // 없으면 새로운 컴포넌트를 추가함
        return component;   // 생성되거나 찾아진 해당 컴포넌트를 반환함
    }
}
