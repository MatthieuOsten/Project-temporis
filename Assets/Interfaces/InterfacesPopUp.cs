using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InterfacesPopUp : MonoBehaviour
{
    [SerializeField] private Button[] _tabButtons = new Button[3];

    [SerializeField] private Message[] _tabMessages = new Message[0];

    [SerializeField] private int _indexActualMessage = -1;

    public enum MessageType
    {
        AlertBox,
        ConfirmBox,
        TimeBox
    }

    [System.Serializable]
    public struct Message
    {
        [SerializeField] private string _name;
        [SerializeField] private GameObject _text;
        [SerializeField] private MessageType _messageType;
        [SerializeField] private int _timer;
        [SerializeField] private UnityEvent _confirm, _ok, _cancel, _endTime;

        public string Name { get { return _name; } }

        public MessageType MessageType { get { return _messageType; } }

        public int Timer { get { return _timer; } }

        public UnityEvent[] UnityEvent { 
            get {
                UnityEvent[] events = new UnityEvent[3];
                
                events[0] = _confirm;
                events[1] = _ok;
                events[2] = _cancel;

                return events; 
            } 
        }

        public UnityEvent EventEndTimer { get { return _endTime; } }

        public void EnableText(bool enabled)
        {
            if (_text != null) { 
                _text.gameObject.SetActive(enabled);

                for (int i = 0; i < _text.transform.childCount; i++)
                {
                    _text.transform.GetChild(i).gameObject.SetActive(enabled);
                }
            }
        }

        public Message(string name)
        {
            _name = name;
            _text = null;
            _messageType = MessageType.AlertBox;

            _timer = 10;

            _confirm = new UnityEvent();
            _ok = new UnityEvent();
            _cancel = new UnityEvent();
            _endTime = new UnityEvent();
        }

        public Message(string name, MessageType type, GameObject text)
        {
            _name = name;
            _text = text;
            _messageType = type;

            _timer = 10;

            _confirm = new UnityEvent();
            _ok = new UnityEvent();
            _cancel = new UnityEvent();
            _endTime = new UnityEvent();
        }

        public Message(string name, MessageType type, GameObject text, int timer)
        {
            _name = name;
            _text = text;
            _messageType = type;

            _timer = timer;

            _confirm = new UnityEvent();
            _ok = new UnityEvent();
            _cancel = new UnityEvent();
            _endTime = new UnityEvent();
        }
    }

    public Message[] Messages { get { return _tabMessages; } }

    [SerializeField] private const int _Timer = 5;
    [SerializeField] private float _actualTime = 0;

    private void Awake()
    {
        CheckButtons();
    }

    private void FixedUpdate()
    {
        if (_actualTime > 0)
        {
            _actualTime -= Time.deltaTime;
            _actualTime = (_actualTime == 0) ? -1 : _actualTime;
        }
        else if (_actualTime < 0)
        {
            _actualTime = 0;
            if (_indexActualMessage >= 0 && _indexActualMessage < _tabMessages.Length)
            {
                _tabMessages[_indexActualMessage].EventEndTimer.Invoke();
                _indexActualMessage = -1;
            }
        }
    }

    /// <summary>
    /// Enable or disable all composants of this object
    /// </summary>
    /// <param name="thisObject">selected object</param>
    /// <param name="enabled">is enable or disable</param>
    private void EnabledComposants(GameObject thisObject, bool enabled)
    {
        if (thisObject == null) { return; }

        // Désactiver tous les composants
        foreach (Behaviour childCompnent in thisObject.GetComponentsInChildren(typeof(Behaviour)))
        {
            childCompnent.enabled= enabled;
        }
    }

    /// <summary>
    /// Check if buttons is getting
    /// </summary>
    /// <returns></returns>
    private bool CheckButtons()
    {
        if (_tabButtons[0] == null || _tabButtons[1] == null || _tabButtons[2] == null)
        {
            _tabButtons = gameObject.GetComponentsInChildren<Button>();
            return (_tabButtons.Length == 3) ? true : false;
        }

        return true;
    }

    /// <summary>
    /// Display buttons of popup configuration
    /// </summary>
    /// <param name="type">Type of popup</param>
    private void DisplayButtons(MessageType type = MessageType.AlertBox)
    {

        if (CheckButtons())
        {
            switch (type)
            {
                case MessageType.TimeBox:
                    _actualTime = _Timer;
                    goto case MessageType.ConfirmBox;
                case MessageType.ConfirmBox:
                    EnabledComposants(_tabButtons[0].gameObject, true);
                    EnabledComposants(_tabButtons[1].gameObject, false);
                    EnabledComposants(_tabButtons[2].gameObject, true);
                    break;
                case MessageType.AlertBox:
                default:
                    EnabledComposants(_tabButtons[0].gameObject, false);
                    EnabledComposants(_tabButtons[1].gameObject, true);
                    EnabledComposants(_tabButtons[2].gameObject, false);
                    break;
            }
        }

    }

    /// <summary>
    /// Set buttons of popup
    /// </summary>
    /// <param name="events"></param>
    private void SettingsButtons(UnityEvent[] events)
    {
        for (int i = 0; i < _tabButtons.Length; i++)
        {
            if (_tabButtons[i] != null)
            {
                _tabButtons[i].onClick.RemoveAllListeners();

                // Store the current value of 'i' in a local variable
                int buttonIndex = i;

                for (int j = 0; j < events.Length; j++)
                {
                    // Capture the current value of 'j' in a local variable
                    int eventIndex = j;

                    // Use the local variables inside the lambda expression
                    _tabButtons[buttonIndex].onClick.AddListener(() => events[eventIndex].Invoke());
                }
            }
        }
    }

    /// <summary>
    /// Display buttons of popup configuration
    /// </summary>
    /// <param name="type">Type of popup on integrer</param>
    private void DisplayButtons(int type = (int)MessageType.AlertBox)
    {
        DisplayButtons((MessageType)type);
    }

    /// <summary>
    /// Display the popup and configurate this
    /// </summary>
    /// <param name="index">Index of the Message</param>
    public bool DisplayPopUp(int index)
    {
        if (index < 0 || index > _tabMessages.Length) { return false; }

        foreach (var message in _tabMessages)
        {
            message.EnableText(false);
        }

        _indexActualMessage = index;
        DisplayButtons(_tabMessages[index].MessageType);
        SettingsButtons(_tabMessages[index].UnityEvent.ToArray());
        _tabMessages[index].EnableText(true);

        return true;
    }

    public bool DisplayPopUp(string nameMessage)
    {
        int indexPopUp = -1;

        for (int i = 0; i < _tabMessages.Length; i++)
        {
            if (_tabMessages[i].Name == nameMessage)
            {
                indexPopUp = i; break;
            }
        }

        if (indexPopUp == -1) { 
            return false; 
        } else {
            return DisplayPopUp(indexPopUp);
        }

    }
}