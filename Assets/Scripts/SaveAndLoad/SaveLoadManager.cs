using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

public class SaveLoadManager : Singleton<SaveLoadManager>
{
    private string savePath;
    public static string nameFileSave = "savedata.json";
    public GameData baseGameData;
    public GameData gameData;
    public List<ISaveLoadData> saveLoadDatas = new List<ISaveLoadData>();
    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, nameFileSave);
    }

    private void Start()
    {
        LoadData();
        Debug.Log($"Save path: {savePath}");
    }

    private void OnDisable()
    {
        // Gọi tất cả ISaveLoadData để cập nhật gameData trước khi save
        foreach (var save in saveLoadDatas)
        {
            save?.Save(gameData);
        }
        SaveData();
    }

    public void RegisterSaveLoadData(ISaveLoadData saveLoadData)
    {
        if (saveLoadData != null && !saveLoadDatas.Contains(saveLoadData))
            saveLoadDatas.Add(saveLoadData);
    }

    public void SaveData()
    {
        if (gameData == null)
        {
            Debug.LogWarning("⚠️ Không có dữ liệu để lưu!");
            return;
        }

        try
        {
            string json = JsonConvert.SerializeObject(gameData, Formatting.Indented);
            File.WriteAllText(savePath, json);
            Debug.Log($"💾 Đã lưu game vào: {savePath}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"❌ Lưu dữ liệu thất bại: {ex.Message}");
        }
    }

    public void LoadData()
    {
        if (!File.Exists(savePath) || new FileInfo(savePath).Length == 0)
        {
            Debug.LogWarning("⚠️ Chưa có file lưu, tạo dữ liệu mới.");
            gameData = baseGameData.Clone();
            return;
        }

        try
        {
            Debug.Log(saveLoadDatas.Count);
            string json = File.ReadAllText(savePath);
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogWarning("⚠️ File lưu rỗng, tạo dữ liệu mới.");
                gameData = baseGameData.Clone();
                return;
            }

            Debug.Log("📄 JSON loaded:\n" + json);

            gameData = JsonConvert.DeserializeObject<GameData>(json) ?? baseGameData.Clone();;
            Debug.Log("📂 Đã load dữ liệu game thành công!");

            // Gọi tất cả ISaveLoadData để load vào runtime
            foreach (var saveLoadData in saveLoadDatas)
            {
                if (saveLoadData == null) continue;
                saveLoadData.Load(gameData);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"❌ Load dữ liệu thất bại: {ex.Message}");
            gameData = baseGameData.Clone();
        }
    }

    [ContextMenu("Delete Save")]
    public void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("🗑️ Đã xóa file lưu.");
        }
    }
}
