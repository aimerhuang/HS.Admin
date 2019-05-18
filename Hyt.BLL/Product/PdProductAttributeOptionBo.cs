using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Hyt.BLL.Authentication;
using Hyt.DataAccess.Product;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Product
{
    public class PdProductAttributeOptionBo : BOBase<PdProductAttributeOptionBo>
    {

        /// <summary>
        /// 图片添加
        /// </summary>
        /// <param name="model">图片信息</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2016-06-13 王耀发 创建</remarks>
        public int Insert(PdProductAttributeOption model)
        {
            return IPdProductAttributeOptionDao.Instance.Insert(model);
        }

        /// <summary>
        /// 图片更新
        /// </summary>
        /// <param name="model">图片信息</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2016-06-13 王耀发 创建</remarks>
        public int Update(PdProductAttributeOption model)
        {
            return IPdProductAttributeOptionDao.Instance.Update(model);
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2016-06-13 王耀发 创建</remarks>
        public bool Delete(int productSysNo)
        {
            return IPdProductAttributeOptionDao.Instance.Delete(productSysNo);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="ProductSysNo"></param>
        /// <param name="AttributeOptionSysNo"></param>
        /// <remarks>2016-06-13 王耀发 创建</remarks>
        public PdProductAttributeOption GetByProOptionSysNo(int ProductSysNo, int AttributeOptionSysNo)
        {
            return IPdProductAttributeOptionDao.Instance.GetByProOptionSysNo(ProductSysNo, AttributeOptionSysNo);
        }
    }
}
