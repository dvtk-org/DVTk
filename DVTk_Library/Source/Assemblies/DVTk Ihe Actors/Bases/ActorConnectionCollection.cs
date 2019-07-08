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

namespace Dvtk.IheActors.Bases
{
	/// <summary>
	/// Summary description for ActorConnectionCollection.
	/// </summary>
	public class ActorConnectionCollection : CollectionBase
	{
		/// <summary>
		/// Class Constructor.
		/// </summary>
		public ActorConnectionCollection() {}

		/// <summary>
		/// Gets or sets an <see cref="ActorConnection"/> from the collection.
		/// </summary>
		/// <param name="index">The zero-based index of the collection member to get or set.</param>
		/// <value>The <see cref="ActorConnection"/> at the specified index.</value>
		public ActorConnection this[int index]  
		{
			get  
			{
				return ((ActorConnection) List[index]);
			}
			set  
			{
				List[index] = value;
			}
		}

		/// <summary>
		/// Adds an object to the end of the <see cref="ActorConnectionCollection"/>.
		/// </summary>
		/// <param name="value">The <see cref="ActorConnection"/> to be added to the end of the <see cref="ActorConnectionCollection"/>.</param>
		/// <returns>The <see cref="ActorConnectionCollection"/> index at which the value has been added.</returns>
		public int Add(ActorConnection value)  
		{
			return (List.Add(value));
		}

		/// <summary>
		/// Searches for the specified <see cref="ActorConnection"/> and 
		/// returns the zero-based index of the first occurrence within the entire <see cref="ActorConnectionCollection"/>.
		/// </summary>
		/// <param name="value">The <see cref="ActorConnection"/> to locate in the <see cref="ActorConnectionCollection"/>.</param>
		/// <returns>
		/// The zero-based index of the first occurrence of value within the entire <see cref="ActorConnectionCollection"/>, 
		/// if found; otherwise, -1.
		/// </returns>
		public int IndexOf(ActorConnection value)  
		{
			return (List.IndexOf(value));
		}

		/// <summary>
		/// Inserts an <see cref="ActorConnection"/> element into the <see cref="ActorConnectionCollection"/> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which value should be inserted.</param>
		/// <param name="value">The <see cref="ActorConnection"/> to insert.</param>
		public void Insert(int index, ActorConnection value)  
		{
			List.Insert(index, value);
		}

		/// <summary>
		/// Removes the first occurrence of a specific <see cref="ActorConnection"/> from the <see cref="ActorConnectionCollection"/>.
		/// </summary>
		/// <param name="value">The <see cref="ActorConnection"/> to remove from the <see cref="ActorConnectionCollection"/>.</param>
		public void Remove(ActorConnection value)  
		{
			List.Remove(value);
		}

		/// <summary>
		/// Determines whether the <see cref="ActorConnectionCollection"/> contains a specific element.
		/// </summary>
		/// <param name="value">The <see cref="ActorConnection"/> to locate in the <see cref="ActorConnectionCollection"/>.</param>
		/// <returns>
		/// <c>true</c> if the <see cref="ActorConnectionCollection"/> contains the specified value; 
		/// otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(ActorConnection value)  
		{
			// If value is not of type Code, this will return false.
			return (List.Contains(value));
		}

		/// <summary>
		/// Performs additional custom processes before inserting a new element into the collection instance.
		/// </summary>
		/// <param name="index">The zero-based index at which to insert value.</param>
		/// <param name="value">The new value of the element at index.</param>
		protected override void OnInsert(int index, Object value)  
		{
			if (!(value is ActorConnection))
				throw new ArgumentException("value must be of type ActorConnection.", "value");
		}

		/// <summary>
		/// Performs additional custom processes when removing an element from the collection instance.
		/// </summary>
		/// <param name="index">The zero-based index at which value can be found.</param>
		/// <param name="value">The value of the element to remove from index.</param>
		protected override void OnRemove(int index, Object value)  
		{
			if (!(value is ActorConnection))
				throw new ArgumentException("value must be of type ActorConnection.", "value");
		}

		/// <summary>
		/// Performs additional custom processes before setting a value in the collection instance.
		/// </summary>
		/// <param name="index">The zero-based index at which oldValue can be found.</param>
		/// <param name="oldValue">The value to replace with newValue.</param>
		/// <param name="newValue">The new value of the element at index.</param>
		protected override void OnSet(int index, Object oldValue, Object newValue)  
		{
			if (!(newValue is ActorConnection))
				throw new ArgumentException("newValue must be of type ActorConnection.", "newValue");
		}

