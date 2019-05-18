using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
namespace Hyt.DataAccess.Sys
{
    /// <summary>
    /// 任务池短信设置
    /// </summary>
    /// <remarks>2014-8-5 陈俊创建</remarks>
    public abstract class ISyJobSmsConfigDao : DaoBase<ISyJobSmsConfigDao>
    {
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>list<Nodel.Gennerated.SyJobSmsconfig></returns>
        /// <remarks>2014-8-5 陈俊 创建</remarks>
        public abstract List<SyJobSmsConfig> GetAll();

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns>受影响的行数</returns>
        /// <remarks>2014-8-5 陈俊 创建</remarks>
        public abstract int Delete(int id);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="syJobSmsConfig"></param>
        /// <returns>受影响的行数</returns>
        /// <remarks>2014-8-5 陈俊创建</remarks>
        public abstract int Update(SyJobSmsConfig syJobSmsConfig);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="syJobSmsConfig"></param>
        /// <returns>返回系统编号</returns>
        /// <remarks>2014-8-5 陈俊创建</remarks>
        public abstract int Create(SyJobSmsConfig syJobSmsConfig);
    }
}
