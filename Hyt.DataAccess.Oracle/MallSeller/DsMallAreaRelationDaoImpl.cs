using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.MallSeller;
using System.Collections;
using Hyt.Model.Transfer;
using Hyt.Model;


namespace Hyt.DataAccess.Oracle.MallSeller
{
    /// <summary>
    /// 分销商城地区关联数据库操作类
    /// </summary>
    /// <remarks>2014-10-14 缪竞华 创建</remarks>
    public class DsMallAreaRelationDaoImpl : IDsMallAreaRelationDao
    {
        /// <summary>
        /// 查询商城地区关联
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>商城地区关联分页数据</returns>
        /// <remarks>2014-10-14 缪竞华 创建</remarks>
        public override Model.Pager<Model.Transfer.CBDsMallAreaRelation> Query(Model.Parameter.ParaDsMallAreaRelationFilter filter)
        {
            const string sql = @"(SELECT a.*,b.DealerName,c.MallName,d.AreaName from DsMallAreaAssociation a
                                    LEFT JOIN DsDealer b on a.DealerSysNo = b.SysNo
                                    LEFT JOIN DsMallType c on a.malltypesysno = c.sysno
                                    LEFT JOIN BsArea d on a.hytareasysno = d.sysno                               
                            WHERE
                                    (@0 is null or charindex(lower(b.DealerName),lower(@0))>0) OR --分销商名称
                                    (@1 is null or charindex(lower(c.MallName),lower(@1))>0) OR --商城名称
                                    (@2 is null or charindex(lower(a.MallAreaName),lower(@2))>0) OR --商城地区名称
                                    (@3 is null or charindex(lower(d.AreaName),lower(@3))>0) --商城地区名称                        
                           ) tb
                        ";

            var paras = new object[]
            {
                filter.DealerName,
                filter.MallName,
                filter.MallAreaName,
                filter.AreaName
            };

            var dataList = Context.Select<CBDsMallAreaRelation>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);
            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CBDsMallAreaRelation>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id,
                TotalRows = dataCount.QuerySingle(),
                Rows = dataList.OrderBy("tb.sysNo desc").Paging(filter.Id, filter.PageSize).QueryMany()
            };

            return pager;
        }

        /// <summary>
        /// 根据sysNo删除DsMallAreaAssociation表中的数据
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>受影响行数</returns>
        /// <remarks>2014-10-14 缪竞华 创建</remarks>
        public override int Delete(int sysNo)
        {
            return Context.Delete("DsMallAreaAssociation")
                    .Where("SysNo", sysNo)
                    .Execute();
        }
    }
}
