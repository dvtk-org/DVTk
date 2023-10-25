// ------------------------------------------------------
// DVTk - The Healthcare Validation Toolkit (www.dvtk.org)
// Copyright © 2009 DVTk
// ------------------------------------------------------
// This file is part of DVTk.
//
// DVTk is free software; you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License as published by the Free Software Foundation; either version 3.0
// of the License, or (at your option) any later version. 
// 
// DVTk is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even
// the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser
// General Public License for more details. 
// 
// You should have received a copy of the GNU Lesser General Public License along with this
// library; if not, see <http://www.gnu.org/licenses/>

using System;
using System.Collections;
using System.Windows.Forms;
using DvtkApplicationLayer;
using System.Diagnostics;
using System.IO;


namespace Dvt
{
    /// <summary>
    /// Class containing extra information on the nodes in the Session Tree.
    /// </summary>
    public class NodesInformation
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public NodesInformation()
        {
            // Empty.
        }

        /// <summary>
        /// Store information on which nodes (the supplied node and all sub nodes)
        /// are expanded.
        /// </summary>
        /// <param name="theTreeNode">The tree node.</param>
        public void StoreExpandInformation(TreeNode theTreeNode)
        {
            if (theTreeNode.IsExpanded)
            {
                _TagsOfExpandedNodes.Add(theTreeNode.Tag);
            }

            // Do the same for all sub nodes.
            foreach (TreeNode subTreeNode in theTreeNode.Nodes)
            {
                StoreExpandInformation(subTreeNode);
            }
        }

        /// <summary>
        /// Expand the supplied node and sub nodes back if they were expanded before.
        /// </summary>
        /// <param name="theTreeNode">The tree node.</param>
        public void RestoreExpandInformation(TreeNode theTreeNode)
        {
            Object theTreeNodeTagForCurrentNode = (Object)theTreeNode.Tag;
            Object theFoundTreeNodeTag = null;
            foreach (Object theTreeNodeTag in _TagsOfExpandedNodes)
            {
                if (theTreeNodeTagForCurrentNode == theTreeNodeTag)
                {
                    theFoundTreeNodeTag = theTreeNodeTag;
                    break;
                }
            }
            // If this node was expanded, expand it back.
            if (theFoundTreeNodeTag != null)
            {
                theTreeNode.Expand();
                _TagsOfExpandedNodes.Remove(theFoundTreeNodeTag);
            }

            // Do the same for all sub nodes.
            foreach (TreeNode subTreeNode in theTreeNode.Nodes)
            {
                RestoreExpandInformation(subTreeNode);
            }

        }

        /// <summary>
        /// Remove all expanded information for the supplied session.
        /// </summary>
        /// <param name="theSession">The session.</param>
        public void RemoveExpandInformationForSession(DvtkApplicationLayer.Session theSession)
        {
            ArrayList theTagsToRemove = new ArrayList();

            foreach (Object theTreeNodeTag in _TagsOfExpandedNodes)
            {
                DvtkApplicationLayer.Session tempSession = null;
                if (theTreeNodeTag is PartOfSession)
                {
                    DvtkApplicationLayer.PartOfSession partOfSession = theTreeNodeTag as PartOfSession;
                    tempSession = partOfSession.ParentSession;
                }
                else
                {
                    tempSession = (DvtkApplicationLayer.Session)theTreeNodeTag;
                }
                if (tempSession == theSession)
                {
                    theTagsToRemove.Add(theTreeNodeTag);
                }
            }

            foreach (Object theTreeNodeTagToRemove in theTagsToRemove)
            {
                _TagsOfExpandedNodes.Remove(theTreeNodeTagToRemove);
            }
        }

        /// <summary>
        /// Remove all expanded information.
        /// </summary>
        public void RemoveAllExpandInformation()
        {
            _TagsOfExpandedNodes.Clear();
        }

        public void StoreSelectedNode(TreeView theTreeView)
        {
            TreeNode theSelectedTreeNode = theTreeView.SelectedNode;

            if (theSelectedTreeNode != null)
            {
                _TagOfSelectedNode = (Object)theSelectedTreeNode.Tag;
            }
            else
            {
                _TagOfSelectedNode = null;
            }
        }

