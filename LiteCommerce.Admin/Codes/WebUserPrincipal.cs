﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin
{
    public class WebUserPrincipal : IPrincipal
    {
        private readonly IIdentity identity;
        private readonly WebUserData userData;

        /// <summary>
        /// Pricipal dùng với trường hợp người dùng không hợp lệ
        /// </summary>
        public static readonly WebUserPrincipal Anonymous = new AnonymousPrincipal();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="userData"></param>
        public WebUserPrincipal(IIdentity identity, WebUserData userData)
        {
            this.identity = identity;
            this.userData = userData;
        }

        /// <summary>
        /// Kiểm tra Role của người dùng
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool IsInRole(string role)
        {
            string[] arrRole = userData.GroupName.Split(',');
            foreach(var item in arrRole)
            {
                if (role.Equals(item))
                {
                    return true;
                }                
            }
            return false;    
        }

        /// <summary>
        /// Identity
        /// </summary>
        public IIdentity Identity
        {
            get { return this.identity; }
        }

        /// <summary>
        /// Thông tin phiên đăng nhập của tài khoản
        /// </summary>
        public WebUserData UserData
        {
            get { return this.userData; }
        }

        /// <summary>
        /// Anonymous Identity: sử dụng cho trường hợp không đăng nhập
        /// </summary>
        private class AnonymousIdentity : IIdentity
        {
            public string Name
            {
                get { return "Anonymous"; }
            }

            public string AuthenticationType
            {
                get { return null; }
            }

            public bool IsAuthenticated
            {
                get { return false; }
            }
        }

        /// <summary>
        /// Principal sử dụng trong trường hợp không đăng nhập hệ thống
        /// </summary>
        private class AnonymousPrincipal : WebUserPrincipal
        {
            public AnonymousPrincipal()
                : base(new AnonymousIdentity(), new WebUserData()
                {
                    UserID = "",
                    GroupName = "",
                    FullName = "",
                    Email = ""
                })
            {
            }
        }
        public class AuthRole : AuthorizeAttribute
        {
            public override void OnAuthorization(AuthorizationContext filterContext)
            {
                // If they are authorized, handle accordingly
                if (this.AuthorizeCore(filterContext.HttpContext))
                {
                    base.OnAuthorization(filterContext);
                }
                else
                {
                    // Otherwise redirect to your specific authorized area
                    filterContext.Result = new RedirectResult("~/Redirect");
                }
            }
        }
    }
}