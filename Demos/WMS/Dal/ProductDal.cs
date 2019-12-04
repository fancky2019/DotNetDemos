using Common;
using CRM.Model.EntityModels.WMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Model.QueryModels;
using WMS.Model.ViewModels;

namespace WMS.Dal
{
    public class ProductDal
    {
        public Tuple<int, List<ProductVM>> LoadData(ProductQM qm)
        {
            try
            {
                using (WMSDbContext dbContext = new Dal.WMSDbContext())
                {
                    var result = from p in dbContext.Product
                                 join s in dbContext.Stock on p.StockID equals s.ID into productStockTemp
                                 from pt in productStockTemp.DefaultIfEmpty()
                                 join b in dbContext.BarCode on p.BarCodeID equals b.ID into productBarCodeTemp
                                 from pb in productBarCodeTemp.DefaultIfEmpty()
                                 join sk in dbContext.Sku on p.SkuID equals sk.ID into productSkuTemp
                                 from ps in productSkuTemp.DefaultIfEmpty()
                                 where p.Status == 1
                                 select new ProductVM
                                 {
                                     ID = p.ID,
                                     GUID = p.GUID,
                                     ProductName = p.ProductName,
                                     ProductStyle = p.ProductStyle,
                                     Price = p.Price,
                                     CreateTime = p.CreateTime,
                                     ModifyTime = p.ModifyTime,
                                     Status = p.Status,
                                     SkuID = p.SkuID,
                                     StockID = p.StockID,
                                     BarCodeID = p.BarCodeID,
                                     StockName = pt == null ? null : pt.StockName,
                                     Count = p.Count,
                                     Unit = ps == null ? null : ps.Unit,
                                     Code = pb == null ? null : pb.Code
                                 };
                    if (!string.IsNullOrEmpty(qm.ProductName))
                    {
                        result = result.Where(p => p.ProductName.Contains(qm.ProductName));
                    }
                    if (!string.IsNullOrEmpty(qm.ProductStyle))
                    {
                        result = result.Where(p => p.ProductName.Contains(qm.ProductStyle));
                    }
                    if (!string.IsNullOrEmpty(qm.Code))
                    {
                        result = result.Where(p => p.ProductName.Contains(qm.Code));
                    }
                    var count = result.Count();
                    var list = result.OrderBy(p => p.CreateTime).Skip(qm.Skip).Take(qm.Take).ToList();
                    return Tuple.Create<int, List<ProductVM>>(count, list);
                }
            }
            catch (Exception ex)
            {
                Log.Error<ProductDal>(ex.ToString());
                return null;
            }
        }
        public int Add(Product product)
        {
            try
            {
                using (WMSDbContext dbContext = new Dal.WMSDbContext())
                {
                    dbContext.Product.Add(product);
                    dbContext.SaveChanges();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                Log.Error<ProductDal>(ex.ToString());
                return 0;
            }
        }

        public int Update(Product product)
        {
            try
            {
                using (WMSDbContext dbContext = new Dal.WMSDbContext())
                {
                    Product pro = dbContext.Product.Where(p => p.ID == product.ID).FirstOrDefault();
                    pro.ProductName = product.ProductName;
                    pro.ProductStyle = product.ProductStyle;
                    pro.StockID = product.StockID;
                    pro.Price = product.Price;
                    pro.ModifyTime = DateTime.Now;
                    dbContext.SaveChanges();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                Log.Error<ProductDal>(ex.ToString());
                return 0;
            }
        }

        public int Delete(Product product)
        {
            try
            {
                using (WMSDbContext dbContext = new Dal.WMSDbContext())
                {
                    Product pro = dbContext.Product.Where(p => p.ID == product.ID).FirstOrDefault();
                    dbContext.Product.Remove(pro);
                    dbContext.SaveChanges();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                Log.Error<ProductDal>(ex.ToString());
                return 0;
            }
        }

        public bool Exist(Product product)
        {
            try
            {
                using (WMSDbContext dbContext = new Dal.WMSDbContext())
                {
                    return dbContext.Product.Any(p => p.ProductName == product.ProductName && p.ProductStyle == product.ProductStyle);
                }
            }
            catch (Exception ex)
            {
                Log.Error<ProductDal>(ex.ToString());
                return true;
            }
        }

        public List<ProductVM> QueryWithNoPage(ProductQM qm)
        {
            try
            {
                using (WMSDbContext dbContext = new Dal.WMSDbContext())
                {
                    var result = from p in dbContext.Product
                                 join s in dbContext.Stock on p.StockID equals s.ID into stockTemp
                                 from st in stockTemp.DefaultIfEmpty()
                                 join s in dbContext.Sku on p.SkuID equals s.ID into skuTemp
                                 from sk in skuTemp.DefaultIfEmpty()
                                 select new ProductVM()
                                 {
                                     ID = p.ID,
                                     GUID = p.GUID,
                                     ProductName = p.ProductName,
                                     ProductStyle = p.ProductStyle,
                                     Price = p.Price,
                                     CreateTime = p.CreateTime,
                                     ModifyTime = p.ModifyTime,
                                     Status = p.Status,
                                     SkuID = p.SkuID,
                                     StockID = p.StockID,
                                     BarCodeID = p.BarCodeID,
                                     StockName = st == null ? null : st.StockName,
                                     Count = p.Count,
                                     Unit = sk == null ? null : sk.Unit
                                 };
                    if (!string.IsNullOrEmpty(qm.ProductName))
                    {
                        result = result.Where(p => p.ProductName.Contains(qm.ProductName));
                    }
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error<ProductDal>(ex.ToString());
                return null;
            }
        }
    }
}
