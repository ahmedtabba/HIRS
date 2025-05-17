using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripleA.Utilities;

namespace BoulevardManagement.DTO
{
    public class UserQuickActionDTO:EntityDTO
    {
        public string UserId { get; set; }
        public UserQuickActionEnum Action { get; set; }
        public string ActionDescription => Action.GetEnumDescription();

        public string Icon
        {
            get
            {
                var icon = "";
                switch (Action)
                {
                    case UserQuickActionEnum.AddPatient:
                        icon = "flaticon2-user";
                        break;
                   
                    default:
                        break;
                }

                return icon;
            }
        }

        public string Url
        {
            get
            {
                var url = "";

                switch (Action)
                {
                    case UserQuickActionEnum.AddPatient:
                        url = "/Patient/Create";
                        break;
                    default:
                        break;
                }

                return url;
            }
        }
    }
}
