using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    /// <summary>
    ///Linq拓展:Enumerable静态类型实现的IEnumerable<int>成员的拓展方法
    /// </summary>
    class LinqDemo
    {

        public void  Test()
        {

        }

        private void  GroupBy()
        {

            #region //Linq  拓展

            //sum()
            //return details.GroupBy(g => new
            //{
            //    g.Id,
            //    g.CreateDate,
            //    g.Amount,
            //    g.FullName
            //})
            //.Select(m =>
            //{
            //    return new SettlementVoucher
            //    {
            //        Id = m.Key.Id,
            //        CreateDate = m.Key.CreateDate,
            //        Amount = m.Key.Amount,
            //        FullName = m.Key.FullName,
            //        Scans = m.Where(p => p.Id == m.Key.Id && p.ImageFileId != null).Select(p => _imgCache.GetImageSrc(p.ImageFileId.Value))
            //    };
            //});



            //count()
            //   var sums = empList
            //.GroupBy(x => new { x.Age, x.Sex })
            //.Select(group => new {
            //    Peo = group.Key,
            //    Count = group.Count()
            //});

            #endregion

            #region   Linq to EF
            // Linq to EF
            //var sums2 = from emp in empList
            //            group emp by new { emp.Age, emp.Sex } into g
            //            select new { Peo = g.Key, Count = g.Count() };
            #endregion
        }
    }
}
