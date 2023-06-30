using System;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace InputEntry
{

    public static class EntryUtilities
    {
        

        public static string getIDTimed
        {
            get
            {
                string day = (DateTime.Now.Day < 10) ? '0' + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString();
                string month = (DateTime.Now.Month < 10) ? '0' + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString();
                string year = DateTime.Now.Year.ToString();
                string hour = (DateTime.Now.Hour < 10) ? '0' + DateTime.Now.Hour.ToString() : DateTime.Now.Hour.ToString();
                string minutes = (DateTime.Now.Minute < 10) ? '0' + DateTime.Now.Minute.ToString() : DateTime.Now.Minute.ToString();
                string second = (DateTime.Now.Second < 10) ? '0' + DateTime.Now.Second.ToString() : DateTime.Now.Second.ToString();

                //return day + month + year + hour + minutes + second;
                return year + month + day + hour + minutes + second; // For the gestion of files 20230101010100 > 20230101010000
            }

        }

        public static DateTime getDateByIDTimedString(string idTimedString)
        {
            string format = "yyyyMMddHHmmss";
            DateTime result;

            if (idTimedString.Length >= format.Length)
            {
                string year = idTimedString.Substring(0, format.IndexOf("y") + 1);
                format = format.Replace("yyyy", new string('y', year.Length));

                // Get data on string
                if (DateTime.TryParseExact(idTimedString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                {
                    return result;
                }
            }

            Debug.LogError("Erreur lors de la conversion de la chaîne en date : " + idTimedString);
            return DateTime.MinValue; // Valeur par défaut en cas d'échec de conversion
        }
    }

    [System.Serializable]
    public class Entry
    {
        [SerializeField] private string _name = "Entry Empty";

        protected string Name { set { _name = value; } }
    }

    /// <summary>
    /// Tracker of the player postion
    /// </summary>
    [System.Serializable]
    public class Tracker : Entry
    {
        [SerializeField] private Vector3 _position;
        [SerializeField] private Quaternion _rotation;

        public Tracker()
        {
            Name = "Default Tracker n" + EntryUtilities.getIDTimed;

            _position = Vector3.zero;
            _rotation = Quaternion.identity;
        }

        public Tracker(GameObject gameObject)
        {
            Name = "Tracker of " + gameObject.name + " n" +  EntryUtilities.getIDTimed;

            _position = gameObject.transform.position;
            _rotation = gameObject.transform.rotation;
        }

        public Tracker(Vector3 position, Quaternion rotation)
        {
            Name = "Tracker Ucknow n" + EntryUtilities.getIDTimed;

            _position = position;
            _rotation = rotation;
        }
    }

    /// <summary>
    /// Settings
    /// </summary>
    [System.Serializable]
    public class Settings : Entry 
    {
        [SerializeField] private Graphics _graphics;
        [SerializeField] private Sounds _sounds;
        [SerializeField] private Gameplay _gameplay;
        [SerializeField] private Inputs _inputs;

        public Graphics Graphics { get { return _graphics; } }
        public Sounds Sounds { get { return _sounds; } }
        public Gameplay Gameplay { get { return _gameplay; } }
        public Inputs inputs { get { return _inputs; } }

        public Settings() {
            Name = "Default Settings";

            _graphics = new Graphics();
            _sounds = new Sounds();
            _inputs = new Inputs();
            _gameplay = new Gameplay();
        }

        public Settings(Inputs input)
        {
            Name = "Custom Settings";

            _graphics = new Graphics();
            _sounds = new Sounds();
            _inputs = input;
            _gameplay = new Gameplay();
        }

        public Settings(Graphics graphics, Sounds sounds, Gameplay gameplay, Inputs input) {
            Name = "Custom Settings";

            _graphics = graphics;
            _sounds = sounds;
            _inputs = input;
            _gameplay = gameplay;
        }
    }

    /// <summary>
    /// Save number of pages and if is available
    /// </summary>
    [System.Serializable]
    public class Notebook : Entry
    {
        [SerializeField] private bool[] _tabPages;

        public bool[] TabPages { get { return _tabPages; } }

        public Notebook()
        {
            Name = "Default Notebook";

            _tabPages = new bool[0];
        }

        public Notebook(int nbrPages)
        {
            Name = "Notebook " + nbrPages + " Pages";

            _tabPages = new bool[nbrPages];
        }

        public Notebook(bool[] tabPages)
        {
            Name = "Notebook " + tabPages.Length + " Pages";

            _tabPages = tabPages;
        }
    }

    /// <summary>
    /// Graphics options
    /// </summary>
    [System.Serializable]
    public class Graphics : Entry
    {

        [SerializeField] private Resolution _resolution;
        [SerializeField] private FullScreenMode _screenMode;
        [SerializeField] private int _quality;
        [SerializeField] private bool _vsync;

        /// <summary>
        /// Quality of the game limited by 
        /// </summary>
        public int Quality
        {
            get
            {
                return _quality;
            }

            set {

                if (value < 0)
                {
                    Quality = 0;
                }
                else if (value > QualitySettings.names.Length)
                {
                    Quality = QualitySettings.names.Length;
                }
                else
                {
                    _quality = value;
                }

            }
        }

        public Graphics() {
            Name = "Default Graphics";

            _screenMode = FullScreenMode.FullScreenWindow;
            _resolution = GetResolution();
            #if UNITY_EDITOR
                _quality = 0;
            #else
                _quality = QualitySettings.GetQualityLevel();
            #endif
            _vsync = false;
        }

        public Graphics(Resolution resolution, FullScreenMode fullScreenMode, float sensitivity, int quality, bool vsync)
        {
            Name = "Custom Graphics";

            _screenMode = fullScreenMode;
            _resolution = resolution;
            Quality = quality;
            _vsync = vsync;
        }

        public Graphics(Resolution resolution, int fullScreenMode, float sensitivity, int quality, bool vsync)
        {
            Name = "Custom Graphics";

            _screenMode = (fullScreenMode <= (int)FullScreenMode.Windowed && fullScreenMode >= 0) ? (FullScreenMode)fullScreenMode : FullScreenMode.Windowed;
            _resolution = resolution;
            Quality = quality;
            _vsync = vsync;
        }

        private Resolution GetResolution()
        {

            #if UNITY_EDITOR
                Resolution resolution = new Resolution();
                resolution.width = 1920;
                resolution.height = 1080;
                resolution.refreshRate = 30;
                return resolution;
            #else
                return Screen.currentResolution;
            #endif

        }
    }

    [System.Serializable]
    public class Sounds : Entry
    {
        public float VolumeGlobal;
        public float VolumeMusics;
        public float VolumeSoundEffects;

        public Sounds() {
            Name = "Default Sounds";

            VolumeGlobal = 1f;
            VolumeMusics = 0.5f;
            VolumeSoundEffects = 1f;
        }

        public Sounds(float volumeGlobal, float volumeMusics, float volumeSoundEffects)
        {
            Name = "Custom Sounds";

            VolumeGlobal = volumeGlobal;
            VolumeMusics = volumeMusics;
            VolumeSoundEffects = volumeSoundEffects;
        }
    }

    [System.Serializable]
    public class Gameplay : Entry
    {
        [SerializeField] private float _sensitivity;

        public Gameplay()
        {
            Name = "Default Gameplay";

            _sensitivity = 0.5f;
        }

        public Gameplay(float sensitivity)
        {
            Name = "Custom Gameplay";

            _sensitivity = sensitivity;
        }
    }

    [System.Serializable]
    public class Inputs : Entry
    {
#if ENABLE_INPUT_SYSTEM

        [SerializeField] private InputActionAsset _inputs;

        public Inputs()
        {
            Name = "Default Inputs";

            _inputs = null;
        }

        public Inputs(InputActionAsset newInputs)
        {
            Name = "Default Inputs";

            _inputs = newInputs;
        }

#else
        [SerializeField] private KeyCode ForwardInput;
        [SerializeField] private KeyCode BackwardInput;
        [SerializeField] private KeyCode LeftInput;
        [SerializeField] private KeyCode RightInput;

        [SerializeField] private KeyCode CameraLeftInput;
        [SerializeField] private KeyCode CameraRightInput;
        [SerializeField] private KeyCode CameraUpInput;
        [SerializeField] private KeyCode CameraDownwardInput;

        public Inputs()
        {
            Name = "Default Inputs";

            ForwardInput = KeyCode.Z;
            BackwardInput = KeyCode.S;
            LeftInput = KeyCode.Q;
            RightInput = KeyCode.D;

            CameraLeftInput = KeyCode.LeftArrow;
            CameraRightInput = KeyCode.RightArrow;
            CameraUpInput = KeyCode.UpArrow;
            CameraDownwardInput = KeyCode.DownArrow;
        }
#endif
    }


}





