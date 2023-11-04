using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using SimpleSaveSystem;
using UnityEditor;

//Please make sure "GameManager" is excuted before every custom script
public class GameManager : Singleton<GameManager>
{
    [SerializeField] private int targetFrameRate = 60;
[Header("Scene Transition")]
    [SerializeField] private CanvasGroup BlackScreenCanvasGroup;
    [SerializeField] private float transitionDuration = 1;
[Header("Init")]
    [SerializeField] private bool loadInitSceneFromGameManager = false;
    [SerializeField] private string InitScene;
[Header("Demo")]
    [SerializeField] private bool isDemo = true;
    [SerializeField] private bool isTesting = true;
    [SerializeField] private Text demoText;
[Header("Debug")]
    [SerializeField] private InputActionMap debugActions;
    private static bool isSwitchingScene = false;
    private static bool isPaused = false;
    protected override void Awake(){
        base.Awake();
        Application.targetFrameRate = targetFrameRate;

        SaveManager.Initialize();

    #if UNITY_EDITOR
        if(loadInitSceneFromGameManager) StartCoroutine(SwitchSceneCoroutine(string.Empty, InitScene, false));
    #else
        StartCoroutine(SwitchSceneCoroutine(string.Empty, InitScene, false));
    #endif

    #if UNITY_EDITOR || DEVELOPMENT_BUILD
        debugActions["restart"].performed += Debug_RestartLevel;
        debugActions["save"].performed += Debug_Save;
        debugActions["load"].performed += Debug_Load;
        debugActions["quit"].performed += Debug_Quit;

        if(isTesting) debugActions.Enable();
    #endif
    }
    protected override void OnDestroy(){
        base.OnDestroy();

    #if UNITY_EDITOR || DEVELOPMENT_BUILD
        debugActions["restart"].performed -= Debug_RestartLevel;
        debugActions["save"].performed -= Debug_Save;
        debugActions["load"].performed -= Debug_Load;
        debugActions["quit"].performed -= Debug_Quit;

        if(debugActions.enabled)debugActions.Disable();
    #endif
    }

#region Game Pause
    public void PauseTheGame(){
        if(isPaused) return;
        
        Time.timeScale = 0;
        AudioListener.pause = true;
        isPaused = true;
    }
    public void ResumeTheGame(){
        if(!isPaused) return;

        AudioListener.pause = false;
        Time.timeScale = 1;
        isPaused = false;
    }
#endregion

    public void EndGame(){
        if(isDemo) demoText.gameObject.SetActive(true);

        string currentLevel = SceneManager.GetActiveScene().name;
        StartCoroutine(EndGameCoroutine(currentLevel));
    }
    public void RestartLevel(){
        string currentLevel = SceneManager.GetActiveScene().name;
        StartCoroutine(RestartLevel(currentLevel));
    }

#region Scene Transition
    public void SwitchingScene(string from, string to, bool resume = false){
        if(!isSwitchingScene){
            StartCoroutine(SwitchSceneCoroutine(from, to, resume));
        }
    }
    public void SwitchingScene(string to, bool resume){
        string from = SceneManager.GetActiveScene().name;
        SwitchingScene(from, to, resume);
    }
    IEnumerator EndGameCoroutine(string level){
        yield return FadeInScreen(3f);

        EventHandler.Call_BeforeUnloadScene();
        yield return SceneManager.UnloadSceneAsync(level);
        yield return new WaitForSeconds(1f);
        Debug.Log("EndGame");
        Application.Quit();
    }
    IEnumerator RestartLevel(string level){
        yield return FadeInScreen(3f);
        isSwitchingScene = true;

        //TO DO: do something before the last scene is unloaded. e.g: call event of saving 
        yield return SceneManager.UnloadSceneAsync(level);
        yield return null;
        //TO DO: do something after the last scene is unloaded.
        yield return SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(level));
        //TO DO: do something after the next scene is loaded. e.g: call event of loading
        yield return FadeOutScreen(transitionDuration);

        isSwitchingScene = false;
    }
    IEnumerator RestartLevelImmediatley(string level){
        isSwitchingScene = true;

        EventHandler.Call_BeforeUnloadScene();

        yield return SceneManager.UnloadSceneAsync(level);
        yield return null;

        yield return SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(level));

        EventHandler.Call_AfterLoadScene();

        isSwitchingScene = false;        
    }
    IEnumerator SwitchSceneCoroutine(string from, string to, bool resume){
        isSwitchingScene = true;

        if(from != string.Empty){
            //TO DO: do something before the last scene is unloaded. e.g: call event of saving 
            EventHandler.Call_BeforeUnloadScene();
            yield return FadeInScreen(transitionDuration);
            yield return SceneManager.UnloadSceneAsync(from);
        }
        //TO DO: do something after the last scene is unloaded.
        yield return SceneManager.LoadSceneAsync(to, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(to));

        //TO DO: do something after the next scene is loaded. e.g: call event of loading
        EventHandler.Call_AfterLoadScene();
        if(resume) SaveManager.LoadGameState();

        yield return FadeOutScreen(transitionDuration);

        isSwitchingScene = false;
    }
    public IEnumerator FadeInScreen(float fadeDuration){
        for(float t=0; t<1; t+=Time.deltaTime/fadeDuration){
            BlackScreenCanvasGroup.alpha = Mathf.Lerp(0, 1, EasingFunc.Easing.QuadEaseOut(t));
            yield return null;
        }
        BlackScreenCanvasGroup.alpha = 1;
    }
    public IEnumerator FadeOutScreen(float fadeDuration){
        for(float t=0; t<1; t+=Time.deltaTime/fadeDuration){
            BlackScreenCanvasGroup.alpha = Mathf.Lerp(1, 0, EasingFunc.Easing.QuadEaseIn(t));
            yield return null;
        }
        BlackScreenCanvasGroup.alpha = 0;        
    }
#endregion
#region DEBUG ACTION
    void Debug_RestartLevel(InputAction.CallbackContext callback){
        if(callback.ReadValueAsButton()){
            Debug.Log("Test Restart Level");
            RestartLevel();
        }
    }
    void Debug_Save(InputAction.CallbackContext callback)=>SaveManager.SaveGameState();
    void Debug_Load(InputAction.CallbackContext callback)=>SaveManager.LoadGameState();
    void Debug_Quit(InputAction.CallbackContext callback)=>EndGame();
#endregion
}
