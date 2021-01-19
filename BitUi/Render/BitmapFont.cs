using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace BitUi
{

	public class BitmapFont
	{
		private readonly Texture2D texture;

		private class CharInfo
		{
			public Rectangle SourceRect;
			public Vector2 Offset;
			public int XAdvance;
		}
		private readonly Dictionary<char, CharInfo> charSet = new Dictionary<char, CharInfo>();

		private int fontHeight;

		private BitmapFont(BitmapFontReader.Font font)
		{
			texture = Asset.LoadTexture2D("font/" + font.Pages[0].File);

			foreach (var chr in font.Chars)
			{
				charSet[(char)chr.Id] = new CharInfo
				{
					SourceRect = new Rectangle(chr.X, chr.Y, chr.Width, chr.Height),
					Offset = new Vector2(chr.XOffset, chr.YOffset),
					XAdvance = chr.XAdvance
				};

				fontHeight = Math.Max(fontHeight, chr.Height + 2);
			}
		}

		public Point GetSize(string text)
		{
			if (text == null)
			{
				return Point.Zero;
			}

			var currentWidth = 0;
			var width = 0;
			var height = fontHeight;

			for (var i = 0; i < text.Length; i++)
			{
				if (text[i] == '\n')
				{
					width = Math.Max(currentWidth, width);
					currentWidth = 0;
					height += fontHeight;
					continue;
				}

				if (charSet.TryGetValue(text[i], out var chr))
				{
					currentWidth += chr.XAdvance;
				}
				else
				{
					Console.WriteLine("ERROR Unknown character '" + text[i] + "' in string '" + text + "',");
				}
			}
			width = Math.Max(currentWidth, width);

			return new Point(width, height);
		}

		public void Draw(SpriteBatch spriteBatch, Vector2 position, string text, Color color)
		{
			var currentPosition = position;

			for (var i = 0; i < text.Length; i++)
			{
				if (text[i] == '\n')
				{
					currentPosition.Y += fontHeight;
					currentPosition.X = position.X;
					continue;
				}

				if (charSet.TryGetValue(text[i], out var chr))
				{
					spriteBatch.Draw(texture, currentPosition + chr.Offset, chr.SourceRect, color);
					currentPosition.X += chr.XAdvance;
				}
				else
				{
					Console.WriteLine("ERROR Unknown character '" + text[i] + "' in string '" + text + "',");
				}
			}
		}

		public static BitmapFont FromFile(string path)
		{

			XmlSerializer serializer = new XmlSerializer(typeof(BitmapFontReader.Font));
			FileStream stream = new FileStream(path, FileMode.Open);
			return new BitmapFont((BitmapFontReader.Font)serializer.Deserialize(stream));
		}
	}

	namespace BitmapFontReader
	{
		[XmlRoot("font")]
		public class Font
		{
			[XmlElement("info")]
			public FontInfo Info { get; set; }

			[XmlElement("common")]
			public FontCommon Common { get; set; }

			[XmlArray("pages")]
			[XmlArrayItem("page")]
			public FontPage[] Pages { get; set; }

			[XmlArray("chars")]
			[XmlArrayItem("char")]
			public FontChar[] Chars { get; set; }

			[XmlArray("kernings")]
			[XmlArrayItem("kerning")]
			public FontKerning[] Kernings { get; set; }
		}

		public class FontInfo
		{
			[XmlAttribute("face")]
			public string Face { get; set; }

			[XmlAttribute("size")]
			public int Size { get; set; }

			[XmlAttribute("bold")]
			public int Bold { get; set; }

			[XmlAttribute("italic")]
			public int Italic { get; set; }

			[XmlAttribute("charset")]
			public string CharSet { get; set; }

			[XmlAttribute("stretchH")]
			public int StretchH { get; set; }

			[XmlAttribute("smooth")]
			public int Smooth { get; set; }

			[XmlAttribute("aa")]
			public int SuperSampling { get; set; }

			[XmlAttribute("padding")]
			public string Padding { get; set; }

			[XmlAttribute("spacing")]
			public string Spacing { get; set; }

			[XmlAttribute("outline")]
			public int Outline { get; set; }
		}

		public class FontCommon
		{
			[XmlAttribute("lineHeight")]
			public int LineHeight { get; set; }

			[XmlAttribute("base")]
			public int Base { get; set; }

			[XmlAttribute("scaleW")]
			public int ScaleW { get; set; }

			[XmlAttribute("scaleH")]
			public int ScaleH { get; set; }

			[XmlAttribute("pages")]
			public int Pages { get; set; }

			[XmlAttribute("packed")]
			public int Packed { get; set; }

			[XmlAttribute("alphaChnl")]
			public int AlphaChnl { get; set; }

			[XmlAttribute("redChnl")]
			public int RedChnl { get; set; }

			[XmlAttribute("blueChnl")]
			public int BlueChnl { get; set; }
		}

		public class FontPage
		{
			[XmlAttribute("id")]
			public int Id { get; set; }

			[XmlAttribute("file")]
			public string File { get; set; }
		}

		public class FontChar
		{
			[XmlAttribute("id")]
			public int Id { get; set; }

			[XmlAttribute("x")]
			public int X { get; set; }

			[XmlAttribute("y")]
			public int Y { get; set; }

			[XmlAttribute("width")]
			public int Width { get; set; }

			[XmlAttribute("height")]
			public int Height { get; set; }

			[XmlAttribute("xoffset")]
			public int XOffset { get; set; }

			[XmlAttribute("yoffset")]
			public int YOffset { get; set; }

			[XmlAttribute("xadvance")]
			public int XAdvance { get; set; }

			[XmlAttribute("page")]
			public int Page { get; set; }

			[XmlAttribute("chnl")]
			public int Channel { get; set; }
		}

		public class FontKerning
		{
			[XmlAttribute("first")]
			public int First { get; set; }

			[XmlAttribute("second")]
			public int Second { get; set; }

			[XmlAttribute("amount")]
			public int Amount { get; set; }
		}
	}

}