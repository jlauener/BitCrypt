using Microsoft.Xna.Framework.Graphics;

namespace BitUi
{

	public class Panel : Layout
	{
		public Panel()
		{
			StyleClass = Style.Panel;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.DrawPatch(CurrentStyle.Patch, Position, Size, ColorDEP);
			base.Draw(spriteBatch);
		}

		public override string ToString()
		{
			return string.Format("[Panel pos={0}]", Position);
		}
	}

}