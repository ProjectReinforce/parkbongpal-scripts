using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestContentsPool : MonoBehaviour
{
    [SerializeField] List<QuestContent> pool;
    [SerializeField] QuestContent origin;

    public QuestContent GetOne()
    {
        foreach (var item in pool)
        {
            pool.Remove(item);  // pool�� ��ȸ�ϸ� QuestContent ������Ʈ�� ã�� ������
            return item;
        }
        GameObject newObject = Instantiate(origin.gameObject);  // orgin�� ������ ���� ���� ������ ����
        if (!newObject.TryGetComponent(out QuestContent component)) // �� ���� ������Ʈ�� tryget������Ʈ �Լ��� ���� qusetcontent�� ã��
            component = newObject.AddComponent<QuestContent>(); // ������ ���ο� ������Ʈ�� �߰���
        return component;   // �����ǰų� ã���� �ش� ������Ʈ�� ��ȯ��
    }
}