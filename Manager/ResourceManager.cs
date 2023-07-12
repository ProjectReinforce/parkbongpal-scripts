using UnityEngine;
using BackEnd;
using LitJson;

namespace Manager
{
    public class ResourceManager: Singleton<ResourceManager>
    {
        private BaseWeaponData[] _baseWeaponData;
        public BaseWeaponData[] baseWeaponData => _baseWeaponData;
        protected override void Awake()
        {
            base.Awake();
            SendQueue.Enqueue(Backend.Chart.GetOneChartAndSave,"85454", bro =>
            {
                if (!bro.IsSuccess())
                {
                    Debug.Log(bro);
                    return;
                }
            
                JsonData json = BackendReturnObject.Flatten(bro.Rows());
                _baseWeaponData = new BaseWeaponData[json.Count];
                JsonMapper.RegisterImporter<string, int>(s => int.Parse(s));
                JsonMapper.RegisterImporter<string, float>(s => float.Parse(s));
                for (int i = 0; i < json.Count; ++i)
                {
                    // 데이터를 디시리얼라이즈 & 데이터 확인
                    BaseWeaponData item = JsonMapper.ToObject<BaseWeaponData>(json[i].ToJson());
            
                    baseWeaponData[i] = item;
                }
            });
        }
    }
}