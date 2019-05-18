using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Logistics;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
namespace Hyt.DataAccess.Oracle.Logistics
{
    /// <summary>
    /// 取商检数据访问类
    /// </summary>
    /// <remarks>
    /// 2015-08-26 王耀发 创建
    /// </remarks>
    public class LogisticsDaoImpl : ILogisticsDao
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        /// <remarkss>2016-04-05 王耀发 创建</remarks>
        public override int InsertLgGaoJieGoodsInfoEntity(LgGaoJieGoodsInfo entity)
        {
            entity.SysNo = Context.Insert("LgGaoJieGoodsInfo", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        /// <remarkss>2016-04-05 王耀发 更新</remarks>
        public override int UpdateLgGaoJieGoodsInfoEntity(LgGaoJieGoodsInfo entity)
        {

            return Context.Update("LgGaoJieGoodsInfo", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        /// <remarkss>2016-04-05 王耀发 创建</remarks>
        public override LgGaoJieGoodsInfo GetLgGaoJieGoodsInfoEntityByPid(int ProductSysNo)
        {

            return Context.Sql("select a.* from LgGaoJieGoodsInfo a where a.ProductSysNo=@ProductSysNo")
                   .Parameter("ProductSysNo", ProductSysNo)
              .QuerySingle<LgGaoJieGoodsInfo>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        /// <remarkss>2016-04-05 王耀发 创建</remarks>
        public override int InsertLgGaoJiePushInfoEntity(LgGaoJiePushInfo entity)
        {
            entity.SysNo = Context.Insert("LgGaoJiePushInfo", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }
        /// <summary>
        /// 获取所有商品备案信息
        /// </summary>
        /// <returns>商品备案信息集合</returns>
        /// <remarks>2015-12-15 王耀发 创建</remarks>
        public override IList<LgGaoJieGoodsInfo> GetAllGaoJieGoodsInfo()
        {
            const string strSql = @"select * from LgGaoJieGoodsInfo";
            var entity = Context.Sql(strSql)
                                .QueryMany<LgGaoJieGoodsInfo>();
            return entity;
        }
        /// <summary>
        /// 新增商品备案信息
        /// </summary>
        /// <param name="models">商品备案信息列表</param>
        /// <returns>空</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public override void CreateExcelGaoJieGoodsInfo(List<LgGaoJieGoodsInfo> models)
        {
            foreach (LgGaoJieGoodsInfo model in models)
            {
                LgGaoJieGoodsInfo GoodsInfoData = new LgGaoJieGoodsInfo();
                GoodsInfoData.ProductSysNo = model.ProductSysNo;
                GoodsInfoData.goods_ptcode = model.goods_ptcode;
                GoodsInfoData.goods_name = model.goods_name;
                GoodsInfoData.brand = model.brand;
                GoodsInfoData.goods_spec = model.goods_spec;
                GoodsInfoData.ycg_code = model.ycg_code;
                GoodsInfoData.hs_code = model.hs_code;
                GoodsInfoData.goods_barcode = model.goods_barcode;
                GoodsInfoData.CreatedBy = model.CreatedBy;
                GoodsInfoData.CreatedDate = model.CreatedDate;
                GoodsInfoData.LastUpdateBy = model.LastUpdateBy;
                GoodsInfoData.LastUpdateDate = model.LastUpdateDate;
                int InfoSysNo = Context.Insert<LgGaoJieGoodsInfo>("LgGaoJieGoodsInfo", GoodsInfoData)
                                       .AutoMap(o => o.SysNo)
                                       .ExecuteReturnLastId<int>("SysNo");
            }
        }

        /// <summary>
        /// 更新商品备案信息
        /// </summary>
        /// <param name="models">商品备案信息列表</param>
        /// <returns>空</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public override void UpdateExcelGaoJieGoodsInfo(List<LgGaoJieGoodsInfo> models)
        {
            foreach (LgGaoJieGoodsInfo model in models)
            {
                string sql = @"update LgGaoJieGoodsInfo set goods_ptcode = @goods_ptcode,goods_name = @goods_name,brand = @brand,goods_spec = @goods_spec
                              ,ycg_code = @ycg_code,hs_code = @hs_code,goods_barcode = @goods_barcode where ProductSysNo = @ProductSysNo";
                Context.Sql(sql)
                .Parameter("ProductSysNo", model.ProductSysNo)
                .Parameter("goods_ptcode", model.goods_ptcode)
                .Parameter("goods_name", model.goods_name)
                .Parameter("brand", model.brand)
                .Parameter("goods_spec", model.goods_spec)
                .Parameter("ycg_code", model.ycg_code)
                .Parameter("hs_code", model.hs_code)
                .Parameter("goods_barcode", model.goods_barcode).Execute();
            }
        }
    }
}
