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
    ulong useGold;
    public ulong UseGold => useGold;
    ulong getGold;
    public ulong GetGold => getGold;
    ulong useDiamond;
    public ulong UseDiamond => useDiamond;
    ulong getDiamond;
    public ulong GetDiamond => getDiamond;
    // uint getItem;
    // public uint GetItem => getItem;
    uint registerItem;
    public uint RegisterItem => registerItem;
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
    uint attendance;
    public uint Attendance => attendance;
    ulong getBonus;
    public ulong GetBonus => getBonus;
    uint seeAds;
    public uint SeeAds => seeAds;
    public Action levelUpEvent;
    public Action getGoldEvent;
    public Action useGoldEvent;
    public Action useDiamondEvent;
    public Action getDiamondEvent;
    // public Action getItemEvent;
    public Action registerItemEvent;
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

    public void LoadOrInitRecord(string _userInDate)
    {
        userID = PlayerPrefs.GetString("UserID");
        Debug.Log($"loaded userID : {userID} / indate : {_userInDate}");
        
        if (_userInDate == userID)
        {
            ulong.TryParse(PlayerPrefs.GetString("UseGold"), out useGold);
            // Debug.Log($"UsedGold : {useGold}");
            ulong.TryParse(PlayerPrefs.GetString("GetGold"), out getGold);
            // Debug.Log($"GetedGold : {getGold}");
            ulong.TryParse(PlayerPrefs.GetString("UseDiamond"), out useDiamond);
            ulong.TryParse(PlayerPrefs.GetString("GetDiamond"), out getDiamond);
            uint.TryParse(PlayerPrefs.GetString("RegisterItem"), out registerItem);
            uint.TryParse(PlayerPrefs.GetString("DisassembleItem"), out disassembleItem);
            uint.TryParse(PlayerPrefs.GetString("ProduceWeapon"), out produceWeapon);
            uint.TryParse(PlayerPrefs.GetString("AdvanceProduceWeapon"), out advanceProduceWeapon);
            uint.TryParse(PlayerPrefs.GetString("TryPromote"), out tryPromote);
            uint.TryParse(PlayerPrefs.GetString("TryAdditional"), out tryAdditional);
            uint.TryParse(PlayerPrefs.GetString("TryReinforce"), out tryReinforce);
            uint.TryParse(PlayerPrefs.GetString("TryMagic"), out tryMagic);
            uint.TryParse(PlayerPrefs.GetString("TrySoul"), out trySoul);
            uint.TryParse(PlayerPrefs.GetString("TryRefine"), out tryRefine);
            uint.TryParse(PlayerPrefs.GetString("Attendance"), out attendance);
            ulong.TryParse(PlayerPrefs.GetString("GetBonus"), out getBonus);
            uint.TryParse(PlayerPrefs.GetString("SeeAds"), out seeAds);
            // public uint GetItem = PlayerPrefs.GetString("UserID");

            return;
        }
        PlayerPrefs.SetString("UserID", _userInDate);
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
        PlayerPrefs.DeleteKey("Attendance");
        PlayerPrefs.DeleteKey("GetBonus");
        PlayerPrefs.DeleteKey("SeeAds");
        useGold = 0;
        getGold = 0;
        useDiamond = 0;
        getDiamond = 0;
        registerItem = 0;
        disassembleItem = 0;
        produceWeapon = 0;
        advanceProduceWeapon = 0;
        tryPromote = 0;
        tryAdditional = 0;
        tryReinforce = 0;
        tryMagic = 0;
        trySoul = 0;
        tryRefine = 0;
        attendance = 0;
        getBonus = 0;
        seeAds = 0;
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

    // uint.TryParse(PlayerPrefs.GetString("RegisterItem"), out registerItem);
    // uint.TryParse(PlayerPrefs.GetString("DisassembleItem"), out disassembleItem);
    // uint.TryParse(PlayerPrefs.GetString("Attendance"), out attendance);
    // ulong.TryParse(PlayerPrefs.GetString("GetBonus"), out getBonus);
    // uint.TryParse(PlayerPrefs.GetString("SeeAds"), out seeAds);
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
