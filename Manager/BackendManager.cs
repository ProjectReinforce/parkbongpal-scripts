using UnityEngine;
using BackEnd;
using LitJson;
using Manager;

public class BackendManager : Singleton<BackendManager>
{
    public Where searchFromMyIndate = new Where();
    public BaseWeaponData[] baseWeaponDatas;
    public Mine[] mines;
    public WeaponData[] WeaponDatas;
    public UserData _userData;
    protected override void Awake()
    {
        base.Awake();
        var bro = Backend.Initialize();

        if(bro.IsSuccess())
            Debug.Log($"초기화 성공 : {bro}");
        else
        {
            Debug.LogError($"초기화 실패 : {bro}");
            return;
        }
        searchFromMyIndate.Equal(nameof(UserData.colum.owner_inDate), BackEnd.Backend.UserInDate);

        JsonMapper.RegisterImporter<string, int>(s => int.Parse(s));
        
        SendQueue.Enqueue(Backend.Chart.GetOneChartAndSave,"85454", bro =>
        {
            if (!bro.IsSuccess())
            {
                Debug.Log(bro);
                return;
            }
            
            JsonData json = BackendReturnObject.Flatten(bro.Rows());
            baseWeaponDatas = new BaseWeaponData[json.Count];
            for (int i = 0; i < json.Count; ++i)
            {
                // 데이터를 디시리얼라이즈 & 데이터 확인
                BaseWeaponData item = JsonMapper.ToObject<BaseWeaponData>(json[i].ToJson());
                baseWeaponDatas[i] = item;
            }
        });

        SendQueue.Enqueue(Backend.Chart.GetOneChartAndSave,"85425", bro =>
        {
            if (!bro.IsSuccess())
            {
                // 요청 실패 처리
                Debug.Log(bro);
                return;
            }
  
            JsonData json = BackendReturnObject.Flatten(bro.Rows());
            mines = new Mine[json.Count];
            for (int i = 0; i < json.Count; ++i)
            {
                // 계수, 스테이지 확인 
                MineData mineData = JsonMapper.ToObject<MineData>(json[i].ToJson());
                mineData.defence =(int)((mineData.defence <<mineData.stage) *0.1f) ;
                mineData.hp = (int)((mineData.hp << mineData.stage) * 0.2f);
                mineData.size = (int)(mineData.size*1.5f) +30;
                mineData.lubricity =(int)( mineData.lubricity*1.5f);
                Debug.Log($"defence:{mineData.defence} hp: {mineData.hp} size: {mineData.size}" +
                          $"lubricity: {mineData.lubricity} stage: {mineData.stage}");
                mines[i] = new Mine(mineData);
            }
        });
        
        SendQueue.Enqueue(Backend.GameData.Get, nameof(WeaponData),
            BackendManager.Instance.searchFromMyIndate, 120, bro =>
            {
                if (!bro.IsSuccess())
                {
                    // 요청 실패 처리
                    Debug.Log(bro);
                    return;
                }

                JsonData json = BackendReturnObject.Flatten(bro.Rows());
                
                for (int i = 0; i < json.Count; ++i)
                {
                    WeaponData item = JsonMapper.ToObject<WeaponData>(json[i].ToJson());
                    WeaponDatas[i] = item;
                    // Weapon weapon = new Weapon(item);
                    //
                    // Quarry.Instance.SetMine(weapon);
                    // Debug.Log(LastWeaponSlot.Value);
                    // LastWeaponSlot.Value.SetWeapon(weapon); 
                    // LastWeaponSlot = LastWeaponSlot.Next;

                    Debug.Log(item.inDate);
                }
            });
        
        SendQueue.Enqueue(Backend.GameData.Get, nameof(UserData),
            BackendManager.Instance.searchFromMyIndate, 1, bro =>
            {
                if (!bro.IsSuccess())
                {
                    // 요청 실패 처리
                    Debug.Log(bro);
                    return;
                }

                JsonData json = BackendReturnObject.Flatten(bro.Rows());
                for (int i = 0; i < json.Count; ++i)
                {
                    // 데이터를 디시리얼라이즈 & 데이터 확인
                    _userData = JsonMapper.ToObject<UserData>(json[i].ToJson());
                }
            });
        
        
    }
}