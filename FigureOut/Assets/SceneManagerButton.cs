using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasRenderer))]
[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]

public class SceneManagerButton : MonoBehaviour
{
	[SerializeField] private string _sceneName;
	[SerializeField] private bool _isGameSceneChosen;

	private void Start() 
	{
		if (_isGameSceneChosen)
		{
			Button button = GetComponent<Button>();
			button.onClick.AddListener(LoadGameScene);
		}
		else
		{
			Button button = GetComponent<Button>();
			button.onClick.AddListener(LoadNeutralScene);
		}
	}

	private void LoadNeutralScene()
	{
		SceneManagerScript.instance.LoadNeutralScene(_sceneName);
	}
	
	private void LoadGameScene()
	{
		SceneManagerScript.instance.LoadGameScene();
	}	
}

