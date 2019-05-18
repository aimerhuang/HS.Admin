using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Generated;
using Hyt.Model.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Sys
{
    /// <summary>
    /// 系统_金蝶_用户关联表
    /// </summary>
    public abstract class ISyKingdeeUserDao : DaoBase<ISyKingdeeUserDao>
    {
        /// <summary>
        /// 获取业务员用户
        /// </summary>
        /// <returns></returns>
        /// 2018-1-4 吴琨 创建
        public abstract List<SyUser> GetSyUser();


        /// <summary>
        /// 获取业务员用户
        /// </summary>
        /// <returns></returns>
        /// 2018-1-4 吴琨 创建
        public abstract SyKingdeeUser GetModels(int sysNo);
       

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="para">查询参数</param>
        /// <returns></returns>
        /// <remarks>2016-6-21 杨浩 增加查询条件</remarks>
        public abstract Pager<SyKingdeeUser> GetPages(ParaPrPurchaseFilter para);
        /// <summary>
        /// 创建第三方关联用户
        /// </summary>
        /// <returns></returns>
        ///  2018-1-4 吴琨 创建
        public abstract bool AddModels(SyKingdeeUser models);



        /// <summary>
        /// 编辑第三方关联用户
        /// </summary>
        /// <returns></returns>
        ///  2018-1-6 吴琨 创建
        public abstract bool UpModels(SyKingdeeUser models);
        

        /// <summary>
        /// 根据编号删除
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public abstract bool delModels(int SysNo);
        /// <summary>
        /// 获取金蝶用户代码
        /// </summary>
        /// <param name="type">类型（10:金蝶）</param>
        /// <param name="userSysno">用户系统编号</param>
        /// <returns></returns>
        /// <remarks>2018-1-5 杨浩 创建</remarks>
        public abstract string GetKingdeeUserCode(int userSysno,int type=10);
      
    }
}
