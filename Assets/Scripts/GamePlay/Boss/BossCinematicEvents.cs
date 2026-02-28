using UnityEngine;

public class BossCinematicEvents : MonoBehaviour
{
    public void AE_PlayRoarSound()
    {
        Manager.Sound.PlaySFX(SFXType.DragonRoar);
    }

    public void AE_PlayDamageSound()
    {
        Manager.Sound.PlaySFX(SFXType.DragonDamaged);
    }

    public void AE_PlayFlySound()
    {
        Manager.Sound.PlaySFX(SFXType.DragonFly);
    }

    public void AE_StopSFXSound()
    {
        Manager.Sound.StopSFX();
    }
}