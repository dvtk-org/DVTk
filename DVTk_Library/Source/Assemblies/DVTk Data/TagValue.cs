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

namespace DvtkData.Dimse
{
	#region AffectedEntityEnum
	/// <summary>
	/// Affected Entity Enum
	/// </summary>
	public enum AffectedEntityEnum
	{
		/// <summary>
		/// Patient level.
		/// </summary>
		PatientEntity,
		/// <summary>
		/// Study level.
		/// </summary>
		StudyEntity,
		/// <summary>
		/// Series level.
		/// </summary>
		SeriesEntity,
		/// <summary>
		/// Instance level.
		/// </summary>
		InstanceEntity,
		/// <summary>
		/// Image Service Request level.
		/// </summary>
		ImageServiceRequestEntity,
		/// <summary>
		/// Requested Procedure level.
		/// </summary>
		RequestedProcedureEntity,
		/// <summary>
		/// Scheduled Procedure Step level.
		/// </summary>
		ScheduledProcedureStepEntity,
		/// <summary>
		/// Performed Procedure Step level.
		/// </summary>
		PerformedProcedureStepEntity,
		/// <summary>
		/// Any level - applies to any of the above.
		/// </summary>
		AnyEntity
	}
	#endregion

	/// <summary>
	/// Summary description for BaseTagValue.
	/// </summary>
	public abstract class BaseTagValue
	{
		/// <summary>
		/// Base Tag Value.
		/// </summary>
		protected System.String _value = System.String.Empty;

		/// <summary>
		/// Value property.
		/// </summary>
		public virtual System.String Value
		{
			get
			{
				return _value;
			}
		}
	}

	/// <summary>
	/// Summary description for BaseDicomTagValue.
	/// </summary>
	public abstract class BaseDicomTagValue : BaseTagValue
	{
		private DvtkData.Dimse.Tag _parentSequenceTag = Tag.UNDEFINED;
		private DvtkData.Dimse.Tag _tag = null;
		private AffectedEntityEnum _affectedEntity = AffectedEntityEnum.AnyEntity;

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="affectedEntity">Affected Entity</param>
		/// <param name="tag">Tag</param>
		public BaseDicomTagValue(AffectedEntityEnum affectedEntity, DvtkData.Dimse.Tag tag)
		{
			_affectedEntity = affectedEntity;
			_tag = tag;
		}

		#region properties
		/// <summary>
		/// ParentSequenceTag property.
		/// </summary>
		public DvtkData.Dimse.Tag ParentSequenceTag
		{
			get
			{
				return _parentSequenceTag;
			}
			set
			{
				_parentSequenceTag = value;
			}
		}

		/// <summary>
		/// Tag property.
		/// </summary>
		public DvtkData.Dimse.Tag Tag
		{
			get
			{
				return _tag;
			}
		}

		/// <summary>
		/// Affected Entity property.
		/// </summary>
		public AffectedEntityEnum AffectedEntity
		{
			get
			{
				return _affectedEntity;
			}
		}
		#endregion
	}

	/// <summary>
	/// Summary description for DicomTagValue.
	/// </summary>
	public class DicomTagValue : BaseDicomTagValue
	{
		/// <summary>
		/// Class constructor.
		/// Value can be empty - universal match.
		/// </summary>
		/// <param name="tag">Tag</param>
		public DicomTagValue(DvtkData.Dimse.Tag tag) : base(AffectedEntityEnum.AnyEntity, tag)
		{
			_value = System.String.Empty;
		}

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="tag">Tag</param>
		/// <param name="lValue">Value</param>
		public DicomTagValue(DvtkData.Dimse.Tag tag, System.String lValue) : base(AffectedEntityEnum.AnyEntity, tag)
		{
			_value = lValue;
		}
	}

	/// <summary>
	/// Summary description for TagValueAutoIncrement.
	/// </summary>
	public class DicomTagValueAutoIncrement : BaseDicomTagValue
	{
		private System.String _prefix = System.String.Empty;
		private int _initialValue = 0;
		private int _increment = 0;
		private int _fieldSize = 0;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="affectedEntity"></param>
		/// <param name="tag"></param>
		/// <param name="prefix"></param>
		/// <param name="initialValue"></param>
		/// <param name="increment"></param>
		/// <param name="fieldSize"></param>
		public DicomTagValueAutoIncrement(AffectedEntityEnum affectedEntity, DvtkData.Dimse.Tag tag, System.String prefix, int initialValue, int increment, int fieldSize) 
			: base(affectedEntity, tag)
		{
			_prefix = prefix;
			_initialValue = initialValue;
			_increment = increment;
			_fieldSize = fieldSize;
		}

