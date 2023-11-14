using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class LocalDataManager : MonoBehaviour
// public class LocalDataManager : DontDestroy<LocalDataManager>
{
    public void SavePrefs()
    {
        PlayerPrefs.SetString("UserId", BackEnd.Backend.UserInDate);
        Debug.Log($"Saved : {BackEnd.Backend.UserInDate}");
    }
    
    public void LoadPrefs()
    {
        string data = PlayerPrefs.GetString("abc");
        Debug.Log($"Loaded : {data}");
    }
}

public class RecordData
{
    string userID;
    public string UserID => userID;
    uint tutorial;
    public uint Tutorial => tutorial;
    ulong useGold;
    public ulong UseGold => useGold;
    ulong getGold;
    public ulong GetGold => getGold;
    ulong useDiamond;
    public ulong UseDiamond => useDiamond;
    ulong getDiamond;
    public ulong GetDiamond => getDiamond;
    uint getItem;
    public uint GetItem => getItem;
    uint disassembleItem;
    public uint DisassembleItem => disassembleItem;
    uint produceWeapon;
    public uint ProduceWeapon => produceWeapon;
    uint advanceProduceWeapon;
    public uint AdvanceProduceWeapon => advanceProduceWeapon;
    uint tryPromote;
    public uint TryPromote => tryPromote;
    uint tryAdditional;
    public uint TryAdditional => tryAdditional;
    uint tryReinforce;
    public uint TryReinforce => tryReinforce;
    uint tryMagic;
    public uint TryMagic => tryMagic;
    uint trySoul;
    public uint TrySoul => trySoul;
    uint tryRefine;
    public uint TryRefine => tryRefine;
    // 일일
    uint dayAttendance;
    public uint DayAttendance => dayAttendance;

    uint dayTryPromote;
    public uint DayTryPromote => dayTryPromote;
    uint dayTryMagic;
    public uint DayTryMagic => dayTryMagic;
    uint dayTryReinforce;
    public uint DayTryReinforce => dayTryReinforce;
    uint dayGetBonus;
    public uint DayGetBonus => dayGetBonus;
    uint daySeeAds;
    public uint DaySeeAds => daySeeAds;
    uint[] dayValGroups;
    string[] dayValGroupsString;
    DateTime saveDay;
    public DateTime SaveDay => saveDay;
    // 주간
    uint weekAttendance;
    public uint WeekAttendance => weekAttendance;
    uint weekTryPromote;
    public uint WeekTryPromote => weekTryPromote;
    uint weekTryMagic;
    public uint WeekTryMagic => weekTryMagic;
    uint weekTryReinforce;
    public uint WeekTryReinforce => weekTryReinforce;
    uint weekGetBonus;
    public uint WeekGetBonus => weekGetBonus;
    uint weekSeeAds;
    public uint WeekSeeAds => weekSeeAds;
    uint[] weekValGroups;
    string[] weekValGroupsString;
    DateTime saveWeek;
    public DateTime SaveWeek => saveWeek;

    public Action tutorialClearEvent;
    public Action levelUpEvent;
    public Action getGoldEvent;
    public Action useGoldEvent;
    public Action useDiamondEvent;
    public Action getDiamondEvent;
    public Action getItemEvent;
    public Action disassembleItemEvent;
    public Action produceWeaponEvent;
    public Action advanceProduceWeaponEvent;
    public Action tryPromoteEvent;
    public Action tryAdditionalEvent;
    public Action tryReinforceEvent;
    public Action tryMagicEvent;
    public Action trySoulEvent;
    public Action tryRefineEvent;
    public Action attendanceEvent;
    public Action getBonusEvent;
    public Action seeAdsEvent;
    // 일일
    public Action dayAttendanceEvent;
    public Action dayTryPromoteEvent;
    public Action dayTryMagicEvent;
    public Action dayTryReinforceEvent;
    public Action dayGetBonusEvent;
    public Action daySeeAdsEvent;
    // 주간
    public Action weekAttendanceEvent;
    public Action weekTryPromoteEvent;
    public Action weekTryMagicEvent;
    public Action weekTryReinforceEvent;
    public Action weekGetBonusEvent;
    public Action weekSeeAdsEvent;

