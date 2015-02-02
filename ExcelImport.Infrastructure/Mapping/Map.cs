using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using AutoMapper;
using ExcelImport.Infrastructure.Entities;

namespace ExcelImport.Infrastructure.Mapping
{
    public static class Mapper
    {
        static Mapper()
        {
            AutoMapper.Mapper.CreateMap<object, Product>();

        }

        public static Product ToProduct(this IDictionary<string, object> entry)
        {
            return AutoMapper.Mapper.DynamicMap<object, Product>(entry);
        }
    }
}
