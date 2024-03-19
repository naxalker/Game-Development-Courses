using System.Collections;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationController : MonoBehaviour
{
    private const string GameSceneName = "Menu";

    [SerializeField] private ClientSingleton _clientPrefab;
    [SerializeField] private HostSingleton _hostPrefab;
    [SerializeField] private ServerSingleton _serverPrefab;
    [SerializeField] private NetworkObject _playerPrefab;

    private ApplicationData _appData;

    private async void Start()
    {
        DontDestroyOnLoad(gameObject);

        await LaunchInMode(SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null);
    }

    private async Task LaunchInMode(bool isDedicatedServer)
    {
        if (isDedicatedServer)
        {
            Application.targetFrameRate = 60;

            _appData = new ApplicationData();

            ServerSingleton serverSingleton = Instantiate(_serverPrefab);
            
            StartCoroutine(LoadGameSceneAsync(serverSingleton));
        }
        else
        {
            HostSingleton hostSingleton = Instantiate(_hostPrefab);
            hostSingleton.CreateHost(_playerPrefab);

            ClientSingleton clientSingleton = Instantiate(_clientPrefab);
            bool authenticated = await clientSingleton.CreateClient();

            if (authenticated)
            {
                clientSingleton.GameManager.GoToMenu();
            }
        }
    }

    private IEnumerator LoadGameSceneAsync(ServerSingleton serverSingleton)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(GameSceneName);

        while (asyncOperation.isDone == false)
        {
            yield return null;
        }

        Task createServerTask = serverSingleton.CreateServer(_playerPrefab);
        yield return new WaitUntil(() => createServerTask.IsCompleted);

        Task startServerTask = serverSingleton.GameManager.StartGameServerAsync();
        yield return new WaitUntil(() => startServerTask.IsCompleted);
    }
}
