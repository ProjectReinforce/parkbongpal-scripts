using System;
using UnityEngine;

namespace Manager
{
    public class Singleton<T> : MonoBehaviour
    {
        protected static class BillPughSingleTon
        {
            public static T instance ;
        }

        public static T Instance => BillPughSingleTon.instance;
        protected virtual void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);
            gameObject.TryGetComponent(out BillPughSingleTon.instance);
        }
    }

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