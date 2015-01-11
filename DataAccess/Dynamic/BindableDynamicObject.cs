﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;

namespace DbParallel.DataAccess
{
	public class BindableDynamicObject : DynamicObject, ICustomTypeDescriptor, IDictionary<string, Object>
	{
		private IDictionary<string, Object> _data;

		public BindableDynamicObject(IDictionary<string, Object> content = null)
		{
			_data = content ?? new ExpandoObject();
		}

		/// <returns>A sequence that contains dynamic member names.</returns>
		public override IEnumerable<string> GetDynamicMemberNames()
		{
			return _data.Keys;
		}

		/// <param name="binder">Provides information about the object that called the dynamic operation. The binder.Name property provides the name of the member on which the dynamic operation is performed. For example, for the Console.WriteLine(sampleObject.SampleProperty) statement, where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, binder.Name returns "SampleProperty". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param>
		/// <param name="result">The result of the get operation. For example, if the method is called for a property, you can assign the property value to <paramref name="result"/>.</param>
		/// <returns>true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a run-time exception is thrown.)</returns>
		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			return _data.TryGetValue(binder.Name, out result);
		}

		/// <param name="binder">Provides information about the object that called the dynamic operation. The binder.Name property provides the name of the member to which the value is being assigned. For example, for the statement sampleObject.SampleProperty = "Test", where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, binder.Name returns "SampleProperty". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param>
		/// <param name="value">The value to set to the member. For example, for sampleObject.SampleProperty = "Test", where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, the <paramref name="value"/> is "Test".</param>
		/// <returns>true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.)</returns>
		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			_data[binder.Name] = value;
			return true;
		}

		#region ICustomTypeDescriptor Members

		AttributeCollection ICustomTypeDescriptor.GetAttributes()
		{
			return AttributeCollection.Empty;
		}

		string ICustomTypeDescriptor.GetClassName()
		{
			return null;
		}

		string ICustomTypeDescriptor.GetComponentName()
		{
			return null;
		}

		TypeConverter ICustomTypeDescriptor.GetConverter()
		{
			return null;
		}

		EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
		{
			return null;
		}

		PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
		{
			return null;
		}

		object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
		{
			return null;
		}

		EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
		{
			return EventDescriptorCollection.Empty;
		}

		EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
		{
			return EventDescriptorCollection.Empty;
		}

		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
		{
			return ((ICustomTypeDescriptor)this).GetProperties();
		}

		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			return new PropertyDescriptorCollection(_data.Select(d => new DynamicPropertyDescriptor(d.Key, d.Value.GetType())).ToArray(), readOnly: true);
		}

		object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
		{
			return this;
		}

		private class DynamicPropertyDescriptor : PropertyDescriptor
		{
			private static readonly Attribute[] _empty = new Attribute[0];
			private readonly Type _type;

			public DynamicPropertyDescriptor(string name, Type type)
				: base(name, _empty)
			{
				_type = type;
			}

			public override Type ComponentType
			{
				get { return typeof(BindableDynamicObject); }
			}

			public override bool IsReadOnly
			{
				get { return false; }
			}

			public override Type PropertyType
			{
				get { return _type; }
			}

			public override bool CanResetValue(object component)
			{
				return false;
			}

			public override object GetValue(object component)
			{
				IDictionary<string, Object> record = component as IDictionary<string, Object>;

				return (record == null) ? null : record[Name];
			}

			public override void ResetValue(object component)
			{
				IDictionary<string, Object> record = component as IDictionary<string, Object>;

				if (record != null)
				{
					Type t = component.GetType();

					if (t.IsValueType && Nullable.GetUnderlyingType(t) == null)
						record[Name] = Activator.CreateInstance(t);
					else
						record[Name] = DBNull.Value;
				}
			}

			public override void SetValue(object component, object value)
			{
				IDictionary<string, Object> record = component as IDictionary<string, Object>;

				if (record != null)
					record[Name] = value;
			}

			public override bool ShouldSerializeValue(object component)
			{
				return false;
			}
		}

		#endregion

		#region IDictionary<string, object> Members

		void IDictionary<string, object>.Add(string key, object value)
		{
			_data.Add(key, value);
		}

		bool IDictionary<string, object>.ContainsKey(string key)
		{
			return _data.ContainsKey(key);
		}

		ICollection<string> IDictionary<string, object>.Keys { get { return _data.Keys; } }

		bool IDictionary<string, object>.Remove(string key)
		{
			return _data.Remove(key);
		}

		bool IDictionary<string, object>.TryGetValue(string key, out object value)
		{
			return _data.TryGetValue(key, out value);
		}

		ICollection<object> IDictionary<string, object>.Values { get { return _data.Values; } }

		object IDictionary<string, object>.this[string key]
		{
			get { return _data[key]; }
			set { _data[key] = value; }
		}

		void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
		{
			_data.Add(item);
		}

		void ICollection<KeyValuePair<string, object>>.Clear()
		{
			_data.Clear();
		}

		bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
		{
			return _data.Contains(item);
		}

		void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			_data.CopyTo(array, arrayIndex);
		}

		int ICollection<KeyValuePair<string, object>>.Count { get { return _data.Count; } }

		bool ICollection<KeyValuePair<string, object>>.IsReadOnly { get { return _data.IsReadOnly; } }

		bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
		{
			return _data.Remove(item);
		}

		IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
		{
			return _data.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _data.GetEnumerator();
		}

		#endregion
	}
}

////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	Copyright 2015 Abel Cheng
//	This source code is subject to terms and conditions of the Apache License, Version 2.0.
//	See http://www.apache.org/licenses/LICENSE-2.0.
//	All other rights reserved.
//	You must not remove this notice, or any other, from this software.
//
//	Original Author:	Abel Cheng <abelcys@gmail.com>
//	Created Date:		2015-01-07
//	Original Host:		http://dbParallel.codeplex.com
//	Primary Host:		http://DataBooster.codeplex.com
//	Change Log:
//	Author				Date			Comment
//
//
//
//
//	(Keep clean code rather than complicated code plus long comments.)
//
////////////////////////////////////////////////////////////////////////////////////////////////////