using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ERPack.EntityFrameworkCore;
using ERPack.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace ERPack.Web.Tests
{
    [DependsOn(
        typeof(ERPackWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class ERPackWebTestModule : AbpModule
    {
        public ERPackWebTestModule(ERPackEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ERPackWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(ERPackWebMvcModule).Assembly);
        }
    }
}