using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Web;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Web
{
    /// <summary>
    /// 前台广告
    /// </summary>
    /// <remarks>2013-08-06 黄波 创建</remarks>
    public class FeAdvertGroupDaoImpl : IFeAdvertItemDao
    {
        /// <summary>
        /// 获取广告内容
        /// </summary>
        /// <param name="platformType">广告平台类型</param>
        /// <param name="groupCode">组代码</param>
        /// <returns>广告项</returns>
        /// <remarks>2013-08-06 黄波 创建</remarks>
        /// <remarks>2016-07-18 杨云奕 修改</remarks>
        public override IList<Model.FeAdvertItem> GetAdvertItems(ForeStatus.广告组平台类型 platformType,string groupCode)
        {
            string sql = @"
                                  select
                                        * 
                                   from
                                        FeAdvertitem fi 
                                   where
                                        fi.GroupSysNo=(select fg.sysno from feadvertgroup fg where fg.code=@Code and fg.Status=@FGStatus and fg.PlatformType=@PlatformType)
                                    and
                                        fi.Status=@Status
                                    order by 
                                        fi.DisplayOrder desc
                                    ";
            return Context.Sql(sql)
                .Parameter("Code", groupCode)
                .Parameter("FGStatus", (int)ForeStatus.广告组状态.启用)
                .Parameter("PlatformType", (int)platformType)
                .Parameter("Status", (int)ForeStatus.广告项状态.已审)
                .QueryMany<Model.FeAdvertItem>();
        }
        /// <summary>
        ///  通过父di
        /// </summary>
        /// <param name="groupSysNo"></param>
        /// <returns></returns>
        public override IList<Model.FeAdvertItem> GetWebAdvertItemsByGroupSysNo(int groupSysNo)
        {
            string sql = @"
                                  select
                                        * 
                                   from
                                        FeAdvertitem fi 
                                   where
                                        fi.GroupSysNo=" + groupSysNo + @"
                                    and
                                        fi.Status=20
                                    order by 
                                        fi.DisplayOrder desc
                                    ";
            return Context.Sql(sql).QueryMany<Model.FeAdvertItem>();
        }
    }
}
