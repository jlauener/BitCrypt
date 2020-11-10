using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;

enum StyleState
{
	Alt,
	Disabled,
	Overed
}

class Style
{
	public const string Panel = "Panel";
	public const string Label = "Label";
	public const string Button = "Button";
	public const string Bar = "Bar";

	public const string WindowTitle = "WindowTitle";

	public BitmapFont Font { get; set; }
	public Patch Patch { get; set; }
	public Patch PatchAlt { get; set; }

	private Color? color;
	public Color Color
	{
		get => color.HasValue ? color.Value : Color.White;
		set => color = value;
	}

	private Vector2? shadowOffset;
	public Vector2 ShadowOffset
	{
		get => shadowOffset.HasValue ? shadowOffset.Value : Vector2.Zero;
		set => shadowOffset = value;
	}

	public Patch ShadowPatch { get; set; }

	private Style parent;

	private readonly Dictionary<string, Style> classes = new Dictionary<string, Style>();

	// TODO maybe there is something more efficient than a dictionary to store an enum map?
	private readonly Dictionary<StyleState, Style> states = new Dictionary<StyleState, Style>();

	public Style()
	{
	}

	public Style(Color color)
	{
		Color = color;
	}

	public Style(Patch patch)
	{
		Patch = patch;
	}

	private void InitChild(Style child)
	{
		child.parent = this;
		if (child.Font == null) child.Font = Font;
		if (child.Patch == null) child.Patch = Patch;
		if (!child.color.HasValue) child.color = Color;
		if (!child.shadowOffset.HasValue) child.shadowOffset = ShadowOffset;
	}

	public Style AddClass(string styleClass, Style style = null)
	{
		if (style == null) style = new Style();		

		InitChild(style);
		classes.Add(styleClass, style);
		return style;
	}

	public Style GetClass(string styleClass)
	{
		if (classes.TryGetValue(styleClass, out var style))
		{
			return style;
		}

		return parent.GetClass(styleClass);
	}

	public Style AddState(StyleState styleState, Style style)
	{
		InitChild(style);
		states.Add(styleState, style);
		return this;
	}

	public Style GetState(StyleState styleState)
	{
		if (states.TryGetValue(styleState, out var style))
		{
			return style;
		}

		return this;
	}
}