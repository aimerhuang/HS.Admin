using System;
using System.Collections.Generic;
using Hyt.DataAccess.Product;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Product
{
    /// <summary>
    /// 商品描述模板数据层实现类
    /// </summary>
    /// <remarks>2013-07-22 杨晗 创建</remarks>
    public class IPdTemplateDaoImpl : IPdTemplateDao
    {
        /// <summary>
        /// 根据商品描述模板系统编号获取模型
        /// </summary>
        /// <param name="sysNo">商品描述模板系统编号</param>
        /// <returns>商品描述模板实体</returns>
        /// <remarks>2013-07-22 杨晗 创建</remarks>
        public override PdTemplate GetModel(int sysNo)
        {
            return Context.Sql(@"select * from PdTemplate where SysNO = @0", sysNo)
                          .QuerySingle<PdTemplate>();
        }

        /// <summary>
        /// 判断商品描述模板名称是否重复
        /// </summary>
        /// <param name="name">商品描述模板名称</param>
        /// <returns>重复为true,否则为false</returns>
        /// <remarks>2013-07-05 杨晗 创建</remarks>
        public override bool PdTemplateVerify(string name)
        {
            string sql = @"Name=@name";
            int countBuilder = Context.Select<int>("count(1)")
                                     .From("PdTemplate")
                                     .Where(sql)
                                     .Parameter("name", name)
                                     .QuerySingle();
            return countBuilder > 0;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex">起始页</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="type">商品描述模板类型</param>
        /// <param name="count">抛出总条数</param>
        /// <param name="searchName">商品描述模板名称</param>
        /// <returns>商品描述模板列表</returns>
        /// <remarks>2013-07-22 杨晗 创建</remarks>
        public override IList<CBPdTemplate> Seach(int pageIndex, int pageSize, ProductStatus.商品描述模板类型? type,
                                                       out int count ,string searchName=null )
        {
            #region sql条件

            string sql = @"(@Name is null or Name like @Name1)
                       and (@Type=0 or Type=@Type1)";

            #endregion
            type = type ?? 0;
            count = Context.Select<int>("count(1)")
                                     .From("PdTemplate pt")
                                     .Where(sql)
                                     .Parameter("Name", searchName ?? "")
                                     .Parameter("Name1", "%" + searchName + "%")
                                     .Parameter("Type", (int)type)
                                     .Parameter("Type1", (int)type)
                                     .QuerySingle();

            var countBuilder = Context.Select<CBPdTemplate>("pt.*,(select UserName from SyUser where sysno=pt.CreatedBy) as CreatedByName,(select UserName from SyUser where sysno=pt.LastUpdateBy) as LastUpdateByName")
                                      .From("PdTemplate pt")
                                      .Where(sql)
                                      .Parameter("Name", searchName??"")
                                      .Parameter("Name1", "%" + searchName + "%")
                                      .Parameter("Type", (int)type)
                                      .Parameter("Type1", (int)type)
                                      .Paging(pageIndex, pageSize).OrderBy("sysno desc").QueryMany();
            return countBuilder;
        }

        /// <summary>
        /// 插入商品描述模板
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-07-22 杨晗 创建</remarks>
        public override int Insert(PdTemplate model)
        {
            return Context.Insert<PdTemplate>("PdTemplate", model)
                            .AutoMap(x => x.SysNo)
                            .ExecuteReturnLastId<int>("Sysno");
        }

        /// <summary>
        /// 更新商品描述模板
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-07-22 杨晗 创建</remarks>
        public override int Update(PdTemplate model)
        {
            return Context.Update<PdTemplate>("PdTemplate", model)
                                      .AutoMap(x => x.SysNo)
                                      .Where(x => x.SysNo)
                                      .Execute();
        }

        /// <summary>
        /// 删除商品描述模板
        /// </summary>
        /// <param name="sysNo">商品描述模板系统编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-07-22 杨晗 创建</remarks>
        public override bool Delete(int sysNo)
        {
            int rowsAffected = Context.Delete("PdTemplate")
                                      .Where("Sysno", sysNo)
                                      .Execute();
            return rowsAffected > 0;
        }
    }
}
