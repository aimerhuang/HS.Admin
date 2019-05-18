using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.CRM;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.CRM
{
    /// <summary>
    /// 收货人地址数据访问类
    /// </summary>
    /// <remarks>2013－07-02 余勇 创建</remarks>
    public class CrReceiveAddressDaoImpl : ICrReceiveAddressDao
    {

        /// <summary>
        /// 修改会员收货地址
        /// </summary>
        /// <param name="address">收货地址实体</param>
        /// <returns>影响行数</returns>
        /// <remarks>2013－07-02 余勇 创建</remarks>
        public override int UpdateReceiveAddress(Model.CrReceiveAddress address)
        {
            //执行修改
            return Context.Update<Model.CrReceiveAddress>("CrReceiveAddress", address)
                          .AutoMap(c => c.SysNo)
                          .Where("SysNo", address.SysNo)
                          .Execute();
        }

        /// <summary>
        /// 添加会员收货地址
        /// </summary>
        /// <param name="address">收货地址实体</param>
        /// <returns>收货地址系统编号</returns>
        /// <remarks>2013－07-02 余勇 创建</remarks>
        public override int InsertReceiveAddress(Model.CrReceiveAddress address)
        {
            return Context.Insert<Model.CrReceiveAddress>("CrReceiveAddress", address)
                          .AutoMap(x => x.SysNo)
                          .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 根据系统号获取收货地址模型
        /// </summary>
        /// <param name="sysNo">收货地址系统号</param>
        /// <returns>收货地址模型数据</returns>
        /// <remarks>2013-08-05 杨晗 创建</remarks>
        public override CrReceiveAddress GetCrReceiveAddress(int sysNo)
        {
            return
                Context.Sql(@"select * from CrReceiveAddress where SysNO = @0", sysNo)
                       .QuerySingle<CrReceiveAddress>();
        }

        /// <summary>
        /// 根据用户系统号获取收货地址
        /// </summary>
        /// <param name="customerSysNo">用户系统号</param>
        /// <param name="num">获取头几条数</param>
        /// <returns>用户收货地址列表</returns>
        /// <remarks>2013-08-05 杨晗 创建</remarks>
        public override IList<CBCrReceiveAddress> GetCrReceiveAddressByCustomerSysNo(int customerSysNo, int num)
        {
            #region sql条件

            string sql = @"customerSysNo=@customerSysNo and (rownum between 1 and @num)";

            #endregion

            var countBuilder = Context.Select<CBCrReceiveAddress>("ca.*")
                                      .From("CrReceiveAddress ca")
                                      .Where(sql)
                                      .Parameter("customerSysNo", customerSysNo)
                                      .Parameter("num", num)
                                      .OrderBy("SysNo desc").QueryMany();
            return countBuilder;
        }

        /// <summary>
        /// 根据用户系统号获取收货地址
        /// </summary>
        /// <param name="customerSysNo">用户系统号</param>
        /// <returns>用户收货地址列表</returns>
        /// <remarks>2013-08-20 郑荣华 创建</remarks>
        //public override IList<CBCrReceiveAddress> GetCrReceiveAddressList(int customerSysNo)
        //{ 未实现扩展
        //    return Context.Sql("select t.* from CrReceiveAddress t where t.customerSysNo=:0", customerSysNo)
        //                  .QueryMany<CBCrReceiveAddress>();

        //}

        /// <summary>
        /// 删除用户收货地址
        /// </summary>
        /// <param name="sysNo">收货地址系统号</param>
        /// <returns>是否成功</returns>
        /// <remarks>2013-08-05 杨晗 创建</remarks>
        public override bool Delete(int sysNo)
        {
            int rowsAffected = Context.Delete("CrReceiveAddress")
                                      .Where("Sysno", sysNo)
                                      .Execute();
            return rowsAffected > 0;
        }

        /// <summary>
        /// 设置某用户收货地址全部为非默认
        /// </summary>
        /// <param name="customerSysNo">用户系统号</param>
        /// <returns>成功或失败信息</returns>
        /// <remarks>2013-08-05 杨晗 创建</remarks>
        public override bool SetUsedAddress(int customerSysNo)
        {
            int rowsAffected =
                Context.Sql(
                    "update CrReceiveAddress set IsDefault=:IsDefault where CustomerSysNo=@CustomerSysNo")
                       .Parameter("IsDefault", (int)CustomerStatus.是否默认地址.否)
                       .Parameter("CustomerSysNo", customerSysNo)
                       .Execute();
            return rowsAffected > 0;
        }

        /// <summary>
        /// 将某个地址设为该用户的默认收货地址
        /// </summary>
        /// <param name="sysNo">收货地址系统号</param>
        /// <param name="customerSysNo">用户系统号</param>
        /// <returns>成功或失败信息</returns>
        /// <remarks>2013-08-26 周瑜 创建</remarks>
        /// <returns>2014-05-28 周唐炬 修改加入判断地址是否属于该用户</returns>
        public override bool SetDefaultAddress(int sysNo, int customerSysNo)
        {
            using (IDbContext context = Context.UseSharedConnection(true))
            {
                context.Sql(
                    "update CrReceiveAddress set IsDefault=@IsDefault where CustomerSysNo=@CustomerSysNo and SysNo <> @SysNo")
                       .Parameter("IsDefault", (int)CustomerStatus.是否默认地址.否)
                       .Parameter("CustomerSysNo", customerSysNo)
                       .Parameter("SysNo", sysNo)
                       .Execute();

                int rowsAffected =
                    context.Sql(
                        "update CrReceiveAddress set IsDefault=@IsDefault where CustomerSysNo=@CustomerSysNo and SysNo=@SysNo")
                           .Parameter("IsDefault", (int)CustomerStatus.是否默认地址.是)
                           .Parameter("CustomerSysNo", customerSysNo)
                           .Parameter("SysNo", sysNo)
                           .Execute();
                return rowsAffected > 0;
            }

        }
    }
}
