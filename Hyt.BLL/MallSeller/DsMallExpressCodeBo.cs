using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.MallSeller;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Infrastructure.Memory;

namespace Hyt.BLL.MallSeller
{
    public class DsMallExpressCodeBo : BOBase<DsMallExpressCodeBo>
    {
        /// <summary>
        /// 获取分销快递物流代码（为空时直接返回物流名称）
        /// </summary>
        /// <param name="mallTypeSysNo">商城类型</param>
        /// <param name="deliverTypeSysno">快递方式</param>
        /// <returns>分销商城快递代码</returns>
        /// <remarks>2014-03-25 唐文均 创建</remarks>
        public string GetDeliveryCompanyCode(int mallTypeSysNo, int deliverTypeSysno)
        {
            var result = string.Empty;
            var ent = IDsMallExpressCodeDao.Instance.GetEntity(mallTypeSysNo, deliverTypeSysno);

            if (ent != null && !string.IsNullOrWhiteSpace(ent.ExpressCode))
            {
                result = ent.ExpressCode;
            }
            else
            {
                var delivertType = Hyt.BLL.Logistics.DeliveryTypeBo.Instance.GetDeliveryType(deliverTypeSysno);
                result = delivertType.DeliveryTypeName;
            }
            return result;
        }
        /// <summary>
        /// 获取分销商城快递代码对象
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        /// <remarks>2015-1-21 缪竞华 创建</remarks>
        public DsMallExpressCode Get(int sysNo)
        {
            return IDsMallExpressCodeDao.Instance.Get(sysNo);
        }

        /// <summary>
        /// 查询经销商城快递代码
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>经销商城快递代码分页数据</returns>
        /// <remarks>2015-1-19 缪竞华 创建</remarks>
        public Model.Pager<Model.Transfer.CBDsMallExpressCode> Query(Model.Parameter.ParaDsMallExpressCodeFilter filter)
        {
            return IDsMallExpressCodeDao.Instance.Query(filter);
        }

        /// <summary>
        /// 添加经销商城快递代码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2015-1-20 缪竞华 创建</remarks>
        public Result Add(Model.DsMallExpressCode model)
        {
            var res = new Result() { Status = false };
            string message = "";

            if (!IsValidModel(model, ref message))
            {
                res.Message = message;
                return res;
            }

            model.ExpressCode = model.ExpressCode.Trim();
            DsMallExpressCode temp = IDsMallExpressCodeDao.Instance.Get(model.MallTypeSysNo, model.DeliveryType, model.ExpressCode);
            if (temp != null)
            {
                res.Message = "已存在相同经销商城快递代码";
                return res;
            }
            model.CreatedDate = DateTime.Now;
            int sysNo = IDsMallExpressCodeDao.Instance.Insert(model);
            res.Status = sysNo > 0;
            res.Message = sysNo > 0 ? "添加成功" : "添加失败";
            return res;
        }

        /// <summary>
        /// 修改经销商城快递代码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2015-1-21 缪竞华 创建</remarks>
        public Result Update(Model.DsMallExpressCode model)
        {
            var res = new Result() { Status = false };
            string message = "";

            if (!IsValidModel(model, ref message))
            {
                res.Message = message;
                return res;
            }

            model.ExpressCode = model.ExpressCode.Trim();
            DsMallExpressCode temp = IDsMallExpressCodeDao.Instance.Get(model.MallTypeSysNo, model.DeliveryType, model.ExpressCode);
            if (temp != null && temp.SysNo != model.SysNo)
            {
                res.Message = "已存在相同经销商城快递代码";
                return res;
            }
       
            int rows = IDsMallExpressCodeDao.Instance.Update(model);
            res.Status = rows > 0;
            res.Message = rows > 0 ? "修改成功" : "修改失败";
            return res;
        }

        /// <summary>
        /// 根据sysNo删除DsMallExpressCode表中的数据
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        /// <remarks>2015-1-19 缪竞华 创建</remarks>
        public Result Delete(int sysNo)
        {
            var res = new Result();
            var r = IDsMallExpressCodeDao.Instance.Delete(sysNo);
            res.Status = r > 0;
            res.Message = r > 0 ? "删除成功" : "删除失败";
            return res;
        }

        /// <summary>
        /// 验证经销商城快递代码对象是否有效
        /// </summary>
        /// <param name="model">经销商城快递代码</param>
        /// <param name="message">错误提示信息</param>
        /// <returns>有效:true,无效:false</returns>
        private bool IsValidModel(Model.DsMallExpressCode model, ref string message)
        {
            DsMallType mallType = BLL.Distribution.DsMallTypeBo.Instance.GetDsMallType(model.MallTypeSysNo);
            if (mallType == null || mallType.Status == (int)Model.WorkflowStatus.DistributionStatus.商城类型状态.禁用)
            {
                message = "商城类型不存在或已禁用";
                return false;
            }

            CBLgDeliveryType deliveryType = BLL.Logistics.DeliveryTypeBo.Instance.GetDeliveryType(model.DeliveryType);
            if (deliveryType == null || deliveryType.Status == (int)Model.WorkflowStatus.LogisticsStatus.配送方式状态.禁用)
            {
                message = "配送方式不存在或已禁用";
                return false;
            }

            if (string.IsNullOrWhiteSpace(model.ExpressCode))
            {
                message = "请填写第三方快递代码";
                return false;
            }
            return true;
        }
    }
}
