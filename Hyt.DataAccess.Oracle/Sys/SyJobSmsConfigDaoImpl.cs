using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Oracle;
using Hyt.Model;
using Hyt.DataAccess.Sys;

namespace Hyt.DataAccess.Oracle.Sys
{
    /// <summary>
    /// 任务池短信设置
    /// </summary>
    /// <returns>List<Model.Gennerated.SyJobSmsConfig></returns>
    /// <remarks>2014-8-5 陈俊创建</remarks>
    public class SyJobSmsConfigDaoImpl : ISyJobSmsConfigDao
    {
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>list<Model.Gennerated.SyJobSmsConfig></returns>
        /// <remarks>2014-8-5 陈俊 创建</remarks>
        public override List<SyJobSmsConfig> GetAll()
        {
            var model = Context.Sql("select * from SyJobSmsConfig")
                .QueryMany<SyJobSmsConfig>();
            return model;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns>受影响的行数</returns>
        /// <remarks>2014-8-5 陈俊 创建</remarks>
        public override int Delete(int id)
        {
           int Result = Context.Delete("SyJobSmsConfig")
                .Where("SysNo", id)
                .Execute();
           return Result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="syJobSmsConfig"></param>
        /// <returns>受影响的行数</returns>
        /// <remarks>2014-8-5 陈俊 创建</remarks>
        public override int Update(SyJobSmsConfig syJobSmsConfig)
        {
            int Result = Context.Update("SyJobSmsConfig",syJobSmsConfig)
                .AutoMap(x=>x.SysNo)
                .Where("SysNo", syJobSmsConfig.SysNo)
                .Execute();
            return Result;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="syJobSmsConfig"></param>
        /// <returns>返回系统编号</returns>
        /// <remarks>2014-8-5 陈俊 创建</remarks>
        public override int Create(SyJobSmsConfig syJobSmsConfig)
        {
            int Result = Context.Insert("SyJobSmsConfig",syJobSmsConfig)
                .AutoMap(x=>x.SysNo)
                .ExecuteReturnLastId<int>("SysNo");
            return Result;
        }
    }
}
