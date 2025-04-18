//Code for Elements/DividerHorizontal (Container)
using GumRuntime;
using Gum.Converters;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;

using RenderingLibrary.Graphics;

using System.Linq;

using MonoGameGum.GueDeriving;
namespace GumFormsSample.Components
{
    public partial class DividerHorizontalRuntime:ContainerRuntime
    {
        [System.Runtime.CompilerServices.ModuleInitializer]
        public static void RegisterRuntimeType()
        {
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Elements/DividerHorizontal", typeof(DividerHorizontalRuntime));
        }
        public SpriteRuntime AccentLeft { get; protected set; }
        public SpriteRuntime Line { get; protected set; }
        public SpriteRuntime AccentRight { get; protected set; }

        public DividerHorizontalRuntime(bool fullInstantiation = true, bool tryCreateFormsObject = true)
        {
            if(fullInstantiation)
            {
                var element = ObjectFinder.Self.GetElementSave("Elements/DividerHorizontal");
                element?.SetGraphicalUiElement(this, global::RenderingLibrary.SystemManagers.Default);
            }



        }
        public override void AfterFullCreation()
        {
            AccentLeft = this.GetGraphicalUiElementByName("AccentLeft") as SpriteRuntime;
            Line = this.GetGraphicalUiElementByName("Line") as SpriteRuntime;
            AccentRight = this.GetGraphicalUiElementByName("AccentRight") as SpriteRuntime;
            CustomInitialize();
        }
        //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
        partial void CustomInitialize();
    }
}
