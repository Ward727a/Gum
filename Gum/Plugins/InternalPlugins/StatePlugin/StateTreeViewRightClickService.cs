﻿using System;
using System.Linq;
using Gum.DataTypes.Variables;
using Gum.ToolStates;
using System.Windows.Forms;
using ToolsUtilities;
using Gum.DataTypes;
using Gum.PropertyGridHelpers;
using Gum.ToolCommands;
using Gum.DataTypes.Behaviors;
using Gum.Plugins;
using Gum.Commands;
using Gum.Mvvm;
using System.Windows;
using Gum.Plugins.InternalPlugins.StatePlugin.Views;

namespace Gum.Managers;


public class StateTreeViewRightClickService
{
    const string mNoCategory = "<no category>";
    private readonly ISelectedState _selectedState;

    System.Windows.Controls.ContextMenu _menuStrip;
    GumCommands _gumCommands;

    public StateTreeViewRightClickService(ISelectedState selectedState, GumCommands gumCommands)
    {
        _selectedState = selectedState;
        _gumCommands = gumCommands;
    }

    public void SetMenuStrip(System.Windows.Controls.ContextMenu menuStrip, FrameworkElement contextMenuOwner)
    {
        contextMenuOwner.ContextMenuOpening += (_, args) =>
        {
            if (_menuStrip.Items.Count == 0)
            {
                args.Handled = true;
            }
        };

        _menuStrip = menuStrip;
        _menuStrip.ContextMenuOpening += (s, e) =>
        {
            if (_menuStrip.Items.Count == 0)
            {
                e.Handled = true; // Prevent the menu from opening
            }
        };
    }

    #region Add to menu

    internal void PopulateMenuStrip()
    {
        ClearMenuStrip();

        if (_selectedState.SelectedStateContainer != null)
        {

            if (_selectedState.SelectedStateCategorySave != null)
            {
                // As of 5/24/2023, we no longer support uncategorized states
                AddMenuItem("Add State", _gumCommands.GuiCommands.ShowAddStateWindow);
            }

            AddMenuItem("Add Category", _gumCommands.GuiCommands.ShowAddCategoryWindow);

            if (_selectedState.SelectedStateSave != null)
            {
                bool isDefault = _selectedState.SelectedStateSave == _selectedState.SelectedElement?.DefaultState;

                if (!isDefault)
                {
                    AddSplitter();
                    AddMenuItem("Rename State", RenameStateClick);
                    AddMenuItem("Delete [" + _selectedState.SelectedStateSave.Name + "]", DeleteStateClick);
                    AddMenuItem("Duplicate State", DuplicateStateClick);

                    AddMoveToCategoryItems();

                    AddSplitter();

                    if (GetIfCanMoveUp(_selectedState.SelectedStateSave, _selectedState.SelectedStateCategorySave))
                    {
                        AddMenuItem("^ Move Up", MoveUpClick, "Alt+Up");
                    }
                    if (GetIfCanMoveDown(_selectedState.SelectedStateSave, _selectedState.SelectedStateCategorySave))
                    {
                        AddMenuItem("v Move Down", MoveDownClick, "Alt+Down");
                    }
                }
            }
            // We used to show the category editing commands if a state was selected 
            // (if a state is selected, a category is implicitly selected too). Now we
            // check if a category is highlighted (not state)
            //if(SelectedState.Self.SelectedStateCategorySave != null)
            if (_selectedState.SelectedStateCategorySave != null && _selectedState.SelectedStateSave == null)
            {

                AddSplitter();

                AddMenuItem("Rename Category", RenameCategoryClick);


                AddMenuItem("Delete [" + _selectedState.SelectedStateCategorySave.Name + "]", DeleteCategoryClick);
            }
        }


        //NewMenuStrip.Visibility = (NewMenuStrip.Items.Count > 0).ToVisibility();
    }

    private void AddSplitter()
    {
        _menuStrip.Items.Add(new System.Windows.Controls.Separator());
    }

    private void ClearMenuStrip()
    {
        _menuStrip.Items.Clear();
    }

    private void MoveUpClick()
    {
        MoveStateInDirection(-1);
    }

    private void MoveDownClick()
    {
        MoveStateInDirection(1);
    }

    public void MoveStateInDirection(int direction)
    {
        var state = _selectedState.SelectedStateSave;
        var list = _selectedState.SelectedStateContainer.UncategorizedStates;
        if (_selectedState.SelectedStateCategorySave != null)
        {
            list = _selectedState.SelectedStateCategorySave.States;
        }

        if (list != null && list.Contains(state))
        {
            int oldIndex = list.IndexOf(state);

            bool shouldSave = false;

            if (direction == -1 && GetIfCanMoveUp(state, _selectedState.SelectedStateCategorySave))
            {
                list.RemoveAt(oldIndex);
                list.Insert(oldIndex - 1, state);
                shouldSave = true;
            }
            else if (direction == 1 && GetIfCanMoveDown(state, _selectedState.SelectedStateCategorySave))
            {
                list.RemoveAt(oldIndex);
                list.Insert(oldIndex + 1, state);
                shouldSave = true;
            }

            if (shouldSave)
            {
                _gumCommands.GuiCommands.RefreshStateTreeView();

                _gumCommands.FileCommands.TryAutoSaveCurrentObject();

                PopulateMenuStrip();
            }
        }
    }

