using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleLogger : MonoBehaviour
{
    private void Awake()
    {
        Messenger.AddListener(StartupNotice.PLAYER_MANAGER_STARTING, Plug);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(StartupNotice.PLAYER_MANAGER_STARTING, Plug);
    }

    /// <summary>
    /// Пустышка, заглушка
    /// </summary>
    private void Plug()
    {

    }
}
