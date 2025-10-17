using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace Keops.AspNetCore.DataTables
{
    internal class ModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder ModelBinder { get; private set; }

        public ModelBinderProvider() { }

        public ModelBinderProvider(IModelBinder modelBinder) { ModelBinder = modelBinder; }

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (IsBindable(context.Metadata.ModelType))
            {
                ModelBinder ??= new ModelBinder();
                return ModelBinder;
            }
            else 
                return null;
        }

        private bool IsBindable(Type type) => type.Equals(typeof(IDataTablesRequest));
    }
}