    public void LoadOrInitRecord(string _userInDate)
    {
        userID = PlayerPrefs.GetString("UserID");
        Debug.Log($"loaded userID : {userID} / indate : {_userInDate}");
        dayValGroups = new uint[6] { dayAttendance, dayTryPromote, dayTryMagic, dayTryReinforce, dayGetBonus, daySeeAds };
        dayValGroupsString = new string[6] { "DayAttendance", "DayTryPromote", "DayTryMagic", "DayTryReinforce", "DayGetBonus", "DaySeeAds" };
        weekValGroups = new uint[6] { WeekAttendance, WeekTryPromote, weekTryMagic, weekTryReinforce, weekGetBonus, weekSeeAds };
        weekValGroupsString = new string[6] { "WeekAttandance", "WeekTryPromote", "WeekTryMagic", "WeekTryReinforce", "WeekGetBonus", "WeekSeeAds" };

        if (_userInDate == userID)
        {
            uint.TryParse(PlayerPrefs.GetString("Tutorial"), out tutorial);
            ulong.TryParse(PlayerPrefs.GetString("UseGold"), out useGold);
            // Debug.Log($"UsedGold : {useGold}");
            ulong.TryParse(PlayerPrefs.GetString("GetGold"), out getGold);
            // Debug.Log($"GetedGold : {getGold}");
            ulong.TryParse(PlayerPrefs.GetString("UseDiamond"), out useDiamond);
            ulong.TryParse(PlayerPrefs.GetString("GetDiamond"), out getDiamond);
            uint.TryParse(PlayerPrefs.GetString("GetItem"), out getItem);
            uint.TryParse(PlayerPrefs.GetString("DisassembleItem"), out disassembleItem);
            uint.TryParse(PlayerPrefs.GetString("ProduceWeapon"), out produceWeapon);
            uint.TryParse(PlayerPrefs.GetString("AdvanceProduceWeapon"), out advanceProduceWeapon);
            uint.TryParse(PlayerPrefs.GetString("TryPromote"), out tryPromote);
            uint.TryParse(PlayerPrefs.GetString("TryAdditional"), out tryAdditional);
            uint.TryParse(PlayerPrefs.GetString("TryReinforce"), out tryReinforce);
            uint.TryParse(PlayerPrefs.GetString("TryMagic"), out tryMagic);
            uint.TryParse(PlayerPrefs.GetString("TrySoul"), out trySoul);
            uint.TryParse(PlayerPrefs.GetString("TryRefine"), out tryRefine);
            // 일일 초기화
            DateTime.TryParse(PlayerPrefs.GetString("SaveDay"),  out saveDay);
            if(saveDay == DateTime.MinValue)
            {
                saveDay = Managers.Etc.GetServerTime().Date;
            }
            ResetRecordDayData(dayValGroups, dayValGroupsString);
            uint.TryParse(PlayerPrefs.GetString("DayAttendance"), out dayAttendance);
            uint.TryParse(PlayerPrefs.GetString("DayTryPromote"), out dayTryPromote);
            uint.TryParse(PlayerPrefs.GetString("DayTryMagic"), out dayTryMagic);
            uint.TryParse(PlayerPrefs.GetString("DayTryReinforce"), out dayTryReinforce);
            uint.TryParse(PlayerPrefs.GetString("DayGetBonus"), out dayGetBonus);
            uint.TryParse(PlayerPrefs.GetString("DaySeeAds"), out daySeeAds);

            // 주간 초기화
            DateTime.TryParse(PlayerPrefs.GetString("SaveWeek"), out saveWeek);
            if (saveWeek == DateTime.MinValue)
            {
                saveWeek = Managers.Etc.GetServerTime().Date;
            }
            ResetRecordWeekData(weekValGroups, weekValGroupsString);
            uint.TryParse(PlayerPrefs.GetString("WeekAttendance"), out weekAttendance);
            uint.TryParse(PlayerPrefs.GetString("WeekTryPromote"), out weekTryPromote);
            uint.TryParse(PlayerPrefs.GetString("WeekTryMagic"), out weekTryMagic);
            uint.TryParse(PlayerPrefs.GetString("WeekTryReinforce"), out weekTryReinforce);
            uint.TryParse(PlayerPrefs.GetString("WeekGetBonus"), out weekGetBonus);
            uint.TryParse(PlayerPrefs.GetString("WeekSeeAds"), out weekSeeAds);

            return;
        }
        PlayerPrefs.SetString("UserID", _userInDate);
        PlayerPrefs.DeleteKey("Tutorial");
        PlayerPrefs.DeleteKey("UseGold");
        PlayerPrefs.DeleteKey("GetGold");
        PlayerPrefs.DeleteKey("UseDiamond");
        PlayerPrefs.DeleteKey("GetDiamond");
        PlayerPrefs.DeleteKey("RegisterItem");
        PlayerPrefs.DeleteKey("DisassembleItem");
        PlayerPrefs.DeleteKey("ProduceWeapon");
        PlayerPrefs.DeleteKey("AdvanceProduceWeapon");
        PlayerPrefs.DeleteKey("TryPromote");
        PlayerPrefs.DeleteKey("TryAdditional");
        PlayerPrefs.DeleteKey("TryReinforce");
        PlayerPrefs.DeleteKey("TryMagic");
        PlayerPrefs.DeleteKey("TrySoul");
        PlayerPrefs.DeleteKey("TryRefine");

        // 일일
        PlayerPrefs.DeleteKey("SaveDay");
        PlayerPrefs.DeleteKey("DayAttendance");
        PlayerPrefs.DeleteKey("DayTryPromote");
        PlayerPrefs.DeleteKey("DayTryMagic");
        PlayerPrefs.DeleteKey("DayTryReinforce");
        PlayerPrefs.DeleteKey("DayGetBonus");
        PlayerPrefs.DeleteKey("DaySeeAds");
        // 주간
        PlayerPrefs.DeleteKey("SaveWeek");
        PlayerPrefs.DeleteKey("WeekAttendance");
        PlayerPrefs.DeleteKey("WeekTryPromote");
        PlayerPrefs.DeleteKey("WeekTryMagic");
        PlayerPrefs.DeleteKey("WeekTryReinforce");
        PlayerPrefs.DeleteKey("WeekGetBonus");
        PlayerPrefs.DeleteKey("WeekSeeAds");

        tutorial = 0;
        useGold = 0;
        getGold = 0;
        useDiamond = 0;
        getDiamond = 0;
        getItem = 0;
        disassembleItem = 0;
        produceWeapon = 0;
        advanceProduceWeapon = 0;
        tryPromote = 0;
        tryAdditional = 0;
        tryReinforce = 0;
        tryMagic = 0;
        trySoul = 0;
        tryRefine = 0;

        // 일일
        saveDay = DateTime.MinValue;
        dayAttendance = 0;
        dayTryPromote = 0;
        dayTryMagic = 0;
        dayTryReinforce = 0;
        dayGetBonus = 0;
        daySeeAds = 0;
        // 주간
        saveWeek = DateTime.MinValue;
        weekAttendance = 0;
        weekTryPromote = 0;
        weekTryMagic = 0;
        weekTryReinforce = 0;
        weekGetBonus = 0;
        weekSeeAds = 0;
    }

