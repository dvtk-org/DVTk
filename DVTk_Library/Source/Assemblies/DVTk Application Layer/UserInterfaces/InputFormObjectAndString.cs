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
	/// Used to be able to have a combination of an Object and a String representation for use in a ComboBox.
	/// </summary>
	internal class InputFormObjectAndString
	{
		//
		// - Fields -
		//

		/// <summary>
		/// See property TheObject
		/// </summary>
		private Object theObject = null;

		/// <summary>
		/// See property TheString
		/// </summary>
		private String theString = null;



		//
		// - Constructors -
		//

		/// <summary>
		/// Hide default constructor.
		/// </summary>
		private InputFormObjectAndString()
		{
			// Do nothing.
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="theObject">The Object.</param>
		/// <param name="theString">The String.</param>
		public InputFormObjectAndString(Object theObject, String theString)
		{
			this.theObject = theObject;
			this.theString = theString;
		}



		//
		// - Properties -
		//

		/// <summary>
		/// Get the Object.
		/// </summary>
		internal Object TheObject
		{
			get
			{
				return(this.theObject);
			}
		}

		/// <summary>
		/// Get the String.
		/// </summary>
		internal String TheString
		{
			get
			{
				return(this.theString);
			}
		}



		//
		// - Methods -
		//

		/// <summary>
		/// This ToString method has been overriden to be able to display a different String then
		/// the ToString Object of the this.theObject instance.
		/// </summary>
		/// <returns></returns>
		public override String ToString()
		{
			return(this.theString);
		}
	}
}
