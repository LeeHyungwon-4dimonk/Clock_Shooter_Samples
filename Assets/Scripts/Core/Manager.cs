public static class Manager
{
    public static GameManager Game => GameManager.Instance;
    public static PoolManager Pool => PoolManager.Instance;
    public static StatusManager Status => StatusManager.Instance;
}