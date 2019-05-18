using Hyt.DataAccess.Weixin;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Oracle.Weixin
{
    /// <summary>
    /// 微信自定义菜单
    /// </summary>
    /// <remarks>2016-1-9 杨浩 创建</remarks>
    public class MkCustomizeMenuDaoImpl : IMkCustomizeMenuDao
    {
        /// <summary>
        /// 获取全部分销商菜单
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-1-9 杨浩 创建</remarks>
        public override IList<MkCustomizeMenu> GetAllMkCustomizeMenuList()
        {
            return Context.Sql("SELECT * FROM MkCustomizeMenu").QueryMany<MkCustomizeMenu>();
        }
        /// <summary>
        /// 根据分销商获取分销商菜单
        /// </summary>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <returns></returns>
        /// <remarks>2016-1-9 杨浩 创建</remarks>
        public override IList<MkCustomizeMenu> GetAllMkCustomizeMenuList(int dealerSysNo)
        {
            return Context.Sql(string.Format("SELECT * FROM MkCustomizeMenu WHERE DealerSysNo={0}",dealerSysNo))
                .QueryMany<MkCustomizeMenu>();
        }
        /// <summary>
        /// 删除分销商的菜单
        /// </summary>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <returns></returns>
        public override int DeleteMkCustomizeMenuByDealerSysNo(int dealerSysNo)
        {
            return Context.Sql(string.Format("DELETE FROM MkCustomizeMenu WHERE DealerSysNo={0}", dealerSysNo))
                .Execute();
        }
        /// <summary>
        /// 删除分销商的菜单
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        public override int DeleteMkCustomizeMenu(int sysNo)
        {
            return Context.Sql(string.Format("DELETE FROM MkCustomizeMenu WHERE SysNo={0}", sysNo))
              .Execute();
        }
        /// <summary>
        /// 获取分销商微信菜单
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public override MkCustomizeMenu GetEntity(int SysNo)
        {

            return Context.Sql("select a.* from MkCustomizeMenu a where a.SysNo=@SysNo")
                   .Parameter("SysNo", SysNo)
              .QuerySingle<MkCustomizeMenu>();
        }
        /// <summary>
        /// 添加分销商微信菜单
        /// </summary>
        /// <param name="customizeMenu">微信菜单实体</param>
        /// <returns></returns>
        public override int AddMkCustomizeMenu(MkCustomizeMenu customizeMenu)
        {
            return Context.Insert<MkCustomizeMenu>("MkCustomizeMenu",customizeMenu)
             .AutoMap(x => x.SysNo)
             .ExecuteReturnLastId<int>();
             
        }
        /// <summary>
        /// 更新分销商微信公众号菜单
        /// </summary>
        /// <param name="customizeMenu">微信菜单实体</param>
        /// <returns></returns>
        public override int UpdateMkCustomizeMenu(MkCustomizeMenu customizeMenu)
        {          
            int rowsAffected = Context.Update<MkCustomizeMenu>("MkCustomizeMenu", customizeMenu)
                                      .AutoMap(x => x.SysNo)
                                      .Where(x => x.SysNo)
                                      .Execute();
            return rowsAffected;
        }
        /// <summary>
        /// 获取子菜单总数
        /// </summary>
        /// <param name="pid">父级编号</param>
        /// <returns></returns>
        public override int GetMkCustomizeMenuChildCount(int pid)
        {
            return Context.Sql(string.Format("SELECT count(1) FROM MkCustomizeMenu WHERE pid={0}", pid))
              .QuerySingle<int>();
        }

        /// <summary>
        /// 获取分销商对应菜单总数
        /// </summary>
        /// <param name="pid">父级编号</param>
        /// <param name="dealerSysNo">经销商编号</param>
        /// <returns></returns>
        /// <remarks>2016-1-11 王耀发 创建</remarks>
        public override int GetMkCustomizeMenuCountInDealerParent(int pid, int dealerSysNo)
        {
            return Context.Sql(string.Format("SELECT count(1) FROM MkCustomizeMenu WHERE pid={0} and DealerSysNo = {1}", pid, dealerSysNo))
              .QuerySingle<int>();
        }

        /// <summary>
        /// 获取子菜单
        /// </summary>
        /// <param name="pid">父级编号</param>
        /// <returns></returns>
        public override IList<MkCustomizeMenu> GetMkCustomizeMenuChilds(int pid)
        {
            return Context.Sql(string.Format("SELECT * FROM MkCustomizeMenu WHERE pid={0}", pid))
             .QueryMany<MkCustomizeMenu>();
        }


        /// <summary>
        /// 获取对应微信菜单(父级)
        /// 2016-1-11 王耀发 创建
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public override Pager<CBMkCustomizeMenu> GetMkCustomizeMenuList(ParaMkCustomizeMenuFilter filter)
        {
            string sqlWhere = "1=1";
            //判断是否绑定所有分销商
            if (!filter.IsBindAllDealer)
            {
                //判断是否绑定分销商
                if (filter.IsBindDealer)
                {
                    sqlWhere += " and d.SysNo = @3";
                }
                else
                {
                    sqlWhere += " and d.CreatedBy = @4";
                }
            }
            if (filter.SelectedDealerSysNo != -1)
            {
                sqlWhere += " and d.SysNo = @5";
            }
            string sql = @"(select a.*
                                    ,b.Name As PName
	                                ,d.DealerName
                                from MkCustomizeMenu a
                                left join MkCustomizeMenu b on a.Pid = b.SysNo
                                left join DsDealer d on a.DealerSysNo = d.SysNo
                                where a.Pid = @0
                                and (@1 is null or a.Type = @1)
	                            and (@2 is null or charindex(a.Name,@2) > 0)
                                and " + sqlWhere + ") tb";

            var dataList = Context.Select<CBMkCustomizeMenu>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);
            var paras = new object[]
                {
                   filter.Pid,
                   filter.Type,
                   filter.Name,
                   filter.DealerSysNo,
                   filter.DealerCreatedBy,
                   filter.SelectedDealerSysNo
                };

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CBMkCustomizeMenu>()
            {
                Rows = dataList.OrderBy(@"tb.SysNo desc").Paging(filter.CurrentPage, filter.PageSize).QueryMany(),
                TotalRows = dataCount.QuerySingle()
            };
            return pager;
        }
        /// <summary>
        /// 获取对应微信菜单(子级)
        /// 2016-1-11 王耀发 创建
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public override Pager<CBMkCustomizeMenu> GetMkCustomizeSubMenuList(ParaMkCustomizeMenuFilter filter)
        {
            string sql = @"(select a.*
                                    ,b.Name As PName
	                                ,d.DealerName
                                from MkCustomizeMenu a
                                left join MkCustomizeMenu b on a.Pid = b.SysNo
                                left join DsDealer d on a.DealerSysNo = d.SysNo
                                where a.Pid = @0
                                and (@1 is null or a.Type = @1)
	                            and (@2 is null or charindex(a.Name,@2) > 0)
                                ) tb";

            var dataList = Context.Select<CBMkCustomizeMenu>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);
            var paras = new object[]
                {
                   filter.Pid,
                   filter.Type,
                   filter.Name
                };

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CBMkCustomizeMenu>()
            {
                Rows = dataList.OrderBy(@"tb.SysNo desc").Paging(filter.CurrentPage, filter.PageSize).QueryMany(),
                TotalRows = dataCount.QuerySingle()
            };
            return pager;
        }
        /// <summary>
        /// 同步信营全球购经销商的菜单，只能同步两级菜单
        /// 王耀发 2016-2-3 创建
        /// </summary>
        /// <param name="DealerSysNo">被同步的经销商系统编号</param>
        /// <returns></returns>
        public override int ProCreateMkCustomizeMenu(int DealerSysNo)
        {
            int MainDealerSysNo = 0; //信营全球购经销商系统编号
            string Sql = string.Format("pro_CreateMkCustomizeMenu {0},{1}", DealerSysNo, MainDealerSysNo);
            int rowsAffected = Context.Sql(Sql).Execute();
            return rowsAffected;
        }
    }
}
