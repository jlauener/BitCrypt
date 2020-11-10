using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

public enum PanelLayout
{
	None,
	Vertical,
	Horizontal
}

class Panel : Widget
{
	public PanelLayout Layout { get; set; }

	// TODO use child's margin for pan, make margin a widget thingy...
	public Point? Margin { get; set; }
	public Point Pan { get; set; } = new Point(2, 2);

	public Panel()
	{
		StyleClass = Style.Panel;
	}

	//public override void Resize()
	//{
	//	base.Resize();

	//	switch (Layout)
	//	{
	//		case PanelLayout.Vertical:
	//			PackVertically();
	//			break;
	//		case PanelLayout.Horizontal:
	//			PackHorizontally();
	//			break;
	//		case PanelLayout.None:
	//			// TODO
	//			break;
	//	}
	//}

	//private void PackVertically()
	//{
	//	var margin = GetMargin();

	//	var size = margin;

	//	for (var i = Children.Count - 1; i >= 0; i--)
	//	{
	//		var child = Children.list[i];
	//		child.LocalPosition = new Vector2(margin.X, size.Y);
	//		size.Y += child.Size.Y;
	//		if (i > 0) size.Y += Pan.Y;
	//		size.X = Math.Max(size.X, child.Size.X + margin.X);
	//	};

	//	size += margin;
	//	Size = size;
	//}

	//private void PackHorizontally()
	//{
	//	var margin = GetMargin();

	//	var size = margin;

	//	for (var i = Children.Count - 1; i >= 0; i--)
	//	{
	//		var child = Children.list[i];
	//		child.LocalPosition = new Vector2(size.X, margin.Y);
	//		size.X += child.Size.X;
	//		if (i > 0) size.X += Pan.X;
	//		size.Y = Math.Max(size.Y, child.Size.Y + margin.Y);
	//	};

	//	size += margin;
	//	Size = size;
	//}

	//private Point GetMargin()
	//{
	//	if (Margin.HasValue)
	//	{
	//		return Margin.Value;
	//	}

	//	//if (patch != null)
	//	//{
	//	//	return new Point
	//	//	{
	//	//		X = patch.Size.X + Pan.X,
	//	//		Y = patch.Size.Y + Pan.Y
	//	//	};
	//	//}

	//	return Point.Zero;
	//}

	public override void Draw(SpriteBatch spriteBatch)
	{
		if (CurrentStyle.Patch != null)
		{
			spriteBatch.DrawPatch(CurrentStyle.Patch, Position, Size, ColorDEP);
		}
		base.Draw(spriteBatch);
	}

	public override string ToString()
	{
		return string.Format("[Panel pos={0}]", Position);
	}
}