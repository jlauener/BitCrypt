using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Skin
{
	public Texture2D Texture { get; set; }
	public BitmapFont Font { get; set; }
	public Color Color { get; set; }
	public Patch Patch { get; set; }
	public Patch WindowFramePatch { get; set; }
	public Patch WindowTitlePatch { get; set; }
	public BitmapFont WindowTitleFont { get; set; }
	public Color WindowTitleTextColor { get; set; }
	public Patch ButtonReleasedPatch { get; set; }
	public Patch ButtonPressedPatch { get; set; }
}