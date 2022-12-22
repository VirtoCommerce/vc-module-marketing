using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GraphQL;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using VirtoCommerce.CustomerModule.Core.Model;
using VirtoCommerce.CustomerModule.Core.Services;
using VirtoCommerce.ExperienceApiModule.Core;
using VirtoCommerce.ExperienceApiModule.Core.BaseQueries;
using VirtoCommerce.ExperienceApiModule.Core.Extensions;
using VirtoCommerce.MarketingModule.Core.Model.DynamicContent;
using VirtoCommerce.MarketingModule.ExperienceApi.Schemas;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.MarketingModule.ExperienceApi.Queries
{
    public class EvaluateDynamicContentQueryBuilder : QueryBuilder<EvaluateDynamicContentQuery, EvaluateDynamicContentResult, EvaluateDynamicContentResultType>
    {
        protected override string Name => "evaluateDynamicContent";

        private readonly Func<UserManager<ApplicationUser>> _userManagerFactory;
        private readonly IMemberService _memberService;

        public EvaluateDynamicContentQueryBuilder(
            IMediator mediator,
            IAuthorizationService authorizationService,
            Func<UserManager<ApplicationUser>> userManagerFactory,
            IMemberService memberService)
            : base(mediator, authorizationService)
        {
            _userManagerFactory = userManagerFactory;
            _memberService = memberService;
        }

        protected override async Task BeforeMediatorSend(IResolveFieldContext<object> context,
            EvaluateDynamicContentQuery request)
        {
            var userGroups = await GetUserGroups(context);
            request.UserGroups = request.UserGroups != null ? userGroups.Intersect(request.UserGroups).ToArray() : userGroups.ToArray();

            await base.BeforeMediatorSend(context, request);
        }


        private async Task<IList<string>> GetUserGroups(IResolveFieldContext<object> context)
        {
            var userId = GetUserId(context);
            var contact = await GetContact(userId);

            if (contact == null)
            {
                return Array.Empty<string>();
            }

            var userGroups = contact.Groups.ToList();

            var organization = await GetOrganization(contact.Organizations.FirstOrDefault());
            if (organization?.Groups != null)
            {
                userGroups.AddDistinct(organization.Groups.ToArray());
            }

            return userGroups;
        }

        private static string GetUserId(IResolveFieldContext<object> context)
        {
            var user = context.GetCurrentPrincipal();

            return
                user.FindFirstValue(ClaimTypes.NameIdentifier) ??
                user.FindFirstValue("name") ??
                AnonymousUser.UserName;
        }

        private async Task<Contact> GetContact(string userId)
        {
            Contact contact = null;

            var userManager = _userManagerFactory();
            var user = await userManager.FindByIdAsync(userId);

            if (!string.IsNullOrEmpty(user?.MemberId))
            {
                contact = await _memberService.GetByIdAsync(user.MemberId) as Contact;
            }

            return contact;
        }

        private async Task<Organization> GetOrganization(string organizationId)
        {
            Organization organization = null;

            if (!string.IsNullOrEmpty(organizationId))
            {
                organization = await _memberService.GetByIdAsync(organizationId) as Organization;
            }

            return organization;
        }
    }
}
