using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

// TODO window error state (blink title red?)
class Window : Widget
{
	public string Title
	{
		get => titleLabel.Text;
		set => titleLabel.Text = value;
	}

	public Window SetTitle(string title)
	{
		Title = title;
		return this;
	}

	public bool Draggable { get; set; } = true;
	private bool drag;
	private Vector2 dragOffset;

	private Panel title;
	private TextLabel titleLabel;
	public Panel Content;

	public Window()
	{
		title = Add<Panel>();
		titleLabel = title.Add<TextLabel>();
		titleLabel.VerticalAlign = VerticalAlign.Center;

		Content = Add<Panel>();
	}

	public override void OnAdded()
	{
		base.OnAdded();

		//title.Patch = Skin.WindowTitlePatch;
		Resize();
	}

	protected override void OnSkinChanged(Skin skin)
	{
		title.Skin = skin.GetChild("Title");
		Content.Skin = skin.GetChild("Content");
	}

	public void Resize()
	{
		Content.Resize();
		Debug.Log("Content size={0}", Content.Size);

		var titleLabelSize = titleLabel.Skin.Font.GetSize(titleLabel.Text);
		var width = Math.Max(titleLabelSize.X + title.Skin.Patch.PatchSize.X * 2 + 2, Content.Size.X);

		title.Size = new Point(width, 12);
		titleLabel.Size = new Point(titleLabelSize.X, 12);
		titleLabel.LocalPosition = new Vector2(title.Skin.Patch.PatchSize.X + 1, 0f);

		Content.Size = new Point(width, Content.Size.Y);

		title.SetLocalPosition(Skin.Patch.PatchSize.X + 1, Skin.Patch.PatchSize.Y + 1);
		Content.SetLocalPosition(Skin.Patch.PatchSize.X + 1, Skin.Patch.PatchSize.Y  + title.Size.Y + 3);

		// TODO if Content's size is not set window takes full screen... mmh. -> will be fixed with proper Resize() impl

		Size = new Point
		{
			X = Content.Size.X + Skin.Patch.PatchSize.X * 2 + 2,
			Y = title.Size.Y + Content.Size.Y + Skin.Patch.PatchSize.Y * 2 + 4
		};
	}

	public override void Update()
	{
		base.Update();

		if (drag)
		{
			var position = Input.MousePosition - Parent.Position + dragOffset;

			// TODO Mathf.Clamp

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

			LocalPosition = position;
		}
	}

	public override void OnMousePressed()
	{
		base.OnMousePressed();

		BringToFront();

		if (Draggable)
		{
			if (title.Contains(Input.MousePosition))
			{
				drag = true;
				dragOffset = Position - Input.MousePosition;
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
		spriteBatch.DrawPatch(Skin.Patch, Position, Size, Color);
		//spriteBatch.DrawPatch(Skin.WindowTitlePatch, ScreenPosition, new Point(Size.X, 8), Color);
		//spriteBatch.DrawPatch(Skin.WindowFramePatch, ScreenPosition + new Vector2(0f, 8f), new Point(Size.X, Size.Y - 8), Color);

		//if (Title != null)
		//{
		//	spriteBatch.DrawText(Skin.WindowTitleFont, Position + new Vector2(4f, 2f), Title, Skin.WindowTitleTextColor);
		//}

		base.Draw(spriteBatch);
	}

	public override string ToString()
	{
		return string.Format("[Window pos={0} title={1}]", Position, Title);
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