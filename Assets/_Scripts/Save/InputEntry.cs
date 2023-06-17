using System;
using System.Collections.Generic;
using UnityEngine;

namespace InputEntry
{
    [System.Serializable]
    public class EntryAll
    {

        public Sounds Sounds;

        public EntryAll() { }
    }

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

                return day + month + year + hour + minutes + second;
            }

        }
    }

    [System.Serializable]
    public class Entry
    {
        private string _name = "Entry Empty";

        protected string Name { set { _name = value; } }
    }

    /// <summary>
    /// Tracker of the player postion
    /// </summary>
    [System.Serializable]
    public class Tracker : Entry
    {
        private Vector3 _position;
        private Quaternion _rotation;

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
        public Graphics Graphics;
        public Sounds Sounds;

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
        public bool[] TabPages;

        public Notebook(int nbrPages)
        {
            Name = "Notebook " + nbrPages + " Pages";

            TabPages = new bool[nbrPages];
        }

        public Notebook(bool[] tabPages)
        {
            Name = "Notebook " + tabPages.Length + " Pages";

            TabPages = tabPages;
        }
    }

    /// <summary>
    /// Graphics options
    /// </summary>
    [System.Serializable]
    public class Graphics : Entry
    {

        private Resolution _resolution;
        private float _sensitivity;
        private int _quality;
        private bool _vsync;

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

            _resolution = Screen.currentResolution;
            _sensitivity = 0.50f;
            _quality = QualitySettings.GetQualityLevel();
            _vsync = false;
        }

        public Graphics(Resolution resolution, float sensitivity, int quality, bool vsync)
        {
            Name = "Custom Graphics";

            _resolution = resolution;
            _sensitivity = sensitivity;
            Quality = quality;
            _vsync = vsync;
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
        public KeyCode ForwardInput;
        public KeyCode BackwardInput;
        public KeyCode LeftInput;
        public KeyCode RightInput;

        public KeyCode CameraLeftInput;
        public KeyCode CameraRightInput;
        public KeyCode CameraUpInput;
        public KeyCode CameraDownwardInput;

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





