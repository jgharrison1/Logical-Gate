using UnityEngine;
using TMPro;

public class Waypoint : MonoBehaviour
{
    public bool shouldStop = false;
    public TextMeshProUGUI platformNumberText;
    private bool hasDamaged = false;

    private int platformNumber;

    void Start()
    {
        if (platformNumberText != null)
        {
            platformNumber = int.Parse(platformNumberText.text);
        }
    }

    public int GetPlatformNumber()
    {
        return platformNumber;
    }

    public bool HasDamaged()
    {
        return hasDamaged;
    }

    public void MarkAsDamaged()
    {
        hasDamaged = true;

        if (platformNumberText != null)
        {
            platformNumberText.gameObject.SetActive(false);
        }
    }
}
