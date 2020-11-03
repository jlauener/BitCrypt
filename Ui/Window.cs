using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Security.Cryptography.X509Certificates;

class Window : Widget
{
	public string Title { get; set; }
	public Window SetTitle(string title)
	{
		Title = title;
		return this;
	}

	public bool Draggable { get; set; } = true;
	private bool drag;
	private Vector2 dragOffset;

	public Window Pack()
	{
		var width = 0;
		var height = 12;
		var position = new Vector2(8f, 16f);

		for (var i = Children.Count - 1; i >= 0; i--)
		{
			var child = Children.list[i];
			child.Position = position;
			position.Y += child.Size.Y + 4f;
			height += child.Size.Y + 4;

			width = Math.Max(width, child.Size.X);
		};

		width += 16;
		height += 8;

		// TODO either make it work with any size, or use 4px basic unit (seems like a good idea ?)
		width = ((int)Math.Ceiling(width / 4f)) * 4;
		height = ((int)Math.Ceiling(height / 4f)) * 4;

		Size = new Point(width, height);
		return this;
	}

	public override void Update()
	{
		base.Update();

		if (drag)
		{
			var position = Input.MousePosition - Parent.ScreenPosition + dragOffset;

			// TODO Mathf.Clamp?

			if (position.X < 0f)
			{
				position.X = 0f;
			}
			else if (position.X + Size.X > Parent.Size.X)
			{
				position.X = Parent.Size.X - Size.X;
			}

			if (position.Y < 0f)
			{
				position.Y = 0f;
			}
			else if (position.Y + Size.Y > Parent.Size.Y)
			{
				position.Y = Parent.Size.Y - Size.Y;
			}

			Position = position;
		}
	}

	public override void OnMousePressed()
	{
		base.OnMousePressed();

		BringToFront();

		if (Draggable)
		{
			var localPosition = Input.MousePosition - ScreenPosition;
			if (localPosition.Y < 12f)
			{
				// TODO use widget for title bar
				// Click on title bar
				drag = true;
				dragOffset = ScreenPosition - Input.MousePosition;
			}
		}
	}

	public override void OnMouseReleased()
	{
		base.OnMouseReleased();

		if (drag)
		{
			drag = false;
		}
	}

	public override void OnMouseLeave()
	{
		base.OnMouseLeave();

		if (drag)
		{
			drag = false;
		}
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Skin.WindowTitlePatch, ScreenPosition, new Point(Size.X, 8));
		spriteBatch.Draw(Skin.WindowFramePatch, ScreenPosition + new Vector2(0f, 8f), new Point(Size.X, Size.Y - 8));

		spriteBatch.DrawText(Skin.WindowTitleFont, ScreenPosition + new Vector2(4f, 2f), Title, Skin.WindowTitleTextColor);

		base.Draw(spriteBatch);
	}

	public override string ToString()
	{
		return string.Format("[Window pos={0} title={1}]", ScreenPosition, Title);
	}

	public Vector2 GetRandomPosition()
	{
		return new Vector2
		{
			X = Core.Random.NextFloat(Parent.Size.X - Size.X),
			Y = Core.Random.NextFloat(Parent.Size.Y - Size.Y)
		};
	}
}