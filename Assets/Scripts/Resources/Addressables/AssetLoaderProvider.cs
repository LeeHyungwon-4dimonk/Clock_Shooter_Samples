public class AssetLoaderProvider
{
    private static IAssetLoader _loader;
    public static IAssetLoader Loader => _loader;

    public static void Initialize(IAssetLoader loader)
    {
        _loader = loader;
    }
}