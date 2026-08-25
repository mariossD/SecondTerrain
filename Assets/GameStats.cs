using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats
{
    public static bool gameOver = false;
    public static bool canShoot = true;

    public static int zombieKills = 0;
    public static event System.Action<int> OnZombieKilled;

    public static void RegisterZombieKill()
    {
        zombieKills++;
        OnZombieKilled?.Invoke(zombieKills);
    }

    public static void ResetZombieKills()
    {
        zombieKills = 0;
        OnZombieKilled?.Invoke(zombieKills);
    }
}
