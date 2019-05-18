using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Product;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using System.Transactions;

namespace Hyt.DataAccess.Oracle.Product
{
    /// <summary>
    /// 商品属性数据操作接口
    /// </summary>
    /// <remarks>2013-07-18 唐永勤 创建</remarks>
    public class IPdProductAttributeDaoImpl : IPdProductAttributeDao
    {
        /// <summary>
        /// 添加商品属性
        /// </summary>
        /// <param name="model">商品属性实体信息</param>
        /// <returns>返回新建记录的sysno</returns>       
        /// <remarks>2013-07-24 唐永勤 创建</remarks>
        public override int Create(PdProductAttribute model)
        {

            int sysno = 0;
            sysno = Context.Insert<PdProductAttribute>("PdProductAttribute", model)
                        .AutoMap(x => x.SysNo)
                        .ExecuteReturnLastId<int>("Sysno");
            return sysno;
        }

        /// <summary>
        /// 根据属性组编号获取属性
        /// </summary>
        /// <param name="listSysNo">属性组编号</param>
        /// <returns>属性列表</returns>
        /// <remarks>2013-07-18 唐永勤 创建</remarks>    
        public override IList<CBPdProductAtttributeRead> GetProductAttributeByGroupSysNo(IList<int> listSysNo)
        {
            IList<CBPdProductAtttributeRead> list = new List<CBPdProductAtttributeRead>();
            if (listSysNo.Count > 0)
            {
                string sql = string.Format(@"select a.SysNo, a.AttributeName,  a.AttributeType, '' as AttributeText, '' as AttributeImage, 0 as AttributeOptionSysNo, ag.sysno as AttributeGroupSysNo, ag.name as AttributeGroupName,a.IsSearchKey,a.IsRelationFlag,a.status 
                                             from (select * from PdAttributeGroup where sysno in ({0})) ag
                                             left join PdAttributeGroupAssociation aga on ag.sysno=aga.attributegroupsysno
                                             left join  PdAttribute a on aga.attributesysno = a.sysno 
                                             order by ag.sysno desc,aga.DisplayOrder asc                                      
                                             ", listSysNo.Join(","));
                list = Context.Sql(sql).QueryMany<CBPdProductAtttributeRead>();
            }
            return list;
        }

        /// <summary>
        /// 根据商品编号获取属性
        /// </summary>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>属性列表</returns>
        /// <remarks>2013-07-18 唐永勤 创建</remarks>    
        public override IList<CBPdProductAtttributeRead> GetProductAttributeByProductSysNo(int productSysNo)
        {
            IList<CBPdProductAtttributeRead> list = new List<CBPdProductAtttributeRead>();
            if (productSysNo > 0)
            {
                string sql = string.Format(@"select a.SysNo, pa.Sysno as ProductAttributeSysno, a.AttributeName, a.AttributeType, pa.AttributeText, pa.AttributeImage, pa.AttributeOptionSysNo, ag.name as AttributeGroupName, ag.sysno as AttributeGroupSysNo,a.IsSearchKey,a.IsRelationFlag ,pa.Status ,pa.ProductSysNo
                                             from (select * from PdProductAttribute where productsysno = {0}) pa
                                             left join PdAttribute a on pa.attributesysno = a.sysno
                                             left join pdattributegroup ag on pa.attributegroupsysno = ag.sysno order by pa.sysno asc", productSysNo);
                list = Context.Sql(sql).QueryMany<CBPdProductAtttributeRead>();
            }
            return list;
        }

        /// <summary>
        /// 根据商品编号获取属性
        /// </summary>
        /// <param name="productSysNo">商品编号</param>
        /// <param name="onlyAssociationAttribute">只读取关联属性</param>
        /// <returns>属性列表</returns>
        /// <remarks>2013-07-24 邵斌 创建</remarks>    
        public override IList<PdProductAttribute> GetProductAttributeByProductSysNo(int productSysNo, bool onlyAssociationAttribute)
        {
            IList<PdProductAttribute> list = new List<PdProductAttribute>();

            //如果没有主商品系统编号将不做任何操作
            if (productSysNo > 0)
            {
                //查询商品并将重复的属性过滤掉，一个属性只出现一次
                string sql = string.Format(@"
                                            select 
                                               pa.* 
                                            from 
                                                (select * from PdProductAttribute where productsysno = @0) pa
                                                inner join (
                                                      select sysno from PdProductAttribute 
                                                  where 
                                                      productsysno = @1 and sysno in (select min(sysno) from PdProductAttribute where productsysno = @2 group by attributesysno)
                                                ) pa2 on pa.sysno = pa2.sysno
                                                inner join PdAttribute a on pa.attributesysno = a.sysno       
                                            where  
                                               a.IsRelationFlag = @3
                                             ", productSysNo);
                list = Context.Sql(sql, productSysNo, productSysNo, productSysNo, (int)ProductStatus.是否用做关联属性.是).QueryMany<PdProductAttribute>();
            }
            return list;
        }

        /// <summary>
        /// 根据商品分类获取属性
        /// </summary>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>属性列表</returns>
        /// <remarks>2013-07-19 唐永勤 创建</remarks>    
        public override IList<CBPdProductAtttributeRead> GetCategoryProductAttributeByProductSysNo(int productSysNo)
        {
            IList<CBPdProductAtttributeRead> list = new List<CBPdProductAtttributeRead>();
            if (productSysNo > 0)
            {
                #region 改为使用主分类的属性

                string sql = string.Format(@"select 
                                                a.SysNo, a.AttributeName, a.AttributeType, '' as AttributeText, '' as AttributeImage, 0 as AttributeOptionSysNo, ag.sysno as AttributeGroupSysNo, ag.name as AttributeGroupName , a.IsSearchKey,a.IsRelationFlag,a.status
                                            from 
                                            (
                                                    select attributegroupsysno from PdCategoryAssociation pca inner join PdCatattributeGroupaso pcg on pca.categorysysno = pcg.productcategorysysno where pca.productsysno = {0} and pca.ismaster={1} group by attributegroupsysno
                                            ) tb
                                            left join PdAttributeGroup ag on tb.attributegroupsysno=ag.sysno
                                            left join PdAttributeGroupAssociation aga on ag.sysno=aga.attributegroupsysno
                                            left join  PdAttribute a on aga.attributesysno = a.sysno                                                                                
                                             ", productSysNo, (int)Model.WorkflowStatus.ProductStatus.是否是主分类.是);

                #endregion

                #region 读取说有分类属性暂时设为过期方式

                /*
            
                string sql = string.Format(@"select a.SysNo, a.AttributeName, a.AttributeType, '' as AttributeText, '' as AttributeImage, 0 as AttributeOptionSysNo, ag.sysno as AttributeGroupSysNo, ag.name as AttributeGroupName , a.IsSearchKey,a.IsRelationFlag,a.status
                                             from (select * from PdCategoryAssociation where productsysno = {0} and IsMaster={1}) ca 
                                             left join PdCatattributeGroupaso cg on ca.categorysysno = cg.productcategorysysno
                                             left join PdAttributeGroup ag on cg.attributegroupsysno=ag.sysno
                                             left join PdAttributeGroupAssociation aga on ag.sysno=aga.attributegroupsysno
                                             left join  PdAttribute a on aga.attributesysno = a.sysno                                                                                
                                             ", productSysno,(int)Model.WorkflowStatus.ProductStatus.是否是主分类.是);
            */
                list = Context.Sql(sql).QueryMany<CBPdProductAtttributeRead>();
            }

                #endregion

            return list;

        }

        /// <summary>
        /// 根据编号获取属性
        /// </summary>
        /// <param name="listSysNo">属性编号集</param>
        /// <returns>属性列表</returns>
        /// <remarks>2013-07-18 唐永勤 创建</remarks>    
        public override IList<CBPdProductAtttributeRead> GetProductAttributeByAttributeSysNo(IList<int> listSysNo)
        {
            IList<CBPdProductAtttributeRead> list = new List<CBPdProductAtttributeRead>();
            if (listSysNo.Count > 0)
            {
                string sql = string.Format(@"select SysNo, AttributeName, AttributeType, '' as AttributeText, '' as AttributeImage, 0 as AttributeOptionSysNo, 0 as AttributeGroupSysNo, '' as AttributeGroupName, IsRelationFlag,IsSearchKey
                                             from PdAttribute where sysno in ({0})
                                             ", listSysNo.Join(","));
                list = Context.Sql(sql).QueryMany<CBPdProductAtttributeRead>();
            }
            return list;
        }

        /// <summary>
        /// 保存商品属性
        /// </summary>
        /// <param name="productSysNo">商品编号</param>
        /// <param name="list">商品属性列表</param>
        /// <returns>保存是否成功</returns>
        /// <remarks>2013-07-19 唐永勤 创建</remarks> 
        public override bool SaveProductAttribute(int productSysNo, IList<PdProductAttribute> list)
        {
            bool result = false;
            if (productSysNo > 0)
            {
                if (productSysNo > 0)
                {
                    //先删除
                    Context.Delete("PdProductAttribute")
                        .Where("productsysno", productSysNo)
                        .Execute();

                    //再添加
                    foreach (PdProductAttribute entity in list)
                    {
                        Context.Insert<PdProductAttribute>("PdProductAttribute", entity)
                            .AutoMap(x => x.SysNo)
                            .Execute();
                    }
                }

                result = true;
            }
            return result;
        }

    }
}
