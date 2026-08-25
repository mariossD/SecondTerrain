using UnityEngine;
using UnityEngine.UI;

public class KillCounter : MonoBehaviour
{
    public Text killCountText;

    void OnEnable()
    {
        GameStats.OnZombieKilled += UpdateDisplay;
    }

    void OnDisable()
    {
        GameStats.OnZombieKilled -= UpdateDisplay;
    }

    void Start()
    {
        UpdateDisplay(GameStats.zombieKills);
    }

    void UpdateDisplay(int kills)
    {
        killCountText.text = kills.ToString();
    }
}
