using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

[Serializable]
public class DirtData
{
    [JsonProperty] public string nameOfEntiy;
    [JsonProperty] public bool hasEntity;
    [JsonProperty] public EntityData entityData;

    public DirtData()
    {
        nameOfEntiy = null;
        hasEntity = false;
        entityData = new EntityData();
    }
    public DirtData(string nameOfEntiy, bool hasEntity)
    {
        this.nameOfEntiy = nameOfEntiy;
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
        var clone = new DirtData(nameOfEntiy, hasEntity);
        clone.entityData = entityData?.Clone();
        return clone;
    }
}