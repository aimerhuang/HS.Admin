using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Basic
{
    /// <summary>
    /// ReceiptManagementDao
    /// </summary>
    /// <remarks>2013-10-9 hw created</remarks>
    public abstract class IReceiptManagementDao : DaoBase<IReceiptManagementDao>
    {
        /// <summary>
        /// query 收款账目
        /// </summary>
        /// <param name="para">ParaBasicReceiptManagement</param>
        /// <param name="id">page index</param>
        /// <param name="pageSize">page size</param>
        /// <returns>list of FnReceiptTitleAssociation</returns>
        /// <remarks>2013-10-9 hw created</remarks>
        public abstract Dictionary<int, List<FnReceiptTitleAssociation>> QueryReceipt(ParaBasicReceiptManagement para, int id, int pageSize);

        /// <summary>
        /// 根据系统编码获取系统编号
        /// </summary>
        /// <param name="code">系统编码</param>
        /// <returns>系统编号</returns>
        /// <remarks>2013-10-10 黄伟 创建</remarks>
        public abstract int GetSysNoByCode(string code);

        /// <summary>
        /// 获取收款科目
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>系统编号</returns>
        /// <remarks>2013-11-13 黄伟 创建</remarks>
        public abstract FnReceiptTitleAssociation GetReceipt(int sysNo);

        /// <summary>
        /// update parentNo for the datas
        /// </summary>
        /// <param name="sysNo">SysNo of the data</param>
        /// <returns>void</returns>
        /// <remarks>2013-10-10 黄伟 创建</remarks>
        public abstract void UpdateParentNo(List<string> sysNo);

        /// <summary>
        /// query all from FnReceiptTitleAssociation
        /// </summary>
        /// <returns>list of FnReceiptTitleAssociation</returns>
        /// <remarks>2013-10-10 黄伟 创建</remarks>
        public abstract List<FnReceiptTitleAssociation> QueryAll();

        /// <summary>
        /// query FnReceiptTitleAssociation by warehouse and paytype
        /// </summary>
        /// <param name="whSysNo">仓库系统编号</param>
        /// <param name="payTypeSysNo">付款方式系统编号</param>
        /// <returns>list of FnReceiptTitleAssociation</returns>
        /// <remarks>2013-10-10 黄伟 创建</remarks>
        public abstract List<FnReceiptTitleAssociation> QueryEasByWhAndPayType(int whSysNo, int payTypeSysNo);

        /// <summary>
        /// 删除财务账目
        /// </summary>
        /// <param name="lstDelSysNos">要删除的财务账目编号集合</param>
        /// <returns></returns>
        /// <remarks>2013-10-9 黄伟 创建</remarks>
        public abstract void DeleteReceipt(List<int> lstDelSysNos);

        /// <summary>
        /// 新增财务账目
        /// </summary>
        /// <param name="models">list of FnReceiptTitleAssociation</param>
        /// <param name="operatorSysNo">操作人员编号</param> 
        /// <returns>Result instance</returns>
        /// <remarks>2013-10-9 黄伟 创建</remarks>
        public abstract void CreateReceipt(List<FnReceiptTitleAssociation> models, int operatorSysNo);

        /// <summary>
        /// 更新财务账目
        /// </summary>
        /// <param name="operatorSysNo">操作人员编号</param>
        /// <param name="models">list of FnReceiptTitleAssociation</param>
        /// <returns></returns>
        /// <remarks>2013-10-9 黄伟 创建</remarks>
        public abstract void UpdateReceipt(List<FnReceiptTitleAssociation> models, int operatorSysNo);

        /// <summary>
        /// 设置科目启用禁用
        /// </summary>
        /// <param name="operatorSysNo">操作人员编号</param>
        /// <param name="id">receipt id</param>
        /// <param name="status">启用或禁用</param>
        /// <returns></returns>
        /// <remarks>2013-10-9 黄伟 创建</remarks>
        public abstract void SetReceiptStatus(int id, int status, int operatorSysNo);
    }

}
