using UnityEngine;

namespace Manager
{
    public class Singleton<T> : MonoBehaviour where T : new()
    {
    private static class BillPughSingleTon
    {
        public static readonly T instance = new T();
    }

    public static T Instance
    {
        get
        {
            return BillPughSingleTon.instance;
        }
    }


    }
}