using Gum.Wireframe;
using Microsoft.Xna.Framework;
using MonoGameGum.Forms.Controls.Editor;
using MonoGameGum.GueDeriving;
using RenderingLibrary.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameGum.Forms.EditorVisuals
{
    public class EditorButtonRuntime : InteractiveGue
    {
        public TextRuntime TextInstance { get; private set; }
        public EditorButtonRuntime(bool fullInstantiation = true, bool tryCreateFormsObject = true) : base(new InvisibleRenderable())
        {
            if (fullInstantiation)
            {
                this.Width = 128;
                this.Height = 32;

                var background = new ColoredRectangleRuntime();
                background.Width = 0;
                background.Height = 0;
                background.WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToParent;
                background.HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToParent;
                background.Name = "ButtonBackground";
                this.Children.Add(background);

                var SizePoint = new RectangleRuntime();
                SizePoint.X = -10;
                SizePoint.Y = -10;
                SizePoint.Width = 10;
                SizePoint.Height = 10;
                SizePoint.LineWidth = 2;
                SizePoint.Name = "ResizeInstance";
                SizePoint.WidthUnits = Gum.DataTypes.DimensionUnitType.ScreenPixel;
                SizePoint.HeightUnits = Gum.DataTypes.DimensionUnitType.ScreenPixel;
                SizePoint.XOrigin = RenderingLibrary.Graphics.HorizontalAlignment.Left;
                SizePoint.YOrigin = RenderingLibrary.Graphics.VerticalAlignment.Top;
                SizePoint.XUnits = Gum.Converters.GeneralUnitType.PixelsFromSmall;
                SizePoint.YUnits = Gum.Converters.GeneralUnitType.PixelsFromSmall;
                SizePoint.Color = Color.White;
                this.Children.Add(SizePoint);

                SizePoint = new RectangleRuntime();
                SizePoint.X = -10;
                SizePoint.Y = this.Height;
                SizePoint.Width = 10;
                SizePoint.Height = 10;
                SizePoint.LineWidth = 2;
                SizePoint.Name = "ResizeInstance2";
                SizePoint.WidthUnits = Gum.DataTypes.DimensionUnitType.ScreenPixel;
                SizePoint.HeightUnits = Gum.DataTypes.DimensionUnitType.ScreenPixel;
                SizePoint.XOrigin = RenderingLibrary.Graphics.HorizontalAlignment.Left;
                SizePoint.YOrigin = RenderingLibrary.Graphics.VerticalAlignment.Top;
                SizePoint.XUnits = Gum.Converters.GeneralUnitType.PixelsFromSmall;
                SizePoint.YUnits = Gum.Converters.GeneralUnitType.PixelsFromSmall;
                SizePoint.Color = Color.White;
                this.Children.Add(SizePoint);

                SizePoint = new RectangleRuntime();
                SizePoint.X = 100;
                SizePoint.Y = 100;
                SizePoint.Width = 10;
                SizePoint.Height = 10;
                SizePoint.LineWidth = 2;
                SizePoint.Name = "ResizeInstance3";
                SizePoint.WidthUnits = Gum.DataTypes.DimensionUnitType.ScreenPixel;
                SizePoint.HeightUnits = Gum.DataTypes.DimensionUnitType.ScreenPixel;
                SizePoint.XOrigin = RenderingLibrary.Graphics.HorizontalAlignment.Left;
                SizePoint.YOrigin = RenderingLibrary.Graphics.VerticalAlignment.Top;
                SizePoint.XUnits = Gum.Converters.GeneralUnitType.Percentage;
                SizePoint.YUnits = Gum.Converters.GeneralUnitType.Percentage;
                SizePoint.Color = Color.White;
                this.Children.Add(SizePoint);

                SizePoint = new RectangleRuntime();
                SizePoint.X = 100;
                SizePoint.Y = -10;
                SizePoint.Width = 10;
                SizePoint.Height = 10;
                SizePoint.LineWidth = 2;
                SizePoint.Name = "ResizeInstance4";
                SizePoint.WidthUnits = Gum.DataTypes.DimensionUnitType.ScreenPixel;
                SizePoint.HeightUnits = Gum.DataTypes.DimensionUnitType.ScreenPixel;
                SizePoint.XOrigin = RenderingLibrary.Graphics.HorizontalAlignment.Left;
                SizePoint.YOrigin = RenderingLibrary.Graphics.VerticalAlignment.Top;
                SizePoint.XUnits = Gum.Converters.GeneralUnitType.Percentage;
                SizePoint.YUnits = Gum.Converters.GeneralUnitType.PixelsFromSmall;
                SizePoint.Color = Color.White;
                this.Children.Add(SizePoint);

                var rotationPoint = new CircleRuntime();
                rotationPoint.X = 50;
                rotationPoint.Y = -20;
                rotationPoint.Radius = 7.5f;
                rotationPoint.LineWidth = rotationPoint.Radius;
                rotationPoint.Name = "RotationInstance";
                rotationPoint.WidthUnits = Gum.DataTypes.DimensionUnitType.ScreenPixel;
                rotationPoint.HeightUnits = Gum.DataTypes.DimensionUnitType.ScreenPixel;
                rotationPoint.XOrigin = RenderingLibrary.Graphics.HorizontalAlignment.Left;
                rotationPoint.YOrigin = RenderingLibrary.Graphics.VerticalAlignment.Top;
                rotationPoint.XUnits = Gum.Converters.GeneralUnitType.Percentage;
                rotationPoint.YUnits = Gum.Converters.GeneralUnitType.PixelsFromSmall;
                rotationPoint.Color = Color.YellowGreen;
                this.Children.Add(rotationPoint);

                TextInstance = new TextRuntime();
                TextInstance.X = 0;
                TextInstance.Y = 0;
                TextInstance.Width = 0;
                TextInstance.Height = 0;
                TextInstance.Name = "TextInstance";
                TextInstance.WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToParent;
                TextInstance.HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToParent;
                TextInstance.XOrigin = RenderingLibrary.Graphics.HorizontalAlignment.Center;
                TextInstance.YOrigin = RenderingLibrary.Graphics.VerticalAlignment.Center;
                TextInstance.XUnits = Gum.Converters.GeneralUnitType.PixelsFromMiddle;
                TextInstance.YUnits = Gum.Converters.GeneralUnitType.PixelsFromMiddle;
                TextInstance.HorizontalAlignment = RenderingLibrary.Graphics.HorizontalAlignment.Left;
                TextInstance.VerticalAlignment = RenderingLibrary.Graphics.VerticalAlignment.Top;
                this.Children.Add(TextInstance);


                var buttonCategory = new Gum.DataTypes.Variables.StateSaveCategory();
                buttonCategory.Name = "ButtonCategory";
                buttonCategory.States.Add(new()
                {
                    Name = "Enabled",
                    Variables = new()
                    {
                        new ()
                        {
                            Name = "ButtonBackground.Color",
                            Value = new Color(0, 0, 128),
                        }
                    }
                });

                buttonCategory.States.Add(new()
                {
                    Name = "Highlighted",
                    Variables = new()
                    {
                        new ()
                        {
                            Name = "ButtonBackground.Color",
                            Value = new Color(0, 0, 160),
                        }
                    }
                });

                buttonCategory.States.Add(new()
                {
                    Name = "Pushed",
                    Variables = new()
                    {
                        new ()
                        {
                            Name = "ButtonBackground.Color",
                            Value = new Color(0, 0, 96),
                        }
                    }
                });

                buttonCategory.States.Add(new()
                {
                    Name = "Disabled",
                    Variables = new()
                    {
                        new ()
                        {
                            Name = "ButtonBackground.Color",
                            Value = new Color(48, 48, 64),
                        }
                    }
                });

                this.AddCategory(buttonCategory);
            }

            if (tryCreateFormsObject)
            {
                FormsControlAsObject = new ButtonEditor(this);
            }

        }

        public ButtonEditor FormsControl => FormsControlAsObject as ButtonEditor;
    }
}
