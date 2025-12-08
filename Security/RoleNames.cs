using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzWinApp_Web.Security
{
    public static class RoleNames
    {
        public const string ReadOnly = @"UMZYAD\App_ReadOnly";
        public const string ReadWrite = @"UMZYAD\App_ReadWrite";
        public const string Admin = @"UMZYAD\App_Admin";
    }
}