        /// <summary>
        /// If nodes exist:
        /// 1. Try to select the node with the same representing tag.
        /// 2. If this fails, find the session node with the same session as the tag.
        /// 3. If this fails, select the first session node.
        /// </summary>
        /// <param name="theTreeView"></param>
        public void RestoreSelectedNode(TreeView theTreeView)
        {
            TreeNode theTreeNodeToSelect = null;

            if (theTreeView.Nodes.Count > 0)
            {
                // 1. Try to select the node with the same representing tag.
                foreach (TreeNode theprojectNode in theTreeView.Nodes)
                {
                    theTreeNodeToSelect = FindTreeNode(theprojectNode, _TagOfSelectedNode);
                    if (theTreeNodeToSelect == null)
                    {
                        foreach (TreeNode theSessionNode in theprojectNode.Nodes)
                        {
                            theTreeNodeToSelect = FindTreeNode(theSessionNode, _TagOfSelectedNode);

                            if (theTreeNodeToSelect != null)
                            {
                                break;
                            }
                        }
                    }
                }

                //				// 2.If this fails, find the session node with the same session as the tag.
                //				if (theTreeNodeToSelect == null)
                //				{
                //					foreach(TreeNode theSessionNode in theTreeView.Nodes)
                //					{   
                //						Object  theSessionTreeNodeTag = (Object)theSessionNode.Tag;
                //                        ///!!! Marco 
                //
                //						if (theSessionTreeNodeTag == _TagOfSelectedNode)
                //						{
                //							theTreeNodeToSelect = theSessionNode;
                //							break;
                //						}
                //					}				
                //				}


                // 2.If this fails, find the session node with the same session as the tag.
                if (theTreeNodeToSelect == null)
                {
                    Session previouslySelectedSession = null; // The session of the previously selected node.
                    Project previouslySelectedProject = null;

                    if (_TagOfSelectedNode is Session)
                    {
                        previouslySelectedSession = (Session)_TagOfSelectedNode;
                    }
                    else if (_TagOfSelectedNode is PartOfSession)
                    {
                        previouslySelectedSession = (_TagOfSelectedNode as PartOfSession).ParentSession;
                    }
                    else if (_TagOfSelectedNode is Project)
                    {
                        previouslySelectedProject = (Project)_TagOfSelectedNode;

                    }
                    else
                    {
                        throw new Exception("Not expected to get here");
                    }
                    foreach (TreeNode theProjectNode in theTreeView.Nodes)
                    {
                        Project project = theProjectNode.Tag as Project;
                        if (project == previouslySelectedProject)
                        {
                            theTreeNodeToSelect = theProjectNode;
                            break;
                        }

                        foreach (TreeNode theSessionNode in theProjectNode.Nodes)
                        {
                            Session session = theSessionNode.Tag as Session;

                            if (session == previouslySelectedSession)
                            {
                                theTreeNodeToSelect = theSessionNode;
                                break;
                            }
                        }
                    }
                }



                // 3. If this fails, select the first project node.
                if (theTreeNodeToSelect == null)
                {
                    theTreeNodeToSelect = theTreeView.Nodes[0];
                }

                theTreeView.SelectedNode = theTreeNodeToSelect;
            }

            _TagOfSelectedNode = null;
        }

        private TreeNode FindTreeNode(TreeNode theTreeNode, Object theRepresentingTreeNodeTag)
        {
            TreeNode theTreeNodeToFind = null;

            Object theTreeNodeTag = (Object)theTreeNode.Tag;
            ///!!!Marco

            if (theTreeNodeTag == theRepresentingTreeNodeTag)
            {
                theTreeNodeToFind = theTreeNode;
            }
            else
            {
                foreach (TreeNode subTreeNode in theTreeNode.Nodes)
                {
                    theTreeNodeToFind = FindTreeNode(subTreeNode, theRepresentingTreeNodeTag);

                    if (theTreeNodeToFind != null)
                    {
                        break;
                    }
                }
            }

            return (theTreeNodeToFind);
        }

        ArrayList _TagsOfExpandedNodes = new ArrayList();
        Object _TagOfSelectedNode = null;
    }
}
