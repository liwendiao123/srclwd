using Newtonsoft.Json;
using Senparc.Core.Models.DataBaseModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Mvc.Models.APIResponse
{
  public  class CompetitionProgramViewModel
    {
        public CompetitionProgramViewModel()
        {
            CateList = new List<FrontCategory>() {

                new FrontCategory{
                     Id = 0,
                      Name = "舞蹈类"
                },
                 new FrontCategory
                 {
                     Id = 1,
                     Name = "声乐类"
                 },
                 new FrontCategory{
                     Id = 2,
                     Name = "器乐类"
                 }
            };
            List = new List<CompetitionProgramVM>();
           // CateName = string.Empty;
        }
        public int Cate { get; set; }

        public string CateName
        {
            get
            {

                var cateitem = this.CateList.Find(x => x.Id == Cate);

                if (cateitem != null)
                {
                    return cateitem.Name;
                }

                return "";
            }

        }

        public List<CompetitionProgramVM> List { get; set; }

      
        [JsonIgnore]
        public List<FrontCategory> CateList { get; set; }
      
    }

    public class CompetitionProgramVM
    {
        public CompetitionProgramVM()
        {
            Id = string.Empty;
            Name = string.Empty;
            Address = string.Empty;
            SignNum = string.Empty;
            BdImgUrl = string.Empty;
            BdImgUrlPwd = string.Empty;
            ProjectMembers = new List<MemberModel>();

        }

        public string Id { get; set; }

        public string Name { get; set; }

        public int Status { get; set; }

        public string Address { get; set; }

        public string SignNum { get; set; }

        public int Sort { get; set; }
        public bool IsSign { get;  set; }
        public string StatusDesc
        {
            get
            {
                switch (this.Status)
                {
                    case 0:
                        return "开始报名";

                    case 1:
                        return "等待抽签";

                    case 2:
                        return "演出中";


                    default:
                        return "开始报名";
                }

            }
        }

       public List<MemberModel> ProjectMembers { get; set; }
        public string BdImgUrl { get; internal set; }
        public string BdImgUrlPwd { get; internal set; }
    }
    public class FrontCategory
    {

        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class MemberModel
    {

        public MemberModel()
        { 
         Id = string.Empty;
         IdCard = string.Empty;
         Name = string.Empty;
         Phone = string.Empty;
         ProjectId = string.Empty;
         IdCardImgUrl = string.Empty;
            Company = string.Empty;
            Nation = string.Empty;
            Duty = string.Empty;

         }
        public string Id{ get; set; }
        public string IdCard{ get; set; }
        public string Name{ get; set; }
        public string Phone{ get; set; }
        public string ProjectId{ get; set; }
        public string IdCardImgUrl { get; set; }
        public string Company { get; internal set; }
        public string Nation { get; internal set; }
        public string Duty { get; internal set; }
    }                   

}
