﻿using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DbParallel.DataAccess
{
	[XmlRoot(Namespace = ""), XmlSchemaProvider("GetSchema")]
	public class StoredProcedureResponse : IXmlSerializable
	{
		#region Original Data Members
		public IList<IList<BindableDynamicObject>> ResultSets { get; set; }
		public BindableDynamicObject OutputParameters { get; set; }
		public object ReturnValue { get; set; }
		#endregion

		public StoredProcedureResponse(BindableDynamicObject.XmlSettings xmlSettings = null)
		{
			ResultSets = new List<IList<BindableDynamicObject>>();
			_xmlSettings = xmlSettings;
		}

		#region Xml Serialization Decoration

		private static readonly XmlQualifiedName _TypeName = new XmlQualifiedName(typeof(StoredProcedureResponse).Name, "");
		public static XmlQualifiedName GetSchema(XmlSchemaSet schemas)
		{
			return _TypeName;
		}

		[DataContract(Namespace = "")]
		private class XStoredProcedureResponse
		{
			[CollectionDataContract(Name = "ResultSets", ItemName = "ResultSet", Namespace = "")]
			internal class XResultSets : List<IList<BindableDynamicObject>>
			{
				internal XResultSets() : base() { }
				internal XResultSets(IList<IList<BindableDynamicObject>> resultSets) : base(resultSets) { }
			}

			[XmlRoot(Namespace = "")]
			internal class XValue : IXmlSerializable
			{
				private object _Value;
				private readonly BindableDynamicObject.XmlSettings.DataSchemaType _EmitDataSchemaType;

				internal object Value
				{
					get { return _Value; }
					set { _Value = value; }
				}

				internal XValue(object simpleValue = null, BindableDynamicObject.XmlSettings.DataSchemaType emitDataSchemaType = BindableDynamicObject.XmlSettings.DataSchemaType.None)
				{
					_Value = simpleValue;
					_EmitDataSchemaType = emitDataSchemaType;
				}

				XmlSchema IXmlSerializable.GetSchema()
				{
					return null;
				}

				void IXmlSerializable.ReadXml(XmlReader reader)
				{
					throw new NotImplementedException();
				}

				void IXmlSerializable.WriteXml(XmlWriter writer)
				{
					if (_Value != null)
						writer.WriteValueWithType(_Value, _EmitDataSchemaType);
				}
			}

			private readonly StoredProcedureResponse _OriginalResponse;
			private XResultSets _ResultSets;
			private XValue _ReturnValue;

			[DataMember(Order = 1)]
			internal XResultSets ResultSets
			{
				get { return _ResultSets; }
				set { if (value != _ResultSets) _OriginalResponse.ResultSets = _ResultSets = value; }
			}

			[DataMember(Order = 2)]
			internal BindableDynamicObject OutputParameters
			{
				get { return _OriginalResponse.OutputParameters; }
				set { _OriginalResponse.OutputParameters = value; }
			}

			[DataMember(Order = 3)]
			internal XValue ReturnValue
			{
				get
				{
					return _ReturnValue;
				}
				set
				{
					_ReturnValue = value;
					_OriginalResponse.ReturnValue = _ReturnValue.Value;
				}
			}

			internal XStoredProcedureResponse(StoredProcedureResponse spResponse = null, BindableDynamicObject.XmlSettings xmlSettings = null)
			{
				_OriginalResponse = spResponse ?? new StoredProcedureResponse(xmlSettings);

				_ResultSets = new XResultSets(_OriginalResponse.ResultSets);

				if (_OriginalResponse.ReturnValue != null)
					_ReturnValue = (xmlSettings == null) ? new XValue(_OriginalResponse.ReturnValue) : new XValue(_OriginalResponse.ReturnValue, xmlSettings.EmitDataSchemaType);
			}

			internal StoredProcedureResponse GetOriginalResponse()
			{
				return _OriginalResponse;
			}
		}

		private readonly BindableDynamicObject.XmlSettings _xmlSettings;

		#region IXmlSerializable Members

		XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			throw new NotImplementedException();
		}

		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			if (_xmlSettings != null)
				writer.PrepareTypeNamespaceRoot(_xmlSettings.EmitDataSchemaType);

			DataContractSerializer serializer = new DataContractSerializer(typeof(XStoredProcedureResponse));
			XStoredProcedureResponse responseXml = new XStoredProcedureResponse(this, _xmlSettings);
			serializer.WriteObjectContent(writer, responseXml);
		}

		#endregion
		#endregion
	}
}

////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	Copyright 2014 Abel Cheng
//	This source code is subject to terms and conditions of the Apache License, Version 2.0.
//	See http://www.apache.org/licenses/LICENSE-2.0.
//	All other rights reserved.
//	You must not remove this notice, or any other, from this software.
//
//	Original Author:	Abel Cheng <abelcys@gmail.com>
//	Created Date:		2014-12-19
//	Original Host:		http://dbParallel.codeplex.com
//	Primary Host:		http://DataBooster.codeplex.com
//	Change Log:
//	Author				Date			Comment
//
//
//
//
//	(Keep code clean rather than complicated code plus long comments.)
//
////////////////////////////////////////////////////////////////////////////////////////////////////
