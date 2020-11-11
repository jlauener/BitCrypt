using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace BitUi
{

	public class Layout : Widget
	{
		public Patch Background { get; set; }

		public Layout SetBackground(Patch background)
		{
			Background = background;
			return this;
		}

		public Point Margin = new Point(4, 4);
		public Point Pan = new Point(4, 4);

		public Layout PackVertically()
		{
			var size = Margin;

			for (var i = Children.Count - 1; i >= 0; i--)
			{
				var child = Children.list[i];
				child.LocalPosition = new Vector2(Margin.X, size.Y);
				size.Y += child.Size.Y;
				if (i > 0) size.Y += Pan.Y;
				size.X = Math.Max(size.X, child.Size.X + Margin.X);
			};

			size += Margin;

			// TODO either make it work with any size, or use 4px basic unit (seems like a good idea ?)
			size.X = ((int)Math.Ceiling(size.X / 4f)) * 4;
			size.Y = ((int)Math.Ceiling(size.Y / 4f)) * 4;

			Size = size;
			return this;
		}

		public Layout PackHorizontally()
		{
			var size = Margin;

			for (var i = Children.Count - 1; i >= 0; i--)
			{
				var child = Children.list[i];
				child.LocalPosition = new Vector2(size.X, Margin.Y);
				size.X += child.Size.X;
				if (i > 0) size.X += Pan.X;
				size.Y = Math.Max(size.Y, child.Size.Y + Margin.Y);
			};

			size += Margin;

			// TODO either make it work with any size, or use 4px basic unit (seems like a good idea ?)
			size.X = ((int)Math.Ceiling(size.X / 4f)) * 4;
			size.Y = ((int)Math.Ceiling(size.Y / 4f)) * 4;

			Size = size;
			return this;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			if (Background != null)
			{
				spriteBatch.DrawPatch(Background, Position, Size, ColorDEP);
			}

			base.Draw(spriteBatch);
		}
	}

}