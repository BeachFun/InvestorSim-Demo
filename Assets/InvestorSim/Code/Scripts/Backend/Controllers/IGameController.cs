using System.Collections;

public interface IGameController
{
    ControllerStatus status { get; }

    IEnumerator Startup();
}
