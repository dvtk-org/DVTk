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
using DvtkData.Dimse;



namespace DvtkHighLevelInterface
{
	/// <summary>
	/// Summary description for SetMethod.
	/// </summary>
	internal class SetMethod
	{
		/// <summary>
		/// Do not use this constructor.
		/// </summary>
		private SetMethod()
		{
		}

		public static void Set(AttributeSet attributeSet, bool setCommandAttributes, bool setDataAttributes, params Object[] parameters)
		{
			ArrayList parameterGroups = GetParameterGroups(parameters);
	
			// For each parameter group (parameters to set one single attribute).
			foreach (SetParameterGroup setParameterGroup in parameterGroups)
			{
				if ( (Tag.IsCommandSetTag(setParameterGroup.tagAsUInt32) && setCommandAttributes) ||
					(!Tag.IsCommandSetTag(setParameterGroup.tagAsUInt32) && setDataAttributes)
					)
				{
					if (Attribute.IsSimpleVR(setParameterGroup.vR))
					{
						SetSimpleAttribute(attributeSet, setParameterGroup);
					}
					else if (setParameterGroup.vR == VR.SQ)
					{
						SetSequenceAttribute(attributeSet, setParameterGroup);
					}
					else
					{
						// InterfaceLogging.WriteWarning("Setting of attribute other then simple attribute or sequence attribute not yet implemented.\r\nAttribute with tag " + setParameterGroup.tagAsString + " is not set.");
					}
				}
			}
		}

		private static void SetSequenceAttribute(AttributeSet attributeSet, SetParameterGroup setParameterGroup)
		{
			foreach(Object parameter in setParameterGroup.values)
			{
				// Check if all parameters are of type sequence item.
				if (!(parameter is SequenceItem))
				{
					DvtkHighLevelInterfaceException.Throw("Error while setting the Sequence attribute with tag " + setParameterGroup.tagAsString + ". Only sequence items are allowed as parameters.");
				}
			}

			ValidAttribute sequenceAttribute = new ValidAttribute(setParameterGroup.tagAsUInt32, VR.SQ);

			foreach(SequenceItem sequenceItem in setParameterGroup.values)
			{
				sequenceAttribute.AddItem(sequenceItem);
			}

			attributeSet.Set(sequenceAttribute);
		}

		private static void SetSimpleAttribute(AttributeSet attributeSet, SetParameterGroup setParameterGroup)
		{
			ArrayList attributeValuesAsStrings = new ArrayList();
			bool validStringValues = true;

			foreach(Object parameter in setParameterGroup.values)
			{
				if (parameter is ValidValues)
				{
					ValidValues validValues = parameter as ValidValues;

					for (int valueIndex = 1; valueIndex <= validValues.Count; valueIndex++)
					{
						attributeValuesAsStrings.Add(validValues.GetString(valueIndex));
					}
				}
				else if (parameter is InvalidValues)
				{
					InvalidValues invalidValues = parameter as InvalidValues;
					// InterfaceLogging.WriteError("Setting an attribute with tag " + setParameterGroup.tagAsString + " fails because one of the supplied values is from a non-existing attribute with tag " + invalidValues.TagSequence);
					validStringValues = false;
					break;
				}
				else
				{
					attributeValuesAsStrings.Add(parameter.ToString());
				}
			}

			if (validStringValues)
			{
				ValidAttribute validAttribute = new ValidAttribute(setParameterGroup.tagAsUInt32, setParameterGroup.vR, attributeValuesAsStrings.ToArray());
				attributeSet.Set(validAttribute);
			}
		}

		public static ArrayList GetParameterGroups(params Object[] parameters)
		{
			int parameterIndex = 0;
			bool interpreted = true;

			ArrayList setParameterGroups = new ArrayList();

			// Loop through all parameters of this method.
			while (parameterIndex < parameters.Length)
			{
				SetParameterGroup setParameterGroup = new SetParameterGroup();
				Object parameter = null;

				// Determine the tag of the attribute.

				parameter = parameters[parameterIndex];
				if (parameter is String)
				{
					setParameterGroup.tagAsString = parameter as String;

					// Check the syntax of the first tag and determine the UInt32 representation.
					if (!setParameterGroup.tagAsString.StartsWith("0x"))
					{
						// InterfaceLogging.WriteError("Invalid tag sequence(missing \"0x\" in a tag).");
						interpreted = false;
						break;
					}

					try
					{
						setParameterGroup.tagAsUInt32 = Convert.ToUInt32(setParameterGroup.tagAsString, 16);
					}
					catch
					{
						// InterfaceLogging.WriteError("Invalid tag sequence.");
						interpreted = false;
						break;
					}
				}
				else
				{
					// InterfaceLogging.WriteError("Tag is not supplied as a String");
					interpreted = false;
					break;
				}
				
				parameterIndex++;
				parameter = parameters[parameterIndex];

				// Determine the VR of the attribute.

				if (parameter is DvtkData.Dimse.VR)
				{
					setParameterGroup.vR = (DvtkData.Dimse.VR)parameter;
				}
				else
				{
					// InterfaceLogging.WriteError("Expecting DvtkData.Dimse.VR");
					interpreted = false;
					break;
				}

				parameterIndex++;
				
				// Determine the attribute values (zero or more).

				bool continueWithAttributeValues = true;

				while (continueWithAttributeValues)
				{
					// If no more parameters are present to process...
					if (parameterIndex >= parameters.Length)
					{
						continueWithAttributeValues = false;
					}
						// If the index is not pointing to the last parameter...
					else if ((parameterIndex + 1) < parameters.Length)
					{
						// If the index is pointing to the next attribute...
						if (parameters[parameterIndex + 1] is DvtkData.Dimse.VR)
						{
							continueWithAttributeValues = false;
						}
					}

					if (continueWithAttributeValues)
					{
						setParameterGroup.values.Add(parameters[parameterIndex]);
						parameterIndex++;
					}
				}

				setParameterGroups.Add(setParameterGroup);
			}

			if (!interpreted)
			{
				DvtkHighLevelInterfaceException.Throw("Error while interpreting parameters of Set method.");
			}

			return setParameterGroups;
		}
	}
}
