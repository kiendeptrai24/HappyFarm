using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button startBtn;
    [SerializeField] private Button exitBtn;

    public static string nameFileSave = "savedata.json";
    private string savePath;

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, nameFileSave);

        // Nếu chưa có file save thì ẩn nút Continue
        if (!File.Exists(savePath))
        {
            Debug.Log("⚠️ Chưa có file lưu, ẩn nút Continue.");
            continueBtn.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        // Nút Continue
        continueBtn.onClick.AddListener(() =>
        {
            SceneLoadManager.Instance.LoadRegularScene("GameScene", true);
        });

        // Nút Start (xoá save cũ nếu có)
        startBtn.onClick.AddListener(() =>
        {
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }
            SceneLoadManager.Instance.LoadRegularScene("GameScene", true);
        });

        // Nút Exit
        exitBtn.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
