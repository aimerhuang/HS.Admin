using Hyt.DataAccess.Base;
using Hyt.DataAccess.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Product
{
    /// <summary>
    /// 商品关联
    /// </summary>
    /// <remarks>2013-07-23 邵斌 创建</remarks>
    public class PdProductAssociationDaoImpl : IPdProductAssociationDao
    {
        /// <summary>
        /// 获取指定商品的关联商品列表
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>关联商品列表</returns>
        /// <remarks>2013-07-23 邵斌 创建</remarks>
        public override IList<CBProductAssociation> ProductList(int productSysNo)
        {

            using (var context = Context.UseSharedConnection(true))
            {
                #region 测试SQL 通过商品自身系统编号，在对应表中读取商品所在的关联商品组，并读取他的关系码

                /*
                select relationcode from PdProductAssociation where productsysno = '1'
                */
                #endregion

                /*
                  select 
                          p.sysno as productsysno,p.productname,p.erpcode,pa.attributename,ppa.attributesysno as attributesysno,ppa.attributetext, ppa.AttributeImage
                        from  
                          (select b.* from PdProductAssociation a right join PdProductAssociation b on a.relationcode=b.relationcode where a.productsysno=25) pas
                          inner join PdAttribute pa on pa.sysno = pas.attributesysno
                          inner join pdproduct p on p.sysno = pas.productsysno
                          inner join PdProductAttribute ppa on ppa.productsysno = p.sysno and ppa.attributesysno = pas.attributesysno
                        where 
                          ppa.status =1
                        order by ppa.attributesysno,ppa.Productsysno
                 */
                //string relactionCode = GetRelactionCode(productSysNo, context);

               

                    #region 测试SQL

                    /*
             select 
                  p.sysno as productsysno,p.productname,p.erpcode,pa.attributename,ppa.attributesysno as attributesysno,ppa.attributetext, ppa.AttributeImage
                from  
                  PdProductAssociation pas
                  inner join PdAttribute pa on pa.sysno = pas.attributesysno
                  inner join pdproduct p on p.sysno = pas.productsysno
                  inner join PdProductAttribute ppa on ppa.productsysno = p.sysno and ppa.attributesysno = pas.attributesysno
                where 
                  pas.relationcode = 187  and ppa.status = 1
                order by ppa.attributesysno,,ppa.Productsysno
             * */

                    #endregion

                    //通过主商品编号作为管理关系码（code）来查询关联商品，并查找属性和属性值，并且保证属性值表状态为可用
                    return context.Sql(@"
                        select 
                          p.sysno as productsysno,p.easname as productname,p.erpcode,p.isfrontdisplay,pa.attributename,ppa.attributesysno as attributesysno,ppa.attributetext, ppa.AttributeImage
                        from  
                          (
                              select relationcode from PdProductAssociation where productsysno = @0
                          ) pas1

                          inner join PdProductAssociation pas on pas.relationcode = pas1.relationcode
                          inner join PdAttribute pa on pa.sysno = pas.attributesysno
                          inner join pdproduct p on p.sysno = pas.productsysno
                          inner join PdProductAttribute ppa on ppa.productsysno = p.sysno and ppa.attributesysno = pas.attributesysno
                        where 
                          ppa.status =@1
                        order by ppa.attributesysno,ppa.Productsysno
                    ", productSysNo, (int)ProductStatus.商品属性关联状态.有效).QueryMany<CBProductAssociation>();
            }
        }

        /// <summary>
        /// 获取指定商品的关联商品列表
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>返回B2CAPP接口转用商品对象</returns>
        /// <remarks>2013-08-23 邵斌 创建</remarks>
        public override IList<CBProductAssociation> GetProductList(int productSysNo)
        {
            #region 测试 SQL

            /*
             *根据商品的系统编号来获取关联关系码，并读取改关联关系下所有的商品属性的值
             */

            /*
            select 
                pa2.productsysno,pa2.attributetext,pa2.attributesysno,pa2.attributename,pa2.attributeimage
            from
                (
                    select 
                        pas.relationcode,pa.productsysno,pa.attributetext,pa.attributesysno,pa.attributename,pa.attributeimage
                    from 
                        (select relationcode from Pdproductassociation where productsysno=1898 and rownum=1) x      --通过商品系统编号读取商品关联关系码
                        inner join Pdproductassociation pas on pas.relationcode = x.relationcode                    --通过关联关系码来读取对应的关联关系商品和商品的属性
                        inner join pdproductattribute pa on pa.productsysno=pas.productsysno and pa.attributesysno = pas.attributesysno     --通过商品属性值表来取得值
                    where 
                        pa.productsysno=1898                                                                        
                ) tt1
                left join pdproductassociation pas2 on pas2.attributesysno = tt1.attributesysno and pas2.relationcode = tt1.relationcode
                inner join pdproductattribute pa2 on pa2.productsysno=pas2.productsysno and pa2.attributesysno = pas2.attributesysno
            group by pa2.productsysno,pa2.attributetext,pa2.attributesysno,pa2.attributename,pa2.attributeimage
            order by pa2.productsysno,pa2.attributesysno
             */

            #endregion

            return Context.Sql(@"
                            select 
                                pa2.productsysno,pa2.attributetext,pa2.attributesysno,pa2.attributename,pa2.attributeimage
                            from
                                (
                                    select 
                                        pas.relationcode,pa.productsysno,pa.attributetext,pa.attributesysno,pa.attributename,pa.attributeimage
                                    from 
                                        (select relationcode from Pdproductassociation where productsysno=@0 and rownum=1) x  
                                        inner join Pdproductassociation pas on pas.relationcode = x.relationcode
                                        inner join pdproductattribute pa on pa.productsysno=pas.productsysno and pa.attributesysno = pas.attributesysno
                                    where 
                                        pa.productsysno=@1
                                ) tt1
                                left join pdproductassociation pas2 on pas2.attributesysno = tt1.attributesysno and pas2.relationcode = tt1.relationcode
                                inner join pdproductattribute pa2 on pa2.productsysno=pas2.productsysno and pa2.attributesysno = pas2.attributesysno
                            group by pa2.productsysno,pa2.attributetext,pa2.attributesysno,pa2.attributename,pa2.attributeimage
                            order by pa2.productsysno,pa2.attributesysno
                ", productSysNo, productSysNo).QueryMany<CBProductAssociation>();
        }

        /// <summary>
        /// 创建商品的商品关联关系
        /// </summary>
        /// <param name="model">商品关联关系模型</param>
        /// <param name="context">共享数据库操作上线文</param>
        /// <returns>返回 true:成功 false:失败</returns>
        /// <remarks>2013-07-24 邵斌 创建</remarks>
        public override bool Create(PdProductAssociation model, IDbContext context = null)
        {
            context = context ?? Context;
            int sysNo = context.Insert<PdProductAssociation>("PdProductAssociation", model)
                                 .AutoMap(m => m.SysNo)
                                 .ExecuteReturnLastId<int>("SysNo");

            model.SysNo = sysNo;

            return sysNo > 0;
        }

        /// <summary>
        /// 创建一组商品的商品关联关系
        /// </summary>
        /// <param name="mainProductSysNo">主商品系统编号</param>
        /// <param name="associationProductSysNoList">关联商品系统编号</param>
        /// <param name="updateUser">更新操作人</param>
        /// <param name="relactionCode">关联关系码</param>
        /// <returns>返回 true:成功 false:失败</returns>
        /// <remarks>2013-07-24 邵斌 创建</remarks>
        public override bool Create(int mainProductSysNo, int[] associationProductSysNoList, int updateUser, string relactionCode)
        {
            bool result = true;                         //返回结果

            //设置共享数据库操作上下文
            using (var sharedContext = Context.UseSharedConnection(true))
            {
                #region 测试SQL 通过商品自身系统编号，在对应表中读取商品所在的关联商品组，并读取他的关系码

                /*
                select relationcode from PdProductAssociation where productsysno = '1'
                */
                #endregion

                //通过主商品系统编号来获取能用于关联的商品属性
                IList<PdAttribute> associationAttributeList = IPdAttributeDao.Instance.GetProductAssociationAttribute(associationProductSysNoList, sharedContext);

                //构建商品关联关联对象并创建关联关系
                PdProductAssociation pdProductAssociation;      //关联关系对象
                int i = 0;                                      //用于显示排序计数器

                //遍历要关联的商品列表
                foreach (var productSysNo in associationProductSysNoList)
                {

                    //遍历主商品用于关联的关联属性
                    foreach (var pdAttribute in associationAttributeList)
                    {
                        i++;

                        //构建关联对象
                        pdProductAssociation = new PdProductAssociation()
                       {
                           RelationCode = relactionCode,
                           AttributeSysNo = pdAttribute.SysNo,
                           CreatedBy = updateUser,
                           CreatedDate = DateTime.Now,
                           LastUpdateBy = updateUser,
                           LastUpdateDate = DateTime.Now,
                           ProductSysNo = productSysNo,
                           DisplayOrder = i
                       };

                        //保存关联关系对象
                        result = result && Create(pdProductAssociation, sharedContext);

                        //如果添加失败那将返回失败，并结束添加操作
                        if (!result)
                            return false;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 清理指定商品的关联关系
        /// </summary>
        /// <param name="relationCode">关联关系码</param>
        /// <returns>返回 True：清理成功 False：清理失败</returns>
        /// <remarks>2013-07-24 邵斌 创建</remarks>
        public override bool Clear(string relationCode)
        {
            //如果管理吗不为空将进行删除操作
                if (!string.IsNullOrWhiteSpace(relationCode))
                {
                    //清除所以关联关系
                    return Context.Sql("delete from PdProductAssociation where RelationCode=@0", relationCode).Execute() > 0;
                }

            return false;

        }

        /// <summary>
        /// 获取商品关联关系码
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="context">数据库操作上下文</param>
        /// <returns>返回管理关系码</returns>
        /// <remarks>2013-07-24 邵斌 创建</remarks>
        public override string GetRelactionCode(int productSysNo, IDbContext context = null)
        {
            context = context ?? Context;

            //读取关联关系码
            return context.Sql("select RelationCode from PdProductAssociation where productSysNo=@0", productSysNo)
                   .QuerySingle<string>();
        }

        /// <summary>
        /// 根据商品关联码读取商品所以关联属性
        /// </summary>
        /// <param name="relationCode">关联关系码</param>
        /// <returns>返回：关联属性列表</returns>
        /// <remarks>2013-08-16 邵斌 创建</remarks>
        public override IList<PdProductAttribute> GetAssociationAttributeList(string relationCode)
        {
            return  Context.Sql(@"
               select 
                    att.attributesysno,att.attributetext,pat.attributename,att.attributeimage
                from
                    ( 
                      select  ppa.attributesysno,ppa.attributetext,ppa.attributeimage from  
                      PdProductAssociation pa
                      inner join pdproduct p on pa.productsysno  = p.sysno
                      inner join pdproductattribute ppa on ppa.productsysno = pa.productsysno
                      where pa.relationcode=@0 and p.CanFrontEndOrder = @1
                      group by ppa.attributesysno,ppa.attributetext,ppa.attributeimage
                      order by ppa.attributesysno
                    ) att
                    inner join PdAttribute pat on att.attributesysno = pat.sysno
                    order by att.attributesysno,att.attributetext
            ", relationCode,(int)ProductStatus.商品是否前台下单.是).QueryMany<PdProductAttribute>();
        }
    }
}
