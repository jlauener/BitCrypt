using System.Collections.Generic;

namespace BitUi
{

	// TODO improve handling of mouse pressed/released event
	// should be able to keep the mouse down, go out of the widget -> mouse released
	// then, while keeping the button down, go back on the widget -> mouse pressed
	// --> even more critical with enemies!!!
	public class RootWidget : Widget
	{
		private readonly List<Widget> overedWidgets = new List<Widget>();
		private readonly List<Widget> nextOveredWidgets = new List<Widget>();
		private readonly HashSet<Widget> mousePressedWidgets = new HashSet<Widget>();

		public override void Update()
		{
			base.Update();

			Query(Input.MousePosition, nextOveredWidgets);

			for (var i = overedWidgets.Count - 1; i >= 0; i--)
			{
				var w = overedWidgets[i];
				if (!nextOveredWidgets.Contains(w))
				{
					w.IsMouseOver = false;
					overedWidgets.RemoveAt(i);
					mousePressedWidgets.Remove(w);
				}
			}

			foreach (var w in nextOveredWidgets)
			{
				if (!overedWidgets.Contains(w))
				{
					w.IsMouseOver = true;
					overedWidgets.Add(w);
				}
			}

			nextOveredWidgets.Clear();

			if (Input.WasMousePressed(MouseButton.Left))
			{
				foreach (var widget in overedWidgets)
				{
					widget.OnMousePressed();
					mousePressedWidgets.Add(widget);
				}
			}

			if (Input.WasMouseReleased(MouseButton.Left))
			{
				foreach (var w in mousePressedWidgets)
				{
					w.OnMouseReleased();
				}
				mousePressedWidgets.Clear();
			}
		}
	}

}