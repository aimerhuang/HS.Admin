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
    public class PdProductImageDaoImpl:IPdProductImageDao
    {
        /// <summary>
        /// 查看商品图片
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>商品图片</returns>
        /// <remarks>2013-07-21 苟治国 创建</remarks>
        public override Model.PdProductImage GetModel(int sysNo)
        {
            const string strSql = @"select * from PdProductImage where sysNo = @sysNo";
            var result = Context.Sql(strSql)
                                .Parameter("SysNO", sysNo)
                                .QuerySingle<PdProductImage>();
            return result;
        }

        /// <summary>
        /// 获取指定商品的图片列表
        /// </summary>
        /// <param name="productsysNo">商品系统编号</param>
        /// <returns>获取指定商品的图片列表</returns>
        /// <remarks>2013-07-21 苟治国 创建</remarks>
        public override IList<PdProductImage> GetProductImg(int productsysNo)
        {
            const string sql = @"select * from PdProductImage where productsysno=@0 order by DisplayOrder,sysno asc";
            var list = Context.Sql(sql, productsysNo).QueryMany<PdProductImage>();

            //返回结果集
            return list;
        }

        /// <summary>
        /// 获取B2B指定商品的图片列表
        /// </summary>
        /// <param name="productsysNo">商品系统编号</param>
        /// <returns>获取指定商品的图片列表</returns>
        /// <remarks>2017-10-11 罗勤瑶 创建</remarks>
        public override IList<PdProductImage> GetB2BProductImg(int productsysNo)
        {
            const string sql = @"select * from PdProductImage where productsysno=@0 order by DisplayOrder,sysno asc";
            var list = ContextB2B.Sql(sql, productsysNo).QueryMany<PdProductImage>();

            //返回结果集
            return list;
        }

        /// <summary>
        /// 图片添加
        /// </summary>
        /// <param name="model">图片信息</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-07-21 苟治国 创建</remarks>
        public override int Insert(Model.PdProductImage model)
        {
            var result = Context.Insert<PdProductImage>("PdProductImage", model)
                                .AutoMap(x => x.SysNo)
                                .ExecuteReturnLastId<int>("SysNo");
            return result;
        }

        /// <summary>
        /// 图片添加
        /// </summary>
        /// <param name="model">图片信息</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-07-21 苟治国 创建</remarks>
        public override int InsertB2B(Model.PdProductImage model)
        {
            var result = ContextB2B.Insert<PdProductImage>("PdProductImage", model)
                                .AutoMap(x => x.SysNo)
                                .ExecuteReturnLastId<int>("SysNo");
            return result;
        }

        /// <summary>
        /// 图片更新
        /// </summary>
        /// <param name="model">图片信息</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-07-21 苟治国 创建</remarks>
        public override int Update(Model.PdProductImage model)
        {
            int rowsAffected = Context.Update<Model.PdProductImage>("PdProductImage", model)
                                      .AutoMap(x => x.SysNo)
                                      .Where(x => x.SysNo)
                                      .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 更新图片状态
        /// </summary>
        /// <param name="productSysNo">系统编号</param>
        /// <param name="status">状态</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-07-21 苟治国 创建</remarks>
        public override int UpdateStatus(int productSysNo, int status)
        {
            string sql = string.Format("update PdProductImage set Status = {0} where productsysno in ({1})", status, productSysNo);
            var rowsAffected = Context.Sql(sql)
                .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-07-23 苟治国 创建</remarks>
        public override bool Delete(int productSysNo)
        {
            int rowsAffected = Context.Delete("PdProductImage")
                                      .Where("sysno", productSysNo)
                                      .Execute();
            return rowsAffected > 0;
        }
        /// <summary>
        /// 获取封面图
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public override Model.PdProductImage GetModelByPdSysNo(int sysNo)
        {
            var result = Context.Sql("select * from PdProductImage where ProductSysNo =" + sysNo + " and Status=1")
                                .QuerySingle<PdProductImage>();
            return result;
        }
    }
}
