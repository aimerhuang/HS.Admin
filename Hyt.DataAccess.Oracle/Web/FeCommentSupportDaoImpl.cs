using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Web;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Web
{
    /// <summary>
    /// 前台商品咨询
    /// </summary>
    /// <remarks>2013-08-13 邵斌 创建</remarks>
    public class FeCommentSupportDaoImpl : IFeCommentSupportDao
    {

        /// <summary>
        /// 添加评论支持
        /// </summary>
        /// <param name="model">评论支持数据模型</param>
        /// <param name="context">数据库操作上下文(用于共用数据库连接)</param>
        /// <returns>返回 true:成功 false:失败</returns>
        /// <remarks>2013-08-13 邵斌 创建</remarks>
        public override bool Add(FeCommentSupport model, IDbContext context = null)
        {
            //判断并获取默认数据库操作上下文
            context = context ?? Context;

            //添加数据库
            model.SysNo = context.Insert<FeCommentSupport>("FeCommentSupport", model).AutoMap(fc => fc.SysNo).ExecuteReturnLastId<int>("SysNo");

            //返回结果
            return model.SysNo > 0;

        }

        /// <summary>
        /// 更新评论支持
        /// </summary>
        /// <param name="isSupport">是否是支持</param>
        /// <param name="feCommentSysNo">评论表系统编号</param>
        /// <param name="customerSysNo">操作人</param>
        /// <param name="context">数据库操作上下文(用于共用数据库连接)</param>
        /// <returns>返回 true:成功 false:失败</returns>
        /// <remarks>2013-08-13 邵斌 创建</remarks>
        public override bool Update(bool isSupport, int feCommentSysNo, int customerSysNo, IDbContext context = null)
        {
            //判断并获取默认数据库操作上下文
            context = context ?? Context;

            //如果为空就先插入新对象
            if (!Exist(feCommentSysNo, customerSysNo,context))
            {
                //插入前检查数据是否完整
                if (feCommentSysNo != 0 && customerSysNo != 0)
                {
                    FeCommentSupport tempMode = new FeCommentSupport();
                    tempMode.ProductCommentSysNo = feCommentSysNo;

                    //判断是支持+1 还是不支持+1
                    if (isSupport)
                    {
                        tempMode.SupportCount = 1;
                    }
                    else
                    {
                        tempMode.UnSupportCount = 1;
                    }

                    //初始化其他数据
                    tempMode = new FeCommentSupport();
                    tempMode.CustomerSysNo = customerSysNo;
                    tempMode.CreatedBy = customerSysNo;
                    tempMode.CreateDate = DateTime.Now;
                    tempMode.LastUpdateBy = customerSysNo;
                    tempMode.LastUpdateDate = DateTime.Now;

                    //添加新对象
                    Add(tempMode, context);
                }
                else
                {
                    //错误提示
                    throw new HytException("添加评论支持失败，没有正确设置评论人或评论商品对象");
                }
            }

            string sql;
            //判断是支持+1 还是不支持+1
            //因为没有时间戳，所以采用动态更新方式，以避免并发问题
            if (isSupport)
            {
                sql = "update FeProductComment set SupportCount = SupportCount+1 where sysno=@0";
            }
            else
            {
                sql = "update FeProductComment set UnSupportCount = UnSupportCount+1 where sysno=@0";
            }

            //返回结果                
            return context.Sql(sql, feCommentSysNo).Execute() > 0;

        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="feCommentSysNo">评论系统编号</param>
        /// <param name="customerSysNo">会员系统编号</param>
        /// <param name="context">数据库操作上下文(用于共用数据库连接)</param>
        /// <returns>返回评论支持对象</returns>
        /// <remarks>2013-08-13 邵斌 创建</remarks>
        public override FeCommentSupport GetMode(int feCommentSysNo, int customerSysNo, IDbContext context = null)
        {
            //判断并获取默认数据库操作上下文
            context = context ?? Context;

            //通过评论系统编号获取评论支持对象
            return context.Sql("select * from FeCommentSupport where  productcommentsysno = @0 and CustomerSysNo=@1", feCommentSysNo, customerSysNo)
                       .QuerySingle<FeCommentSupport>();
        }

        /// <summary>
        /// 判断用户是否已经评价过
        /// </summary>
        /// <param name="feCommentSysNo">评价系统编号</param>
        /// <param name="customerSysNo">会员系统编号</param>
        /// <param name="context">数据库操作上下文(用于共用数据库连接)</param>
        /// <returns>返回 true：已经存在 false：不存在</returns>
        /// <remarks>2013-08-13 邵斌 创建</remarks>
        public override bool Exist(int feCommentSysNo, int customerSysNo, IDbContext context = null)
        {
            //判断并获取默认数据库操作上下文
            context = context ?? Context;

            //通过评论系统编号获取评论支持对象
            return context.Sql(
                "select count(sysno) from FeCommentSupport where  productcommentsysno = @0 and CustomerSysNo=@1",
                feCommentSysNo, customerSysNo)
                          .Execute() > 0;
        }

    }
}
