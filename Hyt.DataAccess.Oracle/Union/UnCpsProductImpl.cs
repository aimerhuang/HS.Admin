using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Union;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.Union
{
    /// <summary>
    /// CPS商品Impl
    /// </summary>
    /// <remarks>2013-10-18 周唐炬 创建</remarks>
    public class UnCpsProductImpl : UnCpsProductDao
    {
        /// <summary>
        /// 添加CPS商品
        /// </summary>
        /// <param name="model">CPS商品实体</param>
        /// <returns>系统编号</returns>
        /// <remarks>2013-10-18 周唐炬 创建</remarks>
        public override int Create(Model.UnCpsProduct model)
        {
            return Context.Insert<UnCpsProduct>("UnCpsProduct", model).AutoMap(x => x.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 删除CPS商品
        /// </summary>
        /// <param name="sysNo">CPS商品系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-10-18 周唐炬 创建</remarks>
        public override void Remove(int sysNo)
        {
            Context.Delete("UnCpsProduct").Where("SysNo", sysNo).Execute();
        }

        /// <summary>
        /// 通过联盟广告删除CPS商品
        /// </summary>
        /// <param name="advertisementSysNo">联盟广告编号</param>
        /// <returns></returns>
        /// <remarks>2013-10-18 周唐炬 创建</remarks>
        public override void RemoveByAdvertisementSysNo(int advertisementSysNo)
        {
            Context.Delete("UnCpsProduct").Where("AdvertisementSysNo", advertisementSysNo).Execute();
        }

        /// <summary>
        /// 获取联盟广告CPS商品列表
        /// </summary>
        /// <param name="advertisementSysNo">联盟广告系统编号</param>
        /// <returns>联盟广告CPS商品列表</returns>
        /// <remarks>2013-10-18 周唐炬 创建</remarks>
        public override List<UnCpsProduct> GetList(int advertisementSysNo)
        {
            return Context.Sql(@"select * from UnCpsProduct where AdvertisementSysNo=@AdvertisementSysNo").Parameter("AdvertisementSysNo", advertisementSysNo).QueryMany<UnCpsProduct>();
        }
    }
}
