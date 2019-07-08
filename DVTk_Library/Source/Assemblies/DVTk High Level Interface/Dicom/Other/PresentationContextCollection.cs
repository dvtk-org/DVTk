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



namespace DvtkHighLevelInterface.Dicom.Other
{
	/// <summary>
    /// Represents a collection of PresentationContexts.
	/// </summary>
	public class PresentationContextCollection: DvtkData.Collections.NullSafeCollectionBase
	{
		//
		// - Constructors -
		//

		/// <summary>
		/// Default constructor.
		/// </summary>
		public PresentationContextCollection()
		{
			// Do nothing.
		}

		/// <summary>
		/// Constructor with initialization. Shallow copy.
		/// </summary>
		/// <param name="arrayOfValues">Values to copy.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="arrayOfValues"/> is null.</exception>
		public PresentationContextCollection(PresentationContext[] arrayOfValues)
		{
			if (arrayOfValues == null)
			{
				throw new ArgumentNullException();
			}

			foreach (PresentationContext value in arrayOfValues)
			{
				this.Add(value);
			}
		}

        /// <summary>
        /// Constructor that initialized this instance with a DvtkData RequestedPresentationContextList.
        /// </summary>
        /// <param name="dvtkDataRequestedPresentationContextList">The DvtkData RequestedPresentationContextList.</param>
		internal PresentationContextCollection(DvtkData.Dul.RequestedPresentationContextList dvtkDataRequestedPresentationContextList)
		{
			foreach (DvtkData.Dul.RequestedPresentationContext dvtkDataRequestedPresentationContext in dvtkDataRequestedPresentationContextList)
			{
				PresentationContext presentationContext = new PresentationContext(dvtkDataRequestedPresentationContext);
				Add(presentationContext);
			}
		}

        /// <summary>
        /// Constructor that initialized this instance with a DvtkData AcceptedPresentationContextList.
        /// </summary>
        /// <param name="dvtkDataAcceptedPresentationContextList">The DvtkData AcceptedPresentationContextList.</param>
		internal PresentationContextCollection(DvtkData.Dul.AcceptedPresentationContextList dvtkDataAcceptedPresentationContextList)
		{
			foreach (DvtkData.Dul.AcceptedPresentationContext dvtkDataAcceptedPresentationContext in dvtkDataAcceptedPresentationContextList)
			{
				PresentationContext presentationContext = new PresentationContext(dvtkDataAcceptedPresentationContext);
				Add(presentationContext);
			}
		}



		//
		// - Properties -
		//

		/// <summary>
		/// Gets all accepted abstract syntaxes.
		/// </summary>
        /// <remarks>
        /// Only use this property for A-ASSOCIATE-AC PresentationContexts.
        /// </remarks>
		public StringCollection AcceptedAbstractSyntaxes
		{
			get
			{
				StringCollection acceptedAbstractSyntaxes = new StringCollection();

				foreach(PresentationContext presentationContext in this)
				{
					if (presentationContext.Result == 0)
					{
						if (!acceptedAbstractSyntaxes.Contains(presentationContext.AbstractSyntax))
						{
							acceptedAbstractSyntaxes.Add(presentationContext.AbstractSyntax);
						}
					}
				}

				return(acceptedAbstractSyntaxes);
			}
		}

		/// <summary>
		/// Gets or sets the item at the specified index.
		/// </summary>
		/// <value>The item at the specified <c>index</c>.</value>
		public new PresentationContext this[int index]
		{
			get 
			{
				return (PresentationContext)base[index];
			}
			set
			{
				base.Insert(index, value);
			}
		}



		//
		// - Methods -
		//

		/// <summary>
		/// Adds an item to the <see cref="System.Collections.IList"/>.
		/// </summary>
		/// <param name="value">The item to add to the <see cref="System.Collections.IList"/>. </param>
		/// <returns>The position into which the new element was inserted.</returns>
		public int Add(PresentationContext value)
		{
			return base.Add(value);
		}

        /// <summary>
        /// Adds another PresentationContextCollection to the end of this instance.
        /// </summary>
        /// <param name="presentationContextCollection">The other PresentationContextCollection to add.</param>
		public void AddRange(PresentationContextCollection presentationContextCollection)
		{
			foreach(PresentationContext presentationContext in presentationContextCollection)
			{
				Add(presentationContext);
			}
		}

		/// <summary>
		/// Determines whether the <see cref="System.Collections.IList"/> contains a specific item.
		/// </summary>
		/// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
		/// <returns><see langword="true"/> if the item is found in the <see cref="System.Collections.IList"/>; otherwise, <see langword="false"/>.</returns>
		public bool Contains(PresentationContext value)
		{
			return base.Contains(value);
		}

		/// <summary>
		/// Determines the index of a specific item in the <see cref="System.Collections.IList"/>.
		/// </summary>
		/// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
		/// <returns>The index of <c>value</c> if found in the list; otherwise, -1.</returns>
		public int IndexOf(PresentationContext value)
		{
			return base.IndexOf(value);
		}

		/// <summary>
		/// Inserts an item to the IList at the specified position.
		/// </summary>
		/// <param name="index">The zero-based index at which <c>value</c> should be inserted. </param>
		/// <param name="value">The item to insert into the <see cref="System.Collections.IList"/>.</param>
		public void Insert(int index, PresentationContext value)
		{
			base.Insert(index, value);
		}

		/// <summary>
		/// Removes the first occurrence of a specific item from the IList.
		/// </summary>
		/// <param name="value">The item to remove from the <see cref="System.Collections.IList"/>.</param>
		public void Remove(PresentationContext value)
		{
			base.Remove(value);
		}
	}
}
