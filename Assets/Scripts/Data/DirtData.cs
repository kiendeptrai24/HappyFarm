using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

[Serializable]
public class DirtData
{
    [JsonProperty] public ProductSaleAble type;
    [JsonProperty] public bool hasEntity;
    [JsonProperty] public EntityData entityData;

    public DirtData()
    {
        hasEntity = false;
        entityData = new EntityData();
    }
    public DirtData(ProductSaleAble type, bool hasEntity)
    {
        this.type = type;
        this.hasEntity = hasEntity;
    }

    // 🧩 Bắt sự kiện sau khi JSON deserialize xong
    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        entityData ??= new EntityData();
    }

    public DirtData Clone()
    {
        var clone = new DirtData(type, hasEntity);
        clone.entityData = entityData?.Clone();
        return clone;
    }
}