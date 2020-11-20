using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrvilleX.Module
{
    /// <summary>
    /// 模块注册
    /// </summary>
    public interface IModuleRegister
    {
        void ConfigureServices(IWindsorContainer container, List<Type> types);
    }
}
