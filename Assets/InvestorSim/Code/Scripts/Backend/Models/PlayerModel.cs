using System;
using System.Linq;
using System.Collections.Generic;

[Serializable]

/// <summary>
/// Хранит данные игрока
/// </summary>
public class PlayerModel : IGameModel
{
    public PlayerModel()
    {
        Messenger.AddListener(StartupNotice.CURRENCY_CONTROLLER_STARTED, this.CurrencyControllerStartedTrigger);
        ResetAssets();
        ResetInvestments();
        ResetDeptors();
    }
    public PlayerModel(string currency, decimal money) : this()
    {
        Wallet[currency] = money;
    }



    public string Name { get; set; }

    public Dictionary<string, decimal> Wallet { get; set; }

    public Dictionary<AssetType, List<AssetPurchase>> Assets { get; set; }

    public List<Investment> Investments { get; set; }

    public List<Dept> Deptors { get; set; }



    public void ResetWallet()
    {
        Wallet = new Dictionary<string, decimal>();

        Wallet.Add(Controllers.CurrencyExchange.RUB, 50000);
        Wallet.Add(Controllers.CurrencyExchange.USD, 0);
        Wallet.Add(Controllers.CurrencyExchange.EUR, 0);
    }

    public void ResetAssets()
    {
        Assets = new Dictionary<AssetType, List<AssetPurchase>>();

        Assets.Add(AssetType.Shares, new List<AssetPurchase>());
        Assets.Add(AssetType.Bonds, new List<AssetPurchase>());
    }

    public void ResetInvestments()
    {
        Investments = new List<Investment>();
    }

    public void ResetDeptors()
    {
        Deptors = new List<Dept>();
    }


    private void CurrencyControllerStartedTrigger()
    {
        // TODO: Убрать когда будет улучшенный запуск контроллеров и менеджеров

        ResetWallet();
        Messenger.RemoveListener(StartupNotice.CURRENCY_CONTROLLER_STARTED, this.CurrencyControllerStartedTrigger);
    }


    public object Clone()
    {
        return new PlayerModel()
        {
            Name = this.Name,
            Wallet = this.Wallet.ToDictionary(pair => pair.Key.Clone() as string, pair => pair.Value),
            Assets = this.Assets.ToDictionary(pair => pair.Key, pair => pair.Value.Select(x => x.Clone() as AssetPurchase).ToList()),
            Investments = this.Investments.Select(x => x.Clone() as Investment).ToList(),
            Deptors = this.Deptors.Select(x => x.Clone() as Dept).ToList()
        };
    }
}
