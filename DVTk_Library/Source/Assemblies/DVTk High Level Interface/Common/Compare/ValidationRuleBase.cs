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
using System.Collections.Specialized;


namespace DvtkHighLevelInterface.Common.Compare
{
	/// <summary>
	/// Abstract base class. 
	/// Specifies for an attribute in some protocol how it should be validated.
	/// </summary>
	abstract public class ValidationRuleBase
	{
		//
		// - Fields -
		//

		/// <summary>
		/// See property Flags.
		/// </summary>
		private FlagsBase flags = FlagsBase.None;



		//
		// - Properties -
		//

		/// <summary>
		/// The flags sepcified for this validation rule.
		/// </summary>
		internal FlagsBase Flags
		{
			get
			{
				return(this.flags);
			}
			set
			{
				this.flags = value;
			}
		}

        /// <summary>
        /// Gets a string collection representing a collection of flags or sets a collection of flags given a string collection.
        /// </summary>
		public String FlagsString
		{
			get
			{
				StringCollection flagsStringCollection = new StringCollection();
				String flagsString = "";
				bool flagsAdded = false;

				if ((this.flags & FlagsBase.Compare_present) != 0)
				{
					flagsStringCollection.Add("Compare_present");
				}

				if ((this.flags & FlagsBase.Compare_values) != 0)
				{
					flagsStringCollection.Add("Compare_values");
				}

				if ((this.flags & FlagsBase.Present) != 0)
				{
					flagsStringCollection.Add("Present");
				}

				if ((this.flags & FlagsBase.Not_present) != 0)
				{
					flagsStringCollection.Add("Not_present");
				}

				if ((this.flags & FlagsBase.Compare_VR) != 0)
				{
					flagsStringCollection.Add("Compare_VR");
				}

				if ((this.flags & FlagsBase.Values) != 0)
				{
					flagsStringCollection.Add("Values");
				}

				if ((this.flags & FlagsBase.No_values) != 0)
				{
					flagsStringCollection.Add("No_values");
				}

				if ((this.flags & FlagsBase.Include_sequence_items) != 0)
				{
					flagsStringCollection.Add("Include_sequence_items");
				}

				foreach(String flagString in flagsStringCollection)
				{
					if (flagsAdded)
					{
						flagsString+= "," + flagString;
					}
					else
					{
						flagsString+= flagString;
						flagsAdded = true;
					}
				}

				return(flagsString);
			}
			set
			{
				this.flags = FlagsBase.None;

				String[] newFlagsAsStringArray = value.Split(',');
				StringCollection newFlagsAsStringCollection = new StringCollection();

				newFlagsAsStringCollection.AddRange(newFlagsAsStringArray);

				if (newFlagsAsStringCollection.Contains("Compare_present"))
				{
					this.flags|= FlagsBase.Compare_present;
				}

				if (newFlagsAsStringCollection.Contains("Compare_values"))
				{
					this.flags|= FlagsBase.Compare_values;
				}

				if (newFlagsAsStringCollection.Contains("Present"))
				{
					this.flags|= FlagsBase.Present;
				}

				if (newFlagsAsStringCollection.Contains("Not_present"))
				{
					this.flags|= FlagsBase.Not_present;
				}

				if (newFlagsAsStringCollection.Contains("Compare_VR"))
				{
					this.flags|= FlagsBase.Compare_VR;
				}

				if (newFlagsAsStringCollection.Contains("Values"))
				{
					this.flags|= FlagsBase.Values;
				}

				if (newFlagsAsStringCollection.Contains("No_values"))
				{
					this.flags|= FlagsBase.No_values;
				}

				if (newFlagsAsStringCollection.Contains("Include_sequence_items"))
				{
					this.flags|= FlagsBase.Include_sequence_items;
				}
			}
		}



	}
}
