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
using System.Windows.Forms;
using DvtkApplicationLayer;

namespace Dvt
{
	/// <summary>
	/// 
	/// </summary>
	public class EventType: object
	{
		public EventType()
		{
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public class EventTypeWithTreeNode: EventType
	{
		public EventTypeWithTreeNode(TreeNode theTreeNode)
		{
			_TreeNode = theTreeNode;
		}

		public TreeNode TreeNode
		{
			get
			{
				return(_TreeNode);
			}
		}

		private TreeNode _TreeNode;
	}

	/// <summary>
	/// 
	/// </summary>
	public class SessionTreeViewSelectionChange: EventTypeWithTreeNode
	{
		public SessionTreeViewSelectionChange(TreeNode theTreeNode): base(theTreeNode)
		{

		}
	}

	/// <summary>
	/// 
	/// </summary>	
	public class UpdateAll: EventType
	{
		public bool _RestoreSessionTreeState = false;
	}

	/// <summary>
	/// 
	/// </summary>	
	public class ClearAll: EventType
	{
		public bool _StoreSessionTreeState = false;
	}

	/// <summary>
	/// 
	/// </summary>
	public class SessionChange: EventType
	{
		public enum SessionChangeSubTypEnum
		{
			RESULTS_DIR,
			SCRIPTS_DIR,
			DESCRIPTION_DIR,
			SOP_CLASSES_OTHER,
			SOP_CLASSES_AE_TITLE_VERSION,
			SOP_CLASSES_LOADED_STATE,
			OTHER
		}

		public DvtkApplicationLayer.Session SessionApp
		{
			get
			{
				return(session);
			}
		}		

		public SessionChangeSubTypEnum SessionChangeSubTyp
		{
			get
			{
				return(_SessionChangeSubTyp);
			}
		}		

		public SessionChange(DvtkApplicationLayer.Session theSession, SessionChangeSubTypEnum theSessionChangeSubTyp)
		{
			session = theSession;
			_SessionChangeSubTyp = theSessionChangeSubTyp;
		}

		private DvtkApplicationLayer.Session session;
		private SessionChangeSubTypEnum _SessionChangeSubTyp;
	}

	/// <summary>
	/// Use this event to notify that an execution is stopped (because e.g. the stop button has been pressed).
	/// </summary>
	public class StopExecution: EventType
	{
		public StopExecution(ProjectForm2 theProjectForm)
		{
			_ProjectForm = theProjectForm;
		}

		public ProjectForm2 _ProjectForm;
	}

	/// <summary>
	/// Use this event to notify that all exection needs to be stopped (because e.g. the project is closed).
	/// </summary>
	public class StopAllExecution: EventType
	{

	}

	/// <summary>
	/// Use this event to notify that an execution (of a script, emulator) is started.
	/// </summary>
	public class StartExecution: EventTypeWithTreeNode
	{
		public StartExecution(TreeNode theTreeNode): base(theTreeNode)
		{

		}
	}

	/// <summary>
	/// Use this event to notify that an execution (of a script, emulator) is ended.
	/// </summary>
	public class EndExecution: EventType
	{
		public EndExecution(Object theTag)
		{
			_Tag = theTag;
		}

		public Object _Tag;
	}

	/// <summary>
	/// Use this event to notify that the user has selected a different tab in the tab control.
	/// </summary>
	public class SelectedTabChangeEvent: EventType
	{
	}

	/// <summary>
	/// Use this event to notify that the user has set focus to a different project form.
	/// </summary>
	public class ProjectFormGetsFocusEvent: EventType
	{
	}

	/// <summary>
	/// Use this event to notify that the session has been removed.
	/// </summary>
	public class SessionRemovedEvent: EventType
	{
		public SessionRemovedEvent(DvtkApplicationLayer.Session theSession)
		{
			_Session = theSession;
		}

		public DvtkApplicationLayer.Session _Session;
	}

	public class WebNavigationComplete: EventType
	{
		public WebNavigationComplete()
		{
		}
	}

	public class scriptWebNavigationComplete: EventType
	{
        public scriptWebNavigationComplete()
		{
		}
	}

	/// <summary>
	/// Use this event to notify that the last project form is closing.
	/// </summary>
	public class ProjectClosed: EventType
	{
		public ProjectClosed()
		{
		}
	}

	/// <summary>
	/// Use this event to notify that a session has been replaced with another session.
	/// </summary>
	public class SessionReplaced: EventType
	{
		public SessionReplaced(DvtkApplicationLayer.Session theOldSession, DvtkApplicationLayer.Session theNewSession)
		{
			_OldSession = theOldSession;
			_NewSession = theNewSession;
		}

		public DvtkApplicationLayer.Session _OldSession;
		public DvtkApplicationLayer.Session _NewSession;
	}

	/// <summary>
	/// Use this event to notify that (some) session(s) or the project has been saved.
	/// </summary>
	public class Saved: EventType
	{
		public Saved()
		{
		}
	}
}

