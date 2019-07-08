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



namespace DvtkApplicationLayer.UserInterfaces
{
	/// <summary>
	/// InputFormDropDownListEntry represents a ComboBox entry in the InputForm class.
	/// </summary>
	internal class InputFormDropDownListEntry: InputFormEntry
	{
		//
		// - Fields -
		//

		/// <summary>
		/// See property Values.
		/// </summary>
		private IList values = null;

		/// <summary>
		/// See propert ValueAsString.
		/// </summary>
		private IList textValues = null;

		/// <summary>
		/// See property ComboBox.
		/// </summary>
		private System.Windows.Forms.ComboBox comboBox = null;

		

		//
		// - Constructors -
		//

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="description">The description.</param>
		/// <param name="values">The values.</param>
		/// <param name="textValues">The values as text.</param>
		/// <param name="thevalue">The value.</param>
		public InputFormDropDownListEntry(String description, IList values, IList textValues, Object thevalue): base(description, thevalue, "")
		{
			this.values = values;
			this.textValues = textValues;
		}



		//
		// - Properties -
		//

		/// <summary>
		/// The ComboBox instance used in the InputForm to visualise this entry.
		/// </summary>
		internal System.Windows.Forms.ComboBox ComboBox
		{
			get
			{
				return(this.comboBox);
			}
			set
			{
				this.comboBox = value;
			}
		}

		/// <summary>
		/// Get the values, i.e. the Objects represented in the ComboBox.
		/// </summary>
		internal IList Values
		{
			get
			{
				return(this.values);
			}
		}

		/// <summary>
		/// Get the text values, i.e. how are the values represented (displayed) in the ComboBox.
		/// </summary>
		internal IList TextValues
		{
			get
			{
				return(this.textValues);
			}
		}
	}
}
