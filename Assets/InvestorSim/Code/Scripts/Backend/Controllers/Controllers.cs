using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CorporationsController))]
[RequireComponent(typeof(StockExchangeController))]
[RequireComponent(typeof(TransactionController))]
[RequireComponent(typeof(NewsController))]
[RequireComponent(typeof(AssetController))]
[RequireComponent(typeof(OfferController))]
[RequireComponent(typeof(StatisticController))]
[RequireComponent(typeof(CurrencyEchangeController))]
[RequireComponent(typeof(EventGenerator))]

/// <summary>
/// Представляет из себя таблицу с указателями на все контроллеры игры
/// </summary>
public class Controllers : MonoBehaviour
{
    /// <summary>
    /// Контроллер управления корпорациями игры
    /// </summary>
    public static CorporationsController Corporations { get; private set; }
    /// <summary>
    /// Контроллер управления фондовыми биржами в игре
    /// </summary>
    public static StockExchangeController StockExchange { get; private set; }
	/// <summary>
	/// Контроллер управления транзакциями в игре
	/// </summary>
	public static TransactionController Transactions { get; private set; }
	/// <summary>
	/// Контроллер игровых событий
	/// </summary>
	public static NewsController News { get; private set; }
	/// <summary>
	/// Контроллер изменения цен на активы
	/// </summary>
	public static AssetController Assets { get; private set; }
	/// <summary>
	/// Контроллер предложений вкладов и тп
	/// </summary>
	public static OfferController Offers { get; private set; }
	/// <summary>
	/// Контроллер статистики игрока
	/// </summary>
	public static StatisticController Statistic { get; private set; }
	/// <summary>
	/// Контроллер предложений вкладов и тп
	/// </summary>
	public static CurrencyEchangeController CurrencyExchange { get; private set; }
	/// <summary>
	/// Генератор событий игры
	/// </summary>
	public static EventGenerator EventGenerator { get; private set; }


	private List<IGameController> _startSequence;


    private void Awake()
    {
		//DontDestroyOnLoad(gameObject);

		Corporations = GetComponent<CorporationsController>();
		StockExchange = GetComponent<StockExchangeController>();
		Transactions = GetComponent<TransactionController>();
		News = GetComponent<NewsController>();
		Offers = GetComponent<OfferController>();
		Statistic = GetComponent<StatisticController>();
		Assets = GetComponent<AssetController>();
		CurrencyExchange = GetComponent<CurrencyEchangeController>();
		EventGenerator = GetComponent<EventGenerator>();

		_startSequence = new List<IGameController>();
		_startSequence.Add(Corporations);
		_startSequence.Add(EventGenerator);
		_startSequence.Add(StockExchange);
		_startSequence.Add(Transactions);
		_startSequence.Add(News);
		_startSequence.Add(Offers);
		_startSequence.Add(Assets);
		_startSequence.Add(CurrencyExchange);
		_startSequence.Add(Statistic);

		Messenger.AddListener(StartupNotice.ALL_MANAGERS_STARTED, Initialize);
    }

    private void OnDestroy()
    {
		Messenger.RemoveListener(StartupNotice.ALL_MANAGERS_STARTED, Initialize);
	}

	private void Initialize()
    {
		Debug.Log("Запуск сопрограммы StartupControllers");
		StartCoroutine(this.StartupControllers());
	}

	/// <summary>
	/// Запуск всех контроллеров, привязанных к этому контроллеру
	/// </summary>
	/// <returns>Перечислитель</returns>
	private IEnumerator StartupControllers()
	{
		Debug.Log("Запуск контроллеров...");

		foreach (IGameController controller in _startSequence)
		{
			StartCoroutine(controller.Startup());
		}

		yield return null;

		int numModules = _startSequence.Count;
		int numReady = 0;

		while (numReady < numModules)
		{
			int lastReady = numReady;
			numReady = 0;

			foreach (IGameController manager in _startSequence)
			{
				if (manager.status == ControllerStatus.Started)
				{
					numReady++;
				}
			}

			if (numReady > lastReady)
			{
				Debug.Log("Progress: " + numReady + "/" + numModules);
				// Messenger<int, int>.Broadcast(StartupEvent.CONTROLLERS_PROGRESS, numReady, numModules);
			}

			yield return null;
		}

		Debug.Log("All controllers started up");
		Messenger.Broadcast(StartupNotice.ALL_CONTROLLERS_STARTED);
	}
}