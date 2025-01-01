﻿using Gum.Managers;
using Gum.Plugins;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Gum.Controls;

/// <summary>
/// Interaction logic for MainPanelControl.xaml
/// </summary>
public partial class MainPanelControl : UserControl
{
    GridLength expandedLeftColumnLength;
    GridLength expandedMiddleColumnLength;
    GridLength bottomRowLength;

    GridLength splitterLength;

    IEnumerable<TabControl> AllControls
    {
        get
        {
            yield return LeftTabControl;
            yield return CenterTopTabControl;
            yield return CenterBottomTabControl;
            yield return RightTopTabControl;
            yield return RightBottomTabControl;
        }
    }

    bool isHidden;
    public MainPanelControl()
    {
        InitializeComponent();

        OverrideFontSizeProperty();
        this.KeyDown += HandleKeyDown;
    }

    private void OverrideFontSizeProperty()
    {
        const double defaultFontSize = 11;

        FontSizeProperty.OverrideMetadata(
            typeof(MainPanelControl),
            new FrameworkPropertyMetadata(
                defaultFontSize,
                FrameworkPropertyMetadataOptions.AffectsMeasure
                | FrameworkPropertyMetadataOptions.AffectsRender
                | FrameworkPropertyMetadataOptions.Inherits,
                null,
                Coerce)
        );

        static object Coerce(DependencyObject d, object baseValue)
        {
            return baseValue is not double size
                ? baseValue
                : size switch
                {
                    < 8 => 8,
                    > 24 => 24,
                    _ => size
                };
        }
    }

