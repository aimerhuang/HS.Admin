using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.DataAccess.Basic;
using Hyt.Model.SystemPredefined;

namespace Hyt.BLL.Basic
{
    /// <summary>
    /// 码表维护
    /// </summary>
    /// <remarks>2013-10-14 唐永勤 创建</remarks>
    public class BsCodeBo : BOBase<BsCodeBo>
    {
        /// <summary>
        /// 根据父级系统编号获取码表
        /// </summary>
        /// <param name="parentSysNo">父级系统编号</param>
        /// <returns>码表</returns>
        /// <remarks>2013-12-04 周唐炬 创建</remarks>
        public List<BsCode> GetCodeList(int parentSysNo)
        {
            return IBsCodeDao.Instance.GetListByParentSysNo(parentSysNo);
        }

        /// <summary>
        /// 删除码表
        /// </summary>
        /// <param name="sysNo">码表系统编号</param>
        /// <returns>影响行</returns>
        /// <remarks>2013-12-04 周唐炬 创建</remarks>
        public int RemoveCode(int sysNo)
        {
            var model = IBsCodeDao.Instance.GetEntity(sysNo);
            if (model == null) throw new HytException("未找该码值，请刷新重试！");
            if (model.ParentSysNo == 0) throw new HytException("不能删除顶级节点，非法操作！");
            return IBsCodeDao.Instance.Remove(model.SysNo);
        }

        /// <summary>
        /// 添加码表记录
        /// </summary>
        /// <param name="model">码表实体信息</param>
        /// <returns>返回新建记录的编号</returns>       
        /// <remarks>2013-10-14 唐永勤 创建</remarks>
        public int Create(BsCode model)
        {
            return IBsCodeDao.Instance.Create(model);
        }

        /// <summary>
        /// 保存品牌信息，添加或修改
        /// </summary>
        /// <param name="model">品牌实体数据</param>
        /// <returns>保存结果对象</returns>
        /// <remarks>2013-07-03 唐永勤 创建</remarks>
        public Result BsCodeSave(BsCode model)
        {
            var result = new Result();
            if (string.IsNullOrEmpty(model.CodeName))
            {
                result.Status = false;
                result.StatusCode = -1;
                result.Message = "码值不能为空！";
            }
            else
            {
                //数据重复性检测
                var isExists = IBsCodeDao.Instance.IsExists(model);
                if (isExists)
                {
                    result.Status = false;
                    result.StatusCode = -2;
                    result.Message = "码表信息已存在";
                    return result;
                }

                //数据操作
                if (model.SysNo > 0)
                {
                    result.StatusCode = IBsCodeDao.Instance.Update(model) == true ? 1 : 0;
                }
                else
                {
                    result.StatusCode = IBsCodeDao.Instance.Create(model);
                }

                if (result.StatusCode > 0)
                {
                    result.Status = true;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取指定编号的码表信息
        /// </summary>
        /// <param name="sysno">码表编号</param>
        /// <returns>码表实体信息</returns>
        /// <remarks>2013-10-14 唐永勤 创建</remarks>
        public BsCode GetEntity(int sysno)
        {
            return IBsCodeDao.Instance.GetEntity(sysno);
        }

        /// <summary>
        /// 根据码表编号更新码表信息
        /// </summary>
        /// <param name="model">码表实体信息</param>
        /// <returns>成功返回true，失败返回false</returns>
        /// <remarks>2013-10-14 唐永勤 创建</remarks>
        public bool Update(BsCode model)
        {
            return IBsCodeDao.Instance.Update(model);
        }

        /// <summary>
        /// 更新码表状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="sysno">码表编号</param>
        /// <returns>更新行数</returns>
        /// <remarks>2013-10-14 唐永勤 创建</remarks>
        public int UpdateStatus(Hyt.Model.WorkflowStatus.BasicStatus.码表状态 status, int sysno)
        {
            return IBsCodeDao.Instance.UpdateStatus(status, sysno);
        }

        /// <summary>
        /// 获取码表列表
        /// </summary>
        /// <param name="pager">码表查询条件</param>
        /// <returns></returns>
        /// <remarks>2013-10-14 唐永勤 创建</remarks>        
        public void GetBsCodeList(ref Pager<BsCode> pager)
        {
            IBsCodeDao.Instance.GetBsCodeList(ref pager);
        }

        #region 获取码值

        /// <summary>
        /// 门店转快递原因
        /// </summary>
        /// <returns>门店转快递原因集合</returns>
        /// <remarks>2013-10-14 吴文强 创建</remarks>
        public List<BsCode> 门店转快递原因()
        {
            return IBsCodeDao.Instance.GetListByParentSysNo(Code.门店转快递原因);
        }

        /// <summary>
        /// 门店延迟自提原因
        /// </summary>
        /// <returns>门店延迟自提原因集合</returns>
        /// <remarks>2013-11-19 余勇 创建</remarks>
        public List<BsCode> 门店延迟自提原因()
        {
            return IBsCodeDao.Instance.GetListByParentSysNo(Code.门店延迟自提原因);
        }

        /// <summary>
        /// 门店转快递原因
        /// </summary>
        /// <returns>门店转快递原因集合</returns>
        /// <remarks>2013-11-13 沈强 创建</remarks>
        public List<BsCode> 出库单作废原因()
        {
            return IBsCodeDao.Instance.GetListByParentSysNo(Code.出库单作废原因);
        }

        /// <summary>
        /// 获取父级系统编号获取码值集合
        /// </summary>
        /// <param name="parentSysNo">父级系统编号</param>
        /// <returns>取码值集合</returns>
        /// <remarks>2014-03-20 周唐炬 创建</remarks>
        public List<BsCode> GetListByParentSysNo(int parentSysNo)
        {
            return IBsCodeDao.Instance.GetListByParentSysNo(parentSysNo);
        }

        /// <summary>
        /// 订单作废原因
        /// </summary>
        /// <returns>订单作废原因</returns>
        /// <remarks>2014-05-26 朱家宏 创建</remarks>
        public List<BsCode> GetCancelReasonCode()
        {
            return IBsCodeDao.Instance.GetListByParentSysNo(Code.订单作废原因);
        }

        #endregion
    }
}
