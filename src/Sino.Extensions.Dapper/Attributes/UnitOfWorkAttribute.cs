using AspectCore.DynamicProxy;
using Sino.Extensions.Dapper.Expressions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sino.Extensions.Dapper.Attributes
{
    public class UnitOfWorkAttribute : AbstractInterceptorAttribute
    {
        private IUnitOfWork iUnitOfWork;

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                iUnitOfWork = context.ServiceProvider.GetService(typeof(IUnitOfWork)) as IUnitOfWork;
                iUnitOfWork.BeginTransaction();
                await next(context);
                iUnitOfWork.Commit();
            }
            catch (Exception ex)
            {
                iUnitOfWork.Rollback();
                //判断异常类型 如果是Sino异常则进行转发
                var sex = ex.InnerException as SinoException;
                if (sex != null)
                    throw new SinoException(sex.Message, sex.Code);
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
