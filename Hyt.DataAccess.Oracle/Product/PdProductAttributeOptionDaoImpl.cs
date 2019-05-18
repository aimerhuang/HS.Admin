using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;

using Hyt.DataAccess.Product;
using Hyt.Model;
using Hyt.Util;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Transfer;
using System.Collections;

namespace Hyt.DataAccess.Oracle.Product
{
    /// <summary>
    /// 商品图片维护数据接口
    /// </summary>
    /// <remarks>2013-07-22 苟治国 创建</remarks>
    public class PdProductAttributeOptionDaoImpl : IPdProductAttributeOptionDao
    {

        /// <summary>
        /// 图片添加
        /// </summary>
        /// <param name="model">图片信息</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2016-06-13 王耀发 创建</remarks>
        public override int Insert(PdProductAttributeOption model)
        {
            var result = Context.Insert<PdProductAttributeOption>("PdProductAttributeOption", model)
                                .AutoMap(x => x.SysNo)
                                .ExecuteReturnLastId<int>("SysNo");
            return result;
        }

        /// <summary>
        /// 图片更新
        /// </summary>
        /// <param name="model">图片信息</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2016-06-13 王耀发 创建</remarks>
        public override int Update(PdProductAttributeOption model)
        {
            int rowsAffected = Context.Update<Model.PdProductAttributeOption>("PdProductAttributeOption", model)
                                      .AutoMap(x => x.SysNo)
                                      .Where(x => x.SysNo)
                                      .Execute();
            return rowsAffected;
        }


        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2016-06-13 王耀发 创建</remarks>
        public override bool Delete(int productSysNo)
        {
            int rowsAffected = Context.Delete("PdProductAttributeOption")
                                      .Where("ProductSysNo", productSysNo)
                                      .Execute();
            return rowsAffected > 0;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="ProductSysNo"></param>
        /// <param name="AttributeOptionSysNo"></param>
        /// <remarks>2016-06-13 王耀发 创建</remarks>
        public override PdProductAttributeOption GetByProOptionSysNo(int ProductSysNo, int AttributeOptionSysNo)
        {
            return Context.Sql("select * from PdProductAttributeOption where ProductSysNo = @0 and AttributeOptionSysNo = @1", ProductSysNo, AttributeOptionSysNo)
                   .QuerySingle<PdProductAttributeOption>();
        }
    }
}
