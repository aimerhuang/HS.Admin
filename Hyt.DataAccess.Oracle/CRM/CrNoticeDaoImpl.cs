using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.CRM;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.CRM
{
    /// <summary>
    /// 商品信息通知业务实现类
    /// </summary>
    /// <remarks>2013-08-09 杨晗 创建</remarks>
    public class CrNoticeDaoImpl : ICrNoticeDao
    {
        /// <summary>
        /// 根据商品信息通知编号获取实体
        /// </summary>
        /// <param name="sysNo">商品信息通知编号</param>
        /// <returns>商品信息通知实体</returns>
        /// <remarks>2013-08-09 杨晗 创建</remarks>
        public override CrNotice GetModel(int sysNo)
        {
            return Context.Sql(@"select * from CrNotice where SysNO = @0", sysNo)
                          .QuerySingle<CrNotice>();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex">起始页</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="customerSysNo">商品信息通知类型</param>
        /// <param name="type">商品信息通知类型</param>
        /// <param name="count">抛出总条数</param>
        /// <returns>商品信息通知列表</returns>
        /// <remarks>2013-08-09 杨晗 创建</remarks>
        public override IList<CBCrNotice> Seach(int pageIndex, int pageSize, int customerSysNo, CustomerStatus.通知类型 type,
                                              out int count)
        {
            #region sql条件

            string sql = @"cn.NoticeType =@NoticeType and cn.CustomerSysNo=@CustomerSysNo";

            #endregion

            count = Context.Select<int>("count(1)")
                           .From("CrNotice cn")
                           .Where(sql)
                           .Parameter("NoticeType", (int) type)
                           .Parameter("CustomerSysNo", customerSysNo)
                           .QuerySingle();

            var countBuilder = Context.Select<CBCrNotice>("cn.*,pd.ProductName as ProductName,pd.Status as ProductStatus")
                                      .From("CrNotice cn,PdProduct pd")
                                      .Where("pd.sysno=cn.ProductSysNo and "+sql)
                                      .Parameter("NoticeType", (int) type)
                                      .Parameter("CustomerSysNo", customerSysNo)
                                      .Paging(pageIndex, pageSize).OrderBy("cn.CreatedDate desc").QueryMany();
            return countBuilder;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-08-09 杨晗 创建</remarks>
        public override int Insert(CrNotice model)
        {
            return Context.Insert<CrNotice>("CrNotice", model)
                          .AutoMap(x => x.SysNo)
                          .ExecuteReturnLastId<int>("Sysno");
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-08-09 杨晗 创建</remarks>
        public override int Update(CrNotice model)
        {
            return Context.Update<CrNotice>("CrNotice", model)
                          .AutoMap(x => x.SysNo)
                          .Where(x => x.SysNo)
                          .Execute();
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="sysNoItems">ID组</param>
        /// <returns></returns>
        public override int Update(string sysNoItems)
        {
            return Context.Sql("update CrNotice set NoticeWay=20 where sysNo in(" + sysNoItems + ")")
                   //.Parameter("SysNo", sysNoItems)
                   .Execute();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sysNo">商品信息通知编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-08-09 杨晗 创建</remarks>
        public override bool Delete(int sysNo)
        {
            var rowsAffected = Context.Delete("CrNotice")
                                      .Where("Sysno", sysNo)
                                      .Execute();
            return rowsAffected > 0;
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="CustomerSysNo">申请人</param>
        /// <param name="ProductSysNo">申请商品ID</param>
        /// <param name="type">通知方式</param>
        /// <param name="NoticeWay">发送方式</param>
        /// <param name="pager">分页对象</param>
        /// <returns>2016-03-30 周海鹏 创建</returns>
        public override IList<CBCrNotice> List(int CustomerSysNo, int ProductSysNo, CustomerStatus.通知类型 type, int NoticeWay, Pager<CBCrNotice> pager)
        {
            using (var content = Context.UseSharedConnection(true))
            {
                string sql = "";
                if (CustomerSysNo != 0)
                    sql += string.Format("and CustomerSysNo={0}", CustomerSysNo);
                if (ProductSysNo!=0)
                    sql += string.Format("and ProductSysNo={0}", ProductSysNo);
                if (NoticeWay != 0)
                    sql += string.Format("and NoticeWay={0}", NoticeWay);

                pager.Rows =
                    content.Select<CBCrNotice>("cr.*")
                           .From("CrNotice cr")
                           .Where("NoticeType=@NoticeType " + sql + "")
                           .Parameter("NoticeType", (int)type)
                           .OrderBy("cr.CreatedDate desc")
                           .Paging(pager.CurrentPage, pager.PageSize)
                           .QueryMany();

                pager.TotalRows =
                    content.Sql(@"select count(cr.sysno) from CrNotice cr where NoticeType=@NoticeType " + sql + "")
                           .Parameter("NoticeType", (int)type)
                           .QuerySingle<int>();
            }

            return pager.Rows;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override CBCrNotice Get()
        {
            return Context.Select<CBCrNotice>("cr.*").From("CrNotice cr").OrderBy("cr.CreatedDate desc").QuerySingle();
        }
    }
}
