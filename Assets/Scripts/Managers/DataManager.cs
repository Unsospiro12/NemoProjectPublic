using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

// 스테이지 데이터
// 스테이지 클리어 여부, 스테이지 별점 갯수 저장
[System.Serializable]
public struct StageData
{
    public bool Clear;
    public int Stars;
}

[System.Serializable]
public class PlayerData
{
    // 볼륨
    public float MasterVolume;
    public float BgmVolume;
    public float SfxVolume;

    // 해금된 유닛 ID 
    public List<int> UserUnitIDs;

    // 스테이지 클리어 정보
    public StageData[] StageDatas = new StageData[3];

    // 열쇠 갯수 저장
    public int KeyCount;

}

public class DataManager : MonoBehaviour
{
    private static DataManager instance;

    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject gameObject = new GameObject();
                gameObject.name = $"{typeof(DataManager).Name}";
                instance = gameObject.AddComponent<DataManager>();
            }
            return instance;
        }
    }

    private PlayerData playerData;

    // stage 표시용
    public int currentStage;

    // 게임 첫 실행시에만 작동하도록 bool값 설정
    private bool isFirstRun = true;

    // 암호화용 키
    public string key = "Nemo";

    // 유닛 SO 모음
    public UnitStatData[] UnitSO;

    // 적 SO 모음
    public EnemyStatData[] EnemySO;

    // 씬 이동용 볼륨 값
    public float masterVolume;
    public float bgmVolume;
    public float sfxVolume;

    //카메라 이동 속도
    public float CameraMovementSpeed = 10.0f;

    // 해금된 유닛 ID 
    public List<int> UserUnitIDs;

    // 스테이지 클리어 정보
    public StageData[] StageDatas = new StageData[3];

    // 열쇠 갯수
    public int KeyCount;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // 게임 처음 실행시 데이터 초기화
        if (isFirstRun)
        {
            isFirstRun = false;

            // 만약 불러올 데이터가 없으면 게임 처음 해보는 사람이기에 기본 데이터로 세팅해줌
            if (!OnLoadData() || UserUnitIDs.Count <= 0)
            {
                // 초기 사용 가능 유닛 목록
                UserUnitIDs = Constants.GameSettings.InitialUsableUnitIDs;

                 // 스테이지 클리어 정보
                StageDatas = Constants.GameSettings.InitialStageDatas;
                KeyCount = Constants.GameSettings.InitialKeyCount;
                //KeyCount = 2;
            }
        }

        SetApplication();
    }

    private void SetApplication()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void OnSaveData()
    {
        // 데이터 저장 과정
        DataSerialize();

        var json = JsonUtility.ToJson(playerData);
        json = AESWithJava.Con.Program.Encrypt(json, key);  // 암호화
        File.WriteAllText(Application.persistentDataPath + "/UserData.json", json);
    }

    // 저장할 데이터 수집
    private void DataSerialize()
    {
        if (playerData == null)
        {
            playerData = new PlayerData();
        }

        // 볼륨 저장
        playerData.MasterVolume = masterVolume;
        playerData.BgmVolume = bgmVolume;
        playerData.SfxVolume = sfxVolume;

        // 사용 가능한 유닛 ID와 스테이지 클리어 데이터 저장
        playerData.UserUnitIDs = UserUnitIDs;
        playerData.StageDatas = StageDatas;

        // 열쇠 갯수 저장
        playerData.KeyCount = KeyCount;
    }

    public bool OnLoadData()
    {
        if (!File.Exists(Application.persistentDataPath + "/UserData.json"))
            return false;

        var jsonData = File.ReadAllText(Application.persistentDataPath + "/UserData.json");
        jsonData = AESWithJava.Con.Program.Decrypt(jsonData, key);  // 복호화
        playerData = JsonUtility.FromJson<PlayerData>(jsonData);

        DataDeserialize();
        return true;
    }

    private void DataDeserialize()
    {
        // 볼륨
        masterVolume = playerData.MasterVolume;
        bgmVolume = playerData.BgmVolume;
        sfxVolume = playerData.SfxVolume;

        UserUnitIDs = playerData.UserUnitIDs;
        StageDatas = playerData.StageDatas;

        KeyCount = playerData.KeyCount;
    }

    public void OnDeleteData()
    {
        // 볼륨 저장
        playerData.MasterVolume = 0;
        playerData.BgmVolume = 0;
        playerData.SfxVolume = 0;

        // 사용 가능한 유닛 ID와 스테이지 클리어 데이터 저장
        playerData.UserUnitIDs.Clear();
        playerData.StageDatas = new StageData[3];

        // 열쇠 갯수 저장
        playerData.KeyCount = KeyCount;

        var json = JsonUtility.ToJson(playerData);
        json = AESWithJava.Con.Program.Encrypt(json, key);  // 암호화
        File.WriteAllText(Application.persistentDataPath + "/UserData.json", json);
    }
}

// 암호화, 복호화
namespace AESWithJava.Con
{
    class Program
    {
        static void Main(string[] args)
        {
            String originalText = "plain text";
            String key = "key";
            String en = Encrypt(originalText, key);
            String de = Decrypt(en, key);

            Console.WriteLine("Original Text is " + originalText);
            Console.WriteLine("Encrypted Text is " + en);
            Console.WriteLine("Decrypted Text is " + de);
        }

        public static string Decrypt(string textToDecrypt, string key)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;

            rijndaelCipher.KeySize = 128;
            rijndaelCipher.BlockSize = 128;
            byte[] encryptedData = Convert.FromBase64String(textToDecrypt);
            byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytes = new byte[16];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length)
            {
                len = keyBytes.Length;
            }
            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = keyBytes;
            byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            return Encoding.UTF8.GetString(plainText);
        }

        public static string Encrypt(string textToEncrypt, string key)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;

            rijndaelCipher.KeySize = 128;
            rijndaelCipher.BlockSize = 128;
            byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytes = new byte[16];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length)
            {
                len = keyBytes.Length;
            }
            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = keyBytes;
            ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
            byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);
            return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
        }
    }
}