    private void HandleKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        HotkeyManager.Self.HandleKeyDownAppWide(e);
    }

    private void CenterBottomTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    public void HideTools()
    {
        if(!isHidden)
        {
            expandedLeftColumnLength = LeftColumn.Width;
            expandedMiddleColumnLength = MiddleColumn.Width;
            bottomRowLength = BottomRightPanel.Height;

            splitterLength = LeftSplitter.Width;


            LeftColumn.Width = new GridLength(0);
            MiddleColumn.Width = new GridLength(0);
            LeftSplitter.Width = new GridLength(0);
            MiddleSplitter.Width = new GridLength(0);

            BottomRightSplitter.Height = new GridLength(0);
            BottomRightPanel.Height = new GridLength(0);

            isHidden = true;
        }
    }

    public void ShowTools()
    {
        if(isHidden)
        {
            LeftColumn.Width = expandedLeftColumnLength;
            MiddleColumn.Width = expandedMiddleColumnLength;
            LeftSplitter.Width = splitterLength;
            MiddleSplitter.Width = splitterLength;


            BottomRightSplitter.Height = splitterLength;
            BottomRightPanel.Height = bottomRowLength;

            isHidden = false;
        }
    }


    public PluginTab AddWinformsControl(System.Windows.Forms.Control control, string tabTitle, TabLocation tabLocation)
    {
        // todo: check if control has already been added. Right now this can't be done trough the Gum commands
        // so it's only used "internally", so no checking is being done.
        //var tabControl = GetTabFromLocation(tabLocation);
        //var tabPage = CreateTabPage(tabTitle);
        //control.Dock = DockStyle.Fill;
        //tabControl.Controls.Add(tabPage);

        //tabPage.Controls.Add(control);

        //return new PluginTab
        //{
        //    Page = tabPage
        //};

        var host = new System.Windows.Forms.Integration.WindowsFormsHost();

        host.Child = control;

        return AddWpfControl(host, tabTitle, tabLocation);
    }

    public PluginTab AddWpfControl(System.Windows.FrameworkElement control, string tabTitle, TabLocation tabLocation = TabLocation.CenterBottom)
    {
        // This should be moved to the MainPanelControl wpf 

        // This should be moved to the MainPanelControl wpf 
        string AppTheme = "Light";
        control.Resources = new System.Windows.ResourceDictionary();
        control.Resources.Source =
            new Uri($"/Themes/{AppTheme}.xaml", UriKind.Relative);

        var tabControl = GetTabFromLocation(tabLocation);

        var pluginTabItem = new PluginTabItem();
        pluginTabItem.Header = tabTitle;
        pluginTabItem.Content = control;

        tabControl.Items.Add(pluginTabItem);

        var tab = new PluginTab()
        {
            Page = pluginTabItem
        };


        return tab;
    }

    private System.Windows.Controls.TabControl GetTabFromLocation(TabLocation tabLocation)
    {
        // This should be moved to the MainPanelControl wpf 

        System.Windows.Controls.TabControl tabControl = null;

        switch (tabLocation)
        {
            case TabLocation.Center:
            case TabLocation.CenterBottom:
                tabControl = CenterBottomTabControl;
                break;
            case TabLocation.RightBottom:
                tabControl = RightBottomTabControl;

                break;
            case TabLocation.RightTop:
                tabControl = RightTopTabControl;
                break;
            case TabLocation.CenterTop:
                tabControl = CenterTopTabControl;
                break;
            case TabLocation.Left:
                tabControl = LeftTabControl;
                break;
            default:
                throw new NotImplementedException($"Tab location {tabLocation} not supported");
        }

        return tabControl;
    }

    public void RemoveWpfControl(FrameworkElement control)
    {
        List<Control> controls = new List<Control>();

        System.Windows.Controls.TabControl tabControl = null;
        System.Windows.Controls.TabItem tabPage = null;
        GetContainers(control, out tabPage, out tabControl);

        if (tabControl != null)
        {
            var controlInTabPage = tabPage.Content;
            {
                if (controlInTabPage is System.Windows.Forms.Integration.ElementHost)
                {
                    (controlInTabPage as System.Windows.Forms.Integration.ElementHost).Child = null;
                }
            }
            tabPage.Content = null;
            tabControl.Items.Remove(tabPage);
        }
    }

    private void GetContainers(FrameworkElement control, out System.Windows.Controls.TabItem tabPage, out System.Windows.Controls.TabControl tabControl)
    {
        tabPage = null;
        tabControl = null;

        foreach (var uncastedTabPage in this.CenterBottomTabControl.Items)
        {
            tabPage = uncastedTabPage as System.Windows.Controls.TabItem;

            if (tabPage != null && DoesTabContainControl(tabPage, control))
            {
                tabControl = this.CenterBottomTabControl;

                break;
            }
            else
            {
                tabPage = null;
            }
        }

        if (tabControl == null)
        {
            foreach (var uncastedTabPage in this.RightBottomTabControl.Items)
            {
                tabPage = uncastedTabPage as System.Windows.Controls.TabItem;

                if (tabPage != null && DoesTabContainControl(tabPage, control))
                {
                    tabControl = this.RightBottomTabControl;
                    break;
                }
                else
                {
                    tabPage = null;
                }
            }
        }
    }

    bool DoesTabContainControl(System.Windows.Controls.TabItem tabPage, System.Windows.FrameworkElement control)
    {
        return tabPage.Content == control;
    }

    public bool IsTabVisible(PluginTab pluginTab)
    {
        foreach (var tabControl in AllControls)
        {
            if (tabControl.Items.Contains(pluginTab.Page))
            {
                return true;
            }
        }
        return false;
    }

    internal bool ShowTabForControl(System.Windows.Controls.UserControl control)
    {
        var found = false;
        foreach(var tabControl in AllControls)
        {
            for(int i = 0; i < tabControl.Items.Count; i++)
            {
                var tabPage = tabControl.Items[i] as TabItem;

                if(tabPage != null && DoesTabContainControl(tabPage, control))
                {
                    tabControl.SelectedIndex = i;
                    found = true;
                    break;
                }
            }

            if(found)
            {
                break;
            }
        }

        return found;

        //TabControl tabControl = null;
        //TabPage tabPage = null;
        //GetContainers(control, out tabPage, out tabControl);

        //var index = tabControl.TabPages.IndexOf(tabPage);

        //tabControl.SelectedIndex = index;
    }
    
    public void ShowTab(PluginTab pluginTab, bool focus = true)
    {
        if(!IsTabVisible(pluginTab))
        {
            var tabControl = GetTabFromLocation(pluginTab.SuggestedLocation);

            tabControl.Items.Add(pluginTab.Page);

            pluginTab.RaiseTabShown();
        }

        if(focus)
        {
            pluginTab.Focus();
        }
    }

    public void HideTab(PluginTab pluginTab)
    {
        var wasRemoved = false;
        foreach (var tabControl in AllControls)
        {
            if (tabControl.Items.Contains(pluginTab.Page))
            {
                tabControl.Items.Remove(pluginTab.Page);
                wasRemoved = true;
            }
        }

        if(wasRemoved)
        {
            pluginTab.RaiseTabHidden();
        }
    }

}