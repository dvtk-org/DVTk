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

using Dvtk.Hl7;
using Dvtk.Hl7.Messages;
//using Dvtk.IheActors.Hl7;

namespace Dvtk.IheActors.Hl7.WebService
{
	/// <summary>
	/// Summary description for NistWebServiceClient.
	/// </summary>
	public class NistWebServiceClient
	{
		private Dvtk.IheActors.Hl7.WebService.Validation.MessageValidation _hl7MessageValidation = null;
		private Dvtk.IheActors.Hl7.WebService.Generation.MessageGeneration _hl7MessageGeneration = null;  

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="validationWebServiceUrl">NIST URL</param>
		public NistWebServiceClient(System.String validationWebServiceUrl)
		{
			_hl7MessageValidation = new Dvtk.IheActors.Hl7.WebService.Validation.MessageValidation(validationWebServiceUrl);
			_hl7MessageGeneration = new Dvtk.IheActors.Hl7.WebService.Generation.MessageGeneration(validationWebServiceUrl);

			// Declare a CookieContainer to handle TransportSession			
			_hl7MessageValidation.CookieContainer = new System.Net.CookieContainer();
			_hl7MessageGeneration.CookieContainer = new System.Net.CookieContainer();
		}

		/// <summary>
		/// Hl7 Message Validation. Validate the hl7Message using the given xmlProfile.
		/// </summary>
		/// <param name="xmlProfile">HL7 Conformance Profile - encoded as XML.</param>
		/// <param name="xmlValidContext">HL7 Validation Context - encoded as XML.</param>
		/// <param name="hl7Message">DVTK HL7 Message.</param>
		/// <param name="isXml">Generated messages are ER7 or XML message (true if XML, false either).</param>
		/// <param name="errorDescription">Out - error description string.</param>
		/// <returns>String - validation results XML stream.</returns>
		public System.String Validate(System.String xmlProfile, System.String xmlValidContext, System.String hl7Message, System.Boolean isXml, out System.String errorDescription)
		{
			System.String validationResultsXmlStream = System.String.Empty;
			errorDescription = System.String.Empty;
 
			Dvtk.IheActors.Hl7.WebService.Validation.setProfile profile = new Dvtk.IheActors.Hl7.WebService.Validation.setProfile();
			Dvtk.IheActors.Hl7.WebService.Validation.setProfileResponse profileResponse = new Dvtk.IheActors.Hl7.WebService.Validation.setProfileResponse();
			profile.param0 = xmlProfile;
			profileResponse = _hl7MessageValidation.setProfile(profile);
			if (profileResponse.@return != true)
			{
				errorDescription = GetErrorDescriptionValidation("_hl7MessageValidation.setProfile(profile)");
			}
			else
			{
				Dvtk.IheActors.Hl7.WebService.Validation.setMessage msg = new Dvtk.IheActors.Hl7.WebService.Validation.setMessage();
				Dvtk.IheActors.Hl7.WebService.Validation.setMessageResponse msgResponse = new Dvtk.IheActors.Hl7.WebService.Validation.setMessageResponse();
				msg.param0 = hl7Message;
				msg.param1 = isXml;
			
				msgResponse = _hl7MessageValidation.setMessage(msg);
				if (msgResponse.@return != true)
				{
					errorDescription = GetErrorDescriptionValidation("_hl7MessageValidation.setMessage(msg)");
				}
				else
				{
					Dvtk.IheActors.Hl7.WebService.Validation.setValidationContext validContext = new Dvtk.IheActors.Hl7.WebService.Validation.setValidationContext();
					Dvtk.IheActors.Hl7.WebService.Validation.setValidationContextResponse validContextResponse = new Dvtk.IheActors.Hl7.WebService.Validation.setValidationContextResponse();
					validContext.param0 = xmlValidContext;
					validContextResponse = _hl7MessageValidation.setValidationContext(validContext);
					if (validContextResponse.@return != true)
					{
						errorDescription = GetErrorDescriptionValidation("_hl7MessageValidation.setValidationContext(validContext)");
					}
					else
					{
						Dvtk.IheActors.Hl7.WebService.Validation.validate val = new Dvtk.IheActors.Hl7.WebService.Validation.validate();
						Dvtk.IheActors.Hl7.WebService.Validation.validateResponse valResponse = new Dvtk.IheActors.Hl7.WebService.Validation.validateResponse();
						valResponse = _hl7MessageValidation.validate(val);
						// validate() returns if the message is valid or not (and not if the method was well executed or not)
						if (valResponse.@return != true)
						{
							errorDescription = GetErrorDescriptionValidation("_hl7MessageValidation.validate(val)");
						}

						Dvtk.IheActors.Hl7.WebService.Validation.getValidationReport valReport = new Dvtk.IheActors.Hl7.WebService.Validation.getValidationReport();
						Dvtk.IheActors.Hl7.WebService.Validation.getValidationReportResponse valReportResponse = new Dvtk.IheActors.Hl7.WebService.Validation.getValidationReportResponse();
						valReportResponse = _hl7MessageValidation.getValidationReport(valReport);
						if (valReportResponse.@return != null)
						{
							validationResultsXmlStream = valReportResponse.@return;
						}
						else
						{
							errorDescription = GetErrorDescriptionValidation("_hl7MessageValidation.getValidationReport(valReport)");
						}
					}
				}
			}

			return validationResultsXmlStream;
		}