    public void TutorialClearRecord(uint _clearCheck)
    {
        if(_clearCheck == 10)
        {
            tutorial = 1;
            PlayerPrefs.SetString("Tutorial", tutorial.ToString());
        }
    }

    public void ModifyGoldRecord(int _gold)
    {
        if (_gold == 0) return;
        else if (_gold > 0)
        {
            getGold += (ulong)_gold;
            PlayerPrefs.SetString("GetGold", getGold.ToString());
            getGoldEvent?.Invoke();
        }
        else
        {
            useGold += (ulong)Mathf.Abs(_gold);
            PlayerPrefs.SetString("UseGold", useGold.ToString());
            useGoldEvent?.Invoke();
        }
    }

    public void ModifyDiamondRecord(int _diamond)
    {
        if (_diamond == 0) return;
        else if (_diamond > 0)
        {
            getDiamond += (ulong)_diamond;
            PlayerPrefs.SetString("GetDiamond", getDiamond.ToString());
            getDiamondEvent?.Invoke();
        }
        else
        {
            useDiamond += (ulong)Mathf.Abs(_diamond);
            PlayerPrefs.SetString("UseDiamond", useDiamond.ToString());
            useDiamondEvent?.Invoke();
        }
    }

    public void ModifyGetItemRecord()
    {
        getItem++;
        PlayerPrefs.SetString("GetItem", getItem.ToString());
        getItemEvent?.Invoke();
    }

