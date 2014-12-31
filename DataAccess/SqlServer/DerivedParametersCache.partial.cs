﻿using System.Data.Common;
using System.Data.SqlClient;

namespace DbParallel.DataAccess
{
	internal static partial class DerivedParametersCache
	{
		static partial void SqlDeriveParameters(DbCommand dbCmd)
		{
			SqlCommand cmd = dbCmd as SqlCommand;

			if (cmd != null)
				SqlCommandBuilder.DeriveParameters(cmd);
		}
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
//	Created Date:		2014-12-23
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