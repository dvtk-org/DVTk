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

namespace Dvtk.Sessions
{

    internal class CountManager
        : Wrappers.ICountingTarget
    {

        private System.UInt32[,] _CountMatrix = new System.UInt32[3,2];

        public CountManager(Session session)
        {
            if (session == null) throw new System.ArgumentNullException();
            this.m_parentSession = session;
        }

        private System.Collections.ArrayList m_childCountManagers =
            new System.Collections.ArrayList();

        public Wrappers.ICountingTarget CreateChildCountingTarget()
        {
            Dvtk.Sessions.Session session = this.m_parentSession;
            //
            // Create child-countManager for the same (parent)-session.
            //
            CountManager countManager = new CountManager(session);
            //
            // Register child-countManager in the child-collection of this parent-countManager
            //
            this.m_childCountManagers.Add(countManager);
            return countManager;
        }

        private Session m_parentSession;

        public System.Boolean SerializeEnabled
        {
            get { return _SerializeEnabled; }
            set { _SerializeEnabled = value; }
        }
        private System.Boolean _SerializeEnabled = false;

        #region ICountingTarget Members

        public void Init()
        {
            //
            // Reset counters
            //
            this._CountMatrix = new System.UInt32[3,2];
        }

        public void Increment(Wrappers.CountGroup group, Wrappers.CountType type)
        {
            this._CountMatrix[(int)group, (int)type] += 1;
        }

        public System.UInt32 NrOfErrors
        {
            get
            {
                return (
                    this.NrOfGeneralErrors +
                    this.NrOfUserErrors +
                    this.NrOfValidationErrors);
            }
        }

        public System.UInt32 NrOfWarnings
        {
            get
            {
                return (
                    this.NrOfGeneralWarnings +
                    this.NrOfUserWarnings +
                    this.NrOfValidationWarnings);
            }
        }

        public System.UInt32 NrOfValidationErrors
        {
            get
            {
                Wrappers.CountGroup group = Wrappers.CountGroup.Validation;
                Wrappers.CountType type = Wrappers.CountType.Error;
                return this._CountMatrix[(int)group, (int)type];
            }
			set
			{
				Wrappers.CountGroup group = Wrappers.CountGroup.Validation;
				Wrappers.CountType type = Wrappers.CountType.Error;
				this._CountMatrix[(int)group, (int)type] = value;
			}
        }

        public System.UInt32 NrOfValidationWarnings
        {
            get
            {
                Wrappers.CountGroup group = Wrappers.CountGroup.Validation;
                Wrappers.CountType type = Wrappers.CountType.Warning;
                return this._CountMatrix[(int)group, (int)type];
            }
			set
			{
				Wrappers.CountGroup group = Wrappers.CountGroup.Validation;
				Wrappers.CountType type = Wrappers.CountType.Warning;
				this._CountMatrix[(int)group, (int)type] = value;
			}
        }

        public System.UInt32 NrOfGeneralErrors
        {
            get
            {
                Wrappers.CountGroup group = Wrappers.CountGroup.General;
                Wrappers.CountType type = Wrappers.CountType.Error;
                return this._CountMatrix[(int)group, (int)type];
            }
			set
			{
				Wrappers.CountGroup group = Wrappers.CountGroup.General;
				Wrappers.CountType type = Wrappers.CountType.Error;
				this._CountMatrix[(int)group, (int)type] = value;
			}
        }

        public System.UInt32 NrOfGeneralWarnings
        {
            get
            {
                Wrappers.CountGroup group = Wrappers.CountGroup.General;
                Wrappers.CountType type = Wrappers.CountType.Warning;
                return this._CountMatrix[(int)group, (int)type];
            }
			set
			{
				Wrappers.CountGroup group = Wrappers.CountGroup.General;
				Wrappers.CountType type = Wrappers.CountType.Warning;
				this._CountMatrix[(int)group, (int)type] = value;
			}
        }

        public System.UInt32 NrOfUserErrors
        {
            get
            {
                Wrappers.CountGroup group = Wrappers.CountGroup.User;
                Wrappers.CountType type = Wrappers.CountType.Error;
                return this._CountMatrix[(int)group, (int)type];
            }
			set
			{
				Wrappers.CountGroup group = Wrappers.CountGroup.User;
				Wrappers.CountType type = Wrappers.CountType.Error;
				this._CountMatrix[(int)group, (int)type] = value;
			}
        }

        public System.UInt32 NrOfUserWarnings
        {
            get
            {
                Wrappers.CountGroup group = Wrappers.CountGroup.User;
                Wrappers.CountType type = Wrappers.CountType.Warning;
                return this._CountMatrix[(int)group, (int)type];
            }
			set
			{
				Wrappers.CountGroup group = Wrappers.CountGroup.User;
				Wrappers.CountType type = Wrappers.CountType.Warning;
				this._CountMatrix[(int)group, (int)type] = value;
			}
        }

		public System.UInt32 TotalNrOfValidationErrors
		{
			get
			{
				Wrappers.CountGroup group = Wrappers.CountGroup.Validation;
				Wrappers.CountType type = Wrappers.CountType.Error;
				System.UInt32 count = 0;
				for (int i = 0; i < this.m_childCountManagers.Count; i++)
				{
					CountManager countManager = (CountManager)this.m_childCountManagers[i];
					count += countManager._CountMatrix[(int)group, (int)type];
				}
				count += this._CountMatrix[(int)group, (int)type];
				return count;
			}
		}

		public System.UInt32 TotalNrOfGeneralErrors
		{
			get
			{
				Wrappers.CountGroup group = Wrappers.CountGroup.General;
				Wrappers.CountType type = Wrappers.CountType.Error;
				System.UInt32 count = 0;
				for (int i = 0; i < this.m_childCountManagers.Count; i++)
				{
					CountManager countManager = (CountManager)this.m_childCountManagers[i];
					count += countManager._CountMatrix[(int)group, (int)type];
				}
				count += this._CountMatrix[(int)group, (int)type];
				return count;
			}
		}

		public System.UInt32 TotalNrOfUserErrors
		{
			get
			{
				Wrappers.CountGroup group = Wrappers.CountGroup.User;
				Wrappers.CountType type = Wrappers.CountType.Error;
				System.UInt32 count = 0;
				for (int i = 0; i < this.m_childCountManagers.Count; i++)
				{
					CountManager countManager = (CountManager)this.m_childCountManagers[i];
					count += countManager._CountMatrix[(int)group, (int)type];
				}
				count += this._CountMatrix[(int)group, (int)type];
				return count;
			}
		}

		public System.UInt32 TotalNrOfValidationWarnings
		{
			get
			{
				Wrappers.CountGroup group = Wrappers.CountGroup.Validation;
				Wrappers.CountType type = Wrappers.CountType.Warning;
				System.UInt32 count = 0;
				for (int i = 0; i < this.m_childCountManagers.Count; i++)
				{
					CountManager countManager = (CountManager)this.m_childCountManagers[i];
					count += countManager._CountMatrix[(int)group, (int)type];
				}
				count += this._CountMatrix[(int)group, (int)type];
				return count;
			}
		}

		public System.UInt32 TotalNrOfGeneralWarnings
		{
			get
			{
				Wrappers.CountGroup group = Wrappers.CountGroup.General;
				Wrappers.CountType type = Wrappers.CountType.Warning;
				System.UInt32 count = 0;
				for (int i = 0; i < this.m_childCountManagers.Count; i++)
				{
					CountManager countManager = (CountManager)this.m_childCountManagers[i];
					count += countManager._CountMatrix[(int)group, (int)type];
				}
				count += this._CountMatrix[(int)group, (int)type];
				return count;
			}
		}

		public System.UInt32 TotalNrOfUserWarnings
		{
			get
			{
				Wrappers.CountGroup group = Wrappers.CountGroup.User;
				Wrappers.CountType type = Wrappers.CountType.Warning;
				System.UInt32 count = 0;
				for (int i = 0; i < this.m_childCountManagers.Count; i++)
				{
					CountManager countManager = (CountManager)this.m_childCountManagers[i];
					count += countManager._CountMatrix[(int)group, (int)type];
				}
				count += this._CountMatrix[(int)group, (int)type];
				return count;
			}
		}

        #endregion
    }
}
