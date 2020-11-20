using Castle.MicroKernel.Registration;
using Castle.Windsor;
using OrvilleX.EventBus.Consumer;
using OrvilleX.Module;
using System;
using System.Collections.Generic;

namespace OrvilleX.EventBus
{
    /// <summary>
    /// 事件总线模块注册
    /// </summary>
    public class EventBusModule : ModuleRegister
    {
        public override void ConfigureServices(IWindsorContainer container, List<Type> types)
        {
            //自动扫描Handler并注册
            container.Register(Classes.From(types)
                .BasedOn(typeof(IAsyncNotificationHandler<>))
                .WithService.Self()
                .WithService.DefaultInterfaces()
                .LifestyleTransient());
        }
    }
}
