using Hyt.DataAccess.InventorySheet;
using Hyt.Model.InventorySheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.InventorySheet
{
    /// <summary>
    /// 商品供货规格
    /// 2017/9/1 吴琨 创建
    /// </summary>
    public class PdProductSpecDao : IPdProductSpecDao
    {
        /// <summary>
        /// 根据产品查询规格
        /// 2017-9-1 吴琨 创建
        /// </summary>
        /// <returns></returns>
        public override PdProductSpec IsPdProductSpec(int ProductSysNo)
        {
            return Context.Sql("select * from  PdProductSpec where ProductSysNo=@ProductSysNo  ").Parameter("ProductSysNo", ProductSysNo).QuerySingle<PdProductSpec>();
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">规格信息</param>
        /// <returns>吴琨 2017-9-1</returns>
        public override int AddPdProductSpec(PdProductSpec model)
        {  
            return Context.Insert("PdProductSpec")
                .Column("ProductSysNo", model.ProductSysNo)
                .Column("SpecValues", model.SpecValues)
                .Execute();
        }

        /// <summary>
        /// 同步添加到B2B平台
        /// </summary>
        /// <param name="model">规格信息</param>
        /// <returns>罗勤瑶 2017-10-11</returns>
        public override int AddPdProductSpecToB2B(PdProductSpec model)
        {
            return ContextB2B.Insert("PdProductSpec")
                .Column("ProductSysNo", model.ProductSysNo)
                .Column("SpecValues", model.SpecValues)
                .Execute();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">规格信息</param>
        /// <returns>修改数</returns>
        /// 吴琨 2017-9-1
        public override int UpdPdProductSpec(PdProductSpec model)
        {
            return Context.Update("PdProductSpec")
                .Column("SpecValues", model.SpecValues)
                .Where("ProductSysNo", model.ProductSysNo)
                .Execute();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ProductSysNo">产品系统编号</param>
        /// <returns></returns>
        public override bool DeletePdProductSpec(int ProductSysNo)
        {
            int sysNo = 0;
            try
            {
                sysNo = Context.Delete("PdProductSpec")
                .Where("ProductSysNo", ProductSysNo)
                .Execute();
                sysNo = Context.Delete("PdProductSpecPrices")
                    .Where("ProductSysNo", ProductSysNo)
                    .Execute();
                Context.Commit();
            }
            catch (Exception)
            {
                sysNo = 0;
                Context.Rollback();  //回滚
            }
            return sysNo > 0;
        }

        /// <summary>
        /// 添加商品规格报价
        /// </summary>
        /// <param name="model">商品规格报价列表</param>
        /// <returns>是否成功</returns>
        /// 吴琨 2017-9-4 创建
        public override bool AddPdProductSpecPrices(IList<PdProductSpecPrices> list)
        {
            int sysNo = 0;
            try
            {
                if (list == null || list.Count == 0)
                {
                    return false;
                }
                sysNo = Context.Delete("PdProductSpecPrices")
                     .Where("ProductSysNo", list[0].ProductSysNo)
                     .Where("WarehouseSysNo", list[0].WarehouseSysNo)
                     .Execute();

                foreach (var item in list)
                {
                    sysNo = Context.Insert<PdProductSpecPrices>
                     ("PdProductSpecPrices", item)
                     .AutoMap(p => p.SpecValueList, p => p.SysNo)
                     .Execute();
                }
                Context.Commit();
            }
            catch (Exception e)
            {
                sysNo = 0;
                Context.Rollback();  //回滚
            }
            return sysNo > 0;
        }

        /// <summary>
        /// 查询商品规格报价
        /// </summary>
        /// <param name="ProductSysNo">商品系统编号</param>
        /// <param name="WarehouseSysNo">仓库系统编号</param>
        /// <returns>商品规格报价列表</returns>
        /// 吴琨 2017-9-4 创建
        public override List<PdProductSpecPrices> GetPdProductSpecPrices(int ProductSysNo, int WarehouseSysNo)
        {
            return Context.Sql("select * from PdProductSpecPrices where ProductSysNo=@ProductSysNo and WarehouseSysNo=@WarehouseSysNo")
                .Parameter("ProductSysNo", ProductSysNo)
                .Parameter("WarehouseSysNo", WarehouseSysNo)
                .QueryMany<PdProductSpecPrices>();
        }


        /// <summary>
        /// 修改商品规格报价的销售价
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>是否成功</returns>
        /// 吴琨 2017-9-6 创建
        public override bool UpdateSpecPrices(PdProductSpecPrices model)
        {
            return Context.Update("PdProductSpecPrices")
                .Column("SalesPrice", model.SalesPrice)
                .Where("SysNo", model.SysNo)
                .Execute() > 0;
        }
    }
}
