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
    /// 商品组商品关联表数据访问 抽象类
    /// </summary>
    /// <remarks>2013－06-21 苟治国 创建</remarks>
    public class FeProductGroupDaoImpl : IFeProductGroupDao
    {
        /// <summary>
        /// 查看商品组
        /// </summary>
        /// <param name="sysNo">商品组编号</param>
        /// <returns>商品组</returns>
        /// <remarks>2013－06-21 苟治国 创建</remarks>
        public override Model.FeProductGroup GetModel(int sysNo)
        {
            return Context.Sql(@"select * from FeProductGroup where SysNO = @0", sysNo).QuerySingle<Model.FeProductGroup>();
        }

        /// <summary>
        /// 根据商品组编号查询商品组
        /// </summary>
        /// <param name="code">商品组编号</param>
        /// <param name="platform"></param>
        /// <returns>商品组</returns>
        /// <remarks>2013－08-21 周瑜 创建</remarks>
        public override IList<FeProductGroup> GetModelByGroupcode(string code, ForeStatus.商品组平台类型 platform)
        {
            return Context.Select<FeProductGroup>("*")
                          .From("FeProductGroup")
                          .Where("Code=@Code and PlatformType=@PlatformType and Status=@Status")
                          .Parameter("Code", code)
                          .Parameter("PlatformType", (int)platform)
                          .Parameter("Status", (int)ForeStatus.商品组状态.启用)
                          .OrderBy("DisplayOrder asc")
                          .QueryMany();
        }

        /// <summary>
        /// 验证商品组名称
        /// </summary>
        /// <param name="key">广告组名称</param>
        /// <param name="sysNo">广告组编号</param>
        /// <returns>条数</returns>
        /// <remarks>2013-06-20 苟治国 创建</remarks>
        public override int FeProductGroupChk(string key, int sysNo)
        {
            int result;
            if (sysNo > 0)
            {
                string strSql = @"select count(1) from FeProductGroup where Name = @Name and SysNo!=@sysNO";
                result = Context.Sql(strSql)
                        .Parameter("Name", key)
                        .Parameter("sysNO", sysNo)
                        .QuerySingle<int>();
            }
            else
            {
                string strSql = @"select count(1) from FeProductGroup where Name = @Name";
                result = Context.Sql(strSql)
                    .Parameter("Name", key)
                    .QuerySingle<int>();
            }
            return result;
        }

        /// <summary>
        /// 根据条件获取产品组的列表
        /// </summary>
        /// <param name="pager">广告组查询条件</param>
        /// <returns>产品组列表</returns>
        /// <remarks>2013－06-21 苟治国 创建</remarks>
        public override Pager<FeProductGroup> Seach(Pager<FeProductGroup> pager)
        {
            #region sql条件
            string sqlWhere = @"(@Status=-1 or Status =@Status) 
 and (@Name is null or Name like @Name1 or  Code=@Name)
 and (@PlatformType=-1 or PlatformType = @PlatformType)
";
            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context.Select<FeProductGroup>("fp.*")
                                    .From("FeProductGroup fp")
                                    .Where(sqlWhere)
                                    .Parameter("Status", pager.PageFilter.Status)
                                    .Parameter("Name", pager.PageFilter.Name)
                                    .Parameter("Name1", "%" + pager.PageFilter.Name + "%")
                                    .Parameter("PlatformType", pager.PageFilter.PlatformType)
                                    .Paging(pager.CurrentPage, pager.PageSize).OrderBy("DisplayOrder asc").QueryMany();
                pager.TotalRows = context.Select<int>("count(1)")
                                    .From("FeProductGroup fp")
                                    .Where(sqlWhere)
                                    .Parameter("Status", pager.PageFilter.Status)
                                    .Parameter("Name", pager.PageFilter.Name)
                                    .Parameter("Name1", "%" + pager.PageFilter.Name + "%")
                                    .Parameter("PlatformType", pager.PageFilter.PlatformType)
                                    .QuerySingle();
            }
            return pager;
        }

        /// <summary>
        /// 新增产品组
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-21 苟治国 创建</remarks>
        public override int Insert(Model.FeProductGroup model)
        {
            if (model.LastUpdateDate == DateTime.MinValue)
            {
                model.LastUpdateDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            var result = Context.Insert<FeProductGroup>("FeProductGroup", model)
                                .AutoMap(x => x.SysNo)
                                .ExecuteReturnLastId<int>("SysNo");
            return result;
        }

        /// <summary>
        /// 更新产品组
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-21 苟治国 创建</remarks>
        public override int Update(Model.FeProductGroup model)
        {
            int rowsAffected = Context.Update<Model.FeProductGroup>("FeProductGroup", model)
                                      .AutoMap(x => x.SysNo)
                                      .Where(x => x.SysNo)
                                      .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 删除产品组
        /// </summary>
        /// <param name="sysNo">商品组编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-06-21 苟治国 创建</remarks>
        public override bool Delete(int sysNo)
        {
            int rowsAffected = Context.Delete("FeProductGroup")
                                      .Where("sysNo", sysNo)
                                      .Execute();
            return rowsAffected > 0;
        }

        public override List<CBFeProductItem> GetModelByGroupSysNo(int groupSysNo)
        {
            string sql = @"select FeProductItem.*,PdProduct.ProductName,PdProduct.EasName,PdProduct.ErpCode,PdPrice.Price as BasicPrice,PdProductImage.ImageUrl as ProductImage from FeProductItem 
                        inner join PdProduct on PdProduct.SysNo=FeProductItem.ProductSysNo left join PdProductImage on PdProduct.SysNo=PdProductImage.ProductSysNo and PdProductImage.Status=1
                        left join PdPrice on PdProduct.SysNo=PdPrice.ProductSysNo and PdPrice.PriceSource=0 and PdPrice.SourceSysNo=0 where FeProductItem.GroupSysNo = '" + groupSysNo + "' and FeProductItem.Status = 20 ";
            List<CBFeProductItem> list = Context.Sql(sql).QueryMany<CBFeProductItem>();
            List<int> productSysNoList=new List<int>();
            foreach(var mod in list)
            {
                productSysNoList.Add(mod.ProductSysNo);
            }
            List<PdPrice> priceList = Hyt.DataAccess.Oracle.Product.PdPriceDaoImpl.Instance.GetProductPrices(productSysNoList, 10);
            foreach (var mod in list)
            {
                List<PdPrice> tempPriceList = priceList.FindAll(p => p.ProductSysNo == mod.ProductSysNo);
                string userPrices = "";
                string userLevels = "";
                foreach(var tempPrice in tempPriceList)
                {
                    if(!string.IsNullOrEmpty(userPrices))
                    {
                        userPrices += ",";
                        userLevels += ",";
                    }
                    userPrices += tempPrice.Price.ToString();
                    userLevels += tempPrice.SourceSysNo.ToString();
                }
                mod.UserPriceList = userPrices;
                mod.LevelValueList = userLevels;
            }
            return list;
        }

        public override List<CBFeProductItem> GetProductInfoList(int levelSysNo, int groupSysNo)
        {
            string sql = @"select Origin.Origin_Img, FeProductItem.*,PdProduct.ProductName,PdProduct.EasName,PdProduct.ErpCode,PdPrice.Price as BasicPrice,PdProductImage.ImageUrl as ProductImage,price2.Price as UserPriceList,'"+levelSysNo+@"' as LevelValueList 
						,Origin.Origin_Img as orginImagePath
						from FeProductItem 
                        inner join PdProduct on PdProduct.SysNo=FeProductItem.ProductSysNo left join PdProductImage on PdProduct.SysNo=PdProductImage.ProductSysNo and PdProductImage.Status=1
                        left join PdPrice on PdProduct.SysNo=PdPrice.ProductSysNo and PdPrice.PriceSource=0 and PdPrice.SourceSysNo=0 
						 left join PdPrice price2 on PdProduct.SysNo=price2.ProductSysNo and price2.PriceSource=10 and price2.SourceSysNo='"+levelSysNo+@"' 
						 inner join Origin on Origin.SysNo=PdProduct.OriginSysNo
						where FeProductItem.GroupSysNo = '"+groupSysNo+@"' 
						and FeProductItem.Status = 20  ";
            List<CBFeProductItem> list = Context.Sql(sql).QueryMany<CBFeProductItem>();
            return list;
        }
    }
}
