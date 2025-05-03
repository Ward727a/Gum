using Gum.Wireframe;
using Microsoft.Xna.Framework;
using MonoGameGum.Forms.Controls;
using MonoGameGum.GueDeriving;
using RenderingLibrary.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameGum.Forms.DefaultVisuals
{
    public class DefaultPanelRuntime : InteractiveGue
    {

        public ColoredRectangleRuntime BackgroundRectangle { get; private set; }

        public DefaultPanelRuntime(bool fullInstantiation = true, bool tryCreateFormsObject = true) : base(new InvisibleRenderable())
        {
            if (fullInstantiation)
            {
                this.Width = 200;
                this.Height = 200;

                BackgroundRectangle = new ColoredRectangleRuntime();
                BackgroundRectangle.Width = 100;
                BackgroundRectangle.Height = 100;
                //BackgroundRectangle.WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToParent;
                //BackgroundRectangle.HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToParent;
                BackgroundRectangle.Name = "PanelBackground";
                this.Children.Add(BackgroundRectangle);
            }

            if (tryCreateFormsObject)
            {
                FormsControlAsObject = new Panel(this);
            }
        }

        public Panel FormsControl => FormsControlAsObject as Panel;

    }
}
