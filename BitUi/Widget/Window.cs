using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BitUi
{

	// TODO window error state (blink title red?)
	public class Window : Widget
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

		private readonly Panel titlePanel;
		private readonly TextLabel titleLabel;

		public Window()
		{
			titlePanel = Add<Panel>();
			titlePanel.StyleClass = Style.WindowTitle;
			titleLabel = titlePanel.Add<TextLabel>();
			Offset = new Vector2(-1f, -1f);
		}

		public override void OnAdded()
		{
			base.OnAdded();

			titlePanel.SetSize(Size.X - Style.Margin.X * 2, 18).SetLocalPosition(Style.Margin.X, Style.Margin.Y);
			titleLabel
				.SetVerticalAlign(VerticalAlign.Center)
				.SetSize(titlePanel.Size.X - 6, titlePanel.Size.Y)
				.SetLocalPosition(6, 0)
			;
		}

		//public override void Resize()
		//{
		//	base.Resize();

		//	Debug.Log("Content size={0}", Content.Size);

		//	//var titleLabelSize = titleLabel.Skin.Font.GetSize(titleLabel.Text);
		//	//var width = Math.Max(titleLabelSize.X + title.Skin.Patch.Size.X * 2 + 2, Content.Size.X);

		//	//title.Size = new Point(width, 12);
		//	//titleLabel.Size = new Point(titleLabelSize.X, 12);
		//	//titleLabel.LocalPosition = new Vector2(title.Skin.Patch.Size.X + 1, 0f);

		//	//Content.Size = new Point(width, Content.Size.Y);

		//	//title.SetLocalPosition(patch.Size.X + 1, patch.Size.Y + 1);
		//	//Content.SetLocalPosition(patch.Size.X + 1, patch.Size.Y  + title.Size.Y + 3);

		//	//// TODO if Content's size is not set window takes full screen... mmh. -> will be fixed with proper Resize() impl

		//	//Size = new Point
		//	//{
		//	//	X = Content.Size.X + patch.Size.X * 2 + 2,
		//	//	Y = title.Size.Y + Content.Size.Y + patch.Size.Y * 2 + 4
		//	//};
		//}

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
				if (titlePanel.Contains(Input.MousePosition))
				{
					drag = true;
					dragOffset = Position - Input.MousePosition;
					Offset = new Vector2(-2f, -6f);

					Style.WindowDragSfx?.Play();
				}
			}
		}

		public override void OnMouseReleased()
		{
			base.OnMouseReleased();

			if (drag)
			{
				drag = false;
				Offset = new Vector2(-1f, -1f);

				Style.WindowDropSfx?.Play();
			}
		}

		public override void OnMouseLeave()
		{
			base.OnMouseLeave();

			// TODO mouse leave should stop drag... related to other mouse event issue
			if (drag)
			{
				drag = false;
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			if (CurrentStyle.ShadowPatch != null)
			{
				spriteBatch.DrawPatch(CurrentStyle.ShadowPatch, Position - Offset, Size, new Color(0f, 0f, 0f, 0.5f));
			}
			spriteBatch.DrawPatch(CurrentStyle.Patch, Position, Size, Color.White);
			base.Draw(spriteBatch);
		}

		public override string ToString()
		{
			return string.Format("[Window pos={0} title={1}]", Position, "TODO");
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

}