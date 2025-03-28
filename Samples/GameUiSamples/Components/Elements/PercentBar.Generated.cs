//Code for Elements/PercentBar (Container)
using GumRuntime;
using MonoGameGum.GueDeriving;
using GameUiSamples.Components;
using Gum.Converters;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;

using RenderingLibrary.Graphics;

using System.Linq;

using MonoGameGum.GueDeriving;
namespace GameUiSamples.Components
{
    partial class PercentBar:MonoGameGum.Forms.Controls.FrameworkElement
    {
        [System.Runtime.CompilerServices.ModuleInitializer]
        public static void RegisterRuntimeType()
        {
            var template = new MonoGameGum.Forms.VisualTemplate((vm, createForms) =>
            {
                var visual = new MonoGameGum.GueDeriving.ContainerRuntime();
                var element = ObjectFinder.Self.GetElementSave("Elements/PercentBar");
                element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
                if(createForms) visual.FormsControlAsObject = new PercentBar(visual);
                return visual;
            });
            MonoGameGum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(PercentBar)] = template;
            ElementSaveExtensions.RegisterGueInstantiation("Elements/PercentBar", () => 
            {
                var gue = template.CreateContent(null, true) as InteractiveGue;
                return gue;
            });
        }
        public enum BarDecorCategory
        {
            None,
            CautionLines,
            VerticalLines,
        }

        BarDecorCategory? _barDecorCategoryState;
        public BarDecorCategory? BarDecorCategoryState
        {
            get => _barDecorCategoryState;
            set
            {
                _barDecorCategoryState = value;
                if(value != null)
                {
                    if(Visual.Categories.ContainsKey("BarDecorCategory"))
                    {
                        var category = Visual.Categories["BarDecorCategory"];
                        var state = category.States.Find(item => item.Name == value.ToString());
                        this.Visual.ApplyState(state);
                    }
                    else
                    {
                        var category = ((Gum.DataTypes.ElementSave)this.Visual.Tag).Categories.FirstOrDefault(item => item.Name == "BarDecorCategory");
                        var state = category.States.Find(item => item.Name == value.ToString());
                        this.Visual.ApplyState(state);
                    }
                }
            }
        }
        public NineSliceRuntime Background { get; protected set; }
        public NineSliceRuntime BarContainer { get; protected set; }
        public NineSliceRuntime Bar { get; protected set; }
        public CautionLines CautionLinesInstance { get; protected set; }
        public VerticalLines VerticalLinesInstance { get; protected set; }


        public float BarPercent
        {
            get => Bar.Width;
            set => Bar.Width = value;
        }

        public PercentBar(InteractiveGue visual) : base(visual) { }
        public PercentBar()
        {



        }
        protected override void ReactToVisualChanged()
        {
            base.ReactToVisualChanged();
            Background = this.Visual?.GetGraphicalUiElementByName("Background") as NineSliceRuntime;
            BarContainer = this.Visual?.GetGraphicalUiElementByName("BarContainer") as NineSliceRuntime;
            Bar = this.Visual?.GetGraphicalUiElementByName("Bar") as NineSliceRuntime;
            CautionLinesInstance = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.GetFrameworkElementByName<CautionLines>(this.Visual,"CautionLinesInstance");
            VerticalLinesInstance = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.GetFrameworkElementByName<VerticalLines>(this.Visual,"VerticalLinesInstance");
            CustomInitialize();
        }
        //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
        partial void CustomInitialize();
    }
}