    public void ModifyProduceRecord(int _count)
    {
        if (_count <= 0) return;
        produceWeapon += (uint)_count;
        PlayerPrefs.SetString("ProduceWeapon", produceWeapon.ToString());
        produceWeaponEvent?.Invoke();
    }

    public void ModifyAdvanceProduceRecord(int _count)
    {
        if (_count <= 0) return;
        advanceProduceWeapon += (uint)_count;
        PlayerPrefs.SetString("AdvanceProduceWeapon", advanceProduceWeapon.ToString());
        advanceProduceWeaponEvent?.Invoke();
    }

    public void ModifyTryPromoteRecord()
    {
        tryPromote ++;
        PlayerPrefs.SetString("TryPromote", tryPromote.ToString());
        tryPromoteEvent?.Invoke();
    }

    public void ModifyTryAdditionalRecord()
    {
        tryAdditional ++;
        PlayerPrefs.SetString("TryAdditional", tryAdditional.ToString());
        tryAdditionalEvent?.Invoke();
    }

    public void ModifyTryReinforceRecord()
    {
        tryReinforce ++;
        PlayerPrefs.SetString("TryReinforce", tryReinforce.ToString());
        tryReinforceEvent?.Invoke();
    }

    public void ModifyTryMagicRecord()
    {
        tryMagic ++;
        PlayerPrefs.SetString("TryMagic", tryMagic.ToString());
        tryMagicEvent?.Invoke();
    }

    public void ModifyTrySoulRecord()
    {
        trySoul ++;
        PlayerPrefs.SetString("TrySoul", trySoul.ToString());
        trySoulEvent?.Invoke();
    }

    public void ModifyTryRefineRecord()
    {
        tryRefine ++;
        PlayerPrefs.SetString("TryRefine", tryRefine.ToString());
        tryRefineEvent?.Invoke();
    }


    // 일일
    public void ModifyDayTryReinforceRecord()
    {
        dayTryReinforce++;
        PlayerPrefs.SetString("DayTryReinforce", dayTryReinforce.ToString());
        dayTryReinforceEvent?.Invoke();
    }

    public void ModifyDayTryPromoteRecord()
    {
        dayTryPromote++;
        PlayerPrefs.SetString("DayTryPromote", dayTryPromote.ToString());
        dayTryPromoteEvent?.Invoke();
    }

    public void ModifyDayTryMagicRecord()
    {
        dayTryMagic++;
        PlayerPrefs.SetString("DayTryMagic", dayTryMagic.ToString());
        dayTryMagicEvent?.Invoke();
    }

    public void ModifyDayAttendanceRecord()
    {
        dayAttendance++;
        PlayerPrefs.SetString("DayAttendance", dayAttendance.ToString());
        dayAttendanceEvent?.Invoke();
    }

    public void ModifyDayGetBonusRecord()
    {
        dayGetBonus++;
        PlayerPrefs.SetString("DayGetBonus", dayGetBonus.ToString());
        dayGetBonusEvent?.Invoke();
    }

    public void ModifyDaySeeAdsRecord()
    {
        daySeeAds++;
        PlayerPrefs.SetString("DaySeeAds", daySeeAds.ToString());
        daySeeAdsEvent?.Invoke();
    }

    public void ResetRecordDayData(uint[] _dayRecord, string[] _dayType)
    {
        if(saveDay.Date != Managers.Etc.GetServerTime().Date)
        {
            for(int i = 0; i < _dayRecord.Length; i++)
            {
                _dayRecord[i] = 0;
                PlayerPrefs.SetString(_dayType[i], _dayRecord[i].ToString());
            }
            PlayerPrefs.SetString("SaveDay", saveDay.ToString());
        }
    }

    // 주간
    public void ModifyWeekTryReinforceRecord()
    {
        weekTryReinforce++;
        PlayerPrefs.SetString("WeekTryReinforce", weekTryReinforce.ToString());
        weekTryReinforceEvent?.Invoke();
    }

    public void ModifyWeekTryPromoteRecord()
    {
        weekTryPromote++;
        PlayerPrefs.SetString("WeekTryPromote", weekTryPromote.ToString());
        weekTryPromoteEvent?.Invoke();
    }

    public void ModifyWeekTryMagicRecord()
    {
        weekTryMagic++;
        PlayerPrefs.SetString("WeekTryMagic", weekTryMagic.ToString());
        weekTryMagicEvent?.Invoke();
    }