		/// <summary>
		/// Performs additional custom processes when validating a value.
		/// </summary>
		/// <param name="value">The object to validate.</param>
		protected override void OnValidate(Object value)  
		{
			if (!(value is ActorConnection))
				throw new ArgumentException("value must be of type ActorConnection.");
		}
	
		/// <summary>
		/// Copies the elements of the <see cref="ICollection"/> to a strong-typed <c>ActorConnection[]</c>, 
		/// starting at a particular <c>ActorConnection[]</c> index.
		/// </summary>
		/// <param name="array">
		/// The one-dimensional <c>ActorConnection[]</c> that is the destination of the elements 
		/// copied from <see cref="ICollection"/>.
		/// The <c>ActorConnection[]</c> must have zero-based indexing. 
		/// </param>
		/// <param name="index">
		/// The zero-based index in array at which copying begins.
		/// </param>
		/// <remarks>
		/// Provides the strongly typed member for <see cref="ICollection"/>.
		/// </remarks>
		public void CopyTo(ActorConnection[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		/// <summary>
		/// Enable connections to all the destination actors 
		/// </summary>
		public void EnableAll()
		{
			// enable connections to all destination actors
			foreach (ActorConnection actorConnection in this)
			{
				actorConnection.IsActive = true;
			}
		}

		/// <summary>
		/// Enable the connection to a specific actor.
		/// </summary>
		/// <param name="actorType">Destination actor type.</param>
		/// <param name="id">Destination actor id.</param>
		/// <returns>bool - destination enabled true/false. False indicates that the destination actor was not found.</returns>
		public bool Enable(ActorTypeEnum actorType, System.String id)
		{
			bool enabled = false;
			ActorName actorName = new ActorName(actorType, id);

			// search for connection to destination with the given actor name
			foreach (ActorConnection actorConnection in this)
			{
				if (actorConnection.ActorName == actorName)
				{
					// enable this specific actor connection
					actorConnection.IsActive = true;
					enabled = true;
				}
			}

			return enabled;
		}

		/// <summary>
		/// Disable connections to all the destination actors 
		/// </summary>
		public void DisableAll()
		{
			// disable connections to all destination actors
			foreach (ActorConnection actorConnection in this)
			{
				actorConnection.IsActive = false;
			}
		}

		/// <summary>
		/// Disable the connection to a specific actor.
		/// </summary>
		/// <param name="actorType">Destination actor type.</param>
		/// <param name="id">Destination actor id.</param>
		/// <returns>bool - destination disabled true/false. False indicates that the destination actor was not found.</returns>
		public bool Disable(ActorTypeEnum actorType, System.String id)
		{
			bool disabled = false;
			ActorName actorName = new ActorName(actorType, id);

			// search for connection to destination with the given actor name
			foreach (ActorConnection actorConnection in this)
			{
				if (actorConnection.ActorName == actorName)
				{
					// disable this specific actor connection
					actorConnection.IsActive = false;
					disabled = true;
				}
			}

			return disabled;
		}

		/// <summary>
		/// Return a list of all the destination actor names of the given actor type that are active.
		/// </summary>
		/// <param name="actorType">Destination actor type.</param>
		/// <returns>ActorNameCollection - all active destination actor names of the given actor type.</returns>
		public ActorNameCollection IsEnabled(ActorTypeEnum actorType)
		{
			ActorNameCollection actorNames = new ActorNameCollection();

			// search for connection to destination with the given actor type
			foreach (ActorConnection actorConnection in this)
			{
				if ((actorConnection.ActorName.Type == actorType) &&
					(actorConnection.IsActive == true))
				{
					// return this active actor name
					actorNames.Add(actorConnection.ActorName);
				}
			}
		
			return actorNames;
		}

		/// <summary>
		/// Check if an actor connection is already enabled for the given actor name.
		/// </summary>
		/// <param name="actorName">Actor Name.</param>
		/// <returns>Boolean indicating whether the actor connection is enabled or not.</returns>
		public bool IsEnabled(ActorName actorName)
		{
			bool isEnabled = false;

			// search for connection to destination with the given actor name
			foreach (ActorConnection actorConnection in this)
			{
				if ((actorConnection.ActorName.TypeId == actorName.TypeId) &&
					(actorConnection.IsActive == true))
				{
					// the actor connection is enabled
					isEnabled = true;
				}
			}

			return isEnabled;
		}
	}
}
