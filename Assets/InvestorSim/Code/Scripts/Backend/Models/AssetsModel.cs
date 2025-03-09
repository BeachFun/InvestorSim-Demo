using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Только хранит данные истории цен активов
/// </summary>
public class AssetsModel : IGameModel
{
    public AssetsModel()
    {
        Assets = new();
    }
    internal AssetsModel(Dictionary<object, List<(decimal, DateTime)>> data)
    {
        var assets = new Dictionary<IAsset, List<(decimal, DateTime)>>();

        foreach (var item in data)
        {
            if (item.Key is Shares)
                assets.Add(item.Key as Shares, item.Value);
            if (item.Key is Bond)
                assets.Add(item.Key as Bond, item.Value);
        }

        Assets = assets;
    }


    /// <summary>
    /// Представляет список активов с историей цен | Данные состоят из: цена / день изменения
    /// </summary>
    public Dictionary<IAsset, List<(decimal, DateTime)>> Assets { get; private set; }

    public void AddAsset(IAsset uniqueAsset)
    {
        Assets.Add(uniqueAsset, new List<(decimal, DateTime)>());
    }


    /// <summary>
    /// Возвращает копию данных модели в 
    /// </summary>
    public object Clone()
    {
        //UnityEngine.Debug.Log("AM save started");
        var data = new Dictionary<object, List<(decimal, DateTime)>>();

        foreach (var item in this.Assets)
        {
            if (item.Key is Shares)
                data.Add((item.Key as Shares).Clone() as Shares, item.Value.ToList());
            if (item.Key is Bond)
                data.Add((item.Key as Bond).Clone() as Bond, item.Value.ToList());
        }
        //UnityEngine.Debug.Log("AM save end");
        return data;
    }
}