    bool GetIfCanMoveUp(StateSave state, StateSaveCategory category)
    {
        var list = _selectedState.SelectedStateCategorySave?.States;
        if (category != null)
        {
            list = category.States;
        }

        if (list == null)
        {
            return false;
        }

        int stateIndex = list.IndexOf(state);

        int indexToBeGreaterThan = 0;
        if (category == null)
        {
            // Uncategorized, so it can't move up above the Default state
            indexToBeGreaterThan = 1;
        }

        return stateIndex > indexToBeGreaterThan;
    }

    bool GetIfCanMoveDown(StateSave state, StateSaveCategory category)
    {
        //var list = _selectedState.SelectedStateContainer.UncategorizedStates;
        var list = _selectedState.SelectedStateCategorySave?.States;
        if (category != null)
        {
            list = category.States;
        }

        if (list == null)
        {
            return false;
        }

        int oldIndex = list.IndexOf(state);
        return oldIndex != list.Count - 1;
    }


    public void DeleteCategoryClick()
    {
        _gumCommands.Edit.RemoveStateCategory(
            _selectedState.SelectedStateCategorySave,
            _selectedState.SelectedStateContainer);
    }

    public void DeleteStateClick()
    {
        _gumCommands.Edit.AskToDeleteState(
            _selectedState.SelectedStateSave,
            _selectedState.SelectedStateContainer);
    }

    private void AddMoveToCategoryItems()
    {

        var categoryNames = _selectedState.SelectedStateContainer?.Categories
            .Where(item => item != _selectedState.SelectedStateCategorySave)
            .Select(item => item.Name).ToList();

        // As of before 2024 we no longer allow uncategorized non-default states
        //if(SelectedState.Self.SelectedStateCategorySave != null)
        //{
        //    categoryNames.Insert(0, mNoCategory);
        //}

        if (categoryNames?.Count != 0)
        {
            AddSplitter();

            AddMenuItem("Move to category", null);

            foreach (var categoryName in categoryNames)
            {

                // make a local var to prevent problems with delayed evaluation
                string categoryNameEvaluated = categoryName;
                AddChildMenuItem("Move to category", categoryName, () => MoveToCategory(categoryName));
            }
        }
    }

    private void AddChildMenuItem(string parent, string text, Action clickAction, string shortcut = null)
    {
        System.Windows.Controls.MenuItem menuItem = CreateNewToolStripMenuItem(text, clickAction, shortcut);
        var parentItem = _menuStrip.Items.FirstOrDefault(item => item is System.Windows.Controls.MenuItem itemMenu && itemMenu.Header.ToString() == parent)
            as System.Windows.Controls.MenuItem;
        parentItem.Items.Add(menuItem);
    }

    private void AddMenuItem(string text, Action clickAction, string shortcut = null)
    {
        System.Windows.Controls.MenuItem menuItem = CreateNewToolStripMenuItem(text, clickAction, shortcut);
        _menuStrip.Items.Add(menuItem);
    }

    private static System.Windows.Controls.MenuItem CreateNewToolStripMenuItem(string text, Action clickAction, string shortcut)
    {
        var menuItem = new System.Windows.Controls.MenuItem
        {
            Header = text,
            InputGestureText = shortcut,
        };
        if (clickAction != null)
        {
            menuItem.Click += (_, _) => clickAction();
        }
        return menuItem;
    }


    #endregion

    internal void AddStateClick()
    {
        _gumCommands.GuiCommands.ShowAddStateWindow();
    }

    private void DuplicateStateClick()
    {
        // Is there a "custom" current state save, like an interpolation or animation?
        if (_selectedState.CustomCurrentStateSave != null)
        {
            _gumCommands.GuiCommands.ShowMessage("Cannot duplicate state while a custom state is displaying. Are you creating or playing animations?");
            return;
        }
        if (_selectedState.SelectedStateCategorySave == null)
        {
            _gumCommands.GuiCommands.ShowMessage("Cannot duplicate uncategorized states. Select a state in a category first.");
            return;
        }
        ////////End Early Out///////////////

        StateSave newState = _selectedState.SelectedStateSave.Clone();


        newState.ParentContainer = _selectedState.SelectedElement;

        int index = _selectedState.SelectedStateCategorySave.States.IndexOf(_selectedState.SelectedStateSave);

        while (_selectedState.SelectedStateContainer.AllStates.Any(item => item != newState && item.Name == newState.Name))
        {
            newState.Name = StringFunctions.IncrementNumberAtEnd(newState.Name);
        }

        ElementCommands.Self.AddState(_selectedState.SelectedStateContainer, _selectedState.SelectedStateCategorySave, newState, index + 1);

        _gumCommands.GuiCommands.RefreshStateTreeView();

        _selectedState.SelectedStateSave = newState;

        _gumCommands.FileCommands.TryAutoSaveCurrentElement();
    }

    public void RenameStateClick()
    {
        _gumCommands.Edit.AskToRenameState(SelectedState.Self.SelectedStateSave,
            SelectedState.Self.SelectedStateContainer);
    }

    public void RenameCategoryClick()
    {
        _gumCommands.Edit.AskToRenameStateCategory(
            SelectedState.Self.SelectedStateCategorySave,
            SelectedState.Self.SelectedElement);
    }

    private void MoveToCategory(string categoryNameToMoveTo)
    {
        var stateToMove = SelectedState.Self.SelectedStateSave;
        var stateContainer = SelectedState.Self.SelectedStateContainer;
        _gumCommands.Edit.MoveToCategory(categoryNameToMoveTo, stateToMove, stateContainer);
    }

}
