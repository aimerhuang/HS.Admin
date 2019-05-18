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
    /// 产品项数据访问 抽象类
    /// </summary>
    /// <remarks>2013-06-21 苟治国 创建</remarks>
    public class FeProductItemDaoImpl : IFeProductItemDao
    {
        /// <summary>
        /// 查看产品项
        /// </summary>
        /// <param name="sysNo">产品项编号</param>
        /// <returns>产品项</returns>
        /// <remarks>2013－06-21 苟治国 创建</remarks>
        public override Model.FeProductItem GetModel(int sysNo)
        {
            return Context.Sql(@"select * from FeProductItem where SysNO = @0", sysNo).QuerySingle<Model.FeProductItem>();
        }

        /// <summary>
        /// 根据条件获取商品项的列表
        /// </summary>
        /// <param name="pager">商品项查询条件</param>
        /// <returns>商品项列表</returns>
        /// <remarks>2013-10-11 苟治国 创建</remarks>
        /// <remarks>2013-12-31 邵  斌 修改，添加返回商品后台显示名称</remarks>
        public override Pager<Model.CBFeProductItem> Seach(Pager<CBFeProductItem> pager)
        {
            #region sql条件
            var sqlWhere = "1=1";

            //判断是否绑定所有分销商
            if (!pager.PageFilter.IsBindAllDealer)
            {
                //判断是否绑定分销商
                if (pager.PageFilter.IsBindDealer)
                {
                    sqlWhere += " and d.SysNo = @DealerSysNo";
                }
                else
                {
                    sqlWhere += " and d.CreatedBy = @DealerCreatedBy";
                }
            }
            if (pager.PageFilter.SelectedDealerSysNo != -1)
            {
                sqlWhere += " and d.SysNo = " + pager.PageFilter.SelectedDealerSysNo;
            }
            sqlWhere += @" and (@GroupSysNo is null or fp.GroupSysNo =@GroupSysNo) 
                           and (@Status=-1 or fp.Status =@Status)
                           and (@ProductName is null or pp.EasName like @ProductName1)
                           and (@ErpCode is null or pp.ErpCode like @ErpCode1)
";
            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context.Select<CBFeProductItem>("fp.*,pp.ProductName,pp.EasName as EasName,pp.ErpCode,d.DealerName")
                              .From(@"FeProductItem fp left join PdProduct pp on fp.ProductSysNo=pp.sysno
                                    left join DsDealer d on fp.DealerSysNo = d.SysNo")
                              .Where(sqlWhere)
                              .Parameter("DealerSysNo", pager.PageFilter.DealerSysNo)
                              .Parameter("DealerCreatedBy", pager.PageFilter.DealerCreatedBy)
                              .Parameter("GroupSysNo", pager.PageFilter.GroupSysNo)
                              .Parameter("Status", pager.PageFilter.Status)
                              .Parameter("ProductName", pager.PageFilter.ProductName)
                              .Parameter("ProductName1", "%" + pager.PageFilter.ProductName + "%")
                              .Parameter("ErpCode", pager.PageFilter.ErpCode)
                              .Parameter("ErpCode1", "%" + pager.PageFilter.ErpCode + "%")
                              .Paging(pager.CurrentPage, pager.PageSize).OrderBy("fp.DisplayOrder desc,fp.LastUpdateDate desc").QueryMany();

                pager.TotalRows = context.Select<int>("count(1)")
                              .From(@"FeProductItem fp left join pdproduct pp on fp.productsysno=pp.sysno
                                   left join DsDealer d on fp.DealerSysNo = d.SysNo")
                              .Where(sqlWhere)
                              .Parameter("DealerSysNo", pager.PageFilter.DealerSysNo)
                              .Parameter("DealerCreatedBy", pager.PageFilter.DealerCreatedBy)
                              .Parameter("groupsysNo", pager.PageFilter.GroupSysNo)
                              .Parameter("Status", pager.PageFilter.Status)
                              .Parameter("ProductName", pager.PageFilter.ProductName)
                              .Parameter("ProductName1", "%" + pager.PageFilter.ProductName + "%")
                              .Parameter("ErpCode", pager.PageFilter.ErpCode)
                              .Parameter("ErpCode1", "%" + pager.PageFilter.ErpCode + "%")
                              .QuerySingle();
            }
            return pager;
        }

        /// <summary>
        /// 根据商品组获取所有商品项分类
        /// </summary>
        /// <param name="groupSysNo">商品组编号</param>
        /// <returns>商品项列表</returns>
        /// <remarks>2013－06-21 苟治国 创建</remarks>
        public override IList<FeProductItem> GetListByGroup(int groupSysNo, int dealersysno)
        {
            #region sql条件
            string sql = @"(@GroupSysNo is null or fp.GroupSysNo=@GroupSysNo) and fp.DealerSysNo=@DealerSysNo";
            #endregion

            var list = Context.Select<FeProductItem>("fp.*")
                                      .From("FeProductItem fp")
                                      .Where(sql)
                                      .Parameter("GroupSysNo", groupSysNo)
                                      .Parameter("DealerSysNo", dealersysno)
                                      .OrderBy("fp.DisplayOrder").QueryMany();
            return list;
        }

        /// <summary>
        /// 查看在当前类型中是否有相同产品称
        /// </summary>
        /// <param name="mid">产品组编号</param>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>总数</returns>
        /// <remarks>2013-06-21 苟治国 创建</remarks>
        public override int GetCount(int mid, int productSysNo)
        {
            #region sql条件
            string sqlWhere = @"(@groupsysNo is null or fp.groupsysNo =@groupsysNo) and (@ProductSysNo is null or fp.ProductSysNo like @ProductSysNo)";
            #endregion

            var single = Context.Select<int>("count(1)")
                              .From("FeProductItem fp")
                              .Where(sqlWhere)
                              .Parameter("groupsysNo", mid)
                              .Parameter("ProductSysNo", productSysNo)
                              .QuerySingle();
            return single;

        }

        /// <summary>
        /// 新增产品项
        /// </summary>
        /// <param name="model">产品项明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-21 苟治国 创建</remarks>
        public override int Insert(Model.FeProductItem model)
        {
            var result = Context.Insert<FeProductItem>("FeProductItem", model)
                                .AutoMap(x => x.SysNo)
                                .ExecuteReturnLastId<int>("SysNo");
            return result;
        }

        /// <summary>
        /// 商品更新
        /// </summary>
        /// <param name="model">产品项明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-21 苟治国 创建</remarks>
        public override int Update(Model.FeProductItem model)
        {
            int rowsAffected = Context.Update<Model.FeProductItem>("FeProductItem", model)
                                      .AutoMap(x => x.SysNo)
                                      .Where(x => x.SysNo)
                                      .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 删除产品项
        /// </summary>
        /// <param name="sysNo">商品项编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-06-21 苟治国 创建</remarks>
        public override bool Delete(int sysNo)
        {
            int rowsAffected = Context.Delete("FeProductItem")
                                      .Where("sysNo", sysNo)
                                      .Execute();
            return rowsAffected > 0;
        }
        /// <summary>
        /// 同步总部已审核的商品项
        /// </summary>
        /// <param name="GroupSysNo"></param>
        /// <param name="DealerSysNo"></param>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        /// <remarks>2016-1-13 王耀发 创建</remarks>
        public override int ProCreateFeProductItem(int GroupSysNo, int DealerSysNo, int CreatedBy)
        {
            string Sql = string.Format("pro_CreateFeProductItem {0},{1},{2}", GroupSysNo, DealerSysNo, CreatedBy);
            int rowsAffected = Context.Sql(Sql).Execute();
            return rowsAffected;
        }
    }
}
