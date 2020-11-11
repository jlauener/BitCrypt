using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace BitUi
{

	public class Widget
	{
		public event Action OnRemovedEvent;

		public Widget Parent { get; set; }

		private SkinDEP skinDEP;
		public SkinDEP SkinDEP
		{
			get => skinDEP != null ? skinDEP : (Parent != null ? Parent.SkinDEP : null);
			set
			{
				skinDEP = value;
			}
		}

		public Widget SetSkinDEP(SkinDEP skin)
		{
			SkinDEP = skin;
			return this;
		}

		public string StyleClass { get; set; }

		public Widget SetStyleClass(string styleClass)
		{
			StyleClass = styleClass;
			return this;
		}

		private Style style;
		public Style Style
		{
			get => style;
			set
			{
				style = StyleClass != null ? value.GetClass(StyleClass) : value;
			}
		}

		public Widget SetStyle(Style style)
		{
			Style = style;
			return this;
		}

		protected Style CurrentStyle { get; private set; }

		public Vector2 LocalPosition { get; set; }
		public Widget SetLocalPosition(Vector2 position)
		{
			LocalPosition = position;
			return this;
		}
		public Widget SetLocalPosition(float x, float y)
		{
			return SetLocalPosition(new Vector2(x, y));
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

		protected Point? size;
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

		//public virtual void Resize()
		//{
		//	foreach (var child in Children.list)
		//	{
		//		child.Resize();
		//	}
		//}

		public Color ColorDEP { get; set; } = Color.White;

		public Widget SetColorDEP(Color color)
		{
			ColorDEP = color;
			return this;
		}

		private bool enabled = true;
		public bool Enabled
		{
			get => (Parent == null || Parent.Enabled) ? enabled : false;
			set => enabled = value;
		}

		public Widget SetEnabled(bool enabled)
		{
			Enabled = enabled;
			return this;
		}

		public bool AlwaysOnTop { get; set; }

		// TODO Optimize ScreenPosition calculation. Maybe add a dirty flag? Something like repaint !
		// same for Enabled, in general optimize "recursive" properties
		public Vector2 Position
		{
			get => (Parent != null ? Parent.Position + LocalPosition : LocalPosition) + Offset;
			// TODO set global position
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

		private bool added;

		public T Add<T>(T child) where T : Widget
		{
			child.Parent = this;
			if (SkinDEP != null) child.SkinDEP = SkinDEP;

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
			var bounds = new Rectangle((int)Position.X, (int)Position.Y, Size.X, Size.Y);
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

		public virtual void OnAdded()
		{
			if (Style == null && Parent != null && Parent.Style != null)
			{
				Style = Parent.Style.GetClass(StyleClass);
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

		// TODO keep it like that, or have a system to register update? avoids going trough the full tree each frame. Worth it?
		public virtual void Update()
		{
			// TODO added handling doesn't belong here
			if (!added)
			{
				OnAdded();
				added = true;
			}

			// TODO resolving patch state doesn't belong here, make it reactive
			if (Style != null)
			{
				if (!Enabled)
				{
					CurrentStyle = Style.GetState(StyleState.Disabled);
				}
				else if (IsMouseOver)
				{
					CurrentStyle = Style.GetState(StyleState.Overed);
				}
				else
				{
					CurrentStyle = Style;
				}
			}

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

		public virtual void DrawDebug(SpriteBatch spriteBatch)
		{
			spriteBatch.DrawRect(new Rectangle((int)Position.X, (int)Position.Y, Size.X, Size.Y), Color.Yellow);
			for (var i = Children.Count - 1; i >= 0; i--)
			{
				Children.list[i].DrawDebug(spriteBatch);
			}
		}
	}

}