using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
	[SerializeField] private string[] _gameScenes;
	
	[SerializeField] private string _uiScene;
	[SerializeField] private string _loadingScreenScene;
	
	private string _scene; //The scene to load
	
	//Async operations for loading screen and other scenes
	private AsyncOperation[] _sceneOperations;
	private AsyncOperation _loadingSceneOperation;
	
	public static SceneManagerScript instance { get; private set; }
	
	private void Awake()
	{
		ManagerInstances();
	}
	
	private void ManagerInstances()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}
	
	//function to load scenes using LoadSceneCoroutine
	public void LoadGameScene() 
	{
		StartCoroutine(LoadGameSceneCoroutine());
	}
	
	public void LoadNeutralScene(string _sceneName)
	{
		StartCoroutine(LoadNeutralSceneCoroutine(_sceneName));
	}

	//Coroutine to select and load random game scene
	private IEnumerator LoadGameSceneCoroutine()
	{
		_loadingSceneOperation = SceneManager.LoadSceneAsync(_loadingScreenScene);
		yield return _loadingSceneOperation;
		
		_scene = _gameScenes[Random.Range(0, _gameScenes.Length)];

		_sceneOperations = new AsyncOperation[2];
		_sceneOperations[0] = SceneManager.LoadSceneAsync(_scene, LoadSceneMode.Additive);
		_sceneOperations[1] = SceneManager.LoadSceneAsync(_uiScene, LoadSceneMode.Additive);
		yield return new WaitUntil(() => _sceneOperations[0].isDone && _sceneOperations[1].isDone);
		
		SceneManager.UnloadSceneAsync(_loadingScreenScene); 
	}
	
	//Coroutine to load neutral scenes (main menu, end screen, etc.)
	private IEnumerator LoadNeutralSceneCoroutine(string _sceneName)
	{
		_loadingSceneOperation = SceneManager.LoadSceneAsync(_loadingScreenScene);
		yield return _loadingSceneOperation;
		
		_scene = _sceneName;
		_sceneOperations = new AsyncOperation[1];
		_sceneOperations[0] = SceneManager.LoadSceneAsync(_scene, LoadSceneMode.Additive);
		yield return new WaitUntil(() => _sceneOperations[0].isDone);
		
		SceneManager.UnloadSceneAsync(_loadingScreenScene);
	}
	
}
