﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonFormsAndControls;
using Gum.DataTypes;
using Gum.DataTypes.Variables;
using System.Windows.Forms;
using Gum.ToolStates;
using Gum.Wireframe;

namespace Gum.Managers
{
    public partial class StateTreeViewManager
    {
        #region Fields

        static StateTreeViewManager mSelf;

        MultiSelectTreeView mTreeView;

        ElementSave mLastElementRefreshedTo;
        ContextMenuStrip mMenuStrip;

        #endregion

        #region Properties

        public static StateTreeViewManager Self
        {
            get
            {
                if (mSelf == null)
                {
                    mSelf = new StateTreeViewManager();
                }
                return mSelf;
            }
        }

        public TreeNode SelectedNode
        {
            get { return mTreeView.SelectedNode; }
        }

        #endregion

        public TreeNode GetTreeNodeFor(StateSave stateSave)
        {
            if (stateSave == null)
            {
                return null;
            }
            // Will need to expand this when we add categories
            foreach (TreeNode node in mTreeView.Nodes)
            {
                if (node.Tag == stateSave)
                {
                    return node;
                }
            }
            return null;
        }

        public void Initialize(MultiSelectTreeView treeView, ContextMenuStrip menuStrip)
        {
            mMenuStrip = menuStrip;
            mTreeView = treeView;
        }

        internal void OnSelect()
        {

            TreeNode treeNode = mTreeView.SelectedNode;

            object selectedObject = null;

            if (treeNode != null)
            {
                selectedObject = treeNode.Tag;
            }

            if (selectedObject == null)
            {
                // What do we do?  This is invalid.  A State should always be selected...
                // What we do is select the first one if it exists
                if (mTreeView.Nodes.Count != 0)
                {
                    selectedObject = mTreeView.Nodes[0].Tag;
                    mTreeView.SelectedNode = mTreeView.Nodes[0];
                }
            }
            SelectedState.Self.CustomCurrentStateSave = null;
            SelectedState.Self.UpdateToSelectedStateSave();
            // refreshes the yellow highlights
            StateTreeViewManager.Self.RefreshUI(SelectedState.Self.SelectedElement);
        }

        public void Select(StateSave stateSave)
        {
            TreeNode treeNode = GetTreeNodeFor(stateSave);

            Select(treeNode);

        }

        public void Select(TreeNode treeNode)
        {
            if (mTreeView.SelectedNode != treeNode)
            {
                mTreeView.SelectedNode = treeNode;

                if (treeNode != null)
                {
                    treeNode.EnsureVisible();
                }
            }
        }

        public void RefreshUI(ElementSave element)
        {

            bool changed = element != mLastElementRefreshedTo;

            mLastElementRefreshedTo = element;

            StateSave lastStateSave = SelectedState.Self.SelectedStateSave;
            InstanceSave instance = SelectedState.Self.SelectedInstance;


            if (element != null)
            {
                while (mTreeView.Nodes.Count > element.States.Count)
                {
                    mTreeView.Nodes.RemoveAt(mTreeView.Nodes.Count - 1);
                }
                while (mTreeView.Nodes.Count < element.States.Count)
                {
                    mTreeView.Nodes.Add(new TreeNode());
                }

                bool wasAnythingSelected = false;
                for(int i = 0; i < element.States.Count; i++)
                {
                    StateSave state = element.States[i];
                    string stateName = state.Name;
                    if (string.IsNullOrEmpty(stateName))
                    {
                        stateName = "Default";
                    }

                    if (mTreeView.Nodes[i].Text != stateName)
                    {
                        mTreeView.Nodes[i].Text = stateName;
                    }
                    if (mTreeView.Nodes[i].Tag != state)
                    {
                        mTreeView.Nodes[i].Tag = state;
                    }

                    if (state == lastStateSave)
                    {

                        SelectedState.Self.SelectedStateSave = state;

                        wasAnythingSelected = true;
                    }
                    else if(!mTreeView.Nodes[i].IsSelected && mTreeView.SelectedNode != mTreeView.Nodes[i])
                    {
                        System.Drawing.Color desiredColor = System.Drawing.Color.White;
                        if (instance != null && state.Variables.Any(item => item.Name.StartsWith(instance.Name + ".")))
                        {
                            desiredColor = System.Drawing.Color.Yellow;
                        }

                        if (mTreeView.Nodes[i].BackColor != desiredColor)
                        {
                            mTreeView.Nodes[i].BackColor = desiredColor;
                        }
                    }
                }

                if (wasAnythingSelected == false && element.States != null && 
                    // The user could be selecting an object that has a missing XML file, so states
                    // were never loaded
                    element.States.Count > 0)
                {
                    SelectedState.Self.SelectedStateSave = element.States[0];

                }
            }
            else
            {
                mTreeView.Nodes.Clear();
            }


        }





        internal void HandleKeyDown(KeyEventArgs e)
        {
            HandleCopyCutPaste(e);
        }

        private void HandleCopyCutPaste(KeyEventArgs e)
        {
            if ((e.Modifiers & Keys.Control) == Keys.Control)
            {
                // copy, ctrl c, ctrl + c
                if (e.KeyCode == Keys.C)
                {
                    EditingManager.Self.OnCopy(CopyType.State);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
                // paste, ctrl v, ctrl + v
                else if (e.KeyCode == Keys.V)
                {
                    EditingManager.Self.OnPaste(CopyType.State);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
                //// cut, ctrl x, ctrl + x
                //else if (e.KeyCode == Keys.X)
                //{
                //    EditingManager.Self.OnCut(CopyType.Instance);
                //    e.Handled = true;
                //    e.SuppressKeyPress = true;
                //}
            }
        }
    }
}