    public void ModifyWeekAttendanceRecord()
    {
        weekAttendance++;
        PlayerPrefs.SetString("WeekAttendance", weekAttendance.ToString());
        Debug.Log(WeekAttendance);
        weekAttendanceEvent?.Invoke();
    }

    public void ModifyWeekGetBonusRecord()
    {
        weekGetBonus++;
        PlayerPrefs.SetString("WeekGetBonus", weekGetBonus.ToString());
        weekGetBonusEvent?.Invoke();
    }

    public void ModifyWeekSeeAdsRecord()
    {
        weekSeeAds++;
        PlayerPrefs.SetString("WeekSeeAds", weekSeeAds.ToString());
        weekSeeAdsEvent?.Invoke();
    }

    public void ResetRecordWeekData(uint[] _weekRecord, string[] _weekType)
    {
        System.Globalization.CultureInfo cultureInfo = System.Globalization.CultureInfo.CurrentCulture;
        System.Globalization.CalendarWeekRule calenderWeekRule = cultureInfo.DateTimeFormat.CalendarWeekRule;
        int saveWeeks = 0;
        saveWeeks = cultureInfo.Calendar.GetWeekOfYear(saveWeek, calenderWeekRule, saveWeek.DayOfWeek);
        int serverData = 0;
        serverData = cultureInfo.Calendar.GetWeekOfYear(Managers.Etc.GetServerTime(), calenderWeekRule, Managers.Etc.GetServerTime().DayOfWeek);
        if (saveWeek.Date != Managers.Etc.GetServerTime().Date) 
        {
            if (saveWeeks != serverData) 
            {
                if(Managers.Etc.GetServerTime().DayOfWeek >= DayOfWeek.Monday)
                {
                    for (int i = 0; i < _weekRecord.Length; i++)
                    {
                        _weekRecord[i] = 0;
                        PlayerPrefs.SetString(_weekType[i], _weekRecord[i].ToString());
                    }
                    saveWeek = Managers.Etc.GetServerTime().Date;
                    PlayerPrefs.SetString("SaveWeek", saveWeek.ToString());
                }
            }
        }
    }
}


// 암호화 예제
// using UnityEngine;
// using System.Security.Cryptography;
// using System.Text;
// using System.IO;
//  
// public class SecurityPlayerPrefs
// {
//     // http://www.codeproject.com/Articles/769741/Csharp-AES-bits-Encryption-Library-with-Salt
//     // http://ikpil.com/1342

//     private static string _saltForKey;

//     private static byte[] _keys;
//     private static byte[] _iv;
//     private static int keySize = 256;
//     private static int blockSize = 128;
//     private static int _hashLen = 32;

//     static SecurityPlayerPrefs()
//     {
//         // 8 바이트로 하고, 변경해서 쓸것
//         byte[] saltBytes = new byte[] { 25, 36, 77, 51, 43, 14, 75, 93 };

//         // 길이 상관 없고, 키를 만들기 위한 용도로 씀
//         string randomSeedForKey = "5b6fcb4aaa0a42acae649eba45a506ec";

//         // 길이 상관 없고, aes에 쓸 key 와 iv 를 만들 용도
//         string randomSeedForValue = "2e327725789841b5bb5c706d6b2ad897";

//         {
//             Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(randomSeedForKey, saltBytes, 1000);
//             _saltForKey = System.Convert.ToBase64String(key.GetBytes(blockSize / 8));
//         }

//         {
//             Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(randomSeedForValue, saltBytes, 1000);
//             _keys = key.GetBytes(keySize / 8);
//             _iv = key.GetBytes(blockSize / 8);
//         }
//     }

//     public static string MakeHash(string original)
//     {
//         using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
//         {
//             byte[] bytes = System.Text.Encoding.UTF8.GetBytes(original);
//             byte[] hashBytes = md5.ComputeHash(bytes);

//             string hashToString = "";
//             for (int i = 0; i < hashBytes.Length; ++i)
//                 hashToString += hashBytes[i].ToString("x2");

//             return hashToString;
//         }
//     }

//     public static byte[] Encrypt(byte[] bytesToBeEncrypted)
//     {
//         using (RijndaelManaged aes = new RijndaelManaged())
//         {
//             aes.KeySize = keySize;
//             aes.BlockSize = blockSize;

//             aes.Key = _keys;
//             aes.IV = _iv;