		#region properties
		/// <summary>
		/// Value property.
		/// </summary>
		public override System.String Value
		{
			get
			{
				// Format the value using the prefix, field size, initial value and increment
				System.String postfix = _initialValue.ToString();
				if (_fieldSize > 0)
				{
					while (postfix.Length < _fieldSize)
					{
						postfix = "0" + postfix;
					}
				}
				_value = _prefix + postfix;
				_initialValue += _increment;

				return base.Value;
			}
		}
		#endregion
	}

	/// <summary>
	/// Summary description for TagValueAutoSetUid.
	/// </summary>
	public class DicomTagValueAutoSetUid : BaseDicomTagValue
	{
		private static System.Random _random = new Random(642135);
		private System.String _root = System.String.Empty;
		private int _format = 0;
		private static int _counter = 1;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="affectedEntity"></param>
		/// <param name="tag"></param>
		/// <param name="root"></param>
		/// <param name="format"></param>
		public DicomTagValueAutoSetUid(AffectedEntityEnum affectedEntity, DvtkData.Dimse.Tag tag, System.String root, int format)
			: base(affectedEntity, tag)
		{
			_root = root.TrimEnd('.');
			_format = format;
		}

		#region properties
		/// <summary>
		/// Value property.
		/// </summary>
		public override System.String Value
		{
			get
			{
				// Generate the next uid
				// - use format
				switch(_format)
				{
					case 1:
					{
						int randomNumber = 0;
						while (randomNumber == 0)
						{
							randomNumber = _random.Next(1000);
						}
						System.String time = System.DateTime.Now.ToString("HHmmss", System.Globalization.CultureInfo.InvariantCulture);
						_value = System.String.Format("{0}.{1}.{2}.{3}.{4}",
							_root,
							System.DateTime.Now.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture),
							time.TrimStart('0'),
							randomNumber,
							_counter++);
					}
					break;
					case 0:
					default:
					{
						int randomNumber = 0;
						while (randomNumber == 0)
						{
							randomNumber = _random.Next(1000);
						}
						_value = System.String.Format("{0}.{1}.{2}",
							_root,
							randomNumber,
							_counter++);
					}
						break;
				}
				return base.Value;
			}
		}
		#endregion
	}

	/// <summary>
	/// Summary description for TagValueAutoSetDate.
	/// </summary>
	public class DicomTagValueAutoSetDate : BaseDicomTagValue
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="affectedEntity"></param>
		/// <param name="tag"></param>
		public DicomTagValueAutoSetDate(AffectedEntityEnum affectedEntity, DvtkData.Dimse.Tag tag)
			: base(affectedEntity, tag) {}

		#region properties
		/// <summary>
		/// Value property.
		/// </summary>
		public override System.String Value
		{
			get
			{
				// Generate the date now
				_value = System.DateTime.Now.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
				return base.Value;
			}
		}
		#endregion
	}

	/// <summary>
	/// Summary description for TagValueAutoSetTime.
	/// </summary>
	public class DicomTagValueAutoSetTime : BaseDicomTagValue
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="affectedEntity"></param>
		/// <param name="tag"></param>
		public DicomTagValueAutoSetTime(AffectedEntityEnum affectedEntity, DvtkData.Dimse.Tag tag)
			: base(affectedEntity, tag) {}

		#region properties
		/// <summary>
		/// Value property.
		/// </summary>
		public override System.String Value
		{
			get
			{
				// Generate the time now
				_value = System.DateTime.Now.ToString("HHmmss", System.Globalization.CultureInfo.InvariantCulture);
				return base.Value;
			}
		}
		#endregion
	}

	/// <summary>
	/// Summary description for TagValueDelete.
	/// </summary>
	public class DicomTagValueDelete : BaseDicomTagValue
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="tag"></param>
		public DicomTagValueDelete(DvtkData.Dimse.Tag tag) : base(AffectedEntityEnum.AnyEntity, tag) {}
	}
}
