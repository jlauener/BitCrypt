using Microsoft.Xna.Framework;
using System;

namespace BitUi
{

	public enum LayoutType
	{
		None,
		Vertical,
		Horizontal
	}

	public class Layout : Widget
	{
		public LayoutType LayoutType { get; set; }

		public Layout SetLayoutType(LayoutType layoutType)
		{
			LayoutType = layoutType;
			return this;
		}

		public override void Resize()
		{
			base.Resize();

			switch (LayoutType)
			{
				case LayoutType.Vertical:
					LayVertically();
					break;
				case LayoutType.Horizontal:
					LayHorizontally();
					break;
				case LayoutType.None:
					break;
			}
		}

		private void LayVertically()
		{
			var size = Style.Margin;

			for (var i = Children.Count - 1; i >= 0; i--)
			{
				var child = Children.list[i];
				child.LocalPosition = new Vector2(Style.Margin.X, size.Y);
				size.Y += child.Size.Y;
				if (i > 0) size.Y += Style.Pan.Y;
				size.X = Math.Max(size.X, child.Size.X + Style.Margin.X);
			};

			size += Style.Margin;
			Size = size;
		}

		private void LayHorizontally()
		{
			var size = Style.Margin;

			for (var i = Children.Count - 1; i >= 0; i--)
			{
				var child = Children.list[i];
				child.LocalPosition = new Vector2(size.X, Style.Margin.Y);
				size.X += child.Size.X;
				if (i > 0) size.X += Style.Pan.X;
				size.Y = Math.Max(size.Y, child.Size.Y + Style.Margin.Y);
			};

			size += Style.Margin;
			ApplyAutoSize(size);
		}

		public override string ToString()
		{
			return string.Format("[Layout type={0} pos={1}]", LayoutType, Position);
		}
	}

}