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
	/// InputFormTextBoxEntry represents a TextBox entry in the InputForm class.
	/// </summary>
	internal class InputFormTextBoxEntry: InputFormEntry
	{
		//
		// - Fields -
		//

		/// <summary>
		/// See property ReadOnly.
		/// </summary>
		private bool readOnly = true;

		/// <summary>
		/// See property TextBox.
		/// </summary>
		private System.Windows.Forms.TextBox textBox = null;



		//
		// - Constructors -
		//

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="description">The description.</param>
		/// <param name="theValue">The value as String.</param>
		/// <param name="readOnly"></param>
		public InputFormTextBoxEntry(String description, String theValue, bool readOnly): base(description, theValue, theValue)
		{
			this.readOnly = readOnly;
		}



		//
		// - Properties -
		//

		/// <summary>
		/// Determines if the TextBox displayed is read only.
		/// </summary>
		internal bool ReadOnly
		{
			get
			{
				return(this.readOnly);
			}
		}

		/// <summary>
		/// The TextBox instance used in the InputForm to visualise this entry.
		/// </summary>
		internal System.Windows.Forms.TextBox TextBox
		{
			get
			{
				return(this.textBox);
			}
			set
			{
				this.textBox = value;
			}
		}
	}
}
