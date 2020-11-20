namespace OrvilleX.Dependency.Scope
{
    internal class ExtensionContainerRootScope : ExtensionContainerScope
    {
		internal static ExtensionContainerRootScope RootScope { get; private set; }
		private ExtensionContainerRootScope() : base(null)
		{

		}

		public static ExtensionContainerRootScope BeginRootScope()
		{
			var scope = new ExtensionContainerRootScope();
			ExtensionContainerScope.current.Value = scope;
			RootScope = scope;
			return scope;
		}
	}
}
