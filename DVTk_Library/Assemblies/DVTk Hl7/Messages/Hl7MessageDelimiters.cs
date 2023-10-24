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
using System.Text;
using Dvtk.Hl7;

namespace Dvtk.Hl7.Messages
{
	/// <summary>
	/// Summary description for Hl7MessageDelimiters.
	/// </summary>
	public class Hl7MessageDelimiters
	{
		private System.String _fieldDelimiter = System.String.Empty;
		private System.String _componentDelimiter = System.String.Empty;
		private System.String _repetitionSeparator = System.String.Empty;
		private System.String _escapeCharacter = System.String.Empty;
		private System.String _subComponentDelimiter = System.String.Empty;

		/// <summary>
		/// Class constructor.
		/// </summary>
		public Hl7MessageDelimiters()
		{
			// constructor activities
			_fieldDelimiter = "|";
			_componentDelimiter = "^";
			_repetitionSeparator = "~";
			_escapeCharacter = "\\";
			_subComponentDelimiter = "&";
		}

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="messageDelimiters">String containg the 5 delimiters.</param>
		public Hl7MessageDelimiters(System.String messageDelimiters) : base()
		{
			// constructor activities
			FromString(messageDelimiters);
		}

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="fieldDelimiter">HL7 Field Delimiter</param>
		/// <param name="componentDelimiter">HL7 Component Delimiter</param>
		/// <param name="repetitionSeparator">HL7 Repetition Separator</param>
		/// <param name="escapeCharacter">HL7 Escape Character</param>
		/// <param name="subComponentDelimiter">HL7 Sub-component Delimiter</param>
		public Hl7MessageDelimiters(System.String fieldDelimiter,
									System.String componentDelimiter,
									System.String repetitionSeparator,
									System.String escapeCharacter,
									System.String subComponentDelimiter)
		{
			// constructor activities
			_fieldDelimiter = fieldDelimiter;
			_componentDelimiter = componentDelimiter;
			_repetitionSeparator = repetitionSeparator;
			_escapeCharacter = escapeCharacter;
			_subComponentDelimiter = subComponentDelimiter;
		}

		/// <summary>
		/// Set the Message Delimiters from the given string.
		/// </summary>
		/// <param name="messageDelimiters"></param>
		public void FromString(System.String messageDelimiters)
		{
			// set the individual delimiters from the input string
			// - we assume that the characters in the input string are in the correct order
			if (messageDelimiters.Length == 5)
			{
				_fieldDelimiter = messageDelimiters.Substring(0,1);
				_componentDelimiter = messageDelimiters.Substring(1,1);
				_repetitionSeparator = messageDelimiters.Substring(2,1);
				_escapeCharacter = messageDelimiters.Substring(3,1);
				_subComponentDelimiter = messageDelimiters.Substring(4,1);
			}
		}

		/// <summary>
		/// Property - FieldDelimiter
		/// </summary>
		public System.String FieldDelimiter
		{
			set
			{
				_fieldDelimiter = value;
			}
			get
			{
				return _fieldDelimiter;
			}
		}
		/// <summary>
		/// Property - ComponentDelimiter
		/// </summary>
		public System.String ComponentDelimiter
		{
			set
			{
				_componentDelimiter = value;
			}
			get
			{
				return _componentDelimiter;
			}
		}

		/// <summary>
		/// Property - RepetitionSeparator
		/// </summary>
		public System.String RepetitionSeparator
		{
			set
			{
				_repetitionSeparator = value;
			}
			get
			{
				return _repetitionSeparator;
			}
		}

		/// <summary>
		/// Property - EscapeCharacter
		/// </summary>
		public System.String EscapeCharacter
		{
			set
			{
				_escapeCharacter = value;
			}
			get
			{
				return _escapeCharacter;
			}
		}

		/// <summary>
		/// Property - SubComponentDelimiter
		/// </summary>
		public System.String SubComponentDelimiter
		{
			set
			{
				_subComponentDelimiter = value;
			}
			get
			{
				return _subComponentDelimiter;
			}
		}

		/// <summary>
		/// To String.
		/// </summary>
		/// <returns>System.String representing the message delimiters.</returns>
		public override System.String ToString()
		{
			return _fieldDelimiter + _componentDelimiter + _repetitionSeparator + _escapeCharacter + _subComponentDelimiter;
		}
	}
}
