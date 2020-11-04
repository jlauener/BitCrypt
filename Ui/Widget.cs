using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

class Widget
{
	public event Action OnRemovedEvent;

	public Widget Parent { get; set; }

	private Skin skin;
	public Skin Skin
	{
		get => skin != null ? skin : (Parent != null ? Parent.Skin : null);
		set
		{
			skin = value;
			if (skin != null) ApplySkin();
		}
	}

	public Widget SetSkin(Skin skin)
	{
		Skin = skin;
		return this;
	}

	protected virtual void ApplySkin()
	{
	}

	public Vector2 Position { get; set; }
	public Widget SetPosition(Vector2 position)
	{
		Position = position;
		return this;
	}
	public Widget SetPosition(float x, float y)
	{
		return SetPosition(new Vector2(x, y));
	}

	public Vector2 Offset { get; set; }
	public Widget SetOffset(Vector2 offset)
	{
		Offset = offset;
		return this;
	}
	public Widget SetOffset(float x, float y)
	{
		return SetOffset(new Vector2(x, y));
	}

	private Point? size;
	public Point Size
	{
		get => size.HasValue ? size.Value : Parent.Size;
		set => size = value;
	}
	public Widget SetSize(Point size)
	{
		Size = size;
		return this;
	}
	public Widget SetSize(int w, int h)
	{
		return SetSize(new Point(w, h));
	}

	public Color Color { get; set; } = Color.White;

	public Widget SetColor(Color color)
	{
		Color = color;
		return this;
	}

	public bool AlwaysOnTop { get; set; }

	// TODO Optimize ScreenPosition calculation. Maybe add a dirty flag? Something like repaint !
	public Vector2 ScreenPosition
	{
		get => (Parent != null ? Parent.ScreenPosition + Position : Position) + Offset;
	}

	public SafeList<Widget> Children { get; } = new SafeList<Widget>();

	private bool isMouseOver;
	public bool IsMouseOver
	{
		get => isMouseOver;
		set
		{
			if (!isMouseOver && value)
			{
				isMouseOver = true;
				OnMouseEnter();
			}
			else if (IsMouseOver && !value)
			{
				isMouseOver = false;
				OnMouseLeave();
			}
		}
	}

	public T Add<T>(T child) where T : Widget
	{
		child.Parent = this;

		var targetIndex = 0;
		for (var i = 0; i < Children.Count; i++)
		{
			if (!Children.list[i].AlwaysOnTop)
			{
				targetIndex = i;
				break;
			}
		}

		Children.Insert(targetIndex, child);
		child.OnAdded(); // TODO must be called after all sets call, or call add a the end.. but not convenient?
		return child;
	}

	public T Add<T>() where T : Widget
	{
		return Add((T)Activator.CreateInstance(typeof(T)));
	}

	public T Get<T>() where T : Widget
	{
		foreach (var child in Children.list)
		{
			if (child is T)
			{
				return child as T;
			}
		}
		return null;
	}

	public void Remove()
	{
		Parent.Remove(this);
	}

	public void Remove(Widget child)
	{
		Debug.Insist(child.Parent == this, "Cannot remove {0} from {1}: Not a child.", child, this);

		Children.Remove(child);
		child.Parent = null;
		child.OnRemoved();

		OnRemovedEvent?.Invoke();
	}

	public void BringToFront()
	{
		if (AlwaysOnTop)
		{
			// cannot be reordered
			return;
		}

		var targetIndex = 0;
		for (var i = 0; i < Parent.Children.Count; i++)
		{
			var other = Parent.Children.list[i];
			if (other == this)
			{
				// nothing to do, already at the correct position in the list
				return;
			}

			if (!other.AlwaysOnTop)
			{
				targetIndex = i;
				break;
			}
		}
		Parent.Children.Remove(this);
		Parent.Children.Insert(targetIndex, this);
	}

	public bool Contains(Vector2 position)
	{
		var bounds = new Rectangle((int)ScreenPosition.X, (int)ScreenPosition.Y, Size.X, Size.Y);
		return bounds.Contains(new Point((int)position.X, (int)position.Y));
	}

	public bool Query(Vector2 position, List<Widget> result)
	{
		if (!Contains(position))
		{
			return false;
		}

		result.Add(this);
		foreach (var child in Children.list)
		{
			if (child.Query(position, result))
			{
				return true;
			}
		}

		return true;
	}

	// TODO On added should be called after all sets method are called. Call it before first active frame.
	public virtual void OnAdded()
	{
		if (skin == null && Parent.Skin != null)
		{
			ApplySkin();
		}
	}

	public virtual void OnRemoved()
	{

	}

	public virtual void OnMouseEnter()
	{
		//Debug.WriteLine("{0} OnMouseEnter", this);
		IsMouseOver = true;
	}

	public virtual void OnMouseLeave()
	{
		//Debug.WriteLine("{0} OnMouseLeave", this);
		IsMouseOver = false;
	}

	public virtual void OnMousePressed()
	{
		//Debug.WriteLine("{0} OnMousePressed", this);
	}

	public virtual void OnMouseReleased()
	{
		//Debug.WriteLine("{0} OnMouseReleased", this);
	}

	public virtual void Update()
	{
		Children.ForEach((child) =>
		{
			child.Update();
		});
	}

	public virtual void Draw(SpriteBatch spriteBatch)
	{
		for (var i = Children.Count - 1; i >= 0; i--)
		{
			Children.list[i].Draw(spriteBatch);
		}
	}
}