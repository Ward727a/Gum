﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gum.DataTypes;
using Gum.ToolStates;
using Gum.DataTypes.Variables;
using System.Windows.Forms;
using Gum.Plugins;

namespace Gum.Managers
{
    public class RenameManager
    {
        static RenameManager mRenameManager;


        public static RenameManager Self
        {
            get
            {
                if (mRenameManager == null)
                {
                    mRenameManager = new RenameManager();
                }
                return mRenameManager;
            }
        }




        public void HandleRename(ElementSave elementSave, InstanceSave instance, string oldName)
        {
            bool succeeded = true;

            try
            {
                bool isRenamingXmlFile = instance == null;

                bool shouldContinue = true;

                if (isRenamingXmlFile)
                {
                    string message = "Are you sure you want to rename " + oldName + "?\n\n" +
                        "This will change the file name for " + oldName + " which may break " +
                        "external references to this object.";
                    var result = MessageBox.Show(message, "Rename Object and File?", MessageBoxButtons.YesNo);

                    shouldContinue = result == DialogResult.Yes;
                }

                if (shouldContinue)
                {
                    // Tell the GumProjectSave to react to the rename.
                    // This changes the names of the ElementSave references.
                    ProjectManager.Self.GumProjectSave.ReactToRenamed(elementSave, instance, oldName);



                    if (SelectedState.Self.SelectedInstance != null)
                    {
                        string newName = SelectedState.Self.SelectedInstance.Name;

                        foreach (StateSave stateSave in SelectedState.Self.SelectedElement.States)
                        {
                            stateSave.ReactToInstanceNameChange(oldName, newName);
                        }
                    }

                    // Even though this gets called from the PropertyGrid methods which eventually
                    // save this object, we want to force a save here to make sure it worked.  If it
                    // does, then we're safe to delete the old files.
                    if (ProjectManager.Self.GeneralSettingsFile.AutoSave)
                    {
                        ProjectManager.Self.SaveElement(elementSave);
                    }

                    if (isRenamingXmlFile)
                    {
                        // If we got here that means all went okay, so we should delete the old files
                        string oldXml = elementSave.GetFullPathXmlFile(oldName);
                        // Delete the XML.
                        // If the file doesn't
                        // exist, no biggie - we
                        // were going to delete it
                        // anyway.
                        if (System.IO.File.Exists(oldXml))
                        {
                            System.IO.File.Delete(oldXml);
                        }

                        PluginManager.Self.ElementRename(elementSave, oldName);

                        ElementTreeViewManager.Self.RefreshUI(elementSave);
                    }


                }
                
                if(!shouldContinue && isRenamingXmlFile)
                {
                    elementSave.Name = oldName;
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Error renaming element " + elementSave.ToString() + "\n\n" + e.ToString());
                succeeded = false;
            }

            


        }
    }
}
