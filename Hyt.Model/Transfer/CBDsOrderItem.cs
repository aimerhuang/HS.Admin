using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 分销商升舱订单明细(扩展)
    /// </summary>
    /// <remarks>2013-09-09 朱成果 创建</remarks>
    public class CBDsOrderItem : DsOrderItem
    {
        #region 构造父对象
        /// <summary>
        /// 升舱订单明细关联
        /// </summary>
        /// <remarks>2013-09-09 朱成果 创建</remarks>
        private DsOrderItem _baseInstance;
        /// <summary>
        /// 升舱订单明细关联
        /// </summary>
        /// <remarks>2013-09-09 朱成果 创建</remarks>
        public DsOrderItem BaseInstance
        {
            get
            {
                return _baseInstance;
            }
        }
        /// <summary>
        /// 升舱订单明细关联
        /// </summary>
        /// <param name="instance">父对象</param>
        /// <remarks>2013-09-09 朱成果 创建</remarks>
        public CBDsOrderItem(DsOrderItem instance)
        {
            _baseInstance = instance;
        }
        /// <summary>
        /// 升舱订单明细关联
        /// </summary>
        /// <remarks>2013-09-09 朱成果 创建</remarks>
        private CBDsOrderItem()
        {

        }
        #endregion
        /// <summary>
        /// 升舱订单明细关联
        /// </summary>
        /// <remarks>2013-09-09 朱成果 创建</remarks>
        public   DsOrderItemAssociation CurrectDsOrderItemAssociations { get; set; }
    }
}
