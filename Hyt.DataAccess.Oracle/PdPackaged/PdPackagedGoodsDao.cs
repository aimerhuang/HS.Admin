using Hyt.DataAccess.PdPackaged;
using Hyt.Model;
using Hyt.Model.PdPackaged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.PdPackaged
{
    /// <summary>
    /// 商品套装
    /// </summary>
    public class PdPackagedGoodsDao : IPdPackagedGoodsDao
    {

        /// <summary>
        /// 分页获取套装商品
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="GetType">是否获取商品明细 1是 0否</param>
        /// <returns></returns>
        /// 2017-8-25 吴琨 创建
        public override Pager<PdPackagedGoods> GetPageList(Pager<PdPackagedGoods> pager,int? GetType=0)
        {
            var strSql = @" PdPackagedGoods ";
            var whereStr = "( @0 is null or Status=@0) and (@1 is null or Code=@1) ";
            var paras = new object[]
                {
                    pager.PageFilter.Status==-1?null:pager.PageFilter.Status,
                    string.IsNullOrEmpty(pager.PageFilter.Code)?null:pager.PageFilter.Code,
                };
            List<PdPackagedGoods> list = Context.Select<PdPackagedGoods>("*").From(strSql).Where(whereStr).Parameters(paras).OrderBy("SysNo desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();
            #region 获取套装商品详情
            if (GetType == 1)
            {
                foreach (var item in list)
                {
                    list[list.IndexOf(item)].PdList= Context.Sql("select * from PdPackagedGoodsEntry where PdPackagedGoodsSysNo=@PdPackagedGoodsSysNo").Parameter("PdPackagedGoodsSysNo", item.SysNo).QueryMany<PdPackagedGoodsEntry>();
                }
            }
            #endregion
            pager.Rows = list;
            pager.TotalRows = Context.Select<int>("count(0)").From(strSql).Where(whereStr).Parameters(paras).QuerySingle();
            return pager;
        }



        /// <summary>
        /// 获取套装商品
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="GetType">是否获取商品明细 1是 0否</param>
        /// <returns></returns>
        /// 2017-8-25 吴琨 创建
        public override PdPackagedGoods GetPageModels(int SysNo, int? GetType = 0)
        {
            var strSql = @" PdPackagedGoods ";
            var whereStr = " SysNo=@SysNo ";
            var models = Context.Select<PdPackagedGoods>("*").From(strSql).Where(whereStr).Parameter("SysNo", SysNo).QuerySingle();
            #region 获取套装商品详情
            if (GetType == 1 && models != null)
            {
                models.PdList = Context.Sql("select * from PdPackagedGoodsEntry where PdPackagedGoodsSysNo=@PdPackagedGoodsSysNo").Parameter("PdPackagedGoodsSysNo",SysNo).QueryMany<PdPackagedGoodsEntry>();
            }
            #endregion
            return models;
        }


        /// <summary>
        /// 创建套装商品
        /// </summary>
        /// <param name="model">套装商品表</param>
        /// <param name="listModel">套装商品商品明细表</param>
        /// <returns></returns>
        /// 2017-8-25 吴琨 创建
        public override bool Add(PdPackagedGoods model, List<PdPackagedGoodsEntry> listModel)
        {
            int sysNo = 0;
            using (var context = Context.UseTransaction(true))
            {
                try
                {
                    sysNo = Context.Insert("PdPackagedGoods", model).AutoMap(x => x.SysNo,x=>x.PdList).ExecuteReturnLastId<int>("SysNo");
                    if (sysNo > 0)
                    {
                        foreach (var item in listModel)
                        {
                            item.PdPackagedGoodsSysNo = sysNo;
                            Context.Insert("PdPackagedGoodsEntry", item).AutoMap(x => x.SysNo, x => x.WarehouseCode).ExecuteReturnLastId<int>("SysNo");
                        }
                    }
                    context.Commit();
                }
                catch (Exception e)
                {
                    //回滚
                    string Sql = string.Format("delete PdPackagedGoods where SysNo = {0}", sysNo);
                    Context.Sql(Sql).Execute();
                    sysNo = 0;
                    context.Rollback();
                }
            }
            return sysNo>0;
        }


        /// <summary>
        /// 更改套装商品状态
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <param name="status">状态值</param>
        /// <returns></returns>
        /// 吴琨 创建
        public override bool UpdateStatus(int sysNo, int status, int Auditor, string AuditorName)
        {
          return  (Context.Update("PdPackagedGoods")
            .Column("Status", status)
            .Column("Auditor", Auditor)
            .Column("AuditorName", AuditorName)
            .Column("AuditorDate",DateTime.Now)
            .Where("SysNo", sysNo)
            .Execute())>0;
        }


        
        /// <summary>
        /// 查询目前最大Id号,用于生成单据编号
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <param name="status">状态值</param>
        /// <returns></returns>
        /// 吴琨 2017/8/29 创建
        public override int GetModelSysNo()
        {
            string sqlstr = "select SysNo from PdPackagedGoods order by  SysNo desc ";
            return Context.Sql(sqlstr).QuerySingle<int>();
        }



    }
}