		/// <summary>
		/// HL7 Message Generation. Generate some HL7 Messages using the given xmlProfile, xmlContext and
		/// xmlFixedData.
		/// </summary>
		/// <param name="xmlProfile">HL7 Conformance Profile - encoded as XML.</param>
		/// <param name="xmlContext">Generation Context - encoded as XML.</param>
		/// <param name="xmlFixedData">Fixed Data - encoded as XML.</param>
		/// <returns>ER7 encoded String based HL7 Messages.</returns>
		public System.String[] Generate(System.String xmlProfile, System.String xmlContext, System.String xmlFixedData)
		{
			System.String[] messages = null;

			Dvtk.IheActors.Hl7.WebService.Generation.setProfile profile = new Dvtk.IheActors.Hl7.WebService.Generation.setProfile();
			Dvtk.IheActors.Hl7.WebService.Generation.setProfileResponse profileResponse = new Dvtk.IheActors.Hl7.WebService.Generation.setProfileResponse();
			profile.param0 = xmlProfile;
			profileResponse = _hl7MessageGeneration.setProfile(profile);
			if (profileResponse.@return != true)
			{
				DisplayErrorDescriptionGeneration("_hl7MessageGeneration.setProfile(profile)");
			}

			Dvtk.IheActors.Hl7.WebService.Generation.setGenerationContext context = new Dvtk.IheActors.Hl7.WebService.Generation.setGenerationContext();
			Dvtk.IheActors.Hl7.WebService.Generation.setGenerationContextResponse contextResponse = new Dvtk.IheActors.Hl7.WebService.Generation.setGenerationContextResponse();
			context.param0 = xmlContext;
			contextResponse = _hl7MessageGeneration.setGenerationContext(context);
			if (contextResponse.@return != true)
			{
				DisplayErrorDescriptionGeneration("_hl7MessageGeneration.setGenerationContext(context)");
			}

			Dvtk.IheActors.Hl7.WebService.Generation.setFixedData fixedData = new Dvtk.IheActors.Hl7.WebService.Generation.setFixedData();
			Dvtk.IheActors.Hl7.WebService.Generation.setFixedDataResponse fixedDataResponse = new Dvtk.IheActors.Hl7.WebService.Generation.setFixedDataResponse();
			fixedData.param0 = xmlFixedData;
			fixedDataResponse = _hl7MessageGeneration.setFixedData(fixedData);
			if (fixedDataResponse.@return != true)
			{
				DisplayErrorDescriptionGeneration("_hl7MessageGeneration.setFixedData(fixedData)");
			}

			Dvtk.IheActors.Hl7.WebService.Generation.generate generate = new Dvtk.IheActors.Hl7.WebService.Generation.generate();
			Dvtk.IheActors.Hl7.WebService.Generation.generateResponse generateResponse = new Dvtk.IheActors.Hl7.WebService.Generation.generateResponse();
			generateResponse = _hl7MessageGeneration.generate(generate);
			if (generateResponse.@return == true)
			{
				Dvtk.IheActors.Hl7.WebService.Generation.getMessages msgs = new Dvtk.IheActors.Hl7.WebService.Generation.getMessages();
				messages = _hl7MessageGeneration.getMessages(msgs);
			}
			else
			{
				DisplayErrorDescriptionGeneration("_hl7MessageGeneration.generate(generate)");
			}

			return messages;
		}

		/// <summary>
		/// Display the Error Description along with the given message.
		/// </summary>
		/// <param name="message">Message to display.</param>
		private void DisplayErrorDescriptionGeneration(System.String message)
		{
			Dvtk.IheActors.Hl7.WebService.Generation.getErrorDescription errorDescription = new Dvtk.IheActors.Hl7.WebService.Generation.getErrorDescription();
			Dvtk.IheActors.Hl7.WebService.Generation.getErrorDescriptionResponse errorDescriptionResponse = new Dvtk.IheActors.Hl7.WebService.Generation.getErrorDescriptionResponse();
			errorDescriptionResponse = _hl7MessageGeneration.getErrorDescription(errorDescription);
			if (errorDescriptionResponse.@return != null)
			{
				Console.WriteLine("{0} - failed. Error Description: {1}", message, errorDescriptionResponse.@return);
			}
			else
			{
				Console.WriteLine("{0} - failed", message);
			}
		}

		/// <summary>
		/// Get the Error Description along with the given message.
		/// </summary>
		/// <param name="message">Message to display.</param>
		private System.String GetErrorDescriptionValidation(System.String message)
		{
			System.String errorDescriptionString = System.String.Empty;

			Dvtk.IheActors.Hl7.WebService.Validation.getErrorDescription errorDescription = new Dvtk.IheActors.Hl7.WebService.Validation.getErrorDescription();
			Dvtk.IheActors.Hl7.WebService.Validation.getErrorDescriptionResponse errorDescriptionResponse = new Dvtk.IheActors.Hl7.WebService.Validation.getErrorDescriptionResponse();
			errorDescriptionResponse = _hl7MessageValidation.getErrorDescription(errorDescription);
			if (errorDescriptionResponse.@return != null)
			{
				if (errorDescriptionResponse.@return != System.String.Empty)
				{
					errorDescriptionString  = System.String.Format("{0} - failed. Error Description: {1}", message, errorDescriptionResponse.@return);
				}
			}
			else
			{
				errorDescriptionString  = System.String.Format("{0} - failed", message);
			}

			return errorDescriptionString;
		}
	}
}
