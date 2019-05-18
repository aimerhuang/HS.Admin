using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.RMA;
using Hyt.Model;
namespace Hyt.DataAccess.Oracle.RMA
{
    /// <summary>
    /// 退换货图片
    /// </summary>
    /// <remarks>2013-09-23  朱成果 创建</remarks>
    public class RcReturnImageDaoImpl : IRcReturnImageDao
    {

        #region 数据记录增，删，改，查
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-09-23  朱成果 创建</remarks>
        public override int Insert(RcReturnImage entity)
        {
            entity.SysNo = Context.Insert("RcReturnImage", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2013-09-23  朱成果 创建</remarks>
        public override void Update(RcReturnImage entity)
        {

            Context.Update("RcReturnImage", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2013-09-23  朱成果 创建</remarks>
        public override RcReturnImage GetEntity(int sysNo)
        {

            return Context.Sql("select * from RcReturnImage where SysNo=@SysNo")
                   .Parameter("SysNo", sysNo)
              .QuerySingle<RcReturnImage>();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-23  朱成果 创建</remarks>
        public override void Delete(int sysNo)
        {
            Context.Sql("Delete from RcReturnImage where SysNo=@SysNo")
                 .Parameter("SysNo", sysNo)
            .Execute();
        }

        /// <summary>
        /// 通过退换货编号获取数据
        /// </summary>
        /// <param name="returnSysNo">退换货编号</param>
        /// <returns>list</returns>
        /// <remarks>2013-09-23  朱家宏 创建</remarks>
        public override IList<RcReturnImage> GetAll(int returnSysNo)
        {
            return Context.Sql(@"select * from RcReturnImage 
                    where (returnSysNo=@returnSysNo) ")
                          .Parameter("returnSysNo", returnSysNo)
                          .QueryMany<RcReturnImage>();
        }

        #endregion

    }
}
