using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InterfacesPopUp : MonoBehaviour
{
    [SerializeField] private Button[] _tabButtons = new Button[3];

    public TextMeshProUGUI Title, Description;

    public enum MessageType
    {
        AlertBox,
        ConfirmBox
    }

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
    public void DisplayPopUp(MessageType type = MessageType.AlertBox)
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
    /// Display buttons of popup configuration
    /// </summary>
    /// <param name="type">Type of popup on integrer</param>
    public void DisplayPopUp(int type = (int)MessageType.AlertBox)
    {

        if (CheckButtons())
        {
            switch (type)
            {
                case (int)MessageType.ConfirmBox:
                    EnabledComposants(_tabButtons[0].gameObject, true);
                    EnabledComposants(_tabButtons[1].gameObject, false);
                    EnabledComposants(_tabButtons[2].gameObject, true);
                    break;
                case (int)MessageType.AlertBox:
                default:
                    EnabledComposants(_tabButtons[0].gameObject, false);
                    EnabledComposants(_tabButtons[1].gameObject, true);
                    EnabledComposants(_tabButtons[2].gameObject, false);
                    break;
            }
        }

    }
}
