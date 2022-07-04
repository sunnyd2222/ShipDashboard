using System.DirectoryServices.AccountManagement;
using Microsoft.Extensions.Configuration;

namespace LoginComponent

{
    public class ActiveDirectory
    {
        public string User_Name { get; set; }
        public string oDisplayName { get; set; }
        public string Domain { get; set; }
        public string Group { get; set; }
        public string AdminGroup { get; set; }
        public bool Has_Group_Access { get; set; }
        public bool User_Exist { get; set; }

        public IConfiguration Configuration { get; }


        public bool CheckIfUserInAd(string DomainEMEA = "EMEA", string DomainNA = "NA")
        {
            //User_Name = "";
            User_Exist = false;

            //fixed administrators group for MTOWN
            AdminGroup = "COF-MNPP-Prod_PC_Support"; 

            //Group = AdGroupName;
            Has_Group_Access = false;
            bool isMember = false;

            if (Group.ToUpper().Contains(User_Name.ToUpper()))
            {
                isMember = true;
            }
            else
            {
                var oPrincilContext = new PrincipalContext(ContextType.Domain, DomainEMEA);
                var oPrincilContextUser = new PrincipalContext(ContextType.Domain, DomainEMEA);
                var oPrincilContextUserNa = new PrincipalContext(ContextType.Domain, DomainNA);
                
                UserPrincipal oUserPrincipal = UserPrincipal.FindByIdentity(oPrincilContextUser, User_Name);
                UserPrincipal oUserPrincipalNa = UserPrincipal.FindByIdentity(oPrincilContextUserNa, User_Name);
                GroupPrincipal oGroupPrincipal = GroupPrincipal.FindByIdentity(oPrincilContext, Group);
                GroupPrincipal oAdminGroupPrincipal = GroupPrincipal.FindByIdentity(oPrincilContext, AdminGroup);

                
                
                if (oUserPrincipal is object & oGroupPrincipal is object)
                {
                    isMember = oUserPrincipal.IsMemberOf(oGroupPrincipal);
                    if (isMember == false)
                    {
                        isMember = oUserPrincipal.IsMemberOf(oAdminGroupPrincipal);
                    }
                    
                    oDisplayName = oUserPrincipal.DisplayName;
                }
                else if ( oUserPrincipalNa is object & oGroupPrincipal is object)
                {
                    //if user not fount in emea group search in NA
                    if (isMember == false)
                    {
                        isMember = oUserPrincipalNa.IsMemberOf(oGroupPrincipal);
                        if (isMember == false)
                        {
                            isMember = oUserPrincipal.IsMemberOf(oAdminGroupPrincipal);
                        }
                        oDisplayName = oUserPrincipalNa.DisplayName;
                    }
                }
            }
            if (isMember)
            {
                Has_Group_Access = true;
            }
            
            return Has_Group_Access;
        }
    }
}