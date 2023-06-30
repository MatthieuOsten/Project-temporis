using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [System.Serializable]
    private struct Background
    {
        [SerializeField] private string _name;
        [SerializeField] private int[] _levelOfDetails;

        public string Name { get { return _name; } }

        public int[] LevelOfDetails { get { return _levelOfDetails; } }

        public int LessDetail
        {
            get {

                if (LevelOfDetails == null || LevelOfDetails.Length <= 0) { return 0; }

                int lessDetail = LevelOfDetails[0];
                foreach (var detail in LevelOfDetails)
                {
                    if (detail < lessDetail) { lessDetail = detail; }
                }

                return lessDetail; 
            }
        }

        public int MoreDetail
        {
            get
            {

                if (LevelOfDetails == null || LevelOfDetails.Length <= 0) { return 0; }

                int lessDetail = LevelOfDetails[0];
                foreach (var detail in LevelOfDetails)
                {
                    if (detail > lessDetail) { lessDetail = detail; }
                }

                return lessDetail;
            }
        }
    }

    [Header("BACKGROUND")]
    [SerializeField] private Background[] _nameSceneBackground;
    [SerializeField] private InputHandler _inputHandlerSettings;

    [Header("INTERFACES")]
    [SerializeField] private InterfacesPopUp _popUp = null;
    [SerializeField] private GameObject[] _panels = new GameObject[0];
    [SerializeField] private int _indexMainPanel;

    private void Awake()
    {
        LoadBackground();
        _popUp.gameObject.SetActive(false);
        EnablePanelAndDisableAll(_indexMainPanel);
    }

    private void LoadBackground()
    {
        string nameScene = string.Empty;
        int quality = 0;
        int backFind = 0;

        if (_inputHandlerSettings != null)
        {
            if (!_inputHandlerSettings.Load())
            {
                _inputHandlerSettings.entrySettings = new InputEntry.Settings(_inputHandlerSettings.entrySettings.inputs);

                _inputHandlerSettings.Save();
            }

            quality = _inputHandlerSettings.entrySettings.Graphics.Quality;

            // If tab is empty dont load scene
            if (_nameSceneBackground == null) {
                Debug.LogError("The tab of _nameSceneBackground is null");
                return; 
            }
            if (_nameSceneBackground.Length <= 0 ) {
                Debug.LogWarning("The tab dont have any scene");
                return; 
            }

            // Get the scene with level of detail graphics
            foreach (var back in _nameSceneBackground)
            {
                foreach (var detail in back.LevelOfDetails)
                {
                    Scene scene = SceneManager.GetSceneByName(back.Name);

                    if (quality == detail && scene != null)
                    {
                        if (SceneUtility.GetBuildIndexByScenePath(scene.path) >= 0)
                        {
                            nameScene = back.Name;
                        }

                    }
                }

                if (nameScene != string.Empty) { break; }
            }

            // If dont find scene, check if dont exist scene with other details level
            if (nameScene == string.Empty)
            {
                bool find = false;

                // Check if exist scene with moins de details mais qui ce rapproche de la qualiter voulu

                for (int i = 0; i < _nameSceneBackground.Length; i++)
                {
                    foreach (var detail in _nameSceneBackground[i].LevelOfDetails)
                    {
                        if (quality > detail && _nameSceneBackground[backFind].LessDetail < detail)
                        {
                            backFind = i;
                            find = true;
                        }
                    }
                }

                // If dont find take the most little quality scene possibly
                if (!find) {
                    int detailFind = quality;

                    for (int i = 0; i < _nameSceneBackground.Length; i++)
                    {
                        Scene scene = SceneManager.GetSceneByName(_nameSceneBackground[i].Name);

                        if (scene != null)
                        {
                            if (SceneUtility.GetBuildIndexByScenePath(scene.path) >= 0)
                            {
                                foreach (var detail in _nameSceneBackground[i].LevelOfDetails)
                                {


                                    if (quality < detail)
                                    {


                                        if (detailFind > detail)
                                        {


                                            detailFind = detail;
                                            backFind = i;
                                        }

                                    }
                                }
                            

                            }
                        }


                    }
                    Debug.Log("Scene with most quality of graphics but less of all scene is find and is this " + detailFind);

                } else { Debug.Log("Scene with less quality is find"); }

                Debug.Log("Scene is find, and is " + nameScene);
                nameScene = _nameSceneBackground[backFind].Name;
            }

        }
        else
        {
            if (_nameSceneBackground != null && _nameSceneBackground.Length > 0) {

                nameScene = _nameSceneBackground[0].Name;
            }
        }

        // Unload all scene and load the selected scene
        if (nameScene != string.Empty)
        {
            foreach (var back in _nameSceneBackground)
            {
                Scene actualScene = SceneManager.GetSceneByName(nameScene);

                if (actualScene != null)
                {
                    if (actualScene.isLoaded) { SceneManager.UnloadSceneAsync(back.Name); }
                }
            }

            Debug.Log("Name " + nameScene + " index " + backFind);

            Scene scene = SceneManager.GetSceneByName(nameScene);

            SceneManager.LoadSceneAsync(nameScene, LoadSceneMode.Additive);

        }
        else
        {
            Debug.Log("Scene with level of details " + quality + " or with most quality egal");
        }
    }

    private void DisplayPanel(bool enabled, string name)
    {
        if (name == string.Empty || name == null) { return; }

        foreach (var panel in _panels)
        {
            if (panel.name == name)
            {
                panel.SetActive(enabled);
            }

        }
    }

    private void DisplayPanel(bool enabled, int index)
    {
        if (index < 0 || index > _panels.Length) { return; }

        _panels[index].SetActive(enabled);
    }

    private void DisplayPanel(bool enabled, GameObject gameObject)
    {
        if (gameObject == null) { return; }

        gameObject.SetActive(enabled);
    }

    public void EnablePanelAndDisableAll(string name)
    {
        int index = -1;

        for (int i = 0; i < _panels.Length; i++)
        {
            if (_panels[i].gameObject.name == name)
            {
                index = i; break;
            }
        }

        if (index == -1)
        {
            return;
        }

        DisplayPanel(true, name);
    }

    public void EnablePanelAndDisableAll(int index)
    {
        foreach (var panel in _panels)
        {
            DisablePanel(panel);
        }

        EnablePanel(index);
    }

    public void EnablePanel(int index) => DisplayPanel(true, index);
    public void DisablePanel(int index) => DisplayPanel(false, index);

    public void EnablePanel(string name) => DisplayPanel(true, name);
    public void DisablePanel(string name) => DisplayPanel(false, name);

    public void EnablePanel(GameObject gameObject) => DisplayPanel(true, gameObject);
    public void DisablePanel(GameObject gameObject) => DisplayPanel(false, gameObject);

    public void EnablePopUp(string name)
    {
        if (_popUp.DisplayPopUp(name))
        {
            _popUp.gameObject.SetActive(true);
        }
    }

    public void EnablePopUp(int index)
    {
        if (_popUp.DisplayPopUp(index))
        {
            _popUp.gameObject.SetActive(true);
        }
    }

    public void DisablePopUp()
    {
        _popUp.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}