using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

class SkinDEP
{
	public Texture2D Texture { get; set; }
	public BitmapFont Font { get; set; }
	public Color Color { get; set; }
	public Color ErrorColor { get; set; }
	public Color DisabledColor { get; set; }
	public Patch Patch { get; set; }
	public Patch WindowFramePatch { get; set; }
	public Patch WindowTitlePatch { get; set; }
	public Patch WindowPanelPatch { get; set; }
	public BitmapFont WindowTitleFont { get; set; }
	public Color WindowTitleTextColor { get; set; }
	public Patch ButtonReleasedPatch { get; set; }
	public Patch ButtonPressedPatch { get; set; }
	public Patch BarBackgroundPatch { get; set; }
	public Patch BarFramePatch { get; set; }
}

class Skin
{
	public Texture2D Texture { get; set; }
	public BitmapFont Font { get; set; }
	public Color Color { get; set; }
	public Color? ErrorColor { get; set; }
	public Color? DisabledColor { get; set; }
	public Patch Patch { get; set; }

	private readonly Dictionary<string, Skin> children = new Dictionary<string, Skin>();

	public Skin CreateChild(string name)
	{
		var child = (Skin)MemberwiseClone();
		children.Add(name, child);
		return child;
	}

	public Skin GetChild(string name)
	{
		if (children.TryGetValue(name, out var skin))
		{
			return skin;
		}

		return null;
	}
}