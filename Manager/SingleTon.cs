using System;
using UnityEngine;

namespace Manager
{
    public class Singleton<T> : MonoBehaviour
    {
        private static class BillPughSingleTon
        {
            public static T instance ;
        }

        public static T Instance => BillPughSingleTon.instance;

        protected virtual void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);
            DontDestroyOnLoad(this);
            gameObject.TryGetComponent(out BillPughSingleTon.instance);
        }

    }
}