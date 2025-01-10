using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UnitySceneManager : MonoBehaviour
{
    #region INTERNAL DATA
    // Scene Tracking
    private Scene _currentScene;
    private int _totalScenesNum;
    private int _mainMenuIndex;
    private int _gameplayIndex;
    private int _servicesIndex;
    #endregion

    private void Awake()
    {
        // If using the Unity editor or development build, enable debug logs
        Debug.unityLogger.logEnabled = Debug.isDebugBuild;

        // Get total number of scenes in build and indexes for main menu, gameplay, and services scenes
        _totalScenesNum = SceneManager.sceneCountInBuildSettings;
        _mainMenuIndex = GetBuildIndex("MainMenu");
        _gameplayIndex = GetBuildIndex("Gameplay");
        _servicesIndex = GetBuildIndex("Services");

        // Init Events
        // EventManager.EventInitialise(EventType.FADING);
    }

    private void OnEnable()
    {
        EventManager.Subscribe(EventType.PLAY_GAME, PlayGameHandler);
        EventManager.Subscribe(EventType.QUIT_GAME, QuitGameHandler);
        EventManager.Subscribe(EventType.MAIN_MENU, MainMenuHandler);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(EventType.PLAY_GAME, PlayGameHandler);
        EventManager.Unsubscribe(EventType.QUIT_GAME, QuitGameHandler);
        EventManager.Unsubscribe(EventType.MAIN_MENU, MainMenuHandler);
    }

    // After Services Scene is loaded in, additively load in the MainMenu scene
    private void Start()
    {
        // If build is running, load in the main menu
        // Otherwise, unload all scenes except Services and reload in order of their layer architecture
#if !UNITY_EDITOR
        StartCoroutine(LoadScene(_mainMenuIndex));
        StartCoroutine(_fader.NormalFadeIn());
#else
        int loadedScenesCount = SceneManager.loadedSceneCount;
        Queue<int> loadedScenes = new Queue<int>();

        for (int i = 0; i < loadedScenesCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if (scene.buildIndex != _servicesIndex)
            {
                loadedScenes.Enqueue(scene.buildIndex);
                SceneManager.UnloadSceneAsync(scene);
            }
        }

        StartCoroutine(ReloadAllScenes(loadedScenes));
#endif
    }

    #region GAME SEQUENCE FUNCTIONALITY
    // To simulate when the build boots up for the first time
    IEnumerator ReloadAllScenes(Queue<int> scenesToReload)
    {
        foreach (int i in scenesToReload)
        {
            yield return StartCoroutine(LoadScene(i));
        }
    }

    // Gameplay to MainMenu Scene Sequence
    IEnumerator GameplayToMainMenu()
    {
        // EventManager.EventTrigger(EventType.DISABLE_GAMEPLAY_INPUTS, null);
        // EventManager.EventTrigger(EventType.FADING, false);
        // yield return StartCoroutine(_fader.NormalFadeOut());
        yield return StartCoroutine(UnloadScene(_gameplayIndex));
        yield return StartCoroutine(LoadScene(_mainMenuIndex));
        // yield return StartCoroutine(_fader.NormalFadeIn());
        // EventManager.EventTrigger(EventType.FADING, true);
        // EventManager.EventTrigger(EventType.ENABLE_MAINMENU_INPUTS, null);
    }

    // MainMenu to Gameplay Scene Sequence
    IEnumerator MainMenuToGameplay(int levelSelected)
    {
        // EventManager.EventTrigger(EventType.DISABLE_MAINMENU_INPUTS, null);
        // EventManager.EventTrigger(EventType.FADING, false);
        // yield return StartCoroutine(_fader.NormalFadeOut());
        yield return StartCoroutine(UnloadScene(_mainMenuIndex));
        yield return StartCoroutine(LoadScene(_gameplayIndex));
        // yield return StartCoroutine(_fader.NormalFadeIn());
        // EventManager.EventTrigger(EventType.FADING, true);
        //EventManager.EventTrigger(EventType.ENABLE_GAMEPLAY_INPUTS, null);
    }
    #endregion

    #region BUILD FUNCTIONALITY
    public int GetBuildIndex(string name)
    {
        for (int index = 0; index < _totalScenesNum; index++)
        {
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(index));

            if (sceneName == name)
            {
                return index;
            }
        }

        Debug.LogError("Scene name not found");
        return -1;
    }

    // Quits game in either build or editor
    private IEnumerator QuitGame()
    {
        // yield return StartCoroutine(_fader.NormalFadeOut());
        yield return null;

        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
	    	Application.Quit();
        #endif
    }
    #endregion

    #region SCENE LOADING/UNLOADING FUNCTIONALITY
    // Loads specified scene
    private IEnumerator LoadScene(int index)
    {
        var levelAsync = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);

        // Wait until the scene fully loads to fade in
        while (!levelAsync.isDone)
        {
            yield return null;
        }

        Scene scene = SceneManager.GetSceneAt(SceneManager.loadedSceneCount - 1);
        SceneManager.SetActiveScene(scene);
        _currentScene = scene;
    }

    // Unloads specified scene
    private IEnumerator UnloadScene(int index)
    {
        var levelAsync = SceneManager.UnloadSceneAsync(index);

        // Wait until the scene fully unloads
        while (!levelAsync.isDone)
        {
            yield return null;
        }
    }
    #endregion

    #region EVENT HANDLERS
    // When Play Button has been clicked in MainMenu Scene
    public void PlayGameHandler(object data)
    {
        StartCoroutine(MainMenuToGameplay(_gameplayIndex));
    }

    // When Main Menu Button has been clicked in Gameplay Scene
    public void MainMenuHandler(object data)
    {
        StartCoroutine(GameplayToMainMenu());
    }

    // When Quit Button has been clicked in MainMenu Scene
    public void QuitGameHandler(object data)
    {
        StartCoroutine(QuitGame());
    }
    #endregion
}
