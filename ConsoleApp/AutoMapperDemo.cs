using AutoMapper;


using Stand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2023
{
    internal class AutoMapperDemo
    {
      public void Test()
        {
            List<Products> list=new List<Products>();
            Products product = new Products();

            product.ProductName = "ProductName";
            list.Add(product);
            product = new Products();

            product.ProductName = "ProductName1";
            list.Add(product);
            //Mapper.CreateMap<DTO, Model>();
            //DTO dtoData = GetdtoDataFromDB();
            //Model modelData = Mapper.Map<DTO, Model>(dtoData);
            //var m = list.<ProductDto>();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.RecognizePrefixes("before");//前缀
                cfg.RecognizePostfixes("after");//后缀
                cfg.CreateMap<Products, ProductsDto>();
            });
            var mapper = configuration.CreateMapper();
            var dest = mapper.Map <List< Products>, List<ProductsDto>> (list);

        }
    }
}
