using Hyt.DataAccess.Atten;
using Hyt.Model;
using Hyt.Model.Generated;
using Hyt.Model.WorkflowStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Atten
{
    public class ClockTypeBo : BOBase<ClockTypeBo>
    {
        /// <summary>
        /// 获取签到类型
        /// </summary>
        /// <param name="sysNo">编码</param>
        /// <returns>model</returns>
        /// <remarks>2016-05-26 周海鹏 创建</remarks>
        public ASClockType Get(int sysNo)
        {
            return ClockTypeDao.Instance.Select(sysNo);
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="currentPage">当前页号</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="status">状态</param>
        /// <param name="keyword">名称</param>
        /// <returns>分页</returns>
        /// <remarks>2016-05-26 周海鹏 创建</remarks>
        public Pager<ASClockType> GetPagerList(int currentPage, int pageSize, int? status, string keyword)
        {
            return ClockTypeDao.Instance.SelectAll(currentPage, pageSize, status, keyword);
        }
        /// <summary>
        /// 保存修改
        /// </summary>
        /// <param name="clocktype">类型</param>
        /// <returns>Result</returns>
        /// <remarks>2016-05-26 周海鹏 创建</remarks>
        public Result SaveClockType(ASClockType clocktype)
        {
            if (clocktype == null)
                throw new ArgumentNullException();

            var result = new Result();

            if (ExistsPrivilege(clocktype.TypeName, clocktype.SysNo))
            {
                result.Status = false;
                result.Message = "不能存在相同的权限名称或代码。";
                return result;
            }

            var savingModel = ClockTypeDao.Instance.Select(clocktype.SysNo);

            savingModel.TypeSort = clocktype.TypeSort;
            savingModel.Remark = clocktype.Remark;
            savingModel.TypeName = clocktype.TypeName;
            savingModel.TypeState = clocktype.TypeState;
            savingModel.TypeTime = DateTime.Now;
            result.Status = ClockTypeDao.Instance.Update(savingModel);
            result.Message = "保存成功。";

            return result;
        }
        /// <summary>
        /// 已存在的检查
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="exceptedSysNo">排除的编号</param>
        /// <returns>t:存在 f:不存在</returns>
        /// <remarks>2016-05-26 周海鹏 创建</remarks>
        public bool ExistsPrivilege(string name = null, int exceptedSysNo = 0)
        {
            var allPrivileges = ClockTypeDao.Instance.SelectAll();

            var existedPrivilege =
                allPrivileges.Where(o => (o.TypeName == name) && o.SysNo != exceptedSysNo);

            return existedPrivilege.Any();
        }
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="clocktype">类型</param>
        /// <returns>Result</returns>
        /// <remarks>2016-05-26 周海鹏 创建</remarks>
        public Result CreateClockType(ASClockType clocktype)
        {
            if (clocktype == null)
                throw new ArgumentNullException();

            var result = new Result();

            if (ExistsPrivilege(clocktype.TypeName))
            {
                result.Status = false;
                result.Message = "不能存在相同的名称。";
                return result;
            }

            //创建
            result.StatusCode = ClockTypeDao.Instance.Insert(clocktype);
            result.Status = result.StatusCode > 0;
            result.Message = "创建成功。";

            return result;
        }
        /// <summary>
        /// 切换状态 (启用/禁用)
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>t:成功 f:失败</returns>
        /// <remarks>2016-05-26 周海鹏 创建</remarks>
        public bool ChangeClockTypeStatus(int sysNo)
        {
            var allClockType = ClockTypeDao.Instance.SelectAll();

            var selectedClockType = allClockType.SingleOrDefault(o => o.SysNo == sysNo);
            if (selectedClockType == null) return false;

            var changedStatus = selectedClockType.TypeState == (int)SystemStatus.权限状态.禁用
                                    ? (int)SystemStatus.权限状态.启用
                                    : (int)SystemStatus.权限状态.禁用;

            selectedClockType.TypeState = changedStatus;
            var r = ClockTypeDao.Instance.Update(selectedClockType);

            return r;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>Result</returns>
        /// <remarks>2016-05-26 周海鹏 创建</remarks>
        public Result RemoveClockType(int sysNo)
        {
            var ClockType = Get(sysNo);
            var result = new Result { Status = false };
            if (ClockType != null)
            {
                result.Status = ClockTypeDao.Instance.Delete(sysNo);
                result.Message = "删除成功。";
            }
            else
            {
                result.Status = false;
                result.Message = "删除失败。";
            }
            return result;
        }
    }
}
