using System.Web.Mvc;
using Hyt.DataAccess.Sys;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.SystemPredefined;

namespace Hyt.BLL.Sys
{
    /// <summary>
    /// 任务池优先级
    /// </summary>
    /// <remarks>2014-02-28 余勇 创建</remarks>
    public class SyJobPoolPriorityBo : BOBase<SyJobPoolPriorityBo>
    {

        /// <summary>
        /// 插入实体记录
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>返回结果对象</returns>
        /// <remarks>2014-02-28 余勇 创建</remarks>
        public Result Insert(SyJobPoolPriority model)
        {
            var res = new Result();
            var r = ISyJobPoolPriorityDao.Instance.Insert(model);
            if (r > 0) res.Status = true;
            return res;
        }

        /// <summary>
        /// 通过系统编号获取实体信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>返回实体</returns>
        /// <remarks>2014-02-28 余勇 创建</remarks>
        public SyJobPoolPriority Get(int sysNo)
        {
            return ISyJobPoolPriorityDao.Instance.Get(sysNo);
        }

        /// <summary>
        /// 通过优先级获取优先级名称
        /// </summary>
        /// <param name="priority">优先级</param>
        /// <returns>返回优先级名称</returns>
        /// <remarks>2014-02-28 余勇 创建</remarks>
        public string GetPriorityName(int priority)
        {
            var model = ISyJobPoolPriorityDao.Instance.GetByPriority(priority);
            return model != null ? model.PriorityDescription + "(" + priority + ")" : "请选择";
        }

        /// <summary>
        /// 通过优先级获取优先级实体信息
        /// </summary>
        /// <param name="priority">优先级</param>
        /// <returns>返回优先级实体</returns>
        /// <remarks>2014-02-28 余勇 创建</remarks>
        public SyJobPoolPriority GetByPriority(int priority)
        {
            return  ISyJobPoolPriorityDao.Instance.GetByPriority(priority);
        }

        /// <summary>
        /// 通过优先级编码获取实体信息
        /// </summary>
        /// <param name="code">优先级编码</param>
        /// <returns>返回实体</returns>
        /// <remarks>2014-02-28 余勇 创建</remarks>
        public SyJobPoolPriority GetByPriorityCode(string code)
        {
            return ISyJobPoolPriorityDao.Instance.GetByPriorityCode(code);
        }

        /// <summary>
        /// 通过优先级编码获取优先级
        /// </summary>
        /// <param name="code">优先级编码</param>
        /// <returns>返回优先级</returns>
        /// <remarks>2014-02-28 余勇 创建</remarks>
        public int GetPriorityByCode(string code)
        {
            var model = GetByPriorityCode(code);
            return model != null ? model.Priority : 0;
        }

        /// <summary>
        /// 通过SysNo删除该记录
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>返回结果对象</returns>
        /// <remarks>2014-02-28 余勇 创建</remarks>
        public Result Delete(int sysNo)
        {
            var res = new Result();
            var r = ISyJobPoolPriorityDao.Instance.Delete(sysNo);
            if (r > 0) res.Status = true;
            return res;
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="model">实体信息</param>
        /// <returns>返回结果对象</returns>
        /// <remarks>2014-02-28 余勇 创建</remarks>
        public Result Update(SyJobPoolPriority model)
        {
            var res = new Result();
            var r = ISyJobPoolPriorityDao.Instance.Update(model);
            if (r > 0) res.Status = true;
            return res;
        }

        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <returns>list</returns>
        /// <remarks>2014-02-28 余勇 创建</remarks>
        public IList<SyJobPoolPriority> SelectAll()
        {
            return ISyJobPoolPriorityDao.Instance.SelectAll();
        }

        /// <summary>
        /// 判断优先级编码是否为系统内置编码
        /// </summary>
        /// <param name="code">优先级编码</param>
        /// <returns>true or false</returns>
        public bool IsSysPriorityCode(string code)
        {
            var res = false;
            var dic = Hyt.Util.EnumUtil.ToDictionary(typeof(JobPriorityCode));  //所有系统内置编码

            //当优先级编码改变时判断：如为系统内置编码则不允许修改
            if (dic.Values.Contains(code.Trim()))
            {
                res = true;
            }
            return res;
        }
    }
}
