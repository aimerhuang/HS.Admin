using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Hyt.Service.Implement.MallSeller
{
    using Extra.UpGrade.Model;
    using Extra.UpGrade.Provider;
    using Hyt.Model;
    using Hyt.Service.Contract.MallSeller;
    using Hyt.Service.Contract.MallSeller.Model;

    public class ExtendMallOrder : BaseService, IExtendMallOrder
    {

        public Result LogisticsSend(LogisticsSendRequest request)
        {
            var result = new Result { Status = false };

            try
            {
                result = UpGradeProvider.GetInstance(request.AuthInfo.MallType).SendDelivery(
                    new DeliveryParameters { CompanyCode = request.CompanyCode, HytExpressNo = request.ExpressCode, MallOrderId = request.OrderID },
                    new Extra.UpGrade.Model.AuthorizationParameters { AuthorizationCode = request.AuthInfo.AuthorizationCode, MallType = request.AuthInfo.MallType, ShopAccount = request.AuthInfo.ShopAccount });
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            BLL.MallSeller.DsDealerLogBo.Instance.Insert(new DsDealerLog
            {
                CreatedBy = 0,
                CreatedDate = DateTime.Now,
                Status = result.Status ? 20 : 10,
                SoOrderSysNo = 0,
                OrderTransactionSysNo = "",
                MallTypeSysNo = request.AuthInfo.MallType,
                MallOrderId = request.OrderID,
                LogContent = string.Format("Type:发货；{0}", result.Message),
                LastUpdateDate = DateTime.Now,
                LastUpdateBy = 0
            });

            return result;
        }

        public Result UpdateMemo(UpdateMemoRequest request)
        {
            var result = new Result { Status = false };

            try
            {
                //result = UpGradeProvider.GetInstance(request.AuthInfo.MallType).UpdateTradeRemarks(
                //   new TaobaoRemarksParameters { Flag = (FlagType)Convert.ToInt32(request.MemoType), MallOrderId = request.OrderID, RemarksContent = request.MemoContent, Reset = false },
                //   new Extra.UpGrade.Model.AuthorizationParameters { AuthorizationCode = request.AuthInfo.AuthorizationCode, MallType = request.AuthInfo.MallType, ShopAccount = request.AuthInfo.ShopAccount });
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            BLL.MallSeller.DsDealerLogBo.Instance.Insert(new DsDealerLog
            {
                CreatedBy = 0,
                CreatedDate = DateTime.Now,
                Status = result.Status ? 20 : 10,
                SoOrderSysNo = 0,
                OrderTransactionSysNo = "",
                MallTypeSysNo = request.AuthInfo.MallType,
                MallOrderId = request.OrderID,
                LogContent = string.Format("Type:修改备注；{0}", result.Message),
                LastUpdateDate = DateTime.Now,
                LastUpdateBy = 0
            });

            return result;
        }
    }
}
