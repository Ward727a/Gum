using Gum.Converters;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;

using GameUiSamples.Components;

using RenderingLibrary.Graphics;

using System.Linq;

using MonoGameGum.GueDeriving;
using System;
using Microsoft.Xna.Framework.Input;
using MonoGameGum.Forms;
namespace GameUiSamples.Components
{
    partial class HotbarRuntime : ContainerRuntime
    {
        public event EventHandler SelectedIndexChanged;

        int selectedIndex = -1;
        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                if(value != selectedIndex)
                {
                    selectedIndex = value;
                    UpdateToSelectedIndex();
                    SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        partial void CustomInitialize()
        {
            foreach(ItemSlotRuntime item in ItemSlotContainer.Children)
            {
                item.Click += HandleItemSlotClicked;
            }
        }

        internal void HandleKeyboardInput()
        {
            var keyboard = FormsUtilities.Keyboard;
            int? indexToSelect = null;
            if (keyboard.KeyPushed(Keys.D1)) indexToSelect = 0;
            if (keyboard.KeyPushed(Keys.D2)) indexToSelect = 1;
            if (keyboard.KeyPushed(Keys.D3)) indexToSelect = 2;
            if (keyboard.KeyPushed(Keys.D4)) indexToSelect = 3;
            if (keyboard.KeyPushed(Keys.D5)) indexToSelect = 4;
            if (keyboard.KeyPushed(Keys.D6)) indexToSelect = 5;
            if (keyboard.KeyPushed(Keys.D7)) indexToSelect = 6;
            if (keyboard.KeyPushed(Keys.D8)) indexToSelect = 7;
            if (keyboard.KeyPushed(Keys.D9)) indexToSelect = 8;

            if (indexToSelect != null)
            {
                SelectedIndex = indexToSelect.Value;
            }
        }

        private void HandleItemSlotClicked(object sender, EventArgs args)
        {
            var itemSlot = (ItemSlotRuntime)sender;

            var index = ItemSlotContainer.Children.IndexOf(itemSlot);
            SelectedIndex = index;
        }

        void UpdateToSelectedIndex()
        {
            for (int i = 0; i < ItemSlotContainer.Children.Count; i++)
            {
                var slot = (ItemSlotRuntime)ItemSlotContainer.Children[i];

                slot.IsHighlighted = i == SelectedIndex;
            }
        }
    }
}
