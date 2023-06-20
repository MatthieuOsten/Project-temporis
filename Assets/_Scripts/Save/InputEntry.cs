using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace InputEntry
{

    public static class EntryUtilities
    {
        public static struct ResolutionRatio
        {
            public int Width;
            public int Height;
            public Ratio ratio;

            public struct Ratio
            {
                private int width;
                private int height;

                public Vector2 GetRatioVector {
                    get { 
                        return new Vector2 (width, height); 
                    }
                }

                public Ratio(int width, int height)
                {
                    this.width = width;
                    this.height = height;
                }

                public float GetRatio()
                {
                    return (float)width / height;
                }

                public bool MatchesRatio(int imageWidth, int imageHeight)
                {
                    float imageRatio = (float)imageWidth / imageHeight;
                    return Mathf.Approximately(GetRatio(), imageRatio);
                }

                public override string ToString()
                {
                    return $"{width}:{height}";
                }

            }
        }

        public static Vector2[] DefaultResolution =
        {
            new Vector2()
            {
                x = 1920,
                y = 1080
            },

        };

        public static int[] FrameRate =
        {
            30, 60, 90, 120, 144, 165
        };

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

                return day + month + year + hour + minutes + second;
            }

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

        public Tracker(GameObject gameObject)
        {
            Name = "Tracker of " + gameObject.name + " n°" +  EntryUtilities.getIDTimed;

            _position = gameObject.transform.position;
            _rotation = gameObject.transform.rotation;
        }

        public Tracker(Vector3 position, Quaternion rotation)
        {
            Name = "Tracker Ucknow n°" + EntryUtilities.getIDTimed;

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
        [SerializeField] private Graphics Graphics;
        [SerializeField] private Sounds Sounds;

        public Settings() {
            Name = "Default Settings";

            Graphics = new Graphics();
            Sounds = new Sounds();
        }

        public Settings(Graphics graphics, Sounds sounds) {
            Name = "Custom Settings";

            Graphics = graphics;
            Sounds = sounds;
        }
    }

    /// <summary>
    /// Save number of pages and if is available
    /// </summary>
    [System.Serializable]
    public class Notebook : Entry
    {
        [SerializeField] private bool[] _tabPages;

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
        [SerializeField] private float _sensitivity;
        [SerializeField] private int _quality;
        [SerializeField] private bool _vsync;

        /// <summary>
        /// Quality of the game limited by 
        /// </summary>
        public int Quality
        {
            get
            {
                if (_quality < 0) {
                    Quality = 0;
                }
                else if (Quality > QualitySettings.names.Length - 1) {
                    Quality = QualitySettings.names.Length - 1;
                }

                return _quality;
            }

            set {

                if (value < 0)
                {
                    Quality = 0;
                }
                else if (value > QualitySettings.names.Length - 1)
                {
                    Quality = QualitySettings.names.Length - 1;
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
            _sensitivity = 0.50f;
            _quality = QualitySettings.GetQualityLevel();
            _vsync = false;
        }

        public Graphics(Resolution resolution, FullScreenMode fullScreenMode, float sensitivity, int quality, bool vsync)
        {
            Name = "Custom Graphics";

            _screenMode = fullScreenMode;
            _resolution = resolution;
            _sensitivity = sensitivity;
            Quality = quality;
            _vsync = vsync;
        }

        public Graphics(Resolution resolution, int fullScreenMode, float sensitivity, int quality, bool vsync)
        {
            Name = "Custom Graphics";

            _screenMode = (fullScreenMode <= (int)FullScreenMode.Windowed && fullScreenMode >= 0) ? (FullScreenMode)fullScreenMode : FullScreenMode.Windowed;
            _resolution = resolution;
            _sensitivity = sensitivity;
            Quality = quality;
            _vsync = vsync;
        }

        private Resolution GetResolution()
        {

            Resolution resolution = new Resolution();

            #if UNITY_EDITOR
                resolution.width = Display.main.systemWidth;
                resolution.height = Display.main.systemHeight;
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
    public class Inputs : Entry
    {
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
    }
}





