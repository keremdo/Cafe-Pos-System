using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dmlcafepos.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DmlCafePos.TagHelpers
{

    [HtmlTargetElement("td",Attributes ="asp-role-user")]
    public class RolesByUsersTagHelper:TagHelper
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        public RolesByUsersTagHelper(UserManager<AppUser> userManager,RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HtmlAttributeName("asp-role-user")]
        public string RoleId { get; set; } = null!;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var userNames = new List<string>();
            var role = await _roleManager.FindByIdAsync(RoleId);

            if(role != null && role.Name != null)
            {
                foreach(var user in _userManager.Users)
                {
                    if(await _userManager.IsInRoleAsync(user,role.Name))
                    {
                        userNames.Add(user.UserName ?? "");
                    }
                   
                }output.Content.SetContent(userNames.Count > 0 ? userNames.Count.ToString() : "Kullanıcı Yok");
            }
        }
    }
}