//             aes.Mode = CipherMode.CBC;
//             aes.Padding = PaddingMode.PKCS7;

//             using (ICryptoTransform ct = aes.CreateEncryptor())
//             {
//                 return ct.TransformFinalBlock(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
//             }
//         }
//     }

//     public static byte[] Decrypt(byte[] bytesToBeDecrypted)
//     {
//         using (RijndaelManaged aes = new RijndaelManaged())
//         {
//             aes.KeySize = keySize;
//             aes.BlockSize = blockSize;

//             aes.Key = _keys;
//             aes.IV = _iv;

//             aes.Mode = CipherMode.CBC;
//             aes.Padding = PaddingMode.PKCS7;

//             using (ICryptoTransform ct = aes.CreateDecryptor())
//             {
//                 return ct.TransformFinalBlock(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
//             }
//         }
//     }

//     public static string Encrypt(string input)
//     {
//         byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
//         byte[] bytesEncrypted = Encrypt(bytesToBeEncrypted);

//         return System.Convert.ToBase64String(bytesEncrypted);
//     }

//     public static string Decrypt(string input)
//     {
//         byte[] bytesToBeDecrypted = System.Convert.FromBase64String(input);
//         byte[] bytesDecrypted = Decrypt(bytesToBeDecrypted);

//         return Encoding.UTF8.GetString(bytesDecrypted);
//     }

//     private static void SetSecurityValue(string key, string value)
//     {
//         string hideKey = MakeHash(key + _saltForKey);
//         string encryptValue = Encrypt(value + MakeHash(value));

//         PlayerPrefs.SetString(hideKey, encryptValue);
//     }

//     private static string GetSecurityValue(string key)
//     {
//         string hideKey = MakeHash(key + _saltForKey);

//         string encryptValue = PlayerPrefs.GetString(hideKey);
//         if (true == string.IsNullOrEmpty(encryptValue))
//             return string.Empty;

//         string valueAndHash = Decrypt(encryptValue);
//         if (_hashLen > valueAndHash.Length)
//             return string.Empty;

//         string savedValue = valueAndHash.Substring(0, valueAndHash.Length - _hashLen);
//         string savedHash = valueAndHash.Substring(valueAndHash.Length - _hashLen);

//         if (MakeHash(savedValue) != savedHash)
//             return string.Empty;

//         return savedValue;
//     }

//     public static void DeleteKey(string key)
//     {
//         PlayerPrefs.DeleteKey(MakeHash(key + _saltForKey));
//     }

//     public static void DeleteAll()
//     {
//         PlayerPrefs.DeleteAll();
//     }

//     public static void Save()
//     {
//         PlayerPrefs.Save();
//     }

//     public static void SetInt(string key, int value)
//     {
//         SetSecurityValue(key, value.ToString());
//     }

//     public static void SetLong(string key, long value)
//     {
//         SetSecurityValue(key, value.ToString());
//     }

//     public static void SetFloat(string key, float value)
//     {
//         SetSecurityValue(key, value.ToString());
//     }

//     public static void SetString(string key, string value)
//     {
//         SetSecurityValue(key, value);
//     }

//     public static int GetInt(string key, int defaultValue)
//     {
//         string originalValue = GetSecurityValue(key);
//         if (true == string.IsNullOrEmpty(originalValue))
//             return defaultValue;

//         int result = defaultValue;
//         if (false == int.TryParse(originalValue, out result))
//             return defaultValue;

//         return result;
//     }

//     public static long GetLong(string key, long defaultValue)
//     {
//         string originalValue = GetSecurityValue(key);
//         if (true == string.IsNullOrEmpty(originalValue))
//             return defaultValue;

//         long result = defaultValue;
//         if (false == long.TryParse(originalValue, out result))
//             return defaultValue;

//         return result;
//     }

//     public static float GetFloat(string key, float defaultValue)
//     {
//         string originalValue = GetSecurityValue(key);
//         if (true == string.IsNullOrEmpty(originalValue))
//             return defaultValue;

//         float result = defaultValue;
//         if (false == float.TryParse(originalValue, out result))
//             return defaultValue;

//         return result;
//     }

//     public static string GetString(string key, string defaultValue)
//     {
//         string originalValue = GetSecurityValue(key);
//         if (true == string.IsNullOrEmpty(originalValue))
//             return defaultValue;

//         return originalValue;
//     }
// }

