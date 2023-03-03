using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
	[Header("Game Scenes")]
	[SerializeField] private string[] _gameScenes;
	
	[Header("Neutral Scenes")]
	[SerializeField] private string _uiScene;
	[SerializeField] private string _loadingScreenScene;
	
	private string _scene; //The scene to load
	
	//Async operations for loading screen and
	private AsyncOperation[] _sceneOperations;
	private AsyncOperation _loadingSceneOperation;
	
	public static SceneManagerScript instance { get; private set; }
	
	private void Awake()
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
	
	
	public void LoadScene(bool isGameScene, string sceneName) //Load the scene
	{
		StartCoroutine(LoadSceneCoroutine(isGameScene, sceneName));
	}

	private IEnumerator SelectandLoadGameScene() //Select a random game scene and add to operations array
	{
		//pick a randome game scene to load
		_scene = _gameScenes[Random.Range(0, _gameScenes.Length)];

		//load the game scene and the UI scene async
		_sceneOperations = new AsyncOperation[2];
		_sceneOperations[0] = SceneManager.LoadSceneAsync(_scene, LoadSceneMode.Additive);
		_sceneOperations[1] = SceneManager.LoadSceneAsync(_uiScene, LoadSceneMode.Additive);
		yield return new WaitUntil(() => _sceneOperations[0].isDone && _sceneOperations[1].isDone);
	}
	
	private IEnumerator LoadNeutralScene(string _sceneName)
	{
		_scene = _sceneName; //set the scene to load from parameter
		
		_sceneOperations = new AsyncOperation[1];
		_sceneOperations[0] = SceneManager.LoadSceneAsync(_scene, LoadSceneMode.Additive);
		yield return new WaitUntil(() => _sceneOperations[0].isDone);
	}
	
	
	private IEnumerator LoadSceneCoroutine(bool isGameScene, string sceneName) //Coroutine to load all scenes
	{
		_loadingSceneOperation = SceneManager.LoadSceneAsync(_loadingScreenScene);

		yield return _loadingSceneOperation; //Wait for the loading screen scene to load

		if (isGameScene)
		{
			StartCoroutine(SelectandLoadGameScene()); //if game scene, load a random game scene
		}
		else
		{
			StartCoroutine(LoadNeutralScene(sceneName)); //else, load described neutral scene
		}
		
		SceneManager.UnloadSceneAsync(_loadingScreenScene); //Unload the loading screen scene
	}
}
