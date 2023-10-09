using System;
using UnityEngine;

namespace Manager
{
    public class Singleton<T> : MonoBehaviour
    {
        /// <summary>
        /// 내부클래스 BillPughSingleTon선언하고
        /// 제네릭 타입 T의 instance 필드를 가진다.
        /// </summary>
        protected static class BillPughSingleTon
        {
            public static T instance;
        }

        // 싱글톤 instance를 반환
        public static T Instance => BillPughSingleTon.instance;
        /// <summary>
        /// Awake 메서드를 재정의하여 인스턴스가 이미 존재하면 현재 게임 오브젝트를 파괴, 그렇지 않으면 인스턴스를 설정함
        /// </summary>
        protected virtual void Awake()
        {
            // if (Instance != null)
            //     Destroy(gameObject);
            gameObject.TryGetComponent(out BillPughSingleTon.instance);
        }
    }

    /// <summary>
    /// Awake 메서드를 재정의하여 인스턴스가 이미 존재하면 현재 게임 오브젝트를 파괴, 그렇지 않으면 인스턴스를 설정하고 DontDestroyOnLoad를 적용함
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DontDestroy<T> : Singleton<T>
    {
        protected override void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);
            DontDestroyOnLoad(this);
            gameObject.TryGetComponent(out BillPughSingleTon.instance);
        }

    }

}