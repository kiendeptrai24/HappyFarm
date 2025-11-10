[System.Serializable]
public class EntityData
{
    public string name;
    public float growTime;         // Thời gian lớn tới chín
    public float timeToHarvest;         // Thời gian lớn tới chín
    public float witherDelay;      // Sau bao lâu không thu hoạch thì héo
    public int yieldAmount;        // Sản lượng khi thu hoạch
    public int lifeCycles;         // Số lần có thể trồng lại (nếu tái sinh)
    public int price;              // Giá bán
}