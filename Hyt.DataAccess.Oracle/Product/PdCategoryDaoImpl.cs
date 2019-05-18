using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Product;
using Hyt.Model;
using Hyt.Util;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Product
{

    /// <summary>
    /// 商品分类维护管理
    /// </summary>
    /// <remarks>2013-06-25 邵斌 创建</remarks>
    public class PdCategoryDaoImpl : IPdCategoryDao
    {
        /// <summary>
        /// 添加商品分类
        /// </summary>
        /// <param name="category">商品分类实体</param>
        /// <param name="attributeGroupAso">商品分类对应属性组</param>
        /// <returns>返回操作是否成功 true:成功   false:不成功</returns>
        /// <remarks>2013-06-25 邵斌 创建</remarks>
        public override bool CreateCategory(PdCategory category, IList<PdCatAttributeGroupAso> attributeGroupAso)
        {
            //接收新添加商品分类的系统编号
            int result = 0;

            //启动事务
            using (var _context = Context.UseSharedConnection(true))
            {
                //添加新分类
                result = _context.Insert<PdCategory>("PdCategory", category)
                                    .AutoMap(c => c.SysNo, c => c.ParentCategory, c => c.IsMaster)
                                    .ExecuteReturnLastId<int>("SysNo");   //记录返还系统编号
                category.SysNo = result;

                //通过系统编号来判断是否添加成功
                if (result > 0)
                {
                    //设置分类路由
                    SetCateorySysNosBySysNo(category.SysNo);
                    int index = 0;              //显示序号
                    foreach (var pdCatAttributeGroupAso in attributeGroupAso)
                    {
                        index++;
                        pdCatAttributeGroupAso.ProductCategorySysNo = category.SysNo;
                        pdCatAttributeGroupAso.DisplayOrder = index;

                        //添加商品属性组对应关系
                        result = (_context.Insert<PdCatAttributeGroupAso>("PdCatAttributeGroupAso",
                                                                        pdCatAttributeGroupAso)
                                        .AutoMap(p => p.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo"));
                        pdCatAttributeGroupAso.SysNo = result;      //记录返回属性组对应关系的系统编号

                        //判断是否添加成功，如果添加失败将回滚
                        if (result <= 0)
                        {
                            return false;
                        }
                    }
                }

                //设置新添加的商品分类到他父级分类末尾显示
                SetCategoryToLastShow(category, _context);

            }
            return true;
        }
        /// <summary>
        /// 获取新的系统编号路由
        /// </summary>
        /// <param name="newParentSysNo">新的父级系统编号</param>
        /// <param name="sysNo">系统编号</param>
        /// <returns>新的系统编号路由</returns>
        /// <remarks>2015-07-20 杨浩 创建</remarks>
        public override string GetNewSysNos(int newParentSysNo, int sysNo)
        {
            string newSysNos = "";
            if (newParentSysNo != 0)
                newSysNos = string.Format("{0}{1},", GetCategory(newParentSysNo).SysNos,sysNo);
            else
                newSysNos = string.Format(",{0},",sysNo);
            return newSysNos;
        }
        /// <summary>
        /// 修改商品分类
        /// </summary>
        /// <param name="category">商品分类实体</param>
        /// <param name="attributeGroupAso">商品分类对应属性组</param>
        /// <returns>返回操作是否成功 true:成功   false:不成功</returns>
        /// <remarks>2013-06-25 邵斌 创建</remarks>
        public override bool EditCategory(PdCategory category, IList<PdCatAttributeGroupAso> attributeGroupAso)
        {
            //接收新添加商品分类的系统编号
            int result = 0;
            using (var _context = Context.UseSharedConnection(true))
            {
                result = _context.Sql(@"update PdCategory set 
                                            CategoryImage = @CategoryImage
                                            ,CategoryName = @CategoryName
                                            ,Code=@Code
                                            ,ParentSysNo=@ParentSysNo
                                            ,SeoDescription=@SeoDescription
                                            ,SeoKeyword=@SeoKeyword
                                            ,SeoTitle=@SeoTitle
                                            ,LastUpdateBy=@LastUpdateBy
                                            ,LastUpdateDate=@LastUpdateDate 
                                            ,SysNos=@SysNos
                                            where SysNo = @SysNo")
                                    .Parameter("CategoryImage", category.CategoryImage)
                                    .Parameter("CategoryName", category.CategoryName)
                                    .Parameter("Code", category.Code)
                                    .Parameter("ParentSysNo", category.ParentSysNo)
                                    .Parameter("SeoDescription", category.SeoDescription)
                                    .Parameter("SeoKeyword", category.SeoKeyword)
                                    .Parameter("SeoTitle", category.SeoTitle)
                                    .Parameter("LastUpdateBy", category.LastUpdateBy)
                                    .Parameter("LastUpdateDate", category.LastUpdateDate)
                                    .Parameter("SysNos", category.SysNos)
                                    .Parameter("SysNo", category.SysNo)
                                    .Execute();

                //如果更新成功才添加属性组对应关系
                if (result > 0)
                {
                    //保存属性组对应关系
                    //先清理原有属性组信息，然后重新添加现有的所有属性组
                    _context.Delete("PdCatAttributeGroupAso")
                           .Where("ProductCategorySysNo", category.SysNo)
                           .Execute();

                    foreach (var pdCatAttributeGroupAso in attributeGroupAso)
                    {
                        //添加分类属性组
                        result = _context.Insert<PdCatAttributeGroupAso>("PdCatAttributeGroupAso", pdCatAttributeGroupAso)
                                        .AutoMap(c => c.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");

                        //如果有一个属性组信息没有保存成功都视为操作失败
                        if (result <= 0)
                        {
                            return false;
                        }
                    }
                }
            }

            return result > 0;
        }

        /// <summary>
        /// 读取所以商品分类包括无效分类
        /// </summary>
        /// <returns>返回商品分类列表</returns>
        /// <remarks>2013-06-25 邵斌 创建</remarks>
        public override IList<PdCategory> GetAllCategory()
        {
            //读取商品分类
            var categoryList = Context.Select<PdCategory>("*")
                                      .From("PdCategory")
                                      .OrderBy("displayorder asc")
                                      .QueryMany();
            //返回结果
            return categoryList;
        }

        /// <summary>
        /// 读取所以有效的商品分类,可以读取指定的商品分类类别
        /// </summary>
        /// <param name="sysNo">父节点商品分类编号</param>
        /// <returns>返回商品分类列表</returns>
        /// <remarks>2013-06-25 邵斌 创建</remarks>
        public override IList<PdCategory> GetCategoryList(int? sysNo)
        {
            //读取列表，但只读取有效的分类

            var categoryList = Context.Select<PdCategory>("*")
                                        .From("PdCategory")
                                        .Where("(@SysNo is null or SysNo =@SysNo) and Status=@Status")
                                        .OrderBy("displayorder asc")
                                        .Parameter("SysNo", sysNo)
                                        //.Parameter("SysNo", sysNo)
                                        .Parameter("Status", (int)ProductStatus.商品分类状态.有效)
                                        .QueryMany();

            //返回结果
            return categoryList;
        }

        /// <summary>
        /// 获取单个商品分类
        /// </summary>
        /// <param name="sysNo">商品分类系统编号</param>
        /// <param name="isGetParent">是否获取父级商品分类对象</param>
        /// <param name="context">共用数据库操作上下文</param>
        /// <returns>返回单个商品分类</returns>
        /// <remarks>2013-07-05 邵斌 创建</remarks>
        public override PdCategory GetCategory(int sysNo, bool isGetParent = false, IDbContext context = null)
        {
            PdCategory category;
            context = context ?? Context.UseSharedConnection(true);
            using (context)
            {

                category = context.Select<PdCategory>("*")
                              .From("PdCategory")
                              .Where("SysNo = @SysNo")
                              .Parameter("SysNo", sysNo)
                              .QuerySingle();

                //由于商品分类时无限极分类所以使用递归来查找上级分类
                //获取分类的父级分类对象
                if (isGetParent && category != null && category.ParentSysNo != 0)
                {
                    category.ParentCategory = GetCategory(category.ParentSysNo, isGetParent, context);
                }
            }

            return category;
        }

        /// <summary>
        /// 更新商品分类状态
        /// </summary>
        /// <param name="sysNo">商品分类系统编号</param>
        /// <param name="status">要变更的状态</param>
        /// <param name="adminSysNo">变更人</param>
        /// <returns>返回： true 成功 false 失败</returns>
        /// <remarks>2013-07-06 邵斌 创建</remarks>
        public override bool ChangeStatus(int sysNo, ProductStatus.商品分类状态 status, int adminSysNo)
        {
//            //获得当前分类的编码Code
//            string categoryCode = Context.Select<string>("code")
//                                         .From("PdCategory")
//                                         .Where("SysNo = :SysNo")
//                                         .Parameter("SysNo", sysNo)
//                                         .QuerySingle();

//#warning 这里不要用事务吧！ 邵斌     ==========已修改===========

            //更新状态
            //var result = Context.Sql(
            //    "update PdCategory set Status=:Status,LastUpdateBy=:UserSysNo,LastUpdateDate=:UpdateDate where sysno in (select sysno from pdcategory start with sysno=:sysno connect by Prior parentsysno=sysno)")
            //        .Parameter("Status", status)
            //       .Parameter("UserSysNo", adminSysNo)
            //       .Parameter("UpdateDate", DateTime.Now)
            //       .Parameter("sysno", sysNo)
            //       .Execute() > 0;

           var result = (Context.Sql(
                "update PdCategory set Status=@Status,LastUpdateBy=@UserSysNo,LastUpdateDate=@UpdateDate where sysnos  like '%,"+sysNo+",%' ")
                    .Parameter("Status", status)
                    .Parameter("UserSysNo", adminSysNo)
                    .Parameter("UpdateDate", DateTime.Now)
                    //.Parameter("sysno", sysNo)
                    .Execute() > 0);

            return result;

            ////更新状态 
            //return Context.Sql(
            //    "update PdCategory set Status=:Status,LastUpdateBy=:UserSysNo,LastUpdateDate=:UpdateDate  Where charindex(Code,:Code) =1")
            //        .Parameter("Status", status)
            //        .Parameter("UserSysNo", adminSysNo)
            //        .Parameter("UpdateDate", DateTime.Now)
            //        .Parameter("Code", categoryCode)
            //        .Execute() > 0;

        }

        /// <summary>
        /// 设置商品分类是否显示
        /// </summary>
        /// <param name="sysNo">商品分类系统编号</param>
        /// <param name="isOnline">是否前台展示 true:展示 false:隐藏</param>
        /// <param name="adminSysNo">管理员系统编号</param>
        /// <returns>返回： true 成功 false 失败</returns>
        /// <remarks>2013-07-06 邵斌 创建</remarks>
        [Obsolete]
        public override bool SetIsOnline(int sysNo, ProductStatus.是否前端展示 isOnline, int adminSysNo)
        {
//#warning 必须要判断吗？ 邵斌     ==========已修改===========
//#warning isonline不是bool类型  应该是枚举     ==========已修改===========
//            //获得当前分类的编码Code
//            string categoryCode = Context.Select<string>("code")
//                                         .From("PdCategory")
//                                         .Where("SysNo = :SysNo")
//                                         .Parameter("SysNo", sysNo)
//                                         .QuerySingle();
//#warning 这里不需要事务！     ==========已修改===========

//            categoryCode = categoryCode ?? "";

            //更新状态
            //var result = Context.Sql(
            //    "update PdCategory set IsOnline=:IsOnline,LastUpdateBy=:UserSysNo,LastUpdateDate=:UpdateDate where sysno in (select sysno from pdcategory start with sysno=:sysno connect by Prior parentsysno=sysno)")
            //    .Parameter("IsOnline", (int)isOnline)
            //       .Parameter("UserSysNo", adminSysNo)
            //       .Parameter("UpdateDate", DateTime.Now)
            //       .Parameter("sysno", sysNo)
            //       .Execute() > 0;

            var result = (Context.Sql("update PdCategory set IsOnline=" + (int)isOnline + ",LastUpdateBy=" + adminSysNo + ",LastUpdateDate='" + DateTime.Now.ToString() + "' where sysnos like '%," + sysNo + ",%'").Execute() > 0);

            return result;

        }

        /// <summary>
        /// 保存分类信息
        /// </summary>
        /// <param name="updateCategory">分类实体对象</param>
        /// <returns>返回： true 成功 false 失败</returns>
        /// <remarks>2013-07-10 邵斌 创建</remarks>
        public override bool Update(PdCategory updateCategory)
        {
            return Context.Sql(@"update PdCategory set 
                                            CategoryName = @CategoryName
                                            ,Code=@Code
                                            ,ParentSysNo=@ParentSysNo
                                            ,SeoDescription=@SeoDescription
                                            ,SeoKeyword=@SeoKeyword
                                            ,SeoTitle=@SeoTitle
                                            ,LastUpdateBy=@LastUpdateBy
                                            ,LastUpdateDate=@LastUpdateDate 
                                            ,DisplayOrder=@DisplayOrder 
                                            where SysNo =@SysNo")
                            .Parameter("CategoryName", updateCategory.CategoryName)
                            .Parameter("Code", updateCategory.Code)
                            .Parameter("ParentSysNo", updateCategory.ParentSysNo)
                            .Parameter("SeoDescription", updateCategory.SeoDescription)
                            .Parameter("SeoKeyword", updateCategory.SeoKeyword)
                            .Parameter("SeoTitle", updateCategory.SeoTitle)
                            .Parameter("LastUpdateBy", updateCategory.LastUpdateBy)
                            .Parameter("LastUpdateDate", updateCategory.LastUpdateDate)
                            .Parameter("DisplayOrder", updateCategory.DisplayOrder)
                            .Parameter("SysNo", updateCategory.SysNo)
                            .Execute() > 0;

        }

        /// <summary>
        /// 获取父节点下的空闲Code编号
        /// </summary>
        /// <param name="parentSysNo">父商品分类系统编号</param>
        /// <returns>返回 可用用的Code编号数字</returns>
        /// <remarks>2013-07-12 邵斌 创建</remarks>
        public override dynamic GetFreeCodeNum(int parentSysNo)
        {
            dynamic data = null;   //返回结果

            //如果父节点系统编号是0表示是对根节点处理
            if (parentSysNo == 0)
            {
                //处理根节点code
                data = Context.Sql(@"
                            select * from
                            (
                                select 
                                    row_number() over(order by code)  as r_n,tt.*,dbo.lpad(row_number() over(order by code),3,'0') as AUTOCODE
                                 from 
                                    (select 
                                           p1.sysno,p1.code
                                    from 
                                           pdcategory p1
                                    where 
                                           p1.parentsysno=0
                                    ) tt
                            )  as pp
                            where code<>autocode and r_n=1
                    ").QuerySingle<dynamic>();
            }
            else
            {
                //查找父节点下的空code
                data = Context.Sql(@"select 
                                       r1.r_n,r1.sysno,r1.code,r1.autocode,r1.parentcode
                                from 
                                (
                                select 
                                       row_number() over(order by code) as r_n,p1.sysno,p1.code,t1.oldCode||dbo.lpad(row_number() over(order by code),3,'0') as autoCode,t1.oldCode as  parentcode
                                from 
                                       pdcategory p1
                                       , (select code as oldCode from pdcategory where sysno=@sysno) t1 
                                where 
                                       p1.parentsysno=@sysno                              
                                ) r1
                                where 
                                r1.code <> r1.autoCode 
                                order by r1.code ")
                                  .Parameter("sysno", parentSysNo)
                                 // .Parameter("sysno", parentSysNo)
                                  .QuerySingle<dynamic>();
            }
            //如果data为空表示在父节点下没有空节点，所以将获得一个新节点
            if (data == null)
            {
                //生产新节点
//                data = Context.Sql(@"
//                                    select 
//                                      row_number() over(order by code) as r_n,t1.sysno,t1.code,(t1.code+t2.autoCode) as autocode
//                                    from 
//                                    (select * from pdcategory where (@sysno=0 or pdcategory.sysno=@sysno) and row_number() over(order by code)=1 order by code desc) t1 ,
//                                    (select  
//                                      dbo.lpad( count(p1.sysno)+1,3,'0') as autoCode
//                                    from 
//                                      pdcategory p1 where p1.parentsysno=@sysno
//                                      order by p1.code
//                                    ) t2
//                                    order by t1.code desc
//                                    ")
                data = Context.Sql(@"
                                    select 
                                     row_number() over(order by code) as r_n,t1.sysno,t1.code,(t1.code+t2.autoCode) as autocode
                                    from                                    
                                    (select * from (select row_number() over(order by code desc) as rownum,pdcategory.* from pdcategory where (@sysno=0 or pdcategory.sysno=@sysno)) t0 where rownum=1) t1 ,
                                    (select  
                                      dbo.lpad( count(p1.sysno)+1,3,'0') as autoCode
                                    from 
                                      pdcategory p1 where p1.parentsysno=@sysno                                    
                                    ) t2
                                    order by t1.code desc
                                    ")
                              .Parameter("sysno", parentSysNo)
                              //.Parameter("sysno", parentSysNo)
                              //.Parameter("sysno", parentSysNo)
                              .QuerySingle<dynamic>();
            }
            return data;
        }
        /// <summary>
        /// 设置分类系统编号路由
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>返回： true 成功 false 失败</returns>
        /// <remarks>2015-07-29 杨浩 创建</remarks>
        public override bool SetCateorySysNosBySysNo(int sysNo)
        {
            return Context.Sql("update pdcategory set SysNos=ISNULL(SysNos,',')+CONVERT(nvarchar(200),sysNo)+',' where sysNo=@sysNo")
                .Parameter("sysNo", sysNo).Execute() > 0;
        }
        /// <summary>
        /// 更新子分类编码
        /// </summary>
        /// <param name="oldParentCode">原父级编码</param>
        /// <param name="newParentCode">新父级编码</param>
        /// <returns>返回： true 成功 false 失败</returns>
        /// <remarks>2013-07-12 邵斌 创建</remarks>
        public override bool UpdataChildrenCode(string oldParentCode, string newParentCode)
        {
            return Context.Sql("update pdcategory set code = trim(REGEXP_REPLACE( code,'^'||@oldCode, @newCode)) where REGEXP_charindex(trim(code),'^'||@oldCode,1,1,1,'i')>0")
                .Parameter("oldCode", oldParentCode)
                .Parameter("newCode", newParentCode)
                //.Parameter("oldCode", oldParentCode)
                .Execute() > 0;
        }
        /// <summary>
        /// 更新子分类系统编码路由
        /// </summary>
        /// <param name="oldParentSysNos">原父级系统编码路由</param>
        /// <param name="newParentSysNos">新父级系统编码路由</param>
        /// <param name="oldParentSysNo">原父级系统编码</param>
        /// <returns>返回： true 成功 false 失败</returns>
        /// <remarks>2015-07-30 杨浩 创建</remarks>
        public override bool UpdateChildrenSysNos(string oldParentSysNos, string newParentSysNos, int oldParentSysNo)
        {
            return Context.Sql("update pdcategory set SysNos =replace(SysNos,@oldParentSysNos,@newParentSysNos) where SysNos like '%," + oldParentSysNo + ",%' and sysNo<>@oldParentSysNo")
              .Parameter("oldParentSysNos", oldParentSysNos)
              .Parameter("newParentSysNos", newParentSysNos)
              .Parameter("oldParentSysNo", oldParentSysNo)
              .Execute() > 0;
        }
        /// <summary>
        /// 将分类排在父分类的末尾显示
        /// </summary>
        /// <param name="category">商品分类对象</param>
        /// <param name="context">供应数据库操作上线文</param>
        /// <returns>返回： true 成功 false 失败</returns>
        /// <remarks>2013-07-12 邵斌 创建</remarks>
        public override bool SetCategoryToLastShow(PdCategory category, IDbContext context = null)
        {
            context = context ?? Context;

            return
                context.Sql(
                    @"update pdcategory set displayorder = (select count(sysno)+1 from pdcategory where parentsysno=@parentSysNo and sysno<>@sysno) where sysno=@sysno")
                       .Parameter("parentSysNo", category.ParentSysNo)
                       .Parameter("sysno", category.SysNo)
                       //.Parameter("sysno", category.SysNo)
                       .Execute() > 0;

        }

        #region 供应链商品分类
        /// <summary>
        /// 添加关联数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override int CreateSupplyChain(PdCategoryRelatedSupplyChain model)
        {
            int id = Context.Insert<PdCategoryRelatedSupplyChain>("PdCategoryRelatedSupplyChain", model)
                     .AutoMap(x => x.SysNo)
                     .ExecuteReturnLastId<int>("Sysno");
            return id;
        }
        /// <summary>
        /// 是否添加过
        /// </summary>
        /// <param name="SCName"></param>
        /// <param name="SupplyChainCode"></param>
        /// <returns></returns>

        public override bool IsExistsSupplyChain(string SCName, int SupplyChainCode)
        {
            bool result = false;
            PdCategoryRelatedSupplyChain entity = Context.Select<PdCategoryRelatedSupplyChain>("*")
                .From("PdCategoryRelatedSupplyChain")
                .Where(" SupplyChainCode= @SupplyChainCode and SupplyChainCategoryName=@SupplyChainCategoryName")
                .Parameter("SupplyChainCode", SupplyChainCode)
                .Parameter("SupplyChainCategoryName", SCName)
                .QuerySingle();
            return result;

        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public override PdCategoryRelatedSupplyChain GetEntitySupplyChain(int SysNo)
        {
            PdCategoryRelatedSupplyChain entity = Context.Select<PdCategoryRelatedSupplyChain>("*")
                .From("PdCategoryRelatedSupplyChain")
                .Where("sysno = @sysno")
                .Parameter("sysno", SysNo)
                .QuerySingle();
            return entity;
        }
        /// <summary>
        /// 根据供应链编号及供应链商品分类名称获取数据
        /// </summary>
        /// <param name="SCName"></param>
        /// <param name="SupplyChainCode"></param>
        /// <returns></returns>

        public override PdCategoryRelatedSupplyChain GetEntityByNameSupplyChain(string SCName, int SupplyChainCode)
        {
            PdCategoryRelatedSupplyChain entity = Context.Select<PdCategoryRelatedSupplyChain>("*")
                .From("PdCategoryRelatedSupplyChain")
                .Where(" SupplyChainCode= @SupplyChainCode and SupplyChainCategoryName=@SupplyChainCategoryName")
                .Parameter("SupplyChainCode", SupplyChainCode)
                .Parameter("SupplyChainCategoryName", SCName)
                .QuerySingle();
            return entity;
        }
        /// <summary>
        /// 根据分类ID及供应商编号获取数据
        /// </summary>
        /// <param name="CategorySysNo"></param>
        /// <param name="SupplyChainCode"></param>
        /// <returns></returns>
        public override PdCategoryRelatedSupplyChain GetEntityByCSysNoSupplyChain(int CategorySysNo, int SupplyChainCode)
        {
            PdCategoryRelatedSupplyChain entity = Context.Select<PdCategoryRelatedSupplyChain>("*")
                .From("PdCategoryRelatedSupplyChain")
                .Where(" SupplyChainCode= @SupplyChainCode and CategorySysNo=@CategorySysNo")
                .Parameter("SupplyChainCode", SupplyChainCode)
                .Parameter("CategorySysNo", CategorySysNo)
                .QuerySingle();
            return entity;
        }
        /// <summary>
        /// 根据类别ID、供应链编号查询供应链绑定数据
        /// </summary>
        /// <param name="CategorySysNo"></param>
        /// <param name="SupplyChainCode"></param>
        /// <returns></returns>
        public override IList<PdCategoryRelatedSupplyChain> GetSupplyChainList(int CategorySysNo, int SupplyChainCode)
        {
            return Context.Sql("select * from PdCategoryRelatedSupplyChain where CategorySysNo=@0 and SupplyChainCode=@1", CategorySysNo, SupplyChainCode).QueryMany<PdCategoryRelatedSupplyChain>();
        }

       /// <summary>
       /// 更新数据
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
        public override bool UpdateSupplyChain(PdCategoryRelatedSupplyChain model)
        {
            int effect = Context.Update<PdCategoryRelatedSupplyChain>("PdCategoryRelatedSupplyChain", model)
                .AutoMap(x => x.SysNo)
                .Where("sysno", model.SysNo)
                .Execute();
            return effect > 0;
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public override int DeleteSupplyChain(int sysNo)
        {
            return Context.Delete("PdCategoryRelatedSupplyChain").Where("sysNo", sysNo).Execute();
        }
        /// <summary>
        /// 根据类别ID查询数据
        /// </summary>
        /// <param name="CategorySysNo"></param>
        /// <returns></returns>
        public override IList<PdCategoryRelatedSupplyChain> SelectAllSupplyChain(int CategorySysNo)
        {
            string sql = "select * from PdCategoryRelatedSupplyChain where CategorySysNo=" + CategorySysNo;
            return Context.Sql(sql).QueryMany<PdCategoryRelatedSupplyChain>();
        }
        #endregion

        public override IList<PdCategory> GetAllCategory(int sysNo)
        {
            string sql = " select * from PdCategory where SysNos like '%," + sysNo + ",%' ";
            return Context.Sql(sql).QueryMany<PdCategory>();
        }

        public override IList<PdCategory> GetCategoryListByParent(int[] parentSysNo)
        {
            string sql = " select * from PdCategory where ParentSysNo in (" + string.Join(",",parentSysNo) + ") ";
            return Context.Sql(sql).QueryMany<PdCategory>();
        }

        public override IList<PdCategory> GetCategoryListByParentName(int parentSysNo)
        {
            string sql = string.Format(" select * from PdCategory where ParentSysNo ='{0}'", parentSysNo);
            return Context.Sql(sql).QueryMany<PdCategory>();
        }
    }
}
