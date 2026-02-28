using System;
using UnityEditor.PackageManager;

public class GameEvents
{
    //public static Action<HitInfo> OnMonsterHit;
    //public static void RaiseMonsterHit(HitInfo info)
        //=> OnMonsterHit?.Invoke(info);

    public static Action<int> OnMonsterPhaseChanged;
    public static void RaiseMonsterPhaseChanged(int phase)
        => OnMonsterPhaseChanged?.Invoke(phase);
}