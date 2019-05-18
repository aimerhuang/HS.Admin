using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess;
using Hyt.DataAccess.Front;
using Hyt.Model.WorkflowStatus;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.Front
{
    /// <summary>
    /// 广告组据访问 抽象类
    /// </summary>
    /// <remarks>2013-06-14 苟治国 创建</remarks>
    public class FeAdvertGroupDaoImpl : IFeAdvertGroupDao
    {
        /// <summary>
        /// 查看广告组
        /// </summary>
        /// <param name="sysNo">广告组编号</param>
        /// <returns>广告组</returns>
        /// <remarks>2013－06-14 苟治国 创建</remarks>
        public override Model.FeAdvertGroup GetModel(int sysNo)
        {
            const string strSql = @"select * from FeAdvertGroup where sysNO = @sysNO";
            var result = Context.Sql(strSql)
                                .Parameter("SysNO", sysNo)
                                .QuerySingle<FeAdvertGroup>();
            return result;
        }

        /// <summary>
        /// 验证广告组名称
        /// </summary>
        /// <param name="key">广告组名称</param>
        /// <param name="sysNo">广告组编号</param>
        /// <returns>条数</returns>
        /// <remarks>2013－06-14 苟治国 创建</remarks>
        public override int FeAdvertGroupChk(string key, int sysNo)
        {
            int result;
            if (sysNo > 0)
            {
                string strSql = @"select count(1) from FeAdvertGroup where Name = @Name and SysNo!=@sysNO";
                result = Context.Sql(strSql)
                        .Parameter("Name", key)
                        .Parameter("sysNO", sysNo)
                        .QuerySingle<int>();
            }
            else
            {
                string strSql = @"select count(1) from FeAdvertGroup where Name = @Name";
                result = Context.Sql(strSql)
                    .Parameter("Name", key)
                    .QuerySingle<int>();
            }
            return result;
        }

        /// <summary>
        /// 根据条件获取广告组的列表
        /// </summary>
        /// <param name="pager">广告组查询条件</param>
        /// <returns>广告组列表</returns>
        /// <remarks>2013-06-17 苟治国 创建</remarks>
        public override Pager<Model.FeAdvertGroup> Seach(Pager<FeAdvertGroup> pager)
        {
            #region sql条件
            string sqlWhere = @"(@Status=-1 or Status =@Status)
                             and (@Type=-1 or Type =@Type)
                             and (@Name is null or Name like @Name1 or Code=@Name)
                             and (@PlatformType=-1 or PlatformType =@PlatformType)";
            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context.Select<FeAdvertGroup>("fe.*")
                                    .From("FeAdvertGroup fe")
                                    .Where(sqlWhere)
                                    .Parameter("Status", pager.PageFilter.Status)
                                    .Parameter("Type", pager.PageFilter.Type)
                                    .Parameter("Name", pager.PageFilter.Name)
                                    .Parameter("Name1", "%" + pager.PageFilter.Name + "%")
                                    .Parameter("PlatformType", pager.PageFilter.PlatformType)
                                    .Paging(pager.CurrentPage, pager.PageSize).OrderBy("DisplayOrder asc").QueryMany();

                pager.TotalRows = context.Select<int>("count(1)")
                                    .From("FeAdvertGroup fe")
                                    .Where(sqlWhere)
                                    .Parameter("Status", pager.PageFilter.Status)
                                    .Parameter("Type", pager.PageFilter.Type)
                                    .Parameter("Name", pager.PageFilter.Name)
                                    .Parameter("Name1", "%" + pager.PageFilter.Name + "%")
                                    .Parameter("PlatformType", pager.PageFilter.PlatformType)
                                    .QuerySingle();
            }
            return pager;
        }

        /// <summary>
        /// 根据条件获取广告组的总条数
        /// </summary>
        /// <param name="type">广告类型</param>
        /// <param name="platformType">广告组平台类型</param>
        /// <param name="status">广告状态</param>
        /// <param name="key">搜索关键字</param>
        /// <returns>总数</returns>
        /// <remarks>2013－06-17 苟治国 创建</remarks>
        public override int GetCount(int? type, int? platformType, int? status, string key = null)
        {
            #region sql条件
            string sqlWhere = @"(@Status is null or Status =@Status)
                                 and (@Type is null or Type =@Type)
                                 and (@Name is null or Name like @Name1)
                                 and (@PlatformType is null or PlatformType =@PlatformType)";
            #endregion

            var list = Context.Select<int>("count(1)")
                  .From("FeAdvertGroup")
                  .Where(sqlWhere)
                  .Parameter("Status", status)
                  .Parameter("Type", type)
                  .Parameter("Name", key)
                  .Parameter("Name1", "%" + key + "%")
                  .Parameter("PlatformType", platformType)
                  .QuerySingle();
            return list;
        }

        /// <summary>
        /// 新增广告组
        /// </summary>
        /// <param name="model">广告组明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-17 苟治国 创建</remarks>
        public override int Insert(Model.FeAdvertGroup model)
        {
            if (model.LastUpdateDate == DateTime.MinValue)
            {
                model.LastUpdateDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            var result = Context.Insert<FeAdvertGroup>("FeAdvertGroup", model)
                                .AutoMap(x => x.SysNo)
                                .ExecuteReturnLastId<int>("SysNo");
            return result;
        }

        /// <summary>
        /// 更新广告组
        /// </summary>
        /// <param name="model">广告组明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-17 苟治国 创建</remarks>
        public override int Update(Model.FeAdvertGroup model)
        {
            int rowsAffected = Context.Update<Model.FeAdvertGroup>("FeAdvertGroup", model)
                                      .AutoMap(x => x.SysNo)
                                      .Where(x => x.SysNo)
                                      .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 删除广告组
        /// </summary>
        /// <param name="sysNo">广告组编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-06-17 苟治国 创建</remarks>
        public override bool Delete(int sysNo)
        {
            int rowsAffected = Context.Delete("FeAdvertGroup")
                                      .Where("SYSNO", sysNo)
                                      .Execute();
            return rowsAffected > 0;
        }
        /// <summary>
        /// 添加店铺关联广告项
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ///<remarks>2016-07-28 周 创建</remarks>
        public override int InsertDealerFeAdvertItem(DsDealerFeAdvertItem model)
        {
            if (model.LastUpdateDate == DateTime.MinValue)
            {
                model.LastUpdateDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            var result = Context.Insert<DsDealerFeAdvertItem>("DsDealerFeAdvertItem", model)
                                .AutoMap(x => x.SysNo)
                                .ExecuteReturnLastId<int>("SysNo");
            return result;
        }
        /// <summary>
        /// 更新店铺关联广告项
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ///<remarks>2016-07-28 周 创建</remarks>
        public override int UpdateDealerFeAdvertItem(Model.DsDealerFeAdvertItem model)
        {
            int rowsAffected = Context.Update<Model.DsDealerFeAdvertItem>("DsDealerFeAdvertItem", model)
                                      .AutoMap(x => x.SysNo)
                                      .Where(x => x.SysNo)
                                      .Execute();
            return rowsAffected;
        }
        /// <summary>
        /// 获取店铺关联广告项表信息
        /// </summary>
        /// <param name="FeAdvertGroupSysNO"></param>
        /// <returns></returns>
        ///<remarks>2016-07-28 周 创建</remarks>
        public override DsDealerFeAdvertItem GetModelDealerFeAdvertItem(int FeAdvertGroupSysNO)
        {
            const string strSql = @"select * from DsDealerFeAdvertItem where FeAdvertItemSysNO = @FeAdvertItemSysNO";
            var result = Context.Sql(strSql)
                                .Parameter("FeAdvertItemSysNO", FeAdvertGroupSysNO)
                                .QuerySingle<DsDealerFeAdvertItem>();
            return result;
        }
        /// <summary>
        /// 删除店铺关联广告项表信息
        /// </summary>
        /// <param name="FeAdvertItemSysNO"></param>
        /// <returns></returns>
        public override bool DeleteDealerFeAdvertItem(int FeAdvertItemSysNO)
        {
            int rowsAffected = Context.Delete("DsDealerFeAdvertItem")
                                      .Where("FeAdvertItemSysNO", FeAdvertItemSysNO)
                                      .Execute();
            return rowsAffected > 0;
        }
        /// <summary>
        /// 是否存在店铺广告项
        /// </summary>
        /// <param name="FeAdvertItemSysNO"></param>
        /// <returns></returns>
        public override int IsExistenceDealerFeAdvertItem(int FeAdvertItemSysNO)
        {
            return Context.Sql("select count(SysNo) from DsDealerFeAdvertItem where FeAdvertItemSysNO=@0", FeAdvertItemSysNO).QuerySingle<int>();
        }
    }
}
