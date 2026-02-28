public static class Manager
{
    public static GameManager Game => GameManager.Instance;
    public static PoolManager Pool => PoolManager.Instance;
    public static StatusManager Status => StatusManager.Instance;
    public static SteamManager Steam => SteamManager.Instance;
    public static DataManager Data => DataManager.Instance;
    public static AudioManager Audio => AudioManager.Instance;
    public static SoundManager Sound => SoundManager.Instance;
}