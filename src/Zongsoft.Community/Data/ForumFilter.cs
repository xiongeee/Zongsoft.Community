﻿/*
 * Authors:
 *   钟峰(Popeye Zhong) <9555843@qq.com>
 * 
 * Copyright (C) 2015-2017 Zongsoft Corporation. All rights reserved.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Linq;
using System.Collections;

using Zongsoft.Data;
using Zongsoft.Community.Models;

namespace Zongsoft.Community.Data
{
	public class ForumFilter : DataAccessFilterBase
	{
		#region 构造函数
		public ForumFilter() : base(DataAccessModule.COMMUNITY_FORUM)
		{
		}
		#endregion

		#region 重写方法
		protected override void OnSelected(DataSelectionContext context)
		{
			//调用基类同名方法
			base.OnSelected(context);

			//设置查询结果的过滤器
			context.ResultFilter = (ctx, item) =>
			{
				var forum = item as Forum;

				if(forum == null)
					return false;

				if(forum.Visiblity == Visiblity.Specifics)
				{
					var credential = ctx.Principal?.Identity?.Credential;

					if(credential == null || credential.IsEmpty)
						return false;

					return context.DataAccess.Exists<Forum.ForumUser>(
						Condition.Equal("SiteId", forum.SiteId) &
						Condition.Equal("ForumId", forum.ForumId) &
						Condition.Equal("UserId", credential.UserId));
				}

				return true;
			};
		}
		#endregion
	}
}
