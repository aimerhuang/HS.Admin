using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.DataAccess.Front;
using System.Transactions;

namespace Hyt.BLL.Front
{
    public class FeSoftwareBo : BOBase<FeSoftwareBo>
    {
        /// <summary>
        /// 保存软件下载信息，添加或修改
        /// </summary>
        /// <param name="model">软件下载实体数据</param>
        /// <param name="list">软件列表</param>
        /// <returns>保存结果对象</returns>
        /// <remarks>2014-01-17 唐永勤 创建</remarks>
        public Result Save(FeSoftware model,IList<FeSoftwareList> list)
        {
            Result result = new Result();
            if (string.IsNullOrEmpty(model.Title))
            {
                result.StatusCode = -1;
            }
            else
            {
                //数据重复性检测
                bool isExists = IFeSoftwareDao.Instance.IsExists(model);
                if (isExists)
                {
                    result.StatusCode = -2;
                    result.Message = "软件下载标题已存在";
                    return result;
                }

                if (list.Count < 1)
                {
                    result.StatusCode = -3;
                    result.Message = "未上传软件";
                    return result;
                }
                //使用事务保存数据一致性
                
                    //数据操作
                    if (model.SysNo > 0)
                    {
                        result.Status = IFeSoftwareDao.Instance.Update(model);
                    }
                    else
                    {
                        model.Status = (int)Hyt.Model.WorkflowStatus.ForeStatus.软件下载状态.待审;
                        model.SysNo = IFeSoftwareDao.Instance.Create(model);
                        if (model.SysNo > 0)
                        {
                            result.Status = true;
                        }
                    }
                    if (result.Status)
                    {
                        result.Status = IFeSoftwareListDao.Instance.SaveList(model.SysNo, list);
                        
                        if (result.Status)
                        {
                            result.StatusCode = 1;
                            result.Message = "软件信息保存成功";
                        }
                        else
                        {
                            result.Message = "软件列表保存失败";
                        }
                    }
                
            }
            return result;
        }

        /// <summary>
        /// 获取指定编号的软件分类信息
        /// </summary>
        /// <param name="sysNo">软件分类编号</param>
        /// <returns>软件分类实体信息</returns>
        /// <remarks>2014-01-17 唐永勤 创建</remarks>
        public FeSoftware GetEntity(int sysNo)
        {
            return IFeSoftwareDao.Instance.GetEntity(sysNo);
        }

        /// <summary>
        /// 更新软件状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="sysNo">软件编号</param>
        /// <returns>更新行数</returns>
        /// <remarks>2014-01-21 唐永勤 创建</remarks>
        public int UpdateStatus(Hyt.Model.WorkflowStatus.ForeStatus.软件下载状态 status, int sysNo)
        {
            return IFeSoftwareDao.Instance.UpdateStatus(status, sysNo);
        }

        /// <summary>
        /// 获取软件分类列表
        /// </summary>
        /// <param name="pager">软件分类查询条件</param>
        /// <returns>软件分类列表</returns>
        /// <remarks>2014-01-17 唐永勤 创建</remarks>
        public Pager<FeSoftware> GetList(Pager<FeSoftware> pager)
        {
            return IFeSoftwareDao.Instance.GetList(pager);
        }

          /// <summary>
        /// 根据软件编号获取软件列表
        /// </summary>
        /// <param name="softwareSysNo">软件编号</param>
        /// <returns>软件列表</returns>
        /// <remarks>2014-01-20 唐永勤 创建</remarks>
        public IList<FeSoftwareList> GetListBySoftwareSysNo(int softwareSysNo)
        {
            return IFeSoftwareListDao.Instance.GetListBySoftwareSysNo(softwareSysNo);
        }

        /// <summary>
        /// 获取指定编号的软件列表项信息
        /// </summary>
        /// <param name="sysNo">软件列表编号</param>
        /// <returns>软件列表实体信息</returns>
        /// <remarks>2014-01-15 唐永勤 创建</remarks>
        public FeSoftwareList GetFeSoftwareListEntity(int sysNo)
        {
            return IFeSoftwareListDao.Instance.GetEntity(sysNo);
        }

        /// <summary>
        /// 添加软件列表记录
        /// </summary>
        /// <param name="model">软件列表实体</param>
        /// <returns>新建记录的编号</returns>
        /// <remarks>2014-01-21 唐永勤 创建</remarks>
        public int CreateSoftwareList(FeSoftwareList model)
        {
            return IFeSoftwareListDao.Instance.Create(model);
        }

    }
}
