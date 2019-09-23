using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Mvc.Models
{
    public class CateManager
    {

       
        public static List<Category> GetAllCateList()
        {

            return new List<Category> {

                new Category
            {
                Id = 0,
                Name = "舞蹈类"
            },
                 new Category
                 {
                     Id = 1,
                     Name = "声乐类"
                 },
                 new Category
                 {
                     Id = 2,
                     Name = "器乐类"
                 }
            };

            
        }


        public static string GetCateName(int cate)
        {


            var cateitem = GetAllCateList().Find(x => x.Id == cate);

            if (cateitem != null)
            {
                return cateitem.Name;
            }

            return "";
        }


    }

    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
