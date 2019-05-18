using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Sys
{
    /// <summary>
    /// 任务池短信设置
    /// </summary>
    /// <remarks>2014-8-5 陈俊 创建</remarks>
    public class SyJobSmsConfigBo : BOBase<SyJobSmsConfigBo>
    {
        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <returns>短信设置列表</returns>
        /// <remarks>2014-8-5 陈俊 创建</remarks>
        public List<SyJobSmsConfig> GetAll()
        {
            return DataAccess.Sys.ISyJobSmsConfigDao.Instance.GetAll();
        }

        /// <summary>
        /// 查找第一条记录
        /// </summary>
        /// <returns>SyJobSmsConfig实体</returns>
        /// <remarks>2014-8-5 余勇 创建</remarks>
        public SyJobSmsConfig GetFirst()
        {
            SyJobSmsConfig model = null;
            var smsConfigList= DataAccess.Sys.ISyJobSmsConfigDao.Instance.GetAll();
            if (smsConfigList != null && smsConfigList.Count>0)
            {
                model = smsConfigList.FirstOrDefault(x => x.Status == (int)SystemStatus.任务池短信设置状态.启用);
            }
            return model;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns>受影响的行数</returns>
        /// <remarks>2014-8-5 陈俊 创建</remarks>
       
        public int Delete(int id)
        {
            return DataAccess.Sys.ISyJobSmsConfigDao.Instance.Delete(id);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="syJobSmsConfig"></param>
        /// <returns>受影响的行数</returns>
        /// <remarks>2014-8-5 陈俊 创建</remarks>
        public int Update(SyJobSmsConfig syJobSmsConfig)
        {
            return DataAccess.Sys.ISyJobSmsConfigDao.Instance.Update(syJobSmsConfig);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="syJobSmsConfig"></param>
        /// <returns>返回系统编号</returns>
        /// <remarks>2014-8-5 陈俊 创建</remarks>
        public int Create(SyJobSmsConfig syJobSmsConfig)
        {
            return DataAccess.Sys.ISyJobSmsConfigDao.Instance.Create(syJobSmsConfig);
        }
    }
}
