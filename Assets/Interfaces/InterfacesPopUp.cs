using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InterfacesPopUp : MonoBehaviour
{
    [SerializeField] private Button[] _tabButtons = new Button[3];

    [SerializeField] private Message[] tabMessages = new Message[0];

    public enum MessageType
    {
        AlertBox,
        ConfirmBox
    }

    [System.Serializable]
    public struct Message
    {
        [SerializeField] private string _name;
        [SerializeField] private GameObject _text;
        [SerializeField] private MessageType _messageType;
        [SerializeField] private UnityEvent _confirm, _ok, _cancel;

        public string Name { get { return _name; } }

        public MessageType MessageType { get { return _messageType; } }

        public UnityEvent[] UnityEvent { 
            get {
                UnityEvent[] events = new UnityEvent[3];
                
                events[0] = _confirm;
                events[1] = _ok;
                events[2] = _cancel;

                return events; 
            } 
        }

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

            _confirm = new UnityEvent();
            _ok = new UnityEvent();
            _cancel = new UnityEvent();
        }

        public Message(string name, MessageType type, GameObject text)
        {
            _name = name;
            _text = text;
            _messageType = type;

            _confirm = new UnityEvent();
            _ok = new UnityEvent();
            _cancel = new UnityEvent();
        }
    }

    public Message[] Messages { get { return tabMessages; } }

    private void Awake()
    {
        CheckButtons();
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
        if (index < 0 || index > tabMessages.Length) { return false; }

        foreach (var message in tabMessages)
        {
            message.EnableText(false);
        }

        DisplayButtons(tabMessages[index].MessageType);
        SettingsButtons(tabMessages[index].UnityEvent.ToArray());
        tabMessages[index].EnableText(true);

        return true;
    }

    public bool DisplayPopUp(string nameMessage)
    {
        int indexPopUp = -1;

        for (int i = 0; i < tabMessages.Length; i++)
        {
            if (tabMessages[i].Name == nameMessage)
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