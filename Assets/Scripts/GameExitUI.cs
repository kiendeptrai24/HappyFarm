using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameExitUI : MonoBehaviour
{
    private string savePath;
    public static string nameFileSave = "savedata.json";
    [SerializeField] private Button exitButton;
    private void Start()
    {
        savePath = Path.Combine(Application.persistentDataPath, nameFileSave);
        exitButton.onClick.AddListener(() =>
        {
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }
            SceneLoadManager.Instance.LoadRegularScene("MenuScene", true);
        });
    }
}
