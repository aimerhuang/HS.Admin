using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.DataAccess.Front;
using System.Transactions;

namespace Hyt.DataAccess.Oracle.Front
{
    public class FeSoftwareListDaoImpl : IFeSoftwareListDao
    {
        /// <summary>
        /// 添加软件列表
        /// </summary>
        /// <param name="model">软件列表实体信息</param>
        /// <returns>返回新建记录的sysno</returns>       
        /// <remarks>2014-01-15 唐永勤 创建</remarks>
        public override int Create(FeSoftwareList model)
        {
            int sysno = 0;
            sysno = Context.Insert<FeSoftwareList>("FeSoftwareList", model)
                        .AutoMap(x => x.SysNo)
                        .ExecuteReturnLastId<int>("SysNo");
            return sysno;
        }

        /// <summary>
        /// 获取指定编号的软件列表项信息
        /// </summary>
        /// <param name="sysNo">软件列表编号</param>
        /// <returns>软件列表实体信息</returns>
        /// <remarks>2014-01-15 唐永勤 创建</remarks>
        public override FeSoftwareList GetEntity(int sysNo)
        {
             FeSoftwareList entity = Context.Select<FeSoftwareList>("*")
                                            .From("FeSoftwareList")
                                            .Where("sysno = @sysno")
                                            .Parameter("sysno", sysNo)
                                            .QuerySingle();
             return entity;
        }

        /// <summary>
        /// 批量更新软件列表
        /// </summary>
        /// <param name="softwareSysNo">软件编号</param>
        /// <param name="list">软件列表</param>
        /// <returns>是否更新成功</returns>
        /// <remarks>2014-01-20 唐永勤 创建</remarks>
        public override bool SaveList(int softwareSysNo, IList<FeSoftwareList> list)
        {
            bool result = false;
            if (softwareSysNo > 0)
            {
                if (softwareSysNo > 0)
                {
                    //先删除
                    Context.Delete("FeSoftwareList")
                        .Where("SoftwareSysNo", softwareSysNo)
                        .Execute();

                    //再添加
                    foreach (FeSoftwareList entity in list)
                    {
                        entity.SoftwareSysNo = softwareSysNo;
                        Context.Insert<FeSoftwareList>("FeSoftwareList", entity)
                            .AutoMap(x => x.SysNo)
                            .Execute();
                    }
                }
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 根据软件编号获取软件列表
        /// </summary>
        /// <param name="softwareSysNo">软件编号</param>
        /// <returns>软件列表</returns>
        /// <remarks>2014-01-20 唐永勤 创建</remarks>
        public override IList<FeSoftwareList> GetListBySoftwareSysNo(int softwareSysNo)
        {
            IList<FeSoftwareList> list = Context.Select<FeSoftwareList>("*")
                                                .From("FeSoftwareList")
                                                .Where("SoftwareSysNo = @SoftwareSysNo")
                                                .Parameter("SoftwareSysNo", softwareSysNo)
                                                .OrderBy("DisplayOrder,SysNo desc")
                                                .QueryMany();
            return list;
        }


    }
}
