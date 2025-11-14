using UnityEngine;

public class GameManger : MonoBehaviour
{
    Inventory inventory;
    private void Awake()
    {
        inventory = FindAnyObjectByType<Inventory>();
        inventory.OnCoinsAndLevelUpdate += (coins, level) =>
        {
            if (coins >= 1000001)
            {
                SceneLoadManager.Instance.LoadRegularScene("WinGameScene");
            }
        };
    }

}
