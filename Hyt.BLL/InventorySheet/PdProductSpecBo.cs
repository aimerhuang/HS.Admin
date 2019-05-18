using Hyt.DataAccess.InventorySheet;
using Hyt.Model.InventorySheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.InventorySheet
{
    /// <summary>
    /// 商品供货规格
    /// 2017-9-1 吴琨 创建
    /// </summary>
    public class PdProductSpecBo : BOBase<PdProductSpecBo>
    {

        /// <summary>
        /// 根据产品查询规格
        /// 2017-9-1 吴琨 创建
        /// </summary>
        /// <returns></returns>
        public PdProductSpec IsPdProductSpec(int ProductSysNo)
        {
            return IPdProductSpecDao.Instance.IsPdProductSpec(ProductSysNo);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">规格信息</param>
        /// <returns>吴琨 2017-9-1</returns>
        public int AddPdProductSpec(PdProductSpec model)
        {
            return IPdProductSpecDao.Instance.AddPdProductSpec(model);
        }

        /// <summary>
        /// 同步添加到B2B平台
        /// </summary>
        /// <param name="model">规格信息</param>
        /// <returns>罗勤瑶 2017-10-11</returns>
        public int AddPdProductSpecToB2B(PdProductSpec model)
        {
            return IPdProductSpecDao.Instance.AddPdProductSpecToB2B(model);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">规格信息</param>
        /// <returns>吴琨 2017-9-1</returns>
        public int UpdPdProductSpec(PdProductSpec model)
        {
            return IPdProductSpecDao.Instance.UpdPdProductSpec(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ProductSysNo">产品系统编号</param>
        /// <returns></returns>
        public bool DeletePdProductSpec(int ProductSysNo)
        {
            return IPdProductSpecDao.Instance.DeletePdProductSpec(ProductSysNo);
        }


        /// <summary>
        /// 添加商品规格报价
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddPdProductSpecPrices(IList<PdProductSpecPrices> model)
        {
            return IPdProductSpecDao.Instance.AddPdProductSpecPrices(model);
        }


        /// <summary>
        /// 查询商品规格报价
        /// </summary>
        /// <param name="ProductSysNo">商品系统编号</param>
        /// <param name="WarehouseSysNo">仓库系统编号</param>
        /// <returns>商品规格报价列表</returns>
        /// 吴琨 2017-9-4 创建
        public List<PdProductSpecPrices> GetPdProductSpecPrices(int ProductSysNo, int WarehouseSysNo)
        {
            return IPdProductSpecDao.Instance.GetPdProductSpecPrices(ProductSysNo, WarehouseSysNo);
        }

        /// <summary>
        /// 修改商品规格报价的销售价
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>是否成功</returns>
        /// 吴琨 2017-9-6 创建
        public bool UpdateSpecPrices(PdProductSpecPrices model)
        {
            return IPdProductSpecDao.Instance.UpdateSpecPrices(model);
        }
    }
}
