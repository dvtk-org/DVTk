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
using System.IO;
using Dvtk.Hl7.Messages;

namespace Dvtk.Hl7
{
	/// <summary>
	/// Summary description for Hl7Logger.
	/// </summary>
	public class Hl7Logger
	{
		System.String _filename = System.String.Empty;
		StreamWriter _streamWriter = null;
		int _counter = 0;
		int _nrOfValidationErrors = 0;
		int _nrOfValidationWarnings = 0;
		int _nrOfGeneralErrors = 0;
		int _nrOfGeneralWarnings = 0;
		int _nrOfUserErrors = 0;
		int _nrOfUserWarnings = 0;

		public Hl7Logger(System.String filename)
		{
			_filename = filename;
		}

		public void Start()
		{
			_streamWriter = new StreamWriter(_filename);
			_streamWriter.WriteLine("<?xml version=\"1.0\"?>");
			_streamWriter.WriteLine("<DvtDetailedResultsFile>");	
		}

		public void Stop()
		{
            if (_streamWriter != null)
            {
                _streamWriter.WriteLine("<ValidationCounters>");
                _streamWriter.WriteLine("<NrOfValidationErrors>{0}</NrOfValidationErrors>", _nrOfValidationErrors);
                _streamWriter.WriteLine("<NrOfValidationWarnings>{0}</NrOfValidationWarnings>", _nrOfValidationWarnings);
                _streamWriter.WriteLine("<NrOfGeneralErrors>{0}</NrOfGeneralErrors>", _nrOfGeneralErrors);
                _streamWriter.WriteLine("<NrOfGeneralWarnings>{0}</NrOfGeneralWarnings>", _nrOfGeneralWarnings);
                _streamWriter.WriteLine("<NrOfUserErrors>{0}</NrOfUserErrors>", _nrOfUserErrors);
                _streamWriter.WriteLine("<NrOfUserWarnings>{0}</NrOfUserWarnings>", _nrOfUserWarnings);
                if ((_nrOfValidationErrors == 0) &&
                    (_nrOfGeneralErrors == 0) &&
                    (_nrOfUserErrors == 0))
                {
                    _streamWriter.WriteLine("<ValidationTest>PASSED</ValidationTest>");
                }
                else
                {
                    _streamWriter.WriteLine("<ValidationTest>FAILED</ValidationTest>");
                }
                _streamWriter.WriteLine("</ValidationCounters>");
                _streamWriter.WriteLine("</DvtDetailedResultsFile>");
                _streamWriter.Close();
            }
		}

		public void LogError(System.String errorMessage)
		{
            if (_streamWriter != null)
            {
                System.String message = System.String.Format("<Activity Index=\"{0}\" Level=\"Error\">{1}</Activity>", _counter++, ConvertForXml(errorMessage));
                _streamWriter.WriteLine(message);
            }
            _nrOfGeneralErrors++;
		}

		public void LogWarning(System.String warningMessage)
		{
            if (_streamWriter != null)
            {
                System.String message = System.String.Format("<Activity Index=\"{0}\" Level=\"Warning\">{1}</Activity>", _counter++, ConvertForXml(warningMessage));
                _streamWriter.WriteLine(message);
            }
			_nrOfGeneralWarnings++;
		}

		public void UpdateValidationErrorCount(int errorCount)
		{
			_nrOfValidationErrors += errorCount;
		}

		public void UpdateValidationWarningCount(int warningCount)
		{
			_nrOfValidationWarnings += warningCount;
		}

		public int NrErrors
		{
			get
			{
				return _nrOfValidationErrors + _nrOfGeneralErrors + _nrOfUserErrors;
			}
		}

		public int NrWarnings
		{
			get
			{
				return _nrOfValidationWarnings + _nrOfGeneralWarnings + _nrOfUserWarnings;
			}
		}

		public void LogInfo(System.String infoMessage)
		{
            if (_streamWriter != null)
            {
                System.String message = System.String.Format("<Activity Index=\"{0}\" Level=\"Info\">{1}</Activity>", _counter++, ConvertForXml(infoMessage));
                _streamWriter.WriteLine(message);
            }
		}

		public void LogXmlString(System.String xmlString)
		{
            if (_streamWriter != null)
            {
                _streamWriter.WriteLine(xmlString);
            }
		}

        /*
        public void LogMessage(System.String message, Hl7Message hl7Message)
        {
            if (_streamWriter != null)
            {
                System.String locMessage = System.String.Format("<Activity Index=\"{0}\" Level=\"Script\">{1}</Activity>", _counter++, ConvertForXml(message));
                _streamWriter.WriteLine(locMessage);
                hl7Message.Log(this);
            }
        }
        */

        private System.String ConvertForXml(System.String inString)
		{
			System.String outString = System.String.Empty;
			for (int i = 0; i < inString.Length; i++)
			{
				switch (inString[i])
				{
					case '&': outString += "&#x26;"; break; // &
					case '<': outString += "&#x3C;"; break; // <
					case '>': outString += "&#x3E;"; break; // >
					default: outString += inString[i]; break;
				}
			}
			return outString;
		}
	}
}
