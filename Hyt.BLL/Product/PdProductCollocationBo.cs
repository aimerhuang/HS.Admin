using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Authentication;
using Hyt.Model.Transfer;
using Hyt.DataAccess.Product;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Product
{
    /// <summary>
    /// 商品搭配销售
    /// </summary>
    /// <remarks>2013-07-09 邵斌 创建</remarks>
    public class PdProductCollocationBo : BOBase<PdProductCollocationBo>
    {
        /// <summary>
        /// 读取一个商品的搭配销售商品集合
        /// </summary>
        /// <param name="productSysNo">主商品系统编号(主商品系统编号是作为Code来使用)</param>
        /// <returns>搭配销售的商品列表</returns>
        /// <remarks>2013-07-09 邵斌 创建</remarks>
        public IList<CBProductListItem> GetList(int productSysNo)
        {
            return IPdProductCollocationDao.Instance.GetList(productSysNo);
        }

        /// <summary>
        /// 添加搭配商品
        /// </summary>
        /// <param name="productCollocation">搭配商品对象</param>
        /// <returns>返回 true:操作成功 false:操作失败</returns>
        /// <remarks>2013-07-09 邵斌 创建</remarks>
        public bool Create(PdProductCollocation productCollocation)
        {
            return IPdProductCollocationDao.Instance.Create(productCollocation);
        }

        /// <summary>
        /// 删除搭配商品
        /// </summary>
        /// <param name="masterProductSysNo">主商品系统编号(主商品系统编号是作为Code来使用)</param>
        /// <param name="collocationProductSysNo">搭配商品系统编号</param>
        /// <returns>返回 true:操作成功 false:操作失败</returns>
        /// <remarks>2013-07-09 邵斌 创建</remarks>
        public bool Delete(int masterProductSysNo=0, int collocationProductSysNo=0)
        {
            return IPdProductCollocationDao.Instance.Delete(masterProductSysNo, collocationProductSysNo);
        }

        /// <summary>
        /// 添加一组搭配商品
        /// </summary>
        /// <param name="productCollocations">搭配商品对象组</param>
        /// <returns>返回 true:操作成功 false:操作失败</returns>
        /// <remarks>2013-07-09 邵斌 创建</remarks>
        public bool Create(IList<PdProductCollocation> productCollocations)
        {
            return IPdProductCollocationDao.Instance.Create(productCollocations);
        }

          /// <summary>
          /// 更新商品的搭配销售商品列表
          /// </summary>
          /// <param name="masterProductSysNo">主商品系统编号(作为code查询条件)</param>
          /// <param name="removeProductSysNoList">要从搭配商品列表中移除的商品列表</param>
          /// <param name="newProductSysNoList">要被加入到搭配商品中的商品列表</param>
          /// <returns>true：更新成功 false:更新失败</returns>
         /// <remarks>2013-07-09 邵斌 创建</remarks>
        public bool Update(int masterProductSysNo, int[] removeProductSysNoList, int[] newProductSysNoList)
          {
              if (IsExist(masterProductSysNo, newProductSysNoList))
              {
                  return false;
              }

              //管理员账号
              int adminSysNo = AdminAuthenticationBo.Instance.Current.Base.SysNo;

               //添加新商品到搭配销售商品
              IList<PdProductCollocation> newPdProductCollocations = new List<PdProductCollocation>();
              foreach (int productSysNo in newProductSysNoList)
              {
                  newPdProductCollocations.Add(new PdProductCollocation()
                      {
                          Code = masterProductSysNo,
                          ProductSysNo = productSysNo,
                          CreatedBy = adminSysNo,
                          CreatedDate = DateTime.Now
                      });
              }

              return IPdProductCollocationDao.Instance.Update(masterProductSysNo, removeProductSysNoList, newPdProductCollocations);

          }

        /// <summary>
        /// 检查搭配商品是否已经在主商品搭配商品列表中
        /// </summary>
        /// <param name="masterProductSysNo">主商品系统编号</param>
        /// <param name="collocationProductSysNo">搭配商品系统编号</param>
        /// <returns>返回 true:存在 false:不存在</returns>
        /// <remarks>2013-07-09 邵斌 创建</remarks>
        public bool IsExist(int masterProductSysNo, int collocationProductSysNo)
        {
            return IPdProductCollocationDao.Instance.IsExist(masterProductSysNo, collocationProductSysNo);
        }

        /// <summary>
        /// 批量插入搭配销售商品
        /// </summary>
        /// <param name="collocation">搭配销售商品集合</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－07-22 苟治国 创建</remarks>
        public int Insert(IList<PdProductCollocation> collocation, int productSysno)
        {
            int result = 0;
            
            IPdProductCollocationDao.Instance.Delete(productSysno, 0);

            if (collocation != null)
            {

                for (int i = 0; i < collocation.Count; i++)
                {
                    var model = new PdProductCollocation();
                    model.ProductSysNo = collocation[i].ProductSysNo;
                    model.Code = collocation[i].Code;
                    model.CreatedBy = AdminAuthenticationBo.Instance.GetAuthenticatedUser().SysNo;
                    model.CreatedDate = DateTime.Now;
                    result = IPdProductCollocationDao.Instance.Insert(model);
                }

                if (result > 0)
                {
                    //用户操作日志
                    BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("商品{0}创建搭配销售", productSysno),
                                                 LogStatus.系统日志目标类型.商品搭配销售, productSysno,
                                                 AdminAuthenticationBo.Instance.Current.Base.SysNo);
                }
                else
                {
                    //用户操作日志
                    BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("商品{0}创建搭配销售失败", productSysno),
                                                  LogStatus.系统日志目标类型.商品搭配销售, productSysno,
                                                  AdminAuthenticationBo.Instance.Current.Base.SysNo);
                }
            }
            else
            {
                return 1;
            }

            return result;
        }

        /// <summary>
         /// 检查一组搭配商品是否已经在主商品搭配商品列表中
         /// </summary>
         /// <param name="masterProductSysNo">主商品系统编号</param>
         /// <param name="collocationProductSysNoList">搭配商品系统编号数组</param>
         /// <returns>返回 true:存在 false:不存在</returns>
         /// <remarks>2013-07-09 邵斌 创建</remarks>
         public bool IsExist(int masterProductSysNo, int[] collocationProductSysNoList)
         {
             return IPdProductCollocationDao.Instance.IsExist(masterProductSysNo, collocationProductSysNoList);
         }

       
    }
}
