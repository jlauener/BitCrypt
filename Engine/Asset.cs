using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

// TODO Remove the default stuffs?
static class Asset
{
	public static Texture2D SkinTexture;
	public static BitmapFont DefaultFont;

	private static readonly Dictionary<string, object> store = new Dictionary<string, object>();

	public static void Init()
	{
		SkinTexture = LoadTexture2D("gfx/skin.png");
		DefaultFont = LoadBitmapFont("font/04b03.fnt");
	}

	public static Texture2D LoadTexture2D(string path)
	{
		if (!TryGetAsset<Texture2D>(path, out var asset))
		{
			asset = Texture2D.FromFile(Core.Instance.GraphicsDevice, GetAssetPath(path));
			store[path] = asset;
		}
		return asset;
	}

	public static BitmapFont LoadBitmapFont(string path)
	{
		if (!TryGetAsset<BitmapFont>(path, out var asset))
		{
			asset = BitmapFont.FromFile(GetAssetPath(path));
			store[path] = asset;
		}
		return asset;
	}

	private static bool TryGetAsset<T>(string path, out T result) where T : class
	{
		if (store.TryGetValue(path, out var asset))
		{
			if (asset is T casted)
			{
				result = casted;
				return true;
			}
			Debug.Fail("Wrong asset type for '{0}', actual {1} but {2} expected.", path, asset.GetType(), typeof(T));
		}

		result = null;
		return false;
	}

	private static T AddAsset<T>(string path, T asset) where T : class
	{
		store[path] = asset;
		return asset;
	}

	private static string GetAssetPath(string path)
	{
		return Core.Instance.Content.RootDirectory + "/" + path;
	}
}