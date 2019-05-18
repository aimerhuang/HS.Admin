using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Logistics;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.Logistics
{
    /// <summary>
    /// 百城当日达区域信息数据访问类
    /// </summary>
    /// <remarks>
    /// 2013-08-01 郑荣华 创建
    /// </remarks>
    public class LgDeliveryScopeDaoImpl : ILgDeliveryScopeDao
    {
        #region 操作

        /// <summary>
        /// 创建百城当日达区域信息
        /// </summary>
        /// <param name="model">百城当日达区域信息实体</param>
        /// <returns>创建的百城当日达区域信息sysNo</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public override int Create(LgDeliveryScope model)
        {
            return Context.Insert("LgDeliveryScope", model)
                          .AutoMap(x => x.SysNo)
                          .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 更新百城当日达区域信息
        /// </summary>
        /// <param name="model">百城当日达区域信息实体，根据sysno</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public override int Update(LgDeliveryScope model)
        {
            return Context.Update("LgDeliveryScope", model)
                          .AutoMap(x => x.SysNo, x => x.CreatedBy, x => x.CreatedDate)
                          .Where(x => x.SysNo)
                          .Execute();
        }

        /// <summary>
        /// 删除百城当日达区域信息
        /// </summary>
        /// <param name="sysNo">要删除的百城当日达区域信息系统编号</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public override int Delete(int sysNo)
        {
            return Context.Delete("LgDeliveryScope")
                          .Where("SysNo", sysNo)
                          .Execute();
        }

        /// <summary>
        /// 根据城市系统编号删除百城当日达区域信息
        /// </summary>
        /// <param name="areaSysNo">要删除的城市的系统编号</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public override int DeleteByAreaSysNo(int areaSysNo)
        {
            return Context.Delete("LgDeliveryScope")
                          .Where("AreaSysNo", areaSysNo)
                          .Execute();
        }

        #endregion

        #region 查询

        /// <summary>
        /// 根据城市编号获取百城当日达区域信息
        /// </summary>
        /// <param name="areaSysNo">城市sysno</param>
        /// <returns>百城当日达区域信息列表</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public override IList<LgDeliveryScope> GetDeliveryScope(int areaSysNo)
        {
            const string sql = "select * from LgDeliveryScope where areasysno=@0";

            return Context.Sql(sql, areaSysNo)
                          .QueryMany<LgDeliveryScope>();
        }



        /// <summary>
        /// 根据城市编号获取仓库信息，用于百度地图显示
        /// </summary>
        /// <param name="areaSysNo">城市sysno</param>
        /// <returns>百城当日达区域信息列表</returns>
        /// <remarks> 
        /// 2015-08-06 LYK 创建
        /// </remarks>
        public override IList<WhWarehouse> GetWhWarehouseDiTu(int areaSysNo)
        {
            const string sql = @"with cte as  (  
               select B.SysNo,B.ParentSysNo,B.AreaName from BsArea B where ParentSysNo=@0  
               union all   
               select k.SysNo,k.ParentSysNo,k.AreaName  from BsArea k inner join cte c on c.SysNo = k.ParentSysNo 
               )select WH.WarehouseName,WH.Latitude,WH.Longitude,WH.ImgUrl,WH.Phone,WH.Contact,cte.AreaName as CityName,WH.StreetAddress from cte inner join WhWarehouse WH on  cte.SysNo=WH.AreaSysNo and WH.Status=1";

            //无需递归查询实现同样效果  2015-08-07 LYK 修改
//            const string sql = @"SELECT WH.WarehouseName,WH.Latitude,WH.Longitude,WH.ImgUrl,WH.Phone,WH.Contact,B.AreaName AS CityName,WH.StreetAddress 
//                               FROM BsArea B INNER JOIN WhWarehouse WH ON  B.SysNo=WH.AreaSysNo where ParentSysNo=@0";

            return Context.Sql(sql, areaSysNo)
                          .QueryMany<WhWarehouse>();
        }


        /// <summary>
        /// 根据系统编号获取百城当日达区域信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>实体</returns>
        /// <remarks>
        /// 2014-05-14 朱家宏 创建
        /// </remarks>
        public override LgDeliveryScope GetDeliveryScopeBySysNo(int sysNo)
        {
            return Context.Sql(@"select * from LgDeliveryScope  where sysno=@0  ", sysNo).QuerySingle<LgDeliveryScope>();
        }

        #endregion
    }
}
