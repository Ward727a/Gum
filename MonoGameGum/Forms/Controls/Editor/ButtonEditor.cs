using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameGum.Forms.Controls.Primitives;
using RenderingLibrary.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameGum.Forms.Controls.Editor
{
    public class ButtonEditor : ButtonBase
    {
        /// <summary>
        /// The name of the Category containing visual states for the Button object.
        /// </summary>
        public const string ButtonCategoryState = "ButtonCategoryState";

        public bool Drag = false;
        public Point DragMargin = new Point(0, 0);

        public event EventHandler StartDrag;
        public event EventHandler Dragged;
        public event EventHandler StopDrag;

        #region Fields/Properties

        GraphicalUiElement textComponent;

        RenderingLibrary.Graphics.Text coreTextObject;

        /// <summary>
        /// Text displayed by the button. This property requires that the TextInstance instance be present in the Gum component.
        /// If the TextInstance instance is not present, an exception will be thrown in DEBUG mode
        /// </summary>
        public string Text
        {
            get
            {
#if DEBUG
                ReportMissingTextInstance();
#endif
                return coreTextObject.RawText;
            }
            set
            {
#if DEBUG
                ReportMissingTextInstance();
#endif
                // go through the component instead of the core text object to force a layout refresh if necessary
                textComponent?.SetProperty("Text", value);
            }
        }

        /// <summary>
        /// Whether the button is enabled or not. When disabled, the button will not respond to user input, and will display
        /// a disabled state.
        /// </summary>
        public override bool IsEnabled
        {
            get
            {
                return base.IsEnabled;
            }
            set
            {
                base.IsEnabled = value;

                UpdateState();
            }
        }

        #endregion

        #region Initialize Methods

        public ButtonEditor() : base() {

        
        }

        public ButtonEditor(InteractiveGue visual) : base(visual) { }

        protected override void ReactToVisualChanged()
        {
            // text component is optional:
            textComponent = base.Visual.GetGraphicalUiElementByName("TextInstance");
            coreTextObject = textComponent?.RenderableComponent as RenderingLibrary.Graphics.Text;
            Visual.Dragging += Visual_Dragging;
            Visual.LosePush += Visual_LosePush;

            UpdateState();
        }

        private void Visual_LosePush(object? sender, EventArgs e)
        {
            if(Drag && Mouse.GetState().LeftButton == ButtonState.Released)
            {
                Drag = false;
                StopDrag?.Invoke(this, EventArgs.Empty);
            }
        }

        private void Visual_Dragging(object? sender, EventArgs e)
        {
            OnDrag();
        }


        #endregion

        #region UpdateTo Methods

        public override void UpdateState()
        {
            var state = base.GetDesiredState();

            Visual.SetProperty(ButtonCategoryState, state);
        }

        #endregion

        #region Utilities

#if DEBUG
        private void ReportMissingTextInstance()
        {
            if (textComponent == null)
            {
                throw new Exception(
                    $"This button was created with a Gum component ({Visual?.ElementSave}) " +
                    "that does not have an instance called 'TextInstance'. A 'TextInstance' instance must be added to modify the button's Text property.");
            }
        }
#endif

        #endregion

        public int tried = 0;
        protected virtual void OnDrag()
        {
            if(!Drag)
            {
                this.StartDrag?.Invoke(this, EventArgs.Empty);
                Drag = true;

                var x = this.AbsoluteLeft;
                DragMargin = new Point(Mouse.GetState().X - (int)this.AbsoluteLeft, Mouse.GetState().Y - (int)this.AbsoluteTop);

            } else
            {
                this.Dragged?.Invoke(this, EventArgs.Empty);
            }
            Point mouse = Mouse.GetState().Position;
            Point newPos = Mouse.GetState().Position - DragMargin;
            this.X = newPos.X;
            this.Y = newPos.Y;
        }

    }
}
