using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Net;

namespace ENTOS.Module.SystemObjects
{

    public class CustomAuthenticationActiveDirectory : DevExpress.ExpressApp.Security.AuthenticationStandard
    {

        protected Type logonParametersType;

        public override object Authenticate(IObjectSpace objectSpace)
        {

            PermissionPolicyUser user = null;
            if (LogonParameters != null && LogonParameters is AuthenticationStandardLogonParameters)
            {
                var logonParameters = LogonParameters as AuthenticationStandardLogonParameters;
                if (string.IsNullOrEmpty(logonParameters.UserName))
                {
                    //return objectSpace.FindObject<PermissionPolicyUser>(new BinaryOperator("UserName", "Admin"));
                    return base.Authenticate(objectSpace);
                }
                string domain = Module.Helpers.ParameterHelper.GetValue(objectSpace, "Domain");
                if (string.IsNullOrEmpty(domain))
                {
                    return base.Authenticate(objectSpace);
                }
                string userName = logonParameters.UserName;
                string fullUserName = logonParameters.UserName;
                int checkExistDomain = userName.LastIndexOf('\\');
                if (checkExistDomain < 0)
                {
                    fullUserName = domain + "\\" + userName;
                }
                else
                {
                    userName = userName.Substring(checkExistDomain + 1);
                }
                //user = objectSpace.FindObject<PermissionPolicyUser>(new BinaryOperator("UserName", fullUserName));
                user = objectSpace.FindObject<PermissionPolicyUser>(CriteriaOperator.Parse("UserName = ? or UserName = ?", fullUserName, logonParameters.UserName));
                if (user != null)
                {
                    if (string.IsNullOrEmpty(logonParameters.Password))
                    {
                        string primaryServer = Module.Helpers.ParameterHelper.GetValue(objectSpace, "LDAPSever");
                        string secondaryServer = Module.Helpers.ParameterHelper.GetValue(objectSpace, "LDAPSeverSecondary");
                        //if (Authenticate(userName, logonParameters.Password, primaryServer, secondaryServer))
                        if (Authenticate(primaryServer, userName) || Authenticate(secondaryServer, userName) || Authenticate(domain, userName))
                        {
                            Serilog.Log.Information($"Đăng nhập thành công: {logonParameters.UserName}", Environment.MachineName);
                            Serilog.Context.LogContext.PushProperty("UserName", logonParameters.UserName);
                            AddTimeSlot(user);
                            return user;
                        }
                        else
                        {
                            Serilog.Log.Warning($"Đăng nhập lỗi: {logonParameters.UserName}");
                            return null;
                            //throw new ArgumentNullException("Password");
                        }
                    }
                    else
                    {
                        if (user.ComparePassword(logonParameters.Password))
                        {

                            AddTimeSlot(user);
                            return user;
                        }
                        else
                        {
                            //string primaryServer = Module.Helpers.ParameterHelper.GetValue(objectSpace, Module.Helpers.ParameterHelper.GetModuleName(GetType()), "LDAPSever");
                            //string secondaryServer = Module.Helpers.ParameterHelper.GetValue(objectSpace, Module.Helpers.ParameterHelper.GetModuleName(GetType()), "LDAPSeverSecondary");
                            //if (Authenticate(userName, logonParameters.Password, primaryServer, secondaryServer))
                            if (AuthenticateUsingPrincipalcontext(domain, userName, logonParameters.Password))
                            {
                                Serilog.Log.Information($"Đăng nhập thành công");
                                Serilog.Context.LogContext.PushProperty("UserName", logonParameters.UserName);
                                AddTimeSlot(user);
                                return user;
                            }
                            else
                            {
                                Serilog.Log.Warning($"Đăng nhập lỗi: {logonParameters.UserName}");
                                return null;
                                //throw new ArgumentNullException("Password");
                            }
                        }
                    }

                }

                Serilog.Log.Warning($"Đăng nhập lỗi: {logonParameters.UserName}", Dns.GetHostEntry(Dns.GetHostName()).AddressList);
            }

            return null;
            //throw new ArgumentNullException("UserName");
        }

        private void AddTimeSlot(PermissionPolicyUser currentUser)
        {
            var memberTypeInfo = XafTypesInfo.Instance.FindTypeInfo(currentUser.GetType());
            if (memberTypeInfo != null && memberTypeInfo.Type != null)
            {
                System.Reflection.MethodInfo theMethod = memberTypeInfo.Type.GetMethod("CreateTimeSlot");
                if (theMethod != null)
                {
                    theMethod.Invoke(currentUser, null);
                }
            }
        }

        private bool AuthenticateUsingPrincipalcontext
            (string strDomain, string strUserName, string strPassword)
        {
            //Chứng thực bằng Ldap
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain, strDomain);
            try
            {
                bool bValid = ctx.ValidateCredentials(strUserName, strPassword);

                // Additional check to search user in directory.
                if (bValid)
                {
                    UserPrincipal prUsr = new UserPrincipal(ctx);
                    prUsr.SamAccountName = strUserName;

                    var srchUser = new PrincipalSearcher(prUsr);
                    var foundUsr = srchUser.FindOne() as UserPrincipal;
                    return foundUsr != null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new ArgumentNullException("Password");
            }
            finally
            {
                ctx.Dispose();
            }
            return false;
        }

        private bool Authenticate(string primaryDomain, string userName)
        {
            //Chứng thực theo user đang đăng nhập windows
            //return true;
            bool authentic = false;
            try
            {
                System.DirectoryServices.DirectoryEntry entry = new System.DirectoryServices.DirectoryEntry("LDAP://" + primaryDomain,
                    userName, null);
                //entry.RefreshCache();
                object nativeObject = entry.NativeObject;

                authentic = true;
            }
            catch (DirectoryServicesCOMException ex)
            {
                Console.WriteLine(ex.Message);
                //throw new ArgumentNullException("Password");
            }
            return authentic;
        }
        public string GetCurrentUserLogin()
        {
            string result = Environment.UserName;
            try
            {
                string domain = Environment.UserDomainName;
                result = domain + "\\" + result;
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public override bool AskLogonParametersViaUI
        {
            get
            {
                //Hàm này dùng cho chức năng tự động đăng nhập
                return base.AskLogonParametersViaUI; //Fix lỗi phiên bản 24 trở đi
                if (LogonParameters != null && LogonParameters is AuthenticationStandardLogonParameters)
                {
                    var logonParameters = LogonParameters as AuthenticationStandardLogonParameters;
                    if (!string.IsNullOrEmpty(logonParameters.UserName))
                    {
                        return false;
                    }
                }
                return base.AskLogonParametersViaUI;
            }
        }
    }

}
