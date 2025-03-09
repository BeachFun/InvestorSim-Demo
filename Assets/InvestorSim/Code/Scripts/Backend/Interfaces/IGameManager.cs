using System.Collections;

public interface IGameManager
{
	ManagerStatus status {get;}

    IEnumerator Startup(/*NetworkService service*/);
}
