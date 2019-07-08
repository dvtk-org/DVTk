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



namespace DvtkApplicationLayer.UserInterfaces
{
	/// <summary>
	/// Base class for all InputForm entries.
	/// </summary>
	internal class InputFormEntry
	{
		//
		// - Fields -
		//

		/// <summary>
		/// See property Description.
		/// </summary>
		private String description = null;

		/// <summary>
		/// See property ValueAsObject.
		/// </summary>
		private Object theValue = null;

		/// <summary>
		/// See property ValueAsString.
		/// </summary>
		private String theTextValue = "";



		//
		// - Constructors -
		//

		/// <summary>
		/// Hide the default constructor.
		/// </summary>
		private InputFormEntry()
		{
			// Do nothing.
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="description">The description.</param>
		/// <param name="theValue">The value, i.e. what is represented.</param>
		/// <param name="theTextValue">The text value as String, i.e. how is the value displayed.</param>
		public InputFormEntry(String description, Object theValue, String theTextValue)
		{
			this.description = description;
			this.theValue = theValue;
			this.theTextValue = theTextValue;
		}



		//
		// - Properties -
		//

		/// <summary>
		/// The description of the entry.
		/// </summary>
		internal String Description
		{
			get
			{
				return(this.description);
			}
		}

		/// <summary>
		/// The value as object, i.e. what is the value represented.
		/// </summary>
		internal Object Value
		{
			get
			{
				return(this.theValue);
			}
			set
			{
				this.theValue = value;
			}
		}

		/// <summary>
		/// The value as Text, i.e. how is the value displayed.
		/// </summary>
		internal String TextValue
		{
			get
			{
				return(this.theTextValue);
			}
			set
			{
				this.theTextValue = value;
			}
		}
	}